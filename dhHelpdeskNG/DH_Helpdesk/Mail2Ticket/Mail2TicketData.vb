Imports DH_Helpdesk.SharedFunctions

Public Class Mail2TicketData

    Public Sub Save(caseid As Integer, logid As Integer, type As String, addresses As String)

        Try

            If Not String.IsNullOrWhiteSpace(addresses) Then
                Dim sSQL As String
                Dim e As String() = addresses.Split(",")
                Dim i As Integer

                For i = 0 To e.Length - 1 Step 1
                    If (e(i) <> "") Then
                        Dim sEmail As String = parseEMailAddress(e(i))
                        If IsValidEmailAddress(sEmail) Then
                            sSQL = "INSERT INTO tblMail2Ticket (Case_Id, Log_id, Type, EmailAddress) Values (" & caseid.ToString() & ", "
                            If logid <> 0 Then
                                sSQL = sSQL & logid.ToString() & ", '" & type & "', '"
                            Else
                                sSQL = sSQL & "null, '" & type & "', '"
                            End If
                            sSQL = sSQL & Replace(sEmail, "'", "''") & "')"

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
