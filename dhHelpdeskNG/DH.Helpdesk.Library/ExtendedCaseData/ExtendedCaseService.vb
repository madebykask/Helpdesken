Imports System.Data.SqlClient
Imports System.Linq
Imports DH.Helpdesk.Library.SharedFunctions

Public Class ExtendedCaseService

    Public Function CreateExtendedCaseData(ByVal extendedCaseFormId As Integer) As Integer
        Dim sSQL = ""

        Try
            Dim exGuid = Guid.NewGuid()

            sSQL = "INSERT INTO ExtendedCaseData(ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy) " &
                   "VALUES(@exGuid, @extendedCaseFormId, @createdOn, @createdBy)" &
                   "SELECT SCOPE_IDENTITY();"

            Dim sqlParameters As New List(Of SqlParameter) From {
                    DbHelper.createDbParameter("@exGuid", exGuid),
                    DbHelper.createDbParameter("@extendedCaseFormId", extendedCaseFormId),
                    DbHelper.createDbParameter("@createdOn", DateTime.UtcNow),
                    DbHelper.createDbParameter("@createdBy", "DH Helpdesk")
                    }

            Dim id = DbHelper.executeScalarQuery(Of Integer)(gsConnectionString, sSQL, CommandType.Text, sqlParameters.ToArray())

            Return id
        Catch ex As Exception

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createExtendedCaseData " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try

    End Function

    Private Function GetExtendedCaseData(exGuid As String) As ExtendedCaseData
        Dim sSQL = ""
        Dim dt As DataTable

        Try

            sSQL = "SELECT * FROM ExtendedCaseData WHERE ExtendedCaseGuid = '" & exGuid & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim c As ExtendedCaseData

                c = New ExtendedCaseData(dt.Rows(0))

                Return c
            Else
                Return Nothing
            End If
        Catch ex As Exception

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createExtendedCaseData " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Function
End Class
