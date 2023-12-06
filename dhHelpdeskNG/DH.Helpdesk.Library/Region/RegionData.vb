Imports DH.Helpdesk.Library.SharedFunctions

Public Class RegionData

    Public Function CreateAndGetRegion(ByVal iCustomer_Id As Integer, ByVal name As String) As Region

        Dim safename As String = Left(name.Replace("'", "''"), 50)

        Dim ret As Region = GetRegionBySearchKey(iCustomer_Id, safename)
        If ret Is Nothing Then
            Save(iCustomer_Id, safename)
            ret = GetRegionBySearchKey(iCustomer_Id, safename)
        End If
        Return ret

    End Function

    Private Function GetRegionBySearchKey(ByVal iCustomer_Id As Integer, ByVal name As String) As Region

        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT Id, Region, SearchKey " & _
                   "FROM tblRegion " & _
                   "WHERE Customer_Id = " & iCustomer_Id & _
                        "AND UPPER(SearchKey) = '" & UCase(name) & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return New Region(dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetRegions(ByVal iCustomer_Id As Integer) As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim col As New Collection

        Try
            sSQL = "SELECT id, SearchKey, Region " & _
                   "FROM tblRegion " & _
                   "WHERE Customer_Id = " & iCustomer_Id

            dt = getDataTable(gsConnectionString, sSQL)

            For Each dr As DataRow In dt.Rows
                Dim d As Region = New Region(dr)
                col.Add(d)
            Next

            Return col

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub Save(ByVal iCustomer_Id As Integer, ByVal name As String)
        Dim sSQL As String

        Try
            sSQL = " INSERT INTO tblRegion(Customer_Id, Region, SearchKey, Status) VALUES (" & iCustomer_Id & ", '" & name & "','" & name & "', 1)"

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

        Catch ex As Exception

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, SaveRegion " & ex.ToString)
            End If
            Throw ex

        End Try
    End Sub

End Class
