Imports System.Data.SqlClient
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.Services.utils

Public Class Mail2TicketData

    Public Function GetByMessageId(messageId As String) as Mail2TicketEntity
        
        Try
            Const sql = "SELECT TOP 1 Id, Case_Id, Log_id, EMailAddress, [Type], UniqueMessageId " & _
                         "FROM dbo.tblMail2Ticket WHERE UniqueMessageId = @messageId ORDER BY Id Desc"

            Dim sqlParam = DbHelper.createDbParameter("@messageId", messageId)
            Dim table = getDataTable(gsConnectionString, sql, sqlParam)

            IF table IsNot Nothing AndAlso table.Rows.Count > 0
                Dim row As DataRow = table.Rows(0)
                Dim mt as Mail2TicketEntity = New Mail2TicketEntity(row)
                Return mt
            Else 
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub Save(iCase_Id As Integer, iLog_Id As Integer, type As String, addresses As String, sSubject As String, messageId As String)
        Dim newId As Integer = 0
        Try
            If Not IsNullOrEmpty(addresses) Then

                Dim addressArray As String() = addresses.Split(",")
                
                For Each address As String in addressArray
                    If (Not IsNullOrEmpty(address)) Then
                        Dim sEmail As String = parseEMailAddress(address)
                        
                        'Add only if valid email address 
                        If IsValidEmailAddress(sEmail) Then

                            Dim caseId As Integer? = Nothing
                            If iCase_Id > 0 Then caseId = iCase_Id

                            Dim logId As Integer? =  Nothing
                            If iLog_Id > 0 Then logId = iLog_Id
                            
                            Dim subject as String = If(IsNullOrEmpty(sSubject), "", sSubject)
                            If (subject.Length > 512) Then 
                                subject = subject.Left(509) & "..."
                            End If
                                     
                            Const sSql = "INSERT INTO tblMail2Ticket (Case_Id, Log_id, Type, EmailAddress, EMailSubject, UniqueMessageId) " & _ 
                                         "Values (@caseId, @logId, @type, @emailAddress, @emailSubject, @uniqueMessageId);" & _
                                         "SELECT SCOPE_IDENTITY();"
                            
                            Dim parameters As New List(Of SqlParameter) From {
                                DbHelper.createNullableDbParameter("@caseId", caseId),
                                DbHelper.createNullableDbParameter("@logId", logId),
                                DbHelper.createDbParameter("@type", type),
                                DbHelper.createDbParameter("@emailAddress", sEmail),
                                DbHelper.createNullableDbParameter("@emailSubject", subject),
                                DbHelper.createNullableDbParameter("@uniqueMessageId", messageId)
                            }
                            
                            newId = DbHelper.executeScalarQuery(Of Integer)(gsConnectionString, sSql, CommandType.Text, parameters.ToArray())

                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
