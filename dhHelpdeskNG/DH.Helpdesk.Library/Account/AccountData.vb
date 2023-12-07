Imports DH.Helpdesk.Library.SharedFunctions

Public Class AccountData
    Public Function getAccountByCaseNumber(ByVal sCaseNumber As String) As Account
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblAccount.Id, tblAccount.AccountActivity_Id " & _
                    "FROM tblAccount " & _
                    "WHERE tblAccount.CaseNumber = " & sCaseNumber

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim a As Account

                a = New Account(dt.Rows(0))

                a.AccountActivity = getAccountActivityById(a.AccountActivity_Id)

                Return a
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getAccountActivityById(ByVal iId As Integer) As AccountActivity
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblAccountActivity.* " & _
                    "FROM tblAccountActivity " & _
                    "WHERE tblAccountActivity.Id = " & iId

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim aa As AccountActivity

                aa = New AccountActivity(dt.Rows(0))

                Return aa
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
