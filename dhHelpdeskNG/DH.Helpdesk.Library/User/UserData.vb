Imports DH.Helpdesk.Library.SharedFunctions

Public Class UserData
    Public Function getUserById(ByVal Id As Integer) As User
        Dim sSQL As String
        Dim u As User = Nothing

        Try
            sSQL = "SELECT Id, FirstName, SurName, EMail, Status, Customer_Id AS DefaultCustomer_Id, AllocateCaseMail, CaseInfoMail " & _
                    "FROM tblUsers " & _
                    "WHERE Id = " & Id

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                u = New User(dt.Rows(0))
            End If

            Return u
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getUserByUserId(ByVal UserId As String) As User
        Dim sSQL As String
        Dim u As User = Nothing

        Try
            sSQL = "SELECT Id, FirstName, SurName, EMail, Status, Customer_Id AS DefaultCustomer_Id, AllocateCaseMail, CaseInfoMail " & _
                    "FROM tblUsers " & _
                    "WHERE LOWER(UserId) = '" & UserId.ToLower() & "'"

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                u = New User(dt.Rows(0))
            End If

            Return u
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseInfoMailUsers() As Collection
        Dim colUser As New Collection
        Dim sSQL As String
        Dim dr As DataRow

        Try
            sSQL = "SELECT Id, FirstName, SurName, EMail, Status, Customer_Id AS DefaultCustomer_Id, AllocateCaseMail, CaseInfoMail " & _
                    "FROM tblUsers " & _
                    "WHERE CaseInfoMail<>0 AND tblUsers.Status=1"

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

            Dim u As User

            For Each dr In dt.Rows
                u = New User(dr)
                colUser.Add(u)
            Next

            Return colUser
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
