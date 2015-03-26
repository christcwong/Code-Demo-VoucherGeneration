Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace BAL
    Public Class SubscribeBusinessClass
        Shared Sub New()

        End Sub

        Public Shared Function GetSubscribe() As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Subscribe")
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetSubscribeByType(ByVal type As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Subscribe WHERE Type=@type")
            cmd.Parameters.AddWithValue("@type", type)
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetSubscribeLikeFbIdAndType(ByVal fbId As String, ByVal type2 As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Subscribe WHERE Type LIKE @fbId AND Type LIKE @type2")
            cmd.Parameters.AddWithValue("@fbId", "%" + fbId + "%")
            cmd.Parameters.AddWithValue("@type2", "%" + type2 + "%")
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function

        Public Shared Function GetSubscribeByEmailAndType(ByVal email As String, ByVal type As String, ByVal type2 As String, ByVal type3 As String) As DataTable
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT * FROM Subscribe WHERE Email=@email AND (Type LIKE @type OR Type LIKE @type2 OR Type LIKE @type3)")
            cmd.Parameters.AddWithValue("@email", email)
            cmd.Parameters.AddWithValue("@type", "%" + type + "%")
            cmd.Parameters.AddWithValue("@type2", "%" + type2 + "%")
            cmd.Parameters.AddWithValue("@type3", "%" + type3 + "%")
            Return DAL.DBClass.GetTable(cmd, ErrorMessage)
        End Function



        Public Shared Function GetSubscribeCount() As Integer
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT COUNT(*) FROM Subscribe")
            Return DAL.DBClass.ExecuteScalar(cmd, ErrorMessage)
        End Function

        Public Shared Function GetSubscribeCountByType(ByVal type As String) As Integer
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("SELECT COUNT(*) FROM Subscribe WHERE Type=@type")
            cmd.Parameters.AddWithValue("@type", type)
            Return DAL.DBClass.ExecuteScalar(cmd, ErrorMessage)
        End Function

        Public Shared Sub DeleteMobileByMobileNumber(ByVal mobileNumber As String)
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("DELETE FROM Mobile WHERE MobileNumber=@mobileNumber")
            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber)
            DAL.DBClass.ExecuteNonQuery(cmd, ErrorMessage)
        End Sub

        Public Shared Sub DeleteMobile()
            Dim ErrorMessage As String = ""
            Dim cmd = New SqlCommand("DELETE FROM Mobile")
            DAL.DBClass.ExecuteNonQuery(cmd, ErrorMessage)
        End Sub

    End Class
End Namespace
