Imports DH.Helpdesk.Library.SharedFunctions
Public Class WorkingGroupUserData

    Public Function getWorkgroupUsers(ByVal workgroupId As Integer) As List(Of WorkingGroupUser)
        Dim users As List(Of WorkingGroupUser) = New List(Of WorkingGroupUser)()
        Try
            Dim sSql As String = "select u.*, uwg.UserRole as WorkingGroupUserRole from tblUsers as u join tblUserWorkingGroup as uwg on " &
                                 "uwg.User_Id = u.Id where uwg.WorkingGroup_Id =" & workgroupId.ToString()
            Dim dt As DataTable = getDataTable(gsConnectionString, sSql)

            For Each dr As DataRow In dt.Rows
                users.Add(New WorkingGroupUser(dr))
            Next

            Return users
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
