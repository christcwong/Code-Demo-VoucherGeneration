Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace BAL
    Public Class VoucherBusinessClass
        Shared Sub New()

        End Sub

        Public Shared Function GetVoucherTypeByVoucherTypeID(ByVal voucherTypeID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM VoucherType WHERE VoucherTypeID = @voucherTypeID")
            cmd.Parameters.AddWithValue("@VoucherTypeID", voucherTypeID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetVoucherTypeByPromoCode(ByVal promoCode As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM VoucherType WHERE PromoCode = @promoCode AND Status ='Active'")
            cmd.Parameters.AddWithValue("@promoCode", promoCode)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetVoucherByCode(ByVal code As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Voucher WHERE Code = @code")
            cmd.Parameters.AddWithValue("@code", code)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

    End Class
End Namespace
