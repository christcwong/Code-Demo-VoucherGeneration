Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Namespace DAL
    Public Class DBClass
        Private Shared ConnectionStringName As String = "ChinaBarDBConnection"
        Private Shared ReadOnly ConnectionString As String = WebConfigurationManager.ConnectionStrings(ConnectionStringName).ConnectionString
        
        Shared Sub New()

        End Sub

        ''' <summary>
        ''' Executes the command object returning an int value.
        ''' </summary>
        ''' <param name="command">Command object filled with the necessary parameters.</param>
        ''' <param name="ErrorMessage">Error Message if any.</param>
        ''' <returns>Int32 value indicating the error.</returns>
        ''' <remarks></remarks>
        Public Shared Function ExecuteNonQuery(ByVal command As SqlCommand, ByRef ErrorMessage As String) As Integer
            Dim conn As SqlConnection = Nothing
            Dim result As Integer = 0
            If (command.Equals("")) Then
                ErrorMessage = "Please initilise the command object."
                Return result = -1
            End If
            Try
                conn = New SqlConnection(ConnectionString)
                command.Connection = conn
                conn.Open()
                result = command.ExecuteNonQuery()
            Catch ex As SqlException
                ErrorMessage = "An exception has occured while executing the database transactions.  <BR>"
                ErrorMessage = ErrorMessage + ex.Message
            Finally
                If command IsNot Nothing Then
                    command.Dispose()
                End If
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                If conn IsNot Nothing Then
                    conn.Dispose()
                End If
            End Try
            Return result
        End Function

        ''' <summary>
        ''' Uses a fast datareader to load and return a datatable.
        ''' </summary>
        ''' <param name="command">Command object filled with necessary parameters.</param>
        ''' <param name="ErrorMessage">Error Message if any.</param>
        ''' <returns>Returns a loaded DataTable from the execution of the given Command.</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTable(ByVal command As SqlCommand, ByRef ErrorMessage As String) As DataTable
            Dim conn As SqlConnection = Nothing
            Dim reader As SqlDataReader = Nothing
            Dim dtable As DataTable = New DataTable

            If (command.Equals("")) Then
                ErrorMessage = "Please initilise the command object."
                Return dtable
            End If

            Try
                conn = New SqlConnection(ConnectionString)
                command.Connection = conn
                conn.Open()
                reader = command.ExecuteReader
                If (reader.HasRows) Then
                    dtable.Load(reader)
                Else
                    ErrorMessage = "No records found.  <BR>"
                    Return Nothing
                End If
            Catch ex As SqlException
                ErrorMessage = "An exception has occured while executing the database transactions.  <BR>"
                ErrorMessage = ErrorMessage + ex.Message
            Finally
                command.Dispose()
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                If conn IsNot Nothing Then
                    conn.Dispose()
                End If
            End Try
            Return dtable
        End Function

        Public Shared Function ExecuteScalar(ByVal command As SqlCommand, ByRef ErrorMessage As String) As Object
            Dim conn As SqlConnection = Nothing
            Dim result As Object = Nothing

            If (command.Equals("")) Then
                ErrorMessage = "Please initilise the command object."
                Return Nothing
            End If

            Try
                conn = New SqlConnection(ConnectionString)
                command.Connection = conn
                conn.Open()
                result = command.ExecuteScalar
            Catch ex As SqlException
                ErrorMessage = "An exception has occured while executing the database transactions.  <BR/>"
                ErrorMessage = ErrorMessage + ex.Message
            Finally
                command.Dispose()
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                If conn IsNot Nothing Then
                    conn.Dispose()
                End If
            End Try
            Return result
        End Function

    End Class
End Namespace

