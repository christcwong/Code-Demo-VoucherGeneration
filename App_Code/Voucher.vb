Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

Public Class CommonVoucher

    Public Shared Sub SaveVoucher(ByVal voucherTypeID As Integer, ByVal templateImage As String, ByVal code As String, ByVal customerName As String, ByVal email As String, ByVal contact As String)
        Dim qr As New GoogleQRGenerator.GoogleQR(code, "100x100")

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim bitMapImage As New System.Drawing.Bitmap(HttpContext.Current.Server.MapPath("~/Data/Voucher/VoucherImage/" + templateImage))
        Using graphicImage As Graphics = Graphics.FromImage(bitMapImage)
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias
            Dim image As Image
            image = image.FromFile(HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg")
            graphicImage.DrawImage(image, New Point(details.CodeXPosition, details.CodeYPosition))
            'graphicImage.DrawString(code, New Font("Arial", 7, FontStyle.Bold), SystemBrushes.WindowText, New Point(details.CodeXPosition - 20, details.CodeYPosition + 140))
            bitMapImage.Save(HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), ImageFormat.Jpeg)
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

    End Sub

    Public Shared Sub SaveHighQualityVoucher(ByVal voucherTypeID As Integer, ByVal templateImage As String, ByVal code As String, ByVal customerName As String, ByVal email As String, ByVal contact As String)
        Dim qr As New GoogleQRGenerator.GoogleQR(code, "100x100")

        Dim urlToDownload As String = qr.ToString
        Dim pathToSave As String = HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg"
        Dim client As New Net.WebClient
        Dim wp As New Net.WebProxy("http://proxy.private.netregistry.net:3128")
        wp.UseDefaultCredentials = True
        client.Proxy = wp
        client.DownloadFile(urlToDownload, pathToSave)

        Dim context As New DAL.ChinaBarDBEntities
        Dim details = (From o In context.VoucherTypes
                       Where o.VoucherTypeID = voucherTypeID
                       Select o).FirstOrDefault

        Dim bitMapImage As New System.Drawing.Bitmap(HttpContext.Current.Server.MapPath("~/Data/Voucher/VoucherImage/" + templateImage))
        bitMapImage.SetResolution(130, 130)
        Using graphicImage As Graphics = Graphics.FromImage(bitMapImage)
            Dim e As EncoderParameters
            graphicImage.CompositingQuality = CompositingQuality.HighQuality
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias
            graphicImage.InterpolationMode = InterpolationMode.HighQualityBicubic

            Dim image As Image
            image = image.FromFile(HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/") + "TempQR.jpg")
            graphicImage.DrawImage(image, New Point(details.CodeXPosition, details.CodeYPosition))
            graphicImage.DrawString(code, New Font("Arial", 7, FontStyle.Bold), SystemBrushes.WindowText, New Point(details.CodeXPosition - 20, details.CodeYPosition + 140))

            e = New EncoderParameters(2)
            e.Param(0) = New EncoderParameter(Encoder.Quality, 100)

            e.Param(1) = New EncoderParameter(Encoder.Compression, CType(EncoderValue.CompressionLZW, Long))

            bitMapImage.Save(HttpContext.Current.Server.MapPath("~/CustomerVoucherImage/" & code & ".jpg"), GetEncoderInfo("image/jpeg"), e)

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

    End Sub

    Public Shared Function GetEncoderInfo(ByVal sMime As String) As ImageCodecInfo
        Dim objEncoders As ImageCodecInfo()
        objEncoders = ImageCodecInfo.GetImageEncoders()
        For iLoop As Integer = 0 To objEncoders.Length - 1
            Return objEncoders(iLoop)
        Next
        Return Nothing
    End Function

End Class
