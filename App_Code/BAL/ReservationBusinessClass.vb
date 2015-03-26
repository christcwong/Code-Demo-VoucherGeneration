Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace BAL
    Public Class ReservationBusinessClass

        Shared Sub New()

        End Sub

        Public Shared Function GetSessionBySessionID(ByVal sessionID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Session WHERE SessionID = @sessionID")
            cmd.Parameters.AddWithValue("@sessionID", sessionID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetBranchSessionByDate(ByVal branchID As Integer, ByVal sessionDate As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Session WHERE BranchID=@branchID AND Date = @sessionDate")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            cmd.Parameters.AddWithValue("@sessionDate", sessionDate)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetAllSpecialSessionByBranchID(ByVal branchID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM SpecialSession WHERE BranchID = @branchID")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetSpecialSessionBySessionID(ByVal specialSessionID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM SpecialSession WHERE SpecialSessionID = @specialSessionID")
            cmd.Parameters.AddWithValue("@specialSessionID", specialSessionID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetBranchSpecialSessionByDate(ByVal branchID As Integer, ByVal sessionDate As Date) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM SpecialSession WHERE BranchID=@branchID AND Date = @sessionDate")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            cmd.Parameters.AddWithValue("@sessionDate", sessionDate)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetBranchSessionByDateOrderByStartTime(ByVal branchID As Integer, ByVal sessionDate As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Session WHERE BranchID=@branchID AND Date = @sessionDate ORDER BY StartTime")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            cmd.Parameters.AddWithValue("@sessionDate", sessionDate)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetAllSpecialSessionByBranchIDOrderByStartTime(ByVal branchID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM SpecialSession WHERE BranchID = @branchID ORDER BY StartTime")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetBranchCustomerTotalByVoucherTypeIDAndReserveDateAndReserveSession(ByVal branchID As Integer, ByVal voucherTypeID As Integer, ByVal reserveDate As Date, ByVal reserveSession As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT Adult, Kid1, Kid2, Adult + Kid1 + Kid2 AS Total, VoucherTypeID FROM Reservation WHERE BranchID = @branchID AND VoucherTypeID = @voucherTypeID AND ReserveDate = @reserveDate AND ReserveSession = @reserveSession AND Status = 'CONFIRMED'")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            cmd.Parameters.AddWithValue("@voucherTypeID", voucherTypeID)
            cmd.Parameters.AddWithValue("@reserveDate", reserveDate)
            cmd.Parameters.AddWithValue("@reserveSession", reserveSession)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function
    End Class
End Namespace

