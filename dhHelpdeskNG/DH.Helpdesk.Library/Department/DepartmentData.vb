Imports System.Linq
Imports DH.Helpdesk.Library.SharedFunctions

Public Class DepartmentData

    Public Function CreateAndGetDepartment(ByVal iCustomer_Id As Integer, ByVal name As String, Optional iRegion_Id As Integer = 0) As Department

        Dim safename As String = Left(name.Replace("'", "''"), 50)

        Dim ret As Department = GetDepartmentBySearchKey(iCustomer_Id, safename, iRegion_Id)
        If ret Is Nothing Then
            Save(iCustomer_Id, safename, iRegion_Id)
            ret = GetDepartmentBySearchKey(iCustomer_Id, safename, iRegion_Id)
        End If
        Return ret

    End Function

    Public Function getDepartmentByName(ByVal iCustomer_Id As Integer, ByVal Name As String) As Department
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblDepartment.Id, tblDepartment.Department, tblDepartment.DepartmentId, tblDepartment.SearchKey, tblDepartment.NDSPath, tblDepartment.Region_Id, Null AS WatchDate " &
                   "FROM tblDepartment " &
                   "WHERE tblDepartment.Customer_Id = " & iCustomer_Id &
                        " AND UPPER(Department) = '" & UCase(Name) & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim d As Department

                d = New Department(dt.Rows(0))

                Return d
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getDepartments(ByVal iCustomer_Id As Integer) As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim colDepartment As New Collection

        Try
            sSQL = "SELECT tblDepartment.Id, tblDepartment.Region_Id, tblDepartment.Department, tblDepartment.DepartmentId, " &
                    "tblDepartment.SearchKey, tblDepartment.NDSPath, " &
                    "(select min(watchdate) from tblWatchDateCalendarValue where tblWatchDateCalendarValue.WatchDateCalendar_Id = tblDepartment.WatchDateCalendar_Id " &
                        "and convert(varchar(10), tblWatchDateCalendarValue.ValidUntilDate, 121) >= convert(varchar(10), getDate(), 121)) AS WatchDate " &
                    "FROM tblDepartment " &
                    "WHERE tblDepartment.Customer_Id = " & iCustomer_Id &
                        "AND tblDepartment.Status=1 "

            dt = getDataTable(gsConnectionString, sSQL)

            Dim d As Department

            For Each dr As DataRow In dt.Rows
                d = New Department(dr)

                colDepartment.Add(d)
            Next

            Return colDepartment

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getUserDepartmentsIds(ByVal usersIds As Integer()) As List(Of KeyValuePair(Of Integer, Integer))
        Dim departmentsIds = New List(Of KeyValuePair(Of Integer, Integer))
        If Not usersIds.Any() Then
            Return departmentsIds
        End If

        Try
            Dim usersIdsStr = String.Join(",", usersIds)
            Dim sSql As String = "select User_Id, Department_Id from tblDepartmentUser where User_Id in (" & usersIdsStr & ")"
            Dim dt As DataTable = getDataTable(gsConnectionString, sSql)

            For Each dr As DataRow In dt.Rows
                departmentsIds.Add(New KeyValuePair(Of Integer, Integer)(dr("User_Id"), dr("Department_Id")))
            Next

            Return departmentsIds
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub Save(ByVal iCustomer_Id As Integer, ByVal name As String, ByVal iRegion_Id As Integer)
        Dim sSQL As String

        Try
            sSQL = " INSERT INTO tblDepartment(Customer_Id, Department, SearchKey, Status"
            If iRegion_Id <> 0 Then
                sSQL = sSQL & ", Region_Id"
            End If
            sSQL = sSQL & ") Values ("
            sSQL = sSQL & iCustomer_Id & ", '" & name & "','" & name & "', 1"
            If iRegion_Id <> 0 Then
                sSQL = sSQL & ", " & iRegion_Id
            End If
            sSQL = sSQL & ")"

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

        Catch ex As Exception

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, SaveDepartment " & ex.ToString)
            End If
            Throw ex

        End Try
    End Sub

    Public Function GetDepartmentBySearchKey(ByVal iCustomer_Id As Integer, ByVal Name As String, iRegion_Id As Integer) As Department
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT Id, Department, DepartmentId, SearchKey, NDSPath, Region_Id, Null AS WatchDate " & _
                   "FROM tblDepartment " & _
                   "WHERE Customer_Id = " & iCustomer_Id & _
                        " AND UPPER(SearchKey) = '" & UCase(Name) & "'"
            If iRegion_Id <> 0 Then
                sSQL = sSQL & " AND Region_Id = " & iRegion_Id
            End If

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return New Department(dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
