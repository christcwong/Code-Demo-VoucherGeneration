Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace BAL
    Public Class EasterVoucherBusinessClass
        Shared Sub New()

        End Sub

        Public Shared Function GetNumOfVoucherGeneratedByVoucherTypeID(ByVal voucherTypeID As Integer) As Integer
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT COUNT(*) FROM Voucher WHERE VoucherTypeID = @VoucherTypeID AND DateDiff(d, GenerateDate, GETDATE()) = 0")
            cmd.Parameters.AddWithValue("@VoucherTypeID", voucherTypeID)
            Return DAL.DBClass.ExecuteScalar(cmd, ErrorMessage)
        End Function
    End Class
End Namespace

