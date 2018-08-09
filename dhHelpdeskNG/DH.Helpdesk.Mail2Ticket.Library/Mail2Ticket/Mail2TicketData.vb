Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions

Public Class Mail2TicketData

    Public Function GetByMessageId(messageId As String) as Mail2TicketEntity
        
        Try
            Dim sql As String = "SELECT TOP 1 Id, Case_Id, Log_id, EMailAddress, [Type], UniqueMessageId FROM dbo.tblMail2Ticket WHERE UniqueMessageId = '@messageId' ORDER BY Id Desc"
            sql = sql.Replace("@messageId", messageId)

            Dim table = getDataTable(gsConnectionString, sql)

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

    Public Sub Save(caseid As Integer, logid As Integer, type As String, addresses As String, messageId As String)

        Try

            If Not String.IsNullOrWhiteSpace(addresses) Then
                Dim sSQL As String
                Dim addressCol As String() = addresses.Split(",")
                Dim i As Integer

                For i = 0 To addressCol.Length - 1 Step 1
                    If (addressCol(i) <> "") Then
                        Dim sEmail As String = parseEMailAddress(addressCol(i))
                        If IsValidEmailAddress(sEmail) Then
                            sSQL = "INSERT INTO tblMail2Ticket (Case_Id, Log_id, Type, EmailAddress, UniqueMessageId) Values (" & caseid.ToString() & ", "
                            If logid <> 0 Then
                                sSQL = sSQL & logid.ToString() & ", '" & type & "',"
                            Else
                                sSQL = sSQL & "null, '" & type & "', "
                            End If
                            
                            sSQL = sSQL & " '" &  Replace(sEmail, "'", "''") & "', "

                            If (String.IsNullOrEmpty(messageId))
                                sSQL = sSQL & " null" 
                            Else 
                                sSQL = sSQL & " '" & messageId & "'"
                            End If

                            sSQL = sSQL & ")" 

                            'If giDBType = 0 Then
                            executeSQL(gsConnectionString, sSQL)
                            'Else
                            '    executeSQLOracle(gsConnectionString, sSQL)
                            'End If
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
