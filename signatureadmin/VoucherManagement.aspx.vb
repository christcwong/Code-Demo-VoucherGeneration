Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.IO
Imports Ionic.Zip
Imports DotM.Html5.WebControls
Imports AjaxControlToolkit
Imports ZXing.QrCode


Partial Class signatureadmin_VoucherManagement
    Inherits System.Web.UI.Page
    'Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
    '    'If (Page.IsPostBack) Then
    '    '    'Dim fuPhoto As FileUpload = CType(lvVoucherType.InsertItem.FindControl("fuPhoto"), FileUpload)
    '    '    'Save the file to Server
    '    '    'If fuPhoto.HasFile Then
    '    '    '    fuPhoto_Change(sender, e)
    '    '    'End If
    '    '    ''Explicitly bind data
    '    '    'lvVoucherType.DataBind()
    '    'End If
    'End Sub
    Protected Sub uploadBtnsClicks(ByVal sender As Object, ByVal e As System.EventArgs, pblnSendEmail As Boolean, pblnQROnly As Boolean)
        'Protected Sub uploadBtnsClicks(ByVal sender As Object, ByVal e As System.EventArgs, strDelegateSaveMethod As Action(Of Integer, String, String, Integer, String, String, String))

        If fuCsv.HasFile Then
            If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
            Else
                IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
            End If

            'Save Csv file
            Dim csvPath As String = "~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") & ddlVoucherType.SelectedItem.Text & ".csv"
            Dim csvServerPath As String = Server.MapPath(csvPath)
            fuCsv.SaveAs(csvServerPath)

            ' Input voucher Generation request to DB.
            Dim entity As New DAL.ChinaBarDBEntities
            Dim voucherGenerationRequest As New DAL.VoucherGenerationRequest
            voucherGenerationRequest.VoucherTypeID = ddlVoucherType.SelectedValue
            voucherGenerationRequest.CsvPath = csvPath
            voucherGenerationRequest.RequestCreateUTC = System.DateTime.UtcNow
            voucherGenerationRequest.LastModifyUTC = System.DateTime.UtcNow
            voucherGenerationRequest.Status = "Created"
            entity.AddToVoucherGenerationRequests(voucherGenerationRequest)
            entity.SaveChanges()

            ' Finally call web service to process it.
            Dim vms As WebServiceProxy.VoucherWebService = New WebServiceProxy.VoucherWebService
            vms.ProcessVoucherGenerationRequestAsync(voucherGenerationRequest.VoucherGenerationRequestID, pblnSendEmail, pblnQROnly)

            lvVoucherGenerationRequest.DataBind()
            lvVoucher.DataBind()
        End If
    End Sub

#Region "Code blocks for btnUploadXYZ_Click"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        uploadBtnsClicks(sender, e, True, False)

        ' ** Old codes without using delegate **
        '
        'If fuCsv.HasFile Then
        '    If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
        '    Else
        '        IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
        '    End If

        '    'Save Csv file
        '    Dim csvPath As String = Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv")
        '    fuCsv.SaveAs(csvPath)

        '    Dim sr As New IO.StreamReader(csvPath)
        '    Using sr
        '        ' Check the headers from the first line
        '        Dim inputHeader As String = sr.ReadLine()

        '        If (inputHeader = "") Then
        '            ' Nothing can be done if it is empty
        '        Else
        '            ' Validate input Header first
        '            Dim csvHeader() As String = inputHeader.Split(",")
        '            Dim blnHeaderValid As Boolean = True
        '            If (Not (csvHeader(0).ToUpper.Contains("NAME"))) Then
        '                blnHeaderValid = False
        '            End If
        '            If (csvHeader(1).ToUpper <> "EMAIL") Then
        '                blnHeaderValid = False
        '            End If
        '            If (csvHeader(2).ToUpper <> "CONTACT") Then
        '                blnHeaderValid = False
        '            End If

        '            If (blnHeaderValid) Then
        '                ' Subsequent reading for main data
        '                Dim inputLine As String = ""
        '                inputLine = sr.ReadLine

        '                While inputLine <> ""
        '                    Dim csv() As String = inputLine.Split(",")

        '                    Dim genCode As String
        '                    genCode = Common.GenerateUniqueKey
        '                    Dim entity As New DAL.ChinaBarDBEntities
        '                    Dim voucher As New DAL.Voucher

        '                    ' Generate a new key if there is any existing key collided
        '                    While ((From v In entity.Vouchers
        '                                            Where v.Code = genCode
        '                                            Select v.Code).Any())
        '                        genCode = Common.GenerateUniqueKey
        '                    End While
        '                    SaveAndSendVoucher(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
        '                    ' Next iteration
        '                    inputLine = sr.ReadLine
        '                End While
        '            End If
        '        End If
        '        lvVoucher.DataBind()
        '        sr.Close()
        '    End Using
        'End If
    End Sub

    Protected Sub btnUploadQROnly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadQROnly.Click
        uploadBtnsClicks(sender, e, False, True)

        ' ** Old codes without using delegate **
        '
        'If fuCsv.HasFile Then

        '    If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
        '    Else
        '        IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
        '    End If

        '    'Save Csv file
        '    Dim csvPath As String = Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv")
        '    fuCsv.SaveAs(csvPath)

        '    Dim sr As New IO.StreamReader(csvPath)
        '    sr.ReadLine()
        '    Dim inputLine As String = ""
        '    inputLine = sr.ReadLine
        '    While inputLine <> ""
        '        Dim csv() As String = inputLine.Split(",")

        '        Dim genCode As String
        '        genCode = Common.GenerateUniqueKey
        '        Dim entity As New DAL.ChinaBarDBEntities
        '        Dim voucher As New DAL.Voucher

        '        ' Modify generate function for all btn
        '        If entity.Vouchers.Any = True Then
        '            Dim codeArray = From v In entity.Vouchers
        '                            Where v.Code = genCode
        '                             Select v.Code
        '            For Each item In codeArray
        '                While item = genCode
        '                    genCode = Common.GenerateUniqueKey
        '                End While
        '            Next
        '            SaveQR(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
        '        Else
        '            SaveQR(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
        '        End If
        '        inputLine = sr.ReadLine
        '    End While

        '    lvVoucher.DataBind()
        '    sr.Close()

        'End If
    End Sub

    Protected Sub btnUploadOnly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadOnly.Click
        uploadBtnsClicks(sender, e, False, False)

        ' ** Old codes without using delegate **
        '
        'If fuCsv.HasFile Then
        '    If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
        '    Else
        '        IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
        '    End If

        '    fuCsv.SaveAs(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv"))

        '    Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv"))
        '    sr.ReadLine()
        '    Dim inputLine As String = ""
        '    inputLine = sr.ReadLine
        '    Using sr
        '        While inputLine <> ""
        '            Dim csv() As String = inputLine.Split(",")

        '            Dim genCode As String
        '            genCode = Common.GenerateUniqueKey
        '            Dim entity As New DAL.ChinaBarDBEntities
        '            Dim voucher As New DAL.Voucher

        '            If entity.Vouchers.Any = True Then
        '                Dim codeArray = From v In entity.Vouchers
        '                                 Select v.Code
        '                For Each item In codeArray
        '                    While item = genCode
        '                        genCode = Common.GenerateUniqueKey
        '                    End While
        '                Next
        '                SaveVoucher(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
        '            Else
        '                SaveVoucher(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
        '            End If
        '            inputLine = sr.ReadLine
        '        End While

        '        lvVoucher.DataBind()
        '        sr.Close()
        '    End Using
        'End If
    End Sub

    'Protected Sub btnUploadWithQR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadWithQR.Click
    '    uploadBtnsClicks(sender, e, False, False)

    '    ' ** Old codes without using delegate **
    '    '
    '    'If fuCsv.HasFile Then

    '    '    If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
    '    '    Else
    '    '        IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
    '    '    End If

    '    '    'Save Csv file
    '    '    Dim csvPath As String = Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv")
    '    '    fuCsv.SaveAs(csvPath)

    '    '    Dim sr As New IO.StreamReader(csvPath)
    '    '    sr.ReadLine()
    '    '    Dim inputLine As String = ""
    '    '    inputLine = sr.ReadLine
    '    '    While inputLine <> ""
    '    '        Dim csv() As String = inputLine.Split(",")

    '    '        Dim genCode As String = csv(3)
    '    '        Dim entity As New DAL.ChinaBarDBEntities
    '    '        Dim voucher As New DAL.Voucher

    '    '        If entity.Vouchers.Any = True Then
    '    '            Dim codeArray = From v In entity.Vouchers
    '    '                             Select v.Code
    '    '            For Each item In codeArray
    '    '                While item = genCode
    '    '                    genCode = Common.GenerateUniqueKey
    '    '                End While
    '    '            Next
    '    '            SaveWithOwnQR(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
    '    '        Else
    '    '            SaveWithOwnQR(ddlVoucherType.SelectedValue, genCode, ddlVoucherType.SelectedItem.Text, csv(0), csv(1), csv(2))
    '    '        End If
    '    '        inputLine = sr.ReadLine
    '    '    End While

    '    '    lvVoucher.DataBind()
    '    '    sr.Close()

    '    'End If
    'End Sub
#End Region

    Private Sub SaveAndSendVoucher(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)

        Dim qr As New GoogleQRGenerator.GoogleQR(code, qrSize.ToString + "x" + qrSize.ToString)

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        'Generate Image
        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim bitMapImage As New System.Drawing.Bitmap(Server.MapPath("~/Data/Voucher/" & details.Name.ToString & "/" & details.Image.ToString))
        ' why the image is resized to 150 * 150 ?
        bitMapImage.SetResolution(150, 150)
        Using graphicImage As Graphics = Graphics.FromImage(bitMapImage)
            Dim e As EncoderParameters
            graphicImage.CompositingQuality = CompositingQuality.HighQuality
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias
            graphicImage.InterpolationMode = InterpolationMode.HighQualityBicubic

            Dim image As Image
            image = image.FromFile(Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg")
            graphicImage.DrawImage(image, New Point(details.CodeXPosition, details.CodeYPosition))
            graphicImage.DrawString(Today.AddDays(21).ToString("dd MMMM yyyy"), New Font("Arial", 13, FontStyle.Bold), SystemBrushes.WindowText, New Point(167, 231))
            graphicImage.DrawString(code, New Font("Arial", 4, FontStyle.Bold), SystemBrushes.WindowText, New Point(details.CodeXPosition + 15, details.CodeYPosition + 145))

            e = New EncoderParameters(2)
            e.Param(0) = New EncoderParameter(Encoder.Quality, 100)

            e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))

            bitMapImage.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)

            graphicImage.Dispose()
            image.Dispose()
            bitMapImage.Dispose()
        End Using

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

        lvVoucher.DataBind()

        'Send Voucher
        Select Case name
            Case "Subscriber Promo Mon to Fri Lunch Voucher"
                Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Mon to Fri Lunch Voucher/edm_vouchers_giveaway_lunch_1_to_5.html"))
                Using sr
                    Dim message As String = sr.ReadToEnd
                    message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
                    Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Lunch Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
                    sr.Close()
                End Using

            Case "Subscriber Promo Mon to Fri Dinner Voucher"
                Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Mon to Fri Dinner Voucher/edm_vouchers_giveaway_dinner_1_to_5.html"))
                Using sr
                    Dim message As String = sr.ReadToEnd
                    message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
                    Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Dinner Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
                    sr.Close()
                End Using

            Case "Subscriber Promo Sat Lunch Voucher"
                Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Sat Lunch Voucher/edm_vouchers_giveaway_lunch_6.html"))
                Using sr
                    Dim message As String = sr.ReadToEnd
                    message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
                    Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Lunch Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
                    sr.Close()
                End Using

            Case "Subscriber Promo Sun Dinner Voucher"
                Dim sr As New IO.StreamReader(Server.MapPath("~/Data/Voucher/Subscriber Promo Sun Dinner Voucher/edm_vouchers_giveaway_dinner_7.html"))
                Using sr
                    Dim message As String = sr.ReadToEnd
                    message = String.Format(message, {customerName, "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg", Today.AddDays(21).ToString("dd MMMM yyyy"), Today.AddDays(21).ToString("dd MMMM yyyy"), "http://www.chinabarsignature.com/CustomerVoucherImage/" & code & ".jpg"})
                    Common.Email(message, "SUBSCRIBER SPECIAL - Free Complimentary Dinner Invitation to China Bar Signature!", email, "info@chinabarsignature.com", "China Bar Signature")
                    sr.Close()
                End Using

        End Select


    End Sub

    Private Sub SaveVoucher(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)

        Dim qrEncoder As New QRCodeWriter
        Dim qrMatrix As ZXing.Common.BitMatrix = qrEncoder.encode(code, ZXing.BarcodeFormat.QR_CODE, qrSize, qrSize)
        Dim barcodeWriter As New ZXing.BarcodeWriter
        Dim qr As Bitmap = barcodeWriter.Write(qrMatrix)

        'Dim qr As New GoogleQRGenerator.GoogleQR(code, qrSize.ToString + "x" + qrSize.ToString)


        'Dim urlToDownload As String = qr.ToString
        'Dim pathToSaveQR As String = Server.MapPath("~/CustomerVoucherImage/") + code + "TempQR.jpg"
        'Dim client As New Net.WebClient
        ''Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        ''wp.UseDefaultCredentials = True
        ''client.Proxy = wp
        'client.DownloadFile(urlToDownload, pathToSaveQR)
        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim img1 As New System.Drawing.Bitmap(Server.MapPath("~/Data/Voucher/VoucherImage/" & details.Image.ToString))
        'Dim img2 As New System.Drawing.Bitmap(pathToSaveQR)
        Dim img2 As New System.Drawing.Bitmap(qr)

        Dim img3 As New Bitmap(img1.Width, img1.Height)
        img1.SetResolution(96.0F, 96.0F)
        Graphics.FromImage(img3).DrawImage(img1, 0, 0)

        Dim img4 As New Bitmap(img2.Width, img2.Height)
        img2.SetResolution(96.0F, 96.0F)
        Graphics.FromImage(img4).DrawImage(img2, 0, 0)

        Dim g As Graphics = Graphics.FromImage(img3)
        g.DrawImage(img4, New Point(details.CodeXPosition, details.CodeYPosition))
        'g.DrawString(code, New Font("Arial", 12, FontStyle.Bold), Brushes.AntiqueWhite, New Point(details.CodeXPosition - 5, details.CodeYPosition + 2 * qrSize + 10))

        'Dim imgStream As New MemoryStream()
        'img1.Save(imgStream, Imaging.ImageFormat.Jpeg)

        'Response.ContentType = "image/jpeg"
        'Response.BinaryWrite(imgStream.ToArray())
        img3.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"))

        'Cleanup
        'imgStream.Close()
        'imgStream.Dispose()
        img1.Dispose()
        img2.Dispose()
        img3.Dispose()
        img4.Dispose()
        g.Dispose()

        'IO.File.Delete(pathToSaveQR)

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

#Region "Unused Old Code - SaveVoucher()"
    'Private Sub SaveVoucher(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal customerName As String, ByVal email As String, ByVal contact As String)
    '    Dim qr As New GoogleQRGenerator.GoogleQR(code, "250x250")

    '    Dim urlToDownload As String = qr.ToString
    '    Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg"
    '    Dim client As New Net.WebClient
    '    'Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
    '    'wp.UseDefaultCredentials = True
    '    'client.Proxy = wp
    '    client.DownloadFile(urlToDownload, pathToSave)

    '    Dim context As New DAL.ChinaBarDBEntities
    '    Dim details = (From o In context.VoucherTypes
    '                   Where o.VoucherTypeID = voucherTypeID
    '                   Select o).FirstOrDefault

    '    Dim tempImage As New System.Drawing.Bitmap(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & details.Image.ToString))
    '    tempImage.SetResolution(100, 100)
    '    Dim bitMapImage As New Bitmap(tempImage.Width - 41, tempImage.Height - 85)
    '    Graphics.FromImage(bitMapImage).DrawImage(tempImage, 0, 0)

    '    Using graphicImage As Graphics = Graphics.FromImage(bitMapImage)
    '        Dim e As EncoderParameters
    '        graphicImage.CompositingQuality = CompositingQuality.HighQuality
    '        graphicImage.SmoothingMode = SmoothingMode.AntiAlias
    '        graphicImage.InterpolationMode = InterpolationMode.HighQualityBicubic

    '        Dim image As Image
    '        image = image.FromFile(Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg")
    '        graphicImage.DrawImage(image, New Point(details.CodeXPosition, details.CodeYPosition))
    '        g.DrawString(code, New Font("Arial", 12, FontStyle.Bold), Brushes.AntiqueWhite, New Point(details.CodeXPosition - 5, details.CodeYPosition + 250))

    '        e = New EncoderParameters(2)
    '        e.Param(0) = New EncoderParameter(Encoder.Quality, 100)

    '        e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))

    '        bitMapImage.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)

    '        graphicImage.Dispose()
    '        image.Dispose()
    '        bitMapImage.Dispose()
    '    End Using


    '    Dim entity As New DAL.ChinaBarDBEntities
    '    Dim voucher As New DAL.Voucher
    '    voucher.VoucherTypeID = voucherTypeID
    '    voucher.Code = code
    '    voucher.CustomerName = customerName
    '    voucher.Email = email
    '    voucher.Image = code & ".jpg"
    '    voucher.Contact = contact
    '    voucher.GenerateDate = DateTime.Now
    '    voucher.Status = "unuse"

    '    entity.AddToVouchers(voucher)
    '    entity.SaveChanges()

    'End Sub
#End Region

    Private Sub SaveQR(ByVal voucherTypeID As Integer, ByVal code As String, ByVal name As String, ByVal qrSize As Integer, ByVal customerName As String, ByVal email As String, ByVal contact As String)
        Dim qr As New GoogleQRGenerator.GoogleQR(code, "100x100")

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg"
        Dim client As New Net.WebClient
        'Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        'wp.UseDefaultCredentials = True
        'client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        Dim bitMapImage As New System.Drawing.Bitmap(Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg")
        bitMapImage.SetResolution(150, 150)
        Dim e As EncoderParameters
        e = New EncoderParameters(2)
        e.Param(0) = New EncoderParameter(Encoder.Quality, 100)
        e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))
        bitMapImage.Save(Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)
        bitMapImage.Dispose()

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

    Protected Sub lvVoucherType_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewInsertEventArgs) Handles lvVoucherType.ItemInserting
        Dim txtName As TextBox = CType(lvVoucherType.InsertItem.FindControl("NameTextBox"), TextBox)
        Dim fuPhoto As FileUpload = CType(lvVoucherType.InsertItem.FindControl("fuPhoto"), FileUpload)
        Dim fileName As HiddenField = CType(lvVoucherType.InsertItem.FindControl("fileName"), HiddenField)
        Dim insertCanvas As WebControls.Image = CType(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)

        e.Values.Add("BranchID", ddlBranch.SelectedValue)
        e.Values.Add("CreateDate", Today)
        e.Values.Add("Status", "Active")

        'Dim tempPath As String = insertCanvas.ImageUrl
        'Dim storePath As String = "~/Data/Voucher/VoucherImage/" + txtName.Text + fileName.Value

        'If (Not String.IsNullOrWhiteSpace(tempPath) And IO.File.Exists(Server.MapPath(tempPath))) Then
        '    e.Values.Add("Image", txtName.Text + fileName.Value)
        '    If IO.File.Exists(Server.MapPath(storePath)) Then
        '        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Filled before", "alert('Sorry. Please use a different file name.');", True)
        '        e.Cancel = True
        '    Else
        '        'Copy from temp path to storePath
        '        'fuPhoto.SaveAs(Server.MapPath("~/Data/Voucher/VoucherImage/" + txtName.Text + fuPhoto.PostedFile.FileName))
        '        IO.File.Copy(Server.MapPath(tempPath), Server.MapPath(storePath))
        '    End If
        'End If


        ' Originally it save image from fuPhoto
        ' Now we use asyncFuPhoto for uploading image
        ' so we will move the temporary file to desired path.

        ' old code
        'Save image to server
        If fuPhoto.HasFile Then
            e.Values.Add("Image", txtName.Text + fuPhoto.PostedFile.FileName)
            If IO.File.Exists(Server.MapPath("~/Data/Voucher/VoucherImage/" + txtName.Text + fuPhoto.PostedFile.FileName)) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Filled before", "alert('Sorry. Please use a different file name.');", True)
                e.Cancel = True
            Else
                fuPhoto.SaveAs(Server.MapPath("~/Data/Voucher/VoucherImage/" + txtName.Text + fuPhoto.PostedFile.FileName))
            End If
        End If



    End Sub

    'Event Handler for Item DataBound.
    ' It will let two other subroutine to handler edit item data bound and insert item data bound
    Protected Sub lvVoucherType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvVoucherType.ItemDataBound
        If (lvVoucherType.EditIndex = DirectCast(e.Item, ListViewDataItem).DataItemIndex) Then
            lvVoucherType_ItemEditingDataBound(sender, e)
        End If
    End Sub

    'Protected Sub lvVoucherType_PreRenderComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRenderComplete
    '    lvVoucherType_ItemInsertRowBound(sender, e)
    '    'lvVoucherType_ItemEditingRow(sender, e)
    'End Sub

    'Protected Sub lvVoucherType_ItemInsertRowBound(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim item As ListViewItem = Me.lvVoucherType.InsertItem
    '    Dim fuPhoto As FileUpload = TryCast(item.FindControl("fuPhoto"), FileUpload)
    '    Dim QRPickerImage As WebControls.Image = TryCast(item.FindControl("insertCanvas"), WebControls.Image)
    '    Dim xTxtBox As TextBox = TryCast(item.FindControl("TextBox3"), TextBox)
    '    Dim yTxtBox As TextBox = TryCast(item.FindControl("TextBox4"), TextBox)
    '    Dim btnXY As TextBox = TryCast(item.FindControl("btnXY"), TextBox)
    '    ' Wants : for fuPhoto.onChange, temporarily show the image in QR Picker img and activate the img area selector. 
    '    ' The img area selector should be able to update Code X / Y TextBox
    '    Dim sb As New StringBuilder
    '    sb.Clear()
    '    ' Test script
    '    ' sb.Append("$(document).ready(function(){alert('InsertDataBound fuPhoto ID" + fuPhoto.ClientID + "')});")
    '    ' fuPhoto.onChange, Temporarily show the image.
    '    sb.Append("function fuPhotoChange(){alert('fuPhotoChanged!');var fuPhoto=$('#" + fuPhoto.ClientID + "');var file=this.files[0];var name=file.name;var size=file.size;var type=file.type;if(!type.match('image.*')){alert('type is '+type+' which is not an image');return;};alert('fileName '+name+'\r\nsize '+size+'\r\ntype '+type);$('#" + lvVoucherType.InsertItem.FindControl("btnXY").ClientID + "').prop('value','Preview');}")
    '    'sb.Append("function enableImgAreaSelect(imgW,imgH,ratio)$('#" + QRPickerImage.ClientID() + "').imgAreaSelect({aspectRatio:'1:1',maxWidth: ratio * imgW,maxHeight:ratio* imgH ,handles:true,onSelectEnd:function(img,selection){$('#" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/ratio));$('#" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/ratio));}});});")
    '    sb.Append("$(document).ready(function(){$('#" + fuPhoto.ClientID + "').change(fuPhotoChange);});")
    '    'sb.Append("$(document).ready(function(){$('#" + e.Item.FindControl("Image2").ClientID + "').imgAreaSelect({aspectRatio:'1:1',maxWidth:100*" + ratio.ToString() + ",maxHeight:100*" + ratio.ToString() + ",handles:true,onSelectEnd:function(img,selection){$('#" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));$('#" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});});")
    '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "Add JS to Insert Image", sb.ToString(), True)
    'End Sub

    Protected Sub lvVoucherType_ItemEditingDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
        Dim item = CType(lvVoucherType.Items(lvVoucherType.EditIndex), ListViewDataItem)
        If lvVoucherType.EditIndex > -1 Then
            Dim fuPhoto As TextBox = TryCast(e.Item.FindControl("TextBox2"), TextBox)
            Dim xTxtBox As TextBox = TryCast(e.Item.FindControl("TextBox3"), TextBox)
            Dim yTxtBox As TextBox = TryCast(e.Item.FindControl("TextBox4"), TextBox)
            Dim QRPickerImage As WebControls.Image = TryCast(e.Item.FindControl("Image2"), WebControls.Image)
            Dim ddlQRDimension As DropDownList = TryCast(e.Item.FindControl("ddlQRDimension"), DropDownList)

            'Fancy Box version <-- buggy
            'sb.Append("function showImgAreaSelect(){if($('#fancybox-img').is(':visible')){$('#fancybox-img').imgAreaSelect({aspectRatio:'1:1',maxWidth:100,maxHeight:100,handles:true,parent:'#fancybox-content',onSelectEnd:function(img,selection){$('#" + xTxtBox.ClientID + "').val(Math.floor(selection.x1));$('#" + yTxtBox.ClientID + "').val(Math.floor(selection.y1));}});$('#fancybox-content').unbind('click');$('#lightbox-nav').remove();}else setTimeout(showImgAreaSelect,50);};$(document).ready(function(){$('#EditImageQRXY').fancybox();$('#EditImageQRXY').click(showImgAreaSelect);});")
            'without fancyBox
            ' Check if there is existing image. If no, forget about it.


            If (Not String.IsNullOrEmpty(fuPhoto.Text)) Then
                '1. Resize the Image
                Dim tempDirectory As String = "~/Data/Voucher/VoucherImage/"
                Dim tempServerDirectory As String = Server.MapPath(tempDirectory)
                Dim fileName As String = fuPhoto.Text
                Dim tempPath As String = tempDirectory + fileName
                Dim uploadPath As String = Server.MapPath(tempPath)
                Dim insertCanvas = CType(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)
                'Need to take care of the resize ratio.
                Dim ratio As Double = 1
                Dim imgW As Integer
                Dim imgH As Integer
                Dim QRPickerImg = CType(e.Item.FindControl("Image2"), WebControls.Image)
                If File.Exists(uploadPath) Then
                    Dim backImage As New System.Drawing.Bitmap(uploadPath)
                    ResizeCanvas(backImage.Height, backImage.Width, imgH, imgW, ratio)
                    QRPickerImg.Width = imgW
                    QRPickerImg.Height = imgH
                End If
                '2. Add client side scripts for image area select
                Dim sb As New StringBuilder
                sb.Clear()
                sb.Append("function hideQRSelector(){alert('Please submit changes and edit again to use the QR Location Picker');$('#" + QRPickerImage.ClientID + "').imgAreaSelect({remove:true});$('#" + QRPickerImage.ClientID + "').hide();};$(document).ready(function(){var QRDimension=$('#" + ddlQRDimension.ClientID + "').val();var x1Value=$('#" + xTxtBox.ClientID + "').val()*" + ratio.ToString() + ";var y1Value=$('#" + yTxtBox.ClientID + "').val()*" + ratio.ToString() + ";$('#" + QRPickerImage.ClientID + "').imgAreaSelect({aspectRatio:'1:1',resizable:false,maxWidth:QRDimension*" + ratio.ToString() + ",minWidth:QRDimension*" + ratio.ToString() + ",maxHeight:QRDimension*" + ratio.ToString() + ",minHeight:QRDimension*" + ratio.ToString() + ",handles:true,onSelectEnd:function(img,selection){$('#" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));$('#" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});$('#" + QRPickerImage.ClientID + "').change(hideQRSelector);});")
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "Add JS to Edit Image", sb.ToString(), True)
            End If
        End If
    End Sub

    Protected Sub lvVoucherType_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewUpdateEventArgs) Handles lvVoucherType.ItemUpdating
        Dim fuPhoto As FileUpload = CType(lvVoucherType.EditItem.FindControl("fuPhoto"), FileUpload)
        Dim xTxtBox As TextBox = CType(lvVoucherType.EditItem.FindControl("TextBox3"), TextBox)
        Dim yTxtBox As TextBox = CType(lvVoucherType.EditItem.FindControl("TextBox4"), TextBox)

        If fuPhoto.HasFile Then
            If e.OldValues("Image") = "" Then
                fuPhoto.SaveAs(Server.MapPath("~/Data/Voucher/VoucherImage/") + e.OldValues("Name") + fuPhoto.PostedFile.FileName)
                e.NewValues("Image") = e.OldValues("Name") + fuPhoto.PostedFile.FileName
            Else
                If IO.File.Exists(Server.MapPath("~/Data/Voucher/VoucherImage/" + e.OldValues("Image"))) Then
                    IO.File.Delete(Server.MapPath("~/Data/Voucher/VoucherImage/" + e.OldValues("Image")))
                End If
                e.NewValues("Image") = e.OldValues("Name") + fuPhoto.PostedFile.FileName
                fuPhoto.SaveAs(Server.MapPath("~/Data/Voucher/VoucherImage/" + e.NewValues("Image")))
            End If
        End If
        lvVoucherType.DataBind()
    End Sub

    Protected Sub lvVoucherType_ItemDeleting(sender As Object, e As System.Web.UI.WebControls.ListViewDeleteEventArgs) Handles lvVoucherType.ItemDeleting

    End Sub


    'Protected Sub btnSubscriber2013_Click(sender As Object, e As System.EventArgs) Handles btnSubscriber2013.Click
    '    If fuCsv.HasFile Then
    '        If IO.Directory.Exists(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/")) Then
    '        Else
    '            IO.Directory.CreateDirectory(Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/"))
    '        End If

    '        'Save Csv file
    '        Dim csvPath As String = Server.MapPath("~/Data/Voucher/" & ddlVoucherType.SelectedItem.Text & "/" & Today.Date.ToString("yyyyMMdd") & ddlVoucherType.SelectedItem.Text & ".csv")
    '        fuCsv.SaveAs(csvPath)

    '        Dim sr As New IO.StreamReader(csvPath)
    '        Using sr

    '            'Count 40 for 100 voucher type ID
    '            For i As Integer = 1 To 40

    '                Dim inputLine As String = ""
    '                inputLine = sr.ReadLine
    '                Dim csv() As String = inputLine.Split(",")
    '                Dim genCode As String
    '                genCode = Common.GenerateUniqueKey
    '                Dim entity As New DAL.ChinaBarDBEntities
    '                Dim voucher As New DAL.Voucher
    '                Dim QRSize As Integer = (From vt In entity.VoucherTypes
    '                                                Where vt.VoucherTypeID = ddlVoucherType.SelectedValue
    '                                                Select vt.QRDimension).FirstOrDefault()
    '                If (QRSize = 0) Then
    '                    QRSize = 100
    '                End If
    '                If entity.Vouchers.Any = True Then
    '                    Dim codeArray = From v In entity.Vouchers
    '                                     Select v.Code
    '                    For Each item In codeArray
    '                        While item = genCode
    '                            genCode = Common.GenerateUniqueKey
    '                        End While
    '                    Next
    '                    SaveAndSendVoucher(100, genCode, "Subscriber Promo Mon to Fri Lunch Voucher", QRSize, csv(0), csv(1), csv(2))
    '                Else
    '                    SaveAndSendVoucher(100, genCode, "Subscriber Promo Mon to Fri Lunch Voucher", QRSize, csv(0), csv(1), csv(2))
    '                End If
    '            Next

    '            'Count 30 for 101 voucher type ID
    '            For i As Integer = 1 To 30
    '                Dim inputLine As String = ""
    '                inputLine = sr.ReadLine
    '                Dim csv() As String = inputLine.Split(",")
    '                Dim genCode As String
    '                genCode = Common.GenerateUniqueKey
    '                Dim entity As New DAL.ChinaBarDBEntities
    '                Dim voucher As New DAL.Voucher
    '                Dim QRSize As Integer = (From vt In entity.VoucherTypes
    '                                                Where vt.VoucherTypeID = ddlVoucherType.SelectedValue
    '                                                Select vt.QRDimension).FirstOrDefault()
    '                If (QRSize = 0) Then
    '                    QRSize = 100
    '                End If
    '                If entity.Vouchers.Any = True Then
    '                    Dim codeArray = From v In entity.Vouchers
    '                                     Select v.Code
    '                    For Each item In codeArray
    '                        While item = genCode
    '                            genCode = Common.GenerateUniqueKey
    '                        End While
    '                    Next
    '                    SaveAndSendVoucher(101, genCode, "Subscriber Promo Mon to Fri Dinner Voucher", QRSize, csv(0), csv(1), csv(2))
    '                Else
    '                    SaveAndSendVoucher(101, genCode, "Subscriber Promo Mon to Fri Dinner Voucher", QRSize, csv(0), csv(1), csv(2))
    '                End If
    '            Next

    '            'Count 15 for 102 voucher type ID
    '            For i As Integer = 1 To 15
    '                Dim inputLine As String = ""
    '                inputLine = sr.ReadLine
    '                Dim csv() As String = inputLine.Split(",")
    '                Dim genCode As String
    '                genCode = Common.GenerateUniqueKey
    '                Dim entity As New DAL.ChinaBarDBEntities
    '                Dim voucher As New DAL.Voucher
    '                Dim QRSize As Integer = (From vt In entity.VoucherTypes
    '                                                Where vt.VoucherTypeID = ddlVoucherType.SelectedValue
    '                                                Select vt.QRDimension).FirstOrDefault()
    '                If (QRSize = 0) Then
    '                    QRSize = 100
    '                End If
    '                If entity.Vouchers.Any = True Then
    '                    Dim codeArray = From v In entity.Vouchers
    '                                     Select v.Code
    '                    For Each item In codeArray
    '                        While item = genCode
    '                            genCode = Common.GenerateUniqueKey
    '                        End While
    '                    Next
    '                    SaveAndSendVoucher(102, genCode, "Subscriber Promo Sat Lunch Voucher", QRSize, csv(0), csv(1), csv(2))
    '                Else
    '                    SaveAndSendVoucher(102, genCode, "Subscriber Promo Sat Lunch Voucher", QRSize, csv(0), csv(1), csv(2))
    '                End If
    '            Next

    '            'Count 15 for 103 voucher type ID
    '            For i As Integer = 1 To 15
    '                Dim inputLine As String = ""
    '                inputLine = sr.ReadLine
    '                Dim csv() As String = inputLine.Split(",")
    '                Dim genCode As String
    '                genCode = Common.GenerateUniqueKey
    '                Dim entity As New DAL.ChinaBarDBEntities
    '                Dim voucher As New DAL.Voucher
    '                Dim QRSize As Integer = (From vt In entity.VoucherTypes
    '                                                Where vt.VoucherTypeID = ddlVoucherType.SelectedValue
    '                                                Select vt.QRDimension).FirstOrDefault()
    '                If (QRSize = 0) Then
    '                    QRSize = 100
    '                End If
    '                If entity.Vouchers.Any = True Then
    '                    Dim codeArray = From v In entity.Vouchers
    '                                     Select v.Code
    '                    For Each item In codeArray
    '                        While item = genCode
    '                            genCode = Common.GenerateUniqueKey
    '                        End While
    '                    Next
    '                    SaveAndSendVoucher(103, genCode, "Subscriber Promo Sun Dinner Voucher", QRSize, csv(0), csv(1), csv(2))
    '                Else
    '                    SaveAndSendVoucher(103, genCode, "Subscriber Promo Sun Dinner Voucher", QRSize, csv(0), csv(1), csv(2))
    '                End If
    '            Next


    '            lvVoucher.DataBind()
    '            sr.Close()


    '        End Using
    '    End If
    'End Sub

    'Protected Sub btnXY_Click(sender As Object, e As EventArgs)
    '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "btnXY alert", "$(document).ready(function(){alert('server side event is triggered.')});", True)
    '    Dim btnXY = CType(sender, LinkButton)
    '    'btnXY.Visible = False
    '    'Dim canvas = CType(lvVoucherType.InsertItem.FindControl("insertCanvas"), Canvas)
    '    'canvas.Visible = True
    '    'Dim backImageUrl = "/Data/Voucher/VoucherImage/Gift Card 200 Dollarsgiftcard_back-01.jpg"
    '    'If File.Exists(Server.MapPath(backImageUrl)) Then
    '    '    Dim backImage As New System.Drawing.Bitmap(Server.MapPath(backImageUrl))
    '    '    'Check image size and resize if needed
    '    '    'Dim outH As Integer
    '    '    'Dim outW As Integer
    '    '    'ResizeCanvas(backImage.Height, backImage.Width, outH, outW)
    '    '    'canvas.Height = outH
    '    '    'canvas.Width = outW
    '    '    'canvas.Style.Add("width", "100%")
    '    '    'canvas.Style.Add("height", "auto")
    '    '    canvas.Style.Add("background", "url('" + backImageUrl + "') no-repeat center center")
    '    '    'canvas.Style.Add("background", "url('" + backImageUrl + "') no-repeat center center fixed")
    '    '    canvas.Style.Add("background-size", "cover")
    '    '    canvas.Style.Add("-o-background-size", "cover")
    '    '    canvas.Style.Add("-moz-background-size", "cover")
    '    '    canvas.Style.Add("-webkit-background-size", "cover")
    '    'End If
    '    Dim txtName As TextBox = CType(lvVoucherType.InsertItem.FindControl("NameTextBox"), TextBox)
    '    Dim fuPhoto As FileUpload = CType(lvVoucherType.InsertItem.FindControl("fuPhoto"), FileUpload)
    '    Dim xTxtBox As TextBox = TryCast(lvVoucherType.InsertItem.FindControl("TextBox8"), TextBox)
    '    Dim yTxtBox As TextBox = TryCast(lvVoucherType.InsertItem.FindControl("TextBox9"), TextBox)
    '    Dim QRPickerImage As WebControls.Image = TryCast(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)
    '    Dim ddlQRDimension As DropDownList = TryCast(lvVoucherType.InsertItem.FindControl("ddlQRDimension"), DropDownList)
    '    Dim insertButton As LinkButton = TryCast(lvVoucherType.InsertItem.FindControl("insertButton"), LinkButton)
    '    insertButton.Enabled = True
    '    'Save the file to Server Temp location
    '    If fuPhoto.HasFile Then
    '        Dim tempDirectory As String = "~/Data/Voucher/VoucherImage/temp/"
    '        Dim tempServerDirectory As String = Server.MapPath(tempDirectory)
    '        Dim fileName As String = fuPhoto.PostedFile.FileName
    '        Dim tempPath As String = tempDirectory + fileName
    '        Dim uploadPath As String = Server.MapPath(tempPath)
    '        If IO.Directory.Exists(tempServerDirectory) Then
    '        Else
    '            IO.Directory.CreateDirectory(tempServerDirectory)
    '        End If
    '        If IO.File.Exists(uploadPath) Then
    '            IO.File.Delete(uploadPath)
    '        End If
    '        fuPhoto.SaveAs(uploadPath)
    '        Dim insertCanvas = CType(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)
    '        btnXY.Attributes.Add("href", tempPath)
    '        insertCanvas.ImageUrl = tempPath
    '        'Need to take care of the resize ratio.
    '        Dim ratio As Double = 1
    '        Dim imgW As Integer
    '        Dim imgH As Integer
    '        If File.Exists(uploadPath) Then
    '            Dim backImage As New System.Drawing.Bitmap(uploadPath)
    '            ResizeCanvas(backImage.Height, backImage.Width, imgH, imgW, ratio)
    '            insertCanvas.Width = imgW
    '            insertCanvas.Height = imgH
    '        End If
    '        'Without fancybox setting
    '        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Add JS to Temp Image", "$(document).ready(function(){alert('" + tempPath + "');$('#" + fuPhoto.ClientID() + "').hide();$('#" + btnXY.ClientID() + "').hide();$('#" + QRPickerImage.ClientID + "').show();$('#" + QRPickerImage.ClientID() + "').imgAreaSelect({aspectRatio:'1:1',maxWidth:" + CType(100 * ratio, Integer).ToString() + ",maxHeight:" + CType(100 * ratio, Integer).ToString() + ",handles:true,onSelectEnd:function(img,selection){$('#" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));$('#" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});});", True)
    '        'With fancy box
    '        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "Add JS to Temp Image", "function showImgAreaSelect(){if($('#fancybox-img').is(':visible')){$('#fancybox-img').imgAreaSelect({aspectRatio:'1:1',maxWidth:" + CType(100 * ratio, Integer).ToString() + ",maxHeight:" + CType(100 * ratio, Integer).ToString() + ",handles:true,parent:'#fancybox-content',onSelectEnd:function(img,selection){$('#" + lvVoucherType.InsertItem.FindControl("TextBox8").ClientID() + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));$('#" + lvVoucherType.InsertItem.FindControl("TextBox9").ClientID() + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});$('#fancybox-content').unbind('click');$('#lightbox-nav').remove();}else setTimeout(showImgAreaSelect,50);};$(document).ready(function(){$('#" + insertCanvas.ClientID + "').show();$('#canvasWrapper').fancybox();$('#canvasWrapper').click(showImgAreaSelect);});", True)

    '    End If


    'End Sub

    Private Sub ResizeCanvas(ByVal imgH As Integer, ByVal imgW As Integer, ByRef outH As Integer, ByRef outW As Integer, Optional ByRef reductionScale As Double = 1)
        Const maxW As Integer = 400
        Const maxH As Integer = 300
        If ((imgH > maxH) Or (imgW > maxW)) Then
            'resize is needed
            Dim ratioH As Double = maxH / imgH
            Dim ratioW As Double = maxW / imgW
            If (ratioH < ratioW) Then
                reductionScale = ratioH
                outH = maxH
                outW = CType(imgW * ratioH, Integer)
                Return
            Else
                reductionScale = ratioW
                outW = maxW
                outH = CType(imgH * ratioW, Integer)
            End If
            Return
        End If
        'no need to resize
        outH = imgH
        outW = imgW
        reductionScale = 1
    End Sub
    'Protected Sub ProcessUpload(sender As Object, e As AsyncFileUploadEventArgs)
    '    Dim asyncFuPhoto As AsyncFileUpload = CType(lvVoucherType.InsertItem.FindControl("asyncFuPhoto"), AsyncFileUpload)
    '    Dim QRPickerImage As WebControls.Image = TryCast(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)
    '    Dim ddlQRDimension As DropDownList = TryCast(lvVoucherType.InsertItem.FindControl("ddlQRDimension"), DropDownList)
    '    Dim xTxtBox As TextBox = TryCast(lvVoucherType.InsertItem.FindControl("TextBox8"), TextBox)
    '    Dim yTxtBox As TextBox = TryCast(lvVoucherType.InsertItem.FindControl("TextBox9"), TextBox)
    '    If (asyncFuPhoto.HasFile) Then
    '        Dim tempDirectory As String = "/Data/Voucher/VoucherImage/temp/"
    '        Dim tempServerDirectory As String = Server.MapPath(tempDirectory)
    '        Dim fileType As String = asyncFuPhoto.PostedFile.ContentType
    '        If (fileType.StartsWith("image")) Then
    '            Dim fileName As String = asyncFuPhoto.PostedFile.FileName
    '            Dim tempPath As String = tempDirectory + fileName
    '            Dim uploadPath As String = Server.MapPath(tempPath)
    '            If IO.Directory.Exists(tempServerDirectory) Then
    '            Else
    '                IO.Directory.CreateDirectory(tempServerDirectory)
    '            End If
    '            If IO.File.Exists(uploadPath) Then
    '                IO.File.Delete(uploadPath)
    '            End If
    '            asyncFuPhoto.SaveAs(uploadPath)
    '            QRPickerImage.ImageUrl = tempPath
    '            'Need to take care of the resize ratio.
    '            Dim ratio As Double = 1
    '            Dim imgW As Integer
    '            Dim imgH As Integer
    '            If File.Exists(uploadPath) Then
    '                Dim backImage As New System.Drawing.Bitmap(uploadPath)
    '                ResizeCanvas(backImage.Height, backImage.Width, imgH, imgW, ratio)
    '                QRPickerImage.Width = imgW
    '                QRPickerImage.Height = imgH
    '            End If
    '            'Without fancybox setting
    '            Dim sb As StringBuilder = New StringBuilder
    '            sb.Append("var imgUploaded=top.document.getElementById('" + QRPickerImage.ClientID + "');imgUploaded.width='" + imgW.ToString + "';imgUploaded.height='" + imgH.ToString + "';imgUploaded.src='" + tempPath + "';")
    '            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "img", sb.ToString, True)
    '            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "img", "top.document.ready(function(){alert('" + tempPath + "');top.document.getElementById('" + asyncFuPhoto.ClientID() + "').hide();top.document.getElementById('" + QRPickerImage.ClientID + "').show();top.document.getElementById('" + QRPickerImage.ClientID() + "').imgAreaSelect({aspectRatio:'1:1',maxWidth:" + CType(100 * ratio, Integer).ToString() + ",maxHeight:" + CType(100 * ratio, Integer).ToString() + ",handles:true,onSelectEnd:function(img,selection){top.document.getElementById('" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));top.document.getElementById('" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});});", True)
    '            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "img", "window.top.getElementById('imgUpload').src='" + tempPath + "';", True)
    '            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "img", "$(top.document).find('#imgUpload').src='" + tempPath + "';", True)
    '            'ClientScript.RegisterClientScriptBlock(Me.GetType(), "Add JS to async upload Image", ""top.document.ready(function(){alert('" + tempPath + "');top.document.getElementById('" + asyncFuPhoto.ClientID() + "').hide();top.document.getElementById('" + QRPickerImage.ClientID + "').show();top.document.getElementById('" + QRPickerImage.ClientID() + "').imgAreaSelect({aspectRatio:'1:1',maxWidth:" + CType(100 * ratio, Integer).ToString() + ",maxHeight:" + CType(100 * ratio, Integer).ToString() + ",handles:true,onSelectEnd:function(img,selection){top.document.getElementById('" + xTxtBox.ClientID + "').val(Math.floor(selection.x1/" + ratio.ToString() + "));top.document.getElementById('" + yTxtBox.ClientID + "').val(Math.floor(selection.y1/" + ratio.ToString() + "));}});});"", True)
    '        End If
    '    End If
    'End Sub


    'Protected Sub asyncUploadComplete(sender As Object, e As EventArgs)
    '    Dim asyncFuPhoto As AsyncFileUpload = CType(lvVoucherType.InsertItem.FindControl("asyncFuPhoto"), AsyncFileUpload)
    '    System.Threading.Thread.Sleep(5000)
    '    If (asyncFuPhoto.HasFile) Then
    '        Dim tempDirectory As String = "~/Data/Voucher/VoucherImage/temp/"
    '        Dim tempServerDirectory As String = Server.MapPath(tempDirectory)
    '        Dim fileName As String = asyncFuPhoto.PostedFile.FileName
    '        Dim tempPath As String = tempDirectory + fileName
    '        Dim uploadPath As String = Server.MapPath(tempPath)
    '        If IO.Directory.Exists(tempServerDirectory) Then
    '        Else
    '            IO.Directory.CreateDirectory(tempServerDirectory)
    '        End If
    '        If IO.File.Exists(uploadPath) Then
    '            IO.File.Delete(uploadPath)
    '        End If
    '        asyncFuPhoto.SaveAs(uploadPath)
    '    End If
    'End Sub

    'Protected Sub fuPhoto_Change(sender As Object, e As EventArgs)
    '    Dim txtName As TextBox = CType(lvVoucherType.InsertItem.FindControl("NameTextBox"), TextBox)
    '    Dim fuPhoto As FileUpload = CType(lvVoucherType.InsertItem.FindControl("fuPhoto"), FileUpload)
    '    'Save the file to Server
    '    If fuPhoto.HasFile Then
    '        Dim fileName As String = txtName.Text + fuPhoto.PostedFile.FileName
    '        Dim tempPath As String = "~/Data/Voucher/VoucherImage/temp/" + fileName
    '        Dim uploadPath As String = Server.MapPath(tempPath)

    '        If IO.File.Exists(uploadPath) Then
    '            IO.File.Delete(uploadPath)
    '        End If

    '        fuPhoto.SaveAs(uploadPath)
    '        Dim insertCanvas = CType(lvVoucherType.InsertItem.FindControl("insertCanvas"), WebControls.Image)
    '        insertCanvas.ImageUrl = tempPath
    '        insertCanvas.Visible = True
    '    End If


    'End Sub

    'Protected Sub ddlQRDimension_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Dim QRPickor As WebControls.Image = CType(Me.lvVoucherType.EditItem.FindControl("Image2"), WebControls.Image)
    '    QRPickor.Visible = False
    'End Sub

    'Protected Sub fuPhoto_Init(sender As Object, e As EventArgs)
    '    Dim fu As FileUpload = CType(sender, FileUpload)
    'End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lvVoucherGenerationRequest.DataBind()
    End Sub
    'Protected Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
    '    lvVoucher.DataBind()
    'End Sub
End Class
