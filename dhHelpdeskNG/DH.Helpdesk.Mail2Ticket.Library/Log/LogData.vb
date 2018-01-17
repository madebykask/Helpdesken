Imports DH_Helpdesk.SharedFunctions

Public Class LogData
    Public Function createLog(ByVal Case_Id As Integer, ByVal EMail As String, ByVal InternalLogText As String, ByVal ExternalLogText As String, ByVal iLogType As Integer, ByVal sRegUser As String, ByVal iCaseHistory_Id As Integer, ByVal iFinishingCause_Id As Integer) As Integer
        Dim sSQL As String = ""
        Dim sGUID As String = System.Guid.NewGuid().ToString
        Dim iLog_Id As Integer
        Dim dt As DataTable

        Try
            If iLogType <> 1 Then
                ' Skapa loggpost
                sSQL = "INSERT INTO tblLog(LogGUID, Case_Id, LogDate, Text_Internal, Text_External, LogType, RegUser, CaseHistory_Id, FinishingType, FinishingDate, RegTime, ChangeTime) " & _
                            "VALUES(" & _
                                getDBStringPrefix() & "'" & _
                                sGUID.ToString & "', " & _
                                Case_Id & ", " & _
                                "getutcdate(), " & _
                                getDBStringPrefix() & "'" & Replace(Left(InternalLogText, 2850), "'", "''") & "', " & _
                                getDBStringPrefix() & "'" & Replace(Left(ExternalLogText, 2850), "'", "''") & "', " & _
                                iLogType & ", " & _
                                getDBStringPrefix() & "'" & Replace(sRegUser, "'", "''") & "', " & _
                                iCaseHistory_Id & ", "

                If iFinishingCause_Id <> 0 Then
                    sSQL = sSQL & iFinishingCause_Id & ", getutcdate(), "
                Else
                    sSQL = sSQL & "null, null, "
                End If

                sSQL = sSQL & "getutcDate(), getutcDate())"

                'If giDBType = 0 Then
                executeSQL(gsConnectionString, sSQL)
                'Else
                '    executeSQLOracle(gsConnectionString, sSQL)
                'End If

                sSQL = "SELECT tblLog.Id " & _
                                   "FROM tblLog " & _
                                   "WHERE LogGUID='" & sGUID.ToString & "'"


                'If giDBType = 0 Then
                dt = getDataTable(gsConnectionString, sSQL)
                'Else
                '    dt = getDataTableOracle(gsConnectionString, sSQL)
                'End If

                If dt.Rows.Count = 0 Then
                    iLog_Id = 0
                Else
                    iLog_Id = dt.Rows(0)("Id")
                End If
            End If

            Return iLog_Id
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
