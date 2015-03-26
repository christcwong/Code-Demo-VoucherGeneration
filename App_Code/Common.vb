Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Net.Mail
Imports System.Text
Imports System.Security.Cryptography
Imports System
Imports System.Net
Imports System.Drawing.Imaging
Imports _5centSms

Public Class Common

    Public Shared Sub SetTitle(ByRef lblTitle As Label, ByVal value As String)
        If Not lblTitle Is Nothing Then
            lblTitle.Text = value
        End If
    End Sub

    Public Shared Sub CreatePhotoFolder(ByVal folderName As String)
        Dim pathToCreate As String = "~/Image/" + folderName
        If Not Directory.Exists(HttpContext.Current.Server.MapPath(pathToCreate)) Then
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(pathToCreate))
        End If
    End Sub

    Public Shared Function CutDesc(ByVal desc As String, ByVal length As Integer) As String
        If Not desc Is Nothing Then
            If desc.Length > length Then
                desc = desc.Substring(0, length) + "..."
            End If
        End If
        Return desc
    End Function

    Public Shared Function CutDesc(ByVal desc As String, ByVal length As Integer, ByVal replaceSymbol As String) As String
        If Not desc Is Nothing Then
            If desc.Length > length Then
                desc = desc.Substring(0, length) + replaceSymbol
            End If
        End If
        Return desc
    End Function

    Public Shared Function CutDesc(ByVal desc As String, ByVal startIndex As Integer, ByVal length As Integer, ByVal replaceSymbol As String) As String
        If Not desc Is Nothing Then
            If desc.Length - startIndex > length Then
                desc = desc.Substring(startIndex, length) + replaceSymbol
            End If
        End If
        Return desc
    End Function

    Public Shared Function StripHtml(ByVal text As String) As String
        If String.IsNullOrWhiteSpace(text) Then
            Return String.Empty
        End If
        Dim r As String = Regex.Replace(text, "<(.|\n)*?>", String.Empty)
        Return Regex.Replace(r, "&(.|\n)*?;", String.Empty)
    End Function

    Public Shared Function Email(ByVal message As String, ByVal subject As String, ByVal toEmail As String, ByVal fromEmail As String) As Boolean
        Dim mailObj As MailMessage = New MailMessage(fromEmail, toEmail, subject, message)
        mailObj.IsBodyHtml = True
        'Dim SMTPServer As SmtpClient = New SmtpClient("mail.bigpond.com")
        Dim SMTPServer As SmtpClient = New SmtpClient("localhost")
        Try
            SMTPServer.Send(mailObj)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Email(ByVal message As String, ByVal subject As String, ByVal toEmail As String, ByVal fromEmail As String, ByVal fromName As String) As Boolean
        Dim mailObj As MailMessage = New MailMessage
        mailObj.From = New MailAddress(fromEmail, fromName)
        mailObj.To.Add(toEmail)
        mailObj.Subject = subject
        mailObj.Body = message
        mailObj.IsBodyHtml = True
        'Dim SMTPServer As SmtpClient = New SmtpClient("mail.bigpond.com")
        Dim SMTPServer As SmtpClient = New SmtpClient("localhost")
        Try
            SMTPServer.Send(mailObj)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Sms(ByVal message As String, ByVal toMobile As String, ByVal fromSender As String) As String
        Try
            Const HOST_NAME As String = "www.5centsms.com.au"
            Const HOST_PATH As String = "/a/send/"
            Dim strUsername As String = "victor@gxm.com.my"
            Dim strApikey As String = "PZTcpckwujTP7jwJeD3DPfwjsuWNbC"
            Dim strMessage As String = message
            Dim strSenderID As String = "0398878511"
            'Dim strSenderID As String = fromSender
            Dim strPhone As String = toMobile
            Dim strMsgID As String = ""
            Dim URL As String = "https://" & HOST_NAME & HOST_PATH & "?" &
            "apikey=" & HttpContext.Current.Server.UrlEncode(strApikey) &
            "&username=" & strUsername &
            "&message=" & HttpContext.Current.Server.UrlEncode(strMessage) &
            "&sender=" & HttpContext.Current.Server.UrlEncode(strSenderID) &
            "&to=" & HttpContext.Current.Server.UrlEncode(strPhone)

            Dim myUri As New Uri(URL, UriKind.Absolute)
            Dim instance As New WebClient
            Dim wp As New WebProxy("http://proxy.private.netregistry.net:3128")
            wp.UseDefaultCredentials = True
            instance.Proxy = wp
            instance.OpenRead(myUri)
            instance.Dispose()

            Return URL

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Using genusis.com
    'Public Shared Function Sms(ByVal message As String, ByVal toMobile As String, ByVal fromSender As String) As Boolean
    '    Try
    '        Const HOST_NAME As String = "gensuite.genusis.com"
    '        Const HOST_PATH As String = "/api/gateway.php"
    '        Dim strClientID As String = "gxm2"
    '        Dim strUsername As String = "system"
    '        Dim strPassword As String = "gxm218692"
    '        Dim strType As String = "SMS"
    '        Dim strMessage As String = message
    '        Dim strSenderID As String = fromSender
    '        Dim strPhone As String = toMobile
    '        Dim strMsgID As String = ""
    '        Dim URL As String = "http://" & HOST_NAME & HOST_PATH & "?" & "ClientID=" &
    '        HttpContext.Current.Server.UrlEncode(strClientID) & "&Username=" & HttpContext.Current.Server.UrlEncode(strUsername) &
    '        "&Password=" & HttpContext.Current.Server.UrlEncode(strPassword) & "&Type=" &
    '        HttpContext.Current.Server.UrlEncode(strType) & "&Message=" & HttpContext.Current.Server.UrlEncode(strMessage) &
    '        "&SenderID=" & HttpContext.Current.Server.UrlEncode(strSenderID) & "&Phone=" &
    '        HttpContext.Current.Server.UrlEncode(strPhone) & "&MsgID=" & HttpContext.Current.Server.UrlEncode(strMsgID)

    '        Dim instance As New WebClient
    '        Dim wp As New WebProxy("http://proxy.private.netregistry.net:3128")
    '        wp.UseDefaultCredentials = True
    '        instance.Proxy = wp
    '        instance.OpenRead(URL)
    '        instance.Dispose()

    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

    Public Shared Function GenerateUniqueKey() As String
        Dim U As New ASCIIEncoding

        'Create an MD5 object
        Dim Md5 As New MD5CryptoServiceProvider

        'Calculate a hash value from the Time
        Dim MyUniqueKey() As Byte = Md5.ComputeHash(U.GetBytes("cbs" & System.DateTime.Now.ToString & System.DateTime.Now.Millisecond.ToString & DateTime.Now.Ticks.ToString))

        'And convert it to String format to return
        Dim str As String = Convert.ToBase64String(MyUniqueKey)
        Return Regex.Replace(str, "([^\d\w])", "")

    End Function

    ' Please do not remove :) 
    ' Written by Kourosh Derakshan 
    ' 

    ' Saves an image as a jpeg image, with the given quality 
    ' Gets:
    '   path    - Path to which the image would be saved.
    '   quality - An integer from 0 to 100, with 100 being the 
    '             highest quality
    Public Shared Sub SaveJpeg(ByVal path As String, ByVal img As System.Drawing.Image, ByVal quality As Long)
        If ((quality < 0) OrElse (quality > 100)) Then
            Throw New ArgumentOutOfRangeException("quality must be between 0 and 100.")
        End If

        ' Encoder parameter for image quality
        Dim qualityParam As New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)
        ' Jpeg image codec 
        Dim jpegCodec As ImageCodecInfo = GetEncoderInfo("image/jpeg")

        Dim encoderParams As New EncoderParameters(1)
        encoderParams.Param(0) = qualityParam
        img.Save(path, jpegCodec, encoderParams)
    End Sub



    ' Returns the image codec with the given mime type 
    Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        ' Get image codecs for all image formats 
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()

        ' Find the correct image codec 
        For i As Integer = 0 To codecs.Length - 1
            If (codecs(i).MimeType = mimeType) Then
                Return codecs(i)
            End If
        Next i

        Return Nothing
    End Function
End Class


