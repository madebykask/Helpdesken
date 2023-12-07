Imports DH.Helpdesk.Library.SharedFunctions

Public Class OUData
    Public Function getOUs(ByVal iCustomer_Id As Integer) As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim colOU As New Collection

        Try
            sSQL = "SELECT tblOU.Id, tblOU.OU, tblOU.OUId, tblOU.SearchKey, tblOU.Department_Id " & _
                   "FROM tblOU " & _
                   "WHERE tblOU.Department_Id IN (SELECT Id FROM tblDepartment WHERE Customer_Id = " & iCustomer_Id & ") AND tblOU.Status=1 "

            dt = getDataTable(gsConnectionString, sSQL)

            Dim o As OU

            For Each dr As DataRow In dt.Rows
                o = New OU(dr)
                colOU.Add(o)
            Next

            Return colOU

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CreateAndGetOU(ByVal name As String, ByVal iDepartment_Id As Integer) As OU

        Dim safename As String = Left(name.Replace("'", "''"), 50)

        Dim ou As OU = GetOUBySearchKey(safename, iDepartment_Id)
        If ou Is Nothing Then
            Save(safename, iDepartment_Id)
            ou = GetOUBySearchKey(safename, iDepartment_Id)
        End If
        Return ou

    End Function

    Private Sub Save(ByVal name As String, ByVal iDepartment_Id As Integer)
        Dim sSQL As String

        Try
            sSQL = " INSERT INTO tblOU(OU, SearchKey, Department_Id) " & _
                        "Values ('" & name & "','" & name & "', " & iDepartment_Id & ")"

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, SaveOU " & ex.ToString)
            End If
            Throw ex

        End Try
    End Sub

    Private Function GetOUBySearchKey(ByVal Name As String, iDepartment_Id As Integer) As OU
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblOU.Id, tblOU.OU, tblOU.OUId, tblOU.SearchKey, tblOU.Department_Id " & _
                   "FROM tblOU " & _
                   "WHERE Department_Id = " & iDepartment_Id & _
                        " AND UPPER(SearchKey) = '" & UCase(Name) & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return New OU(dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
