Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace BAL
    Public Class BranchBusinessClass
        Shared Sub New()

        End Sub

        Public Shared Function GetBranchByBranchID(ByVal branchID As Integer) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Branch WHERE BranchID = @branchID")
            cmd.Parameters.AddWithValue("@branchID", branchID)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function
    End Class
End Namespace

