Imports DH.Helpdesk.Library.SharedFunctions

Public Class CaseTypeData
    Public Function getCaseTypeById(ByVal iId As Integer) As CaseType
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblCaseType.* " & _
                   "FROM tblCaseType " & _
                   "WHERE tblCaseType.Id = " & iId

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim ct As CaseType

                ct = New CaseType(dt.Rows(0))

                Return ct
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function 

End Class
