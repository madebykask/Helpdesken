Imports System.Data.SqlClient
Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions

Public Class LogData
    Public Function createLog(Case_Id As Integer, EMail As String, InternalLogText As String, ExternalLogText As String, _
                              iLogType As Integer, sRegUser As String, iCaseHistory_Id As Integer, _
                              iFinishingCause_Id As Integer) As Integer
        Dim sSql As String = ""
        Dim logGuid = Guid.NewGuid()
        Dim iLogId As Integer
        

        Try
            If iLogType <> 1 Then

                Dim textInternal = Left(InternalLogText, 2850)
                Dim textExternal = Left(ExternalLogText, 2850)
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
                
                #Region "Old implementation"
                'Dim dt As DataTable
                'Dim sGuid = logGuid.ToString()
                'sSQL = "INSERT INTO tblLog(LogGUID, Case_Id, LogDate, Text_Internal, Text_External, LogType, RegUser, CaseHistory_Id, FinishingType, FinishingDate, RegTime, ChangeTime) " & _
                '            "VALUES(" & _
                '                getDBStringPrefix() & "'" & _
                '                sGUID.ToString & "', " & _
                '                Case_Id & ", " & _
                '                "getutcdate(), " & _
                '                getDBStringPrefix() & "'" & Replace(Left(InternalLogText, 2850), "'", "''") & "', " & _
                '                getDBStringPrefix() & "'" & Replace(Left(ExternalLogText, 2850), "'", "''") & "', " & _
                '                iLogType & ", " & _
                '                getDBStringPrefix() & "'" & Replace(sRegUser, "'", "''") & "', " & _
                '                iCaseHistory_Id & ", "

                'If iFinishingCause_Id <> 0 Then
                '    sSQL = sSQL & iFinishingCause_Id & ", getutcdate(), "
                'Else
                '    sSQL = sSQL & "null, null, "
                'End If

                'sSQL = sSQL & "getutcDate(), getutcDate())"

                ''If giDBType = 0 Then
                'executeSQL(gsConnectionString, sSQL)
                'Else
                '    executeSQLOracle(gsConnectionString, sSQL)
                'End If

                'sSQL = "SELECT tblLog.Id " & _
                '                   "FROM tblLog " & _
                '                   "WHERE LogGUID='" & sGUID.ToString & "'"


                ''If giDBType = 0 Then
                'dt = getDataTable(gsConnectionString, sSQL)
                ''Else
                ''    dt = getDataTableOracle(gsConnectionString, sSQL)
                ''End If

                'If dt.Rows.Count = 0 Then
                '    iLogId = 0
                'Else
                '    iLogId = dt.Rows(0)("Id")
                'End If
                #End Region

            End If

            Return iLogId
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createLog " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Function

    Public Sub createEMailLog(ByVal iCaseHistory_Id As Integer, ByVal sEMail As String, ByVal iMailId As Integer, ByVal sMessageId As String, ByVal dtSendTime As DateTime, ByVal sEMailLogGUID As String, ByVal sResponseMessage As String)
        Dim sSQL As String = ""

        Try
            sSQL = "INSERT INTO tblEMailLog (EMailLogGUID, CaseHistory_Id, EMailAddress, MailID, MessageId, SendTime, ResponseMessage) " & _
                     "VALUES('" & _
                      sEMailLogGUID & "', " & _
                      iCaseHistory_Id & ", '" & _
                      Replace(sEMail, "'", "''") & "', " & _
                      iMailId & ", '" & _
                      sMessageId & "', '" & _
                      dtSendTime.ToString("yyyy-MM-dd HH:mm:ss") & "', " & _
                      getDBStringPrefix() & "'" & sResponseMessage & "')"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createEMailLog " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Sub

    Public Sub saveFileInfo(ByVal iLog_Id As Integer, ByVal sFileName As String)
        Dim sSQL As String

        Try
            ' Skapa nytt ärende
            sSQL = "INSERT INTO tblLogFile(Log_Id, FileName) " & _
                    "VALUES (" & iLog_Id & ", " & getDBStringPrefix() & "'" & Replace(sFileName, "'", "''") & "')"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
