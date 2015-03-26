Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Ionic.Zip
Imports ZXing.QrCode
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Net

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://www.chinabarsignature.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class VoucherWebService
    Inherits System.Web.Services.WebService

    ' it do nothing but waiting.
    '<SoapDocumentMethod(OneWay:=True), WebMethod()> _
    <SoapDocumentMethod(OneWay:=True), WebMethod()> _
    Public Sub HelloWorld()
        HelloWorld("Hello World")
    End Sub

    Private Sub HelloWorld(strWordToPrint As String)
        Dim savePath As String = Server.MapPath("/Upload/test.txt")
        IO.File.AppendAllLines(savePath, {System.DateTime.UtcNow + " Touched."})
        Threading.Thread.Sleep(60000)
        IO.File.AppendAllLines(savePath, {System.DateTime.UtcNow + " " + strWordToPrint})
    End Sub

    Private Sub Log(strLogMsg As String)
        Const maxFileSize = 4194304 ' 4096*1024 (4MB)
        Dim savePath As String = Server.MapPath("~/log/voucherGenerationService.log")
        If (Not IO.File.Exists(savePath)) Then
            Dim fileStream As IO.FileStream = IO.File.Create(savePath)
            fileStream.Close()
        End If
        Dim logInfo As IO.FileInfo = New IO.FileInfo(savePath)
        If (logInfo.Length > maxFileSize) Then
            ' mv file with timestamp
            IO.File.Move(savePath, savePath + Today.ToString("yyyymmdd"))
        End If
        IO.File.AppendAllLines(savePath, {System.DateTime.UtcNow + " " + strLogMsg})

    End Sub
    'WrapperFunction
    Private Sub Log(intId As Integer, strLogMsg As String)
        Log("[" + intId.ToString + "] " + strLogMsg)
    End Sub

    '<WebMethod()> _
    'Public Function ProcessVoucherGenerationRequest(ByVal intVoucherGenerationRequestId As Integer, blnSendEmail As Boolean, blnQROnly As Boolean) As String
    '<WebMethod()> _
    <WebMethod(), SoapDocumentMethod(OneWay:=True)> _
    Public Sub ProcessVoucherGenerationRequest(ByVal intVoucherGenerationRequestId As Integer, ByVal blnSendEmail As Boolean, ByVal blnQROnly As Boolean)
        Try
            'HelloWorld(intVoucherGenerationRequestId.ToString)
            Log("") ' Blank Line with date
            Log(intVoucherGenerationRequestId, "Start to Process Voucher Generation Request")
            ' 1 Read request from db
            Dim entity As New DAL.ChinaBarDBEntities
            Dim currentRequest = (From req In entity.VoucherGenerationRequests
                                 Where req.VoucherGenerationRequestID = intVoucherGenerationRequestId
                                 Select req).FirstOrDefault()
            If (Not (currentRequest Is Nothing)) Then
                Log(currentRequest.VoucherGenerationRequestID, "ID found")
                ' 2 Generate Vouchers and update db
                ' 2.1 Get basic info + validation
                currentRequest.RequestStartUTC = DateTime.UtcNow
                currentRequest.LastModifyUTC = DateTime.UtcNow
                entity.SaveChanges()
                If (currentRequest.VoucherTypeID Is Nothing) Then
                    Log(currentRequest.VoucherGenerationRequestID, "Error - VoucherTypeID is null")
                    ' 2.1.1 if csv file not found, update to error and end.
                    currentRequest.Status = "Error - VoucherTypeID is null"
                    currentRequest.LastModifyUTC = DateTime.UtcNow
                    entity.SaveChanges()
                    Log(currentRequest.VoucherGenerationRequestID, "Process Stopped")
                    Return
                    'Return "Error - VoucherTypeID is null"
                End If
                Dim csvFile As String = Server.MapPath(currentRequest.CsvPath)
                If (Not IO.File.Exists(csvFile)) Then
                    Log(currentRequest.VoucherGenerationRequestID, "Error - CSV File Not Found")
                    ' 2.1.1 if csv file not found, update to error and end.
                    currentRequest.Status = "Error - CSV File not found"
                    currentRequest.LastModifyUTC = DateTime.UtcNow
                    entity.SaveChanges()
                    Log(currentRequest.VoucherGenerationRequestID, "Process Stopped")
                    Return
                    'Return "Error - CSV File Not Found"
                End If
                Dim voucherList As List(Of String) = New List(Of String)
                Dim sr As New IO.StreamReader(csvFile)
                Using sr
                    ' 2.1.2 check csv content correct
                    ' Check the headers from the first line
                    Dim inputHeader As String = sr.ReadLine()
                    ' Validate input Header first
                    Dim csvHeader() As String = inputHeader.Split(",")
                    Dim blnHeaderValid As Boolean = True
                    If (inputHeader = "") Then
                        blnHeaderValid = False
                    End If
                    If (Not (csvHeader(0).ToUpper.Contains("NAME"))) Then
                        blnHeaderValid = False
                    End If
                    If (csvHeader(1).ToUpper <> "EMAIL") Then
                        blnHeaderValid = False
                    End If
                    If (csvHeader(2).ToUpper <> "CONTACT") Then
                        blnHeaderValid = False
                    End If

                    If (Not blnHeaderValid) Then
                        Log(currentRequest.VoucherGenerationRequestID, "Error - CSV File Header Invalid")
                        ' 2.1.1 if csv file not found, update to error and end.
                        currentRequest.Status = "Error - CSV File Header Invalid"
                        currentRequest.LastModifyUTC = DateTime.UtcNow
                        entity.SaveChanges()
                        Log(currentRequest.VoucherGenerationRequestID, "Process Stopped")
                        sr.Close()
                        Return
                        'Return "Error - CSV File Header Invalid"
                    End If
                    currentRequest.Status = "CSV File Found and Header is valid..."
                    currentRequest.LastModifyUTC = DateTime.UtcNow
                    entity.SaveChanges()
                    Log(currentRequest.VoucherGenerationRequestID, "CSV File Found and Header is valid...")
                    ' 2.2 Read and Generate Voucher for each line
                    Dim intNbVoucherGenerated = 0
                    Dim inputLine As String = ""
                    inputLine = sr.ReadLine
                    While inputLine <> ""
                        Dim csv() As String = inputLine.Split(",")

                        Dim genCode As String
                        genCode = Common.GenerateUniqueKey
                        Dim voucher As New DAL.Voucher

                        ' Generate a new key if there is any existing key collided
                        While ((From v In entity.Vouchers
                                                Where v.Code = genCode
                                                Select v.Code).Any())
                            genCode = Common.GenerateUniqueKey
                        End While
                        voucherList.Add(Server.MapPath("~/CustomerVoucherImage/" + genCode + ".jpg"))
                        Dim QRSize As Integer = (From vt In entity.VoucherTypes
                                                Where vt.VoucherTypeID = currentRequest.VoucherTypeID
                                                Select vt.QRDimension).FirstOrDefault()
                        If (QRSize = 0) Then
                            QRSize = 100
                        End If

                        intNbVoucherGenerated = intNbVoucherGenerated + 1
                        Try
                            If (blnSendEmail) Then
                                SaveAndSendVoucher(currentRequest.VoucherTypeID, genCode, currentRequest.VoucherType.Name, QRSize, csv(0), csv(1), csv(2))
                                currentRequest.Status = intNbVoucherGenerated.ToString + " Voucher(s) Generated and Sent ..."
                            Else
                                If (blnQROnly) Then
                                    SaveQR(currentRequest.VoucherTypeID, genCode, currentRequest.VoucherType.Name, QRSize, csv(0), csv(1), csv(2))
                                Else
                                    SaveVoucher(currentRequest.VoucherTypeID, genCode, currentRequest.VoucherType.Name, QRSize, csv(0), csv(1), csv(2))
                                End If
                                currentRequest.Status = intNbVoucherGenerated.ToString + " Voucher(s) Generated ..."
                            End If
                        Catch ex As Exception When TypeOf ex Is WebException OrElse TypeOf ex Is InvalidOperationException
                            currentRequest.LastModifyUTC = DateTime.UtcNow
                            Log(intVoucherGenerationRequestId, ex.ToString())
                            currentRequest.Status = "Error - Exception Occuried when Generating Voucher " + genCode
                            entity.SaveChanges()
                            Return
                        End Try

                        ' Update DB
                        currentRequest.LastModifyUTC = DateTime.UtcNow
                        entity.SaveChanges()
                        ' Next iteration
                        inputLine = sr.ReadLine
                    End While
                End Using
                Log(currentRequest.VoucherGenerationRequestID, "All vouchers Generated")
                currentRequest.LastModifyUTC = DateTime.UtcNow
                entity.SaveChanges()

                ' 3 Zip Vouchers and publish link
                ' After all vouchers generated, we need to pack them as zip
                ' Also put the csv inside.
                ' 1 pack them as zip with password
                Try

                    Using zip1 As ZipFile = New ZipFile
                        zip1.Password = "chinabarsignature"
                        zip1.Encryption = EncryptionAlgorithm.WinZipAes256
                        zip1.AddFiles(voucherList, String.Empty)
                        zip1.AddFile(csvFile, String.Empty)
                        Dim genCode = Common.GenerateUniqueKey
                        Dim zipPath = "~/CustomerVoucherImage/Request" + currentRequest.VoucherGenerationRequestID.ToString + "_" + genCode.ToString + ".zip"
                        Log(currentRequest.VoucherGenerationRequestID, "Zipping Vouchers...")
                        currentRequest.Status = "Zipping Vouchers..."
                        currentRequest.LastModifyUTC = DateTime.UtcNow
                        entity.SaveChanges()
                        zip1.Save(Server.MapPath(zipPath))
                        currentRequest.ZipFileLink = zipPath
                        currentRequest.LastModifyUTC = DateTime.UtcNow
                        entity.SaveChanges()
                    End Using
                Catch ex As Exception
                    'What should I do with the exception?

                    Log(ex.ToString())
                    'Throw ex
                    Return
                End Try

                ' 2 then delete voucher images if they are not in the email

                If Not blnSendEmail Then
                    Log("Deleting Generated Vouchers outside zip file...")
                    For Each voucher In voucherList
                        Try
                            IO.File.Delete(voucher)
                        Catch ex As Exception
                            Log(intVoucherGenerationRequestId, ex.ToString)
                            ' then nothing should be done.
                        End Try
                    Next
                End If

                Log(currentRequest.VoucherGenerationRequestID, "Completed")
                currentRequest.Status = "Completed"
                currentRequest.LastModifyUTC = DateTime.UtcNow
                entity.SaveChanges()
                Return
                'Return "Completed"
            Else
                Log(intVoucherGenerationRequestId, "Error - Request ID " + intVoucherGenerationRequestId.ToString + " not found")
                Log(intVoucherGenerationRequestId, "Process Stopped")
                Return
                'Return "Request ID not found"
            End If
        Catch ex As Exception
            Log(intVoucherGenerationRequestId, ex.ToString)
            Return
            'Return ex.ToString
        End Try
    End Sub

    <WebMethod()> _
    Public Function MyTestAsynchronousMethod(strName As String, waitTime As Integer) As String
        Log(1997, "My test AsynchronousMethod is being invoked : " + strName + " I waited for " + waitTime.ToString)
        System.Threading.Thread.Sleep(waitTime)
        Return strName + " I waited for " + waitTime.ToString
    End Function

    Private Sub SaveVoucher(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)

        Dim qr As New GoogleQRGenerator.GoogleQR(code, qrSize.ToString + "x" + qrSize.ToString)

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR" + code + ".jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        'Dim qrEncoder As New QRCodeWriter
        'Dim qrMatrix As ZXing.Common.BitMatrix = qrEncoder.encode(code, ZXing.BarcodeFormat.QR_CODE, qrSize, qrSize)
        'Dim barcodeWriter As New ZXing.BarcodeWriter
        'Dim qr As Bitmap = barcodeWriter.Write(qrMatrix)

        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim img1 As New System.Drawing.Bitmap(Server.MapPath("~/Data/Voucher/VoucherImage/" & details.Image.ToString))
        Dim img2 As New System.Drawing.Bitmap(pathToSave)
        'Dim img2 As New System.Drawing.Bitmap(qr)

        Dim img3 As New Bitmap(img1.Width, img1.Height)
        img1.SetResolution(96.0F, 96.0F)
        Graphics.FromImage(img3).DrawImage(img1, 0, 0)

        Dim img4 As New Bitmap(img2.Width, img2.Height)
        img2.SetResolution(96.0F, 96.0F)
        Graphics.FromImage(img4).DrawImage(img2, 0, 0)

        Dim g As Graphics = Graphics.FromImage(img3)
        g.DrawImage(img4, New Point(details.CodeXPosition, details.CodeYPosition))
        Common.SaveJpeg(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), img3, 100)
        'img3.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"))

        'Cleanup
        img1.Dispose()
        img2.Dispose()
        img3.Dispose()
        img4.Dispose()
        g.Dispose()

        IO.File.Delete(pathToSave)

        Dim entity As New DAL.ChinaBarDBEntities
        Dim voucher As New DAL.Voucher
        voucher.VoucherTypeID = voucherTypeID
        voucher.Code = code
        voucher.CustomerName = customerName
        voucher.Email = email
        voucher.Image = code & ".jpg"
        voucher.Contact = contact
        voucher.GenerateDate = DateTime.Now
        voucher.Status = "unuse"

        entity.AddToVouchers(voucher)
        entity.SaveChanges()

    End Sub
    Private Sub SaveAndSendVoucher(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)
        'Dim qrEncoder As New QRCodeWriter
        'Dim qrMatrix As ZXing.Common.BitMatrix = qrEncoder.encode(code, ZXing.BarcodeFormat.QR_CODE, qrSize, qrSize)
        'Dim barcodeWriter As New ZXing.BarcodeWriter
        'Dim qr As Bitmap = barcodeWriter.Write(qrMatrix)
        Dim qr As New GoogleQRGenerator.GoogleQR(code, qrSize.ToString + "x" + qrSize.ToString)

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR" + code + ".jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim img1 As New System.Drawing.Bitmap(Server.MapPath("~/Data/Voucher/VoucherImage/" & details.Image.ToString))
        'Dim img2 As New System.Drawing.Bitmap(qr)
        Dim img2 As New System.Drawing.Bitmap(pathToSave)

        Dim drawingPath As String = Server.MapPath("~/Data/Voucher/VoucherImage/" & details.Image.ToString)
        Dim bitMapImage As New System.Drawing.Bitmap(drawingPath)
        bitMapImage.SetResolution(96.0F, 96.0F)

        Using graphicImage As Graphics = Graphics.FromImage(bitMapImage)
            Dim e As EncoderParameters
            graphicImage.CompositingQuality = CompositingQuality.HighQuality
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias
            graphicImage.InterpolationMode = InterpolationMode.HighQualityBicubic

            Dim image As Image = img2
            graphicImage.DrawImage(image, New Point(details.CodeXPosition, details.CodeYPosition))
            ' Expiry Day ...  why should it be hard-coded ?
            'graphicImage.DrawString(Today.AddDays(21).ToString("dd MMMM yyyy"), New Font("Arial", 13, FontStyle.Bold), SystemBrushes.WindowText, New Point(167, 231))
            ' Strings on the image ... really ?
            'graphicImage.DrawString(code, New Font("Arial", 4, FontStyle.Bold), SystemBrushes.WindowText, New Point(details.CodeXPosition + 15, details.CodeYPosition + 145))

            e = New EncoderParameters(2)
            e.Param(0) = New EncoderParameter(Encoder.Quality, 100)

            e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))
            Common.SaveJpeg(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), bitMapImage, 100)
            'bitMapImage.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)

            graphicImage.Dispose()
            image.Dispose()
            bitMapImage.Dispose()
        End Using
        IO.File.Delete(pathToSave)

        Dim entity As New DAL.ChinaBarDBEntities
        Dim voucher As New DAL.Voucher
        voucher.VoucherTypeID = voucherTypeID
        voucher.Code = code
        voucher.CustomerName = customerName
        voucher.Email = email
        voucher.Image = code & ".jpg"
        voucher.Contact = contact
        voucher.GenerateDate = DateTime.Now
        voucher.Status = "unuse"

        entity.AddToVouchers(voucher)
        entity.SaveChanges()

        'Send Voucher
        Dim sr As New IO.StreamReader(Server.MapPath("~/Data/PromotionEmailTemplate.html"))
        Using sr
            Dim message As String = sr.ReadToEnd
            message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
            Common.Email(message, "SUBSCRIBER SPECIAL - Free Voucher to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
            sr.Close()
        End Using

        'Select Case name
        '    Case "Subscriber Promo Mon to Fri Lunch Voucher"
        '        Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Mon to Fri Lunch Voucher/edm_vouchers_giveaway_lunch_1_to_5.html"))
        '        Using sr
        '            Dim message As String = sr.ReadToEnd
        '            message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
        '            Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Lunch Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
        '            sr.Close()
        '        End Using

        '    Case "Subscriber Promo Mon to Fri Dinner Voucher"
        '        Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Mon to Fri Dinner Voucher/edm_vouchers_giveaway_dinner_1_to_5.html"))
        '        Using sr
        '            Dim message As String = sr.ReadToEnd
        '            message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
        '            Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Dinner Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
        '            sr.Close()
        '        End Using

        '    Case "Subscriber Promo Sat Lunch Voucher"
        '        Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Sat Lunch Voucher/edm_vouchers_giveaway_lunch_6.html"))
        '        Using sr
        '            Dim message As String = sr.ReadToEnd
        '            message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
        '            Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Lunch Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
        '            sr.Close()
        '        End Using

        '    Case "Subscriber Promo Sun Dinner Voucher"
        '        Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Sun Dinner Voucher/edm_vouchers_giveaway_dinner_7.html"))
        '        Using sr
        '            Dim message As String = sr.ReadToEnd
        '            message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
        '            Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Dinner Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
        '            sr.Close()
        '        End Using

        'End Select
    End Sub
    Private Sub SaveQR(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)

        'Dim qrEncoder As New QRCodeWriter
        'Dim qrMatrix As ZXing.Common.BitMatrix = qrEncoder.encode(code, ZXing.BarcodeFormat.QR_CODE, 100, 100)
        'Dim barcodeWriter As New ZXing.BarcodeWriter
        'Dim qr As Bitmap = barcodeWriter.Write(qrMatrix)

        'Dim context As New DAL.ChinaBarDBEntities
        'Dim details = (From o In context.VoucherTypes
        '               Where o.VoucherTypeID = voucherTypeID
        '               Select o).FirstOrDefault

        'Dim bitMapImage As New System.Drawing.Bitmap(qr)



        Dim qr As New GoogleQRGenerator.GoogleQR(code, "100x100")

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR" + code + ".jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        Dim bitMapImage As New System.Drawing.Bitmap(pathToSave)
        bitMapImage.SetResolution(150, 150)
        Dim e As EncoderParameters
        e = New EncoderParameters(2)
        e.Param(0) = New EncoderParameter(Encoder.Quality, 100)
        e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))
        'bitMapImage.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)
        Common.SaveJpeg(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), bitMapImage, 100)
        bitMapImage.Dispose()

        IO.File.Delete(pathToSave)

        Dim entity As New DAL.ChinaBarDBEntities
        Dim voucher As New DAL.Voucher
        voucher.VoucherTypeID = voucherTypeID
        voucher.Code = code
        voucher.CustomerName = customerName
        voucher.Email = email
        voucher.Image = code & ".jpg"
        voucher.Contact = contact
        voucher.GenerateDate = DateTime.Now
        voucher.Status = "unuse"

        entity.AddToVouchers(voucher)
        entity.SaveChanges()

    End Sub

    Private Sub SaveWithOwnQR(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)
        Dim entity As New DAL.ChinaBarDBEntities
        Dim voucher As New DAL.Voucher
        voucher.VoucherTypeID = voucherTypeID
        voucher.Code = code
        voucher.CustomerName = customerName
        voucher.Email = email
        voucher.Image = code & ".jpg"
        voucher.Contact = contact
        voucher.GenerateDate = DateTime.Now
        voucher.Status = "unuse"

        entity.AddToVouchers(voucher)
        entity.SaveChanges()

    End Sub

    Function GetEncoderInfo(ByVal sMime As String) As ImageCodecInfo
        Dim objEncoders As ImageCodecInfo()
        objEncoders = ImageCodecInfo.GetImageEncoders()
        For iLoop As Integer = 0 To objEncoders.Length - 1
            Return objEncoders(iLoop)
        Next
        Return Nothing
    End Function
End Class