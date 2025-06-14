Imports System.Data.SqlClient
Imports DH.Helpdesk.Library.SharedFunctions

Public Class LogData
    Public Function createLog(Case_Id As Integer, EMail As String, InternalLogText As String, ExternalLogText As String, _
                              iLogType As Integer, sRegUser As String, iCaseHistory_Id As Integer, _
                              iFinishingCause_Id As Integer) As Integer
        Dim sSql As String = ""
        Dim logGuid = Guid.NewGuid()
        Dim iLogId As Integer
        

        Try
            If iLogType <> 1 Then

                Dim textInternal = If(InternalLogText, "")
                Dim textExternal = If(ExternalLogText, "")

                Dim utcNow = DateTime.UtcNow

                Dim finishingType as Integer? = Nothing
                Dim finishingDate as DateTime? = Nothing

                If iFinishingCause_Id <> 0 Then
                    finishingType = iFinishingCause_Id 
                    finishingDate = utcNow
                End If


                Dim caseHistoryId as Integer? = Nothing
                If iCaseHistory_Id > 0
                    caseHistoryId = iCaseHistory_Id
                End If

                sSql = "INSERT INTO tblLog(LogGUID, Case_Id, LogDate, Text_Internal, Text_External, LogType, RegUser, CaseHistory_Id, FinishingType, FinishingDate, RegTime, ChangeTime) " & _
                       "VALUES(@logGuid, @caseId, @logDate, @textInternal, @textExternal, @logType, @regUser, @caseHistoryId, @finishingType, @finishingDate, @regTme, @changeTime); " & _
                       "SELECT SCOPE_IDENTITY();" ' returns new id for inserted record
                
                Dim sqlParameters As New List(Of SqlParameter) From {
                        DbHelper.createDbParameter("@logGuid", logGuid),
                        DbHelper.createDbParameter("@caseId", Case_Id),
                        DbHelper.createDbParameter("@logDate", utcNow),
                        DbHelper.createDbParameter("@textInternal", textInternal),
                        DbHelper.createDbParameter("@textExternal", textExternal),
                        DbHelper.createDbParameter("@logType", iLogType),
                        DbHelper.createDbParameter("@regUser", sRegUser),
                        DbHelper.createNullableDbParameter("@caseHistoryId", caseHistoryId),
                        DbHelper.createNullableDbParameter("@finishingType", finishingType),
                        DbHelper.createNullableDbParameter("@finishingDate", finishingDate),
                        DbHelper.createDbParameter("@regTme", utcNow),
                        DbHelper.createDbParameter("@changeTime", utcNow)
                }

                ' Skapa loggpost
                iLogId = DbHelper.executeScalarQuery(Of Integer)(gsConnectionString, sSql, CommandType.Text, sqlParameters.ToArray())

            End If

            Return iLogId
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createLog " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Function

    Public Sub createEMailLog(iCaseHistory_Id As Integer, sEMail As String, iMailId As Integer, sMessageId As String, dtSendTime As DateTime, sEMailLogGUID As String, sResponseMessage As String)
        Dim sSQL As String = "INSERT INTO tblEMailLog (EMailLogGUID, CaseHistory_Id, EMailAddress, MailID, MessageId, SendTime, ResponseMessage) " &
                         "VALUES (@guid, @caseHistoryId, @email, @mailId, @messageId, @sendTime, @responseMessage)"
        Try
            Using conn As New SqlConnection(gsConnectionString)
                Using cmd As New SqlCommand(sSQL, conn)
                    cmd.Parameters.AddWithValue("@guid", sEMailLogGUID)
                    cmd.Parameters.AddWithValue("@caseHistoryId", iCaseHistory_Id)
                    cmd.Parameters.AddWithValue("@email", sEMail)
                    cmd.Parameters.AddWithValue("@mailId", iMailId)
                    cmd.Parameters.AddWithValue("@messageId", sMessageId)
                    cmd.Parameters.AddWithValue("@sendTime", dtSendTime)
                    'I gamla versionen fanns det med getDbPrefix innan sResponseMessage men vet inte vad den g�r f�r nytta
                    'cmd.Parameters.AddWithValue("@responseMessage", getDBStringPrefix() & sResponseMessage)
                    cmd.Parameters.AddWithValue("@responseMessage", sResponseMessage)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Logga eller kasta vidare felet
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createEMailLog " & ex.Message.ToString & " Sql: " & sSQL)
            End If
            Throw ex
        End Try
    End Sub
    Public Sub saveFileInfo(ByVal iLog_Id As Integer, ByVal sFileName As String, ByVal bIsInternal As Boolean)
        Dim sSQL As String

        Try
            Dim sFileNameNormalized = Replace(sFileName, "'", "''")
            Dim iLogType = If(bIsInternal, 1, 0)

            ' Skapa nytt arende
            sSQL = "INSERT INTO tblLogFile(Log_Id, FileName, LogType) " &
                   "VALUES (@logId, @fileName, @logType)"

            Dim parameters As New List(Of SqlParameter) From {
                    DbHelper.createDbParameter("@logId", iLog_Id),
                    DbHelper.createDbParameter("@fileName", sFileNameNormalized, False, DbType.String, 200),
                    DbHelper.createDbParameter("@logType", iLogType)
            }
            DbHelper.executeNonQuery(gsConnectionString, sSQL, CommandType.Text, parameters.ToArray())

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub updateInternalLogNote(ByVal iLog_Id As Integer, ByVal objBody As String)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblLog SET Text_Internal='" & objBody & "' where Id=" & iLog_Id

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub updateExternalLogNote(ByVal iLog_Id As Integer, ByVal objBody As String)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblLog SET Text_External='" & objBody & "' where Id=" & iLog_Id

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
