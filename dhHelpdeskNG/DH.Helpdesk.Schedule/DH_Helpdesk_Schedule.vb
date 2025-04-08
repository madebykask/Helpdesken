Imports System.Configuration
Imports System.Data.SqlClient
Imports DH.Helpdesk.Library
Imports System.IO
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.Common.Constants

Module DH_Helpdesk_Schedule
    'Private objLogFile As StreamWriter

    Public Const ConnectionStringName As String = "Helpdesk"

    Public Sub Main()

        ' encrypt connection string if exists
        Dim secureConnectionString = ConfigurationManager.AppSettings("SecureConnectionString")
        If (Not String.IsNullOrEmpty(secureConnectionString) AndAlso Boolean.Parse(secureConnectionString)) Then
            Dim fileName = Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location)
            SecureConnectionStringSection(fileName)
        End If

        'WorkModes:
        ' 1: planDate
        ' 2: approveCase
        ' 3: getContractInNoticeOfRemoval
        ' 4: getContractForFollowUp
        ' 5: sendCaseInfoMail
        ' 6: watchDate
        ' 7: Checklist Schedule
        ' 8: Case Solution Schedule

        Dim sCommand As String = Command()
        Dim aArguments() As String = sCommand.Split(",")


        'Debugging lines for autoclose - testmode
        'Dim sConnectionstringTest = GetConnectionString(aArguments)
        'openLogFile()
        'objLogFile.WriteLine(Now() & ", CaseAutoClose")
        'caseAutoClose(sConnectionstringTest)
        'closeLogFile()
        'end test autoclose

        If aArguments.Length > 0 Then


            ' parse command line args
            Dim workMode = GetWorkMode(aArguments)
            Dim sConnectionstring = GetConnectionString(aArguments)
            'Console.WriteLine("Connectionstring from args:" & sConnectionstring)
            giSendMail = GetSendEmail(aArguments, giSendMail) 'used in 12 mode

            'Log cmd line args
            Try
                openLogFile()
                LogToFile(String.Format(
                    "Cmd Line Args:" & vbCrLf & vbTab &
                    "- WorkMode: {0}" & vbCrLf & vbTab &
                    "- ConnectionString: {1}" & vbCrLf & vbTab &
                    "- SendMail: {2}" & vbCrLf & vbTab,
                    workMode, FormatConnectionString(sConnectionstring), giSendMail), 1)
            Catch ex As Exception
                LogError(ex.ToString())
            Finally
                closeLogFile()
            End Try


            Select Case workMode
                Case 1
                    planDate(sConnectionstring)
                Case 2
                    approveCase(sConnectionstring)
                Case 3
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", getContractInNoticeOfRemoval")
                    getContractInNoticeOfRemoval(sConnectionstring)
                    closeLogFile()
                Case 4
                    getContractForFollowUp(sConnectionstring)
                Case 5
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", sendCaseInfoMail")
                    sendCaseInfoMail(sConnectionstring)
                    closeLogFile()
                Case 6
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", watchDate")
                    watchDate(sConnectionstring)
                    closeLogFile()
                Case 7
                    checklistSchedule(sConnectionstring)
                Case 8
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", CaseSolutionSchedule")
                    caseSolutionSchedule(sConnectionstring)
                    closeLogFile()
                Case 9
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", Clear FileViewLog")
                    clearFileViewLog(sConnectionstring)
                    closeLogFile()
                Case 10
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", CaseCleanUp")
                    caseCleanUp(sConnectionstring)
                    closeLogFile()
                Case 11
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", CaseReminder")
                    caseReminder(sConnectionstring)
                    closeLogFile()
                Case 12
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", Questionnaire")
                    sendQuestionnaire(sConnectionstring)
                    closeLogFile()
                Case 13
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", CaseAutoClose")
                    caseAutoClose(sConnectionstring)
                    closeLogFile()
                Case 14
                    giLoglevel = 1
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", Case Statistics")
                    sendCaseStatistics(sConnectionstring)
                    closeLogFile()
                Case Else
                    openLogFile()
                    objLogFile.WriteLine(Now() & ", Error: Not supported work mode value!")
                    closeLogFile()

            End Select


        End If
    End Sub
    Private Function GetCmdArgSafe(args As String(), index As Int32) As String
        If index < args.Length Then
            Dim val = args(index)
            val = IIf(val Is Nothing, val, val.Trim()) 'trim
            Return val
        End If
        Return Nothing
    End Function
    Private Function GetWorkMode(aArguments As String()) As Integer
        Dim val = GetCmdArgSafe(aArguments, 0)
        Dim workMode = 0
        Integer.TryParse(val, workMode)
        Return workMode
    End Function
    Private Function GetSendEmail(aArguments As String(), defaultValue As Integer) As Integer
        Dim sendEmail = defaultValue
        If aArguments.Length > 2 Then
            Dim val = GetCmdArgSafe(aArguments, 2)
            If Integer.TryParse(val, sendEmail) Then
                Return sendEmail
            Else
                Return defaultValue
            End If
        End If
        Return defaultValue
    End Function
    Private Function GetConnectionString(aArguments As String()) As String
        Dim connectionString = GetCmdArgSafe(aArguments, 1)
        If String.IsNullOrEmpty(connectionString) Then
            connectionString = ConfigurationManager.ConnectionStrings(ConnectionStringName)?.ConnectionString
        End If
        Return connectionString
    End Function
    Private Sub LogToFile(msg As String, level As Integer)
        If level > 0 Then
            If objLogFile IsNot Nothing Then
                objLogFile.WriteLine("{0}: {1}", Now(), msg)
            End If
        End If
    End Sub
    Private Sub LogError(msg As String)
        If objLogFile IsNot Nothing Then
            objLogFile.WriteLine("{0}: {1}", Now(), msg)
        End If
    End Sub
    Private Sub SecureConnectionStringSection(exeConfigName As String)
        openLogFile()
        Try
            EncryptSection(Of ConnectionStringsSection)(exeConfigName, "connectionStrings")
            LogToFile("app.config section is protected", giLoglevel)
        Catch ex As Exception
            LogError(ex.ToString())
        Finally
            closeLogFile()
        End Try

    End Sub
    Private Function FormatConnectionString(connectionString As String) As String
        Dim builder = New SqlConnectionStringBuilder(connectionString)
        Return String.Format("Data Source={0}; Initial Catalog={1};Network Library={2}", builder.DataSource, builder.InitialCatalog, builder.NetworkLibrary)
    End Function

    Private Sub EncryptSection(Of TSection As ConfigurationSection)(exeConfigName As String, sectionName As String)
        ' Open the configuration file And retrieve 
        Dim config = ConfigurationManager.OpenExeConfiguration(exeConfigName)

        Dim section = CType(config.GetSection(sectionName), TSection)
        If section IsNot Nothing Then
            If (Not section.SectionInformation.IsProtected) Then
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
            End If

            ' Save the current configuration.
            config.Save()
        End If
    End Sub

    Private Sub planDate(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objUserData As New UserData
        Dim objUser As User
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData
        Dim objLog As New Log
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        ' Hämta ärenden där vi ska skicka mail att planeringsdatum inträffat
        Dim colCase As Collection = objCaseData.getTodayPlanDate()

        For i As Integer = 1 To colCase.Count
            objCase = colCase(i)

            objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

            objUser = objUserData.getUserById(objCase.Performer_User_Id)

            objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailPlanDate, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

            If objMailTemplate IsNot Nothing Then
                sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                Dim sSendTime As DateTime = Date.Now()
                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                Dim objMail As New Mail
                Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, SharedFunctions.EMailType.EMailPlanDate, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
            End If
        Next
    End Sub

    Private Sub watchDate(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objUserData As New UserData
        Dim objUser As User
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData
        Dim objLog As New Log
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        ' Hämta bevakade ärenden
        Dim colCase As Collection = objCaseData.getTodayWatchDate()

        For i As Integer = 1 To colCase.Count
            objCase = colCase(i)

            objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

            objUser = objUserData.getUserById(objCase.Performer_User_Id)

            objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailWatchDate, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

            If objMailTemplate IsNot Nothing Then
                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                Dim sSendTime As DateTime = Date.Now()
                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                Dim objMail As New Mail
                Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                objLogFile.WriteLine(Now() & ", watchDate, Case:" & objCase.Casenumber.ToString & ", to: " & objUser.EMail)

                objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, SharedFunctions.EMailType.EMailWatchDate, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
            End If
        Next
    End Sub

    Private Sub caseSolutionSchedule(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCaseSolution As CCase
        Dim objCase As CCase
        Dim objExtendedCaseService As New ExtendedCaseService
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objUserData As New UserData
        Dim objUser As User
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData
        Dim objLog As New Log
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        objLogFile.WriteLine(Now() & ", CaseSolutionSchedule, isTodayHoliday:" & objGlobalSettingsData.isTodayHoliday.ToString)

        ' Kontrollera att det inte är helgdag idag.
        'If objGlobalSettingsData.isTodayHoliday = False Then

        ' Hämta ärendemallar
        Dim colCase As Collection = objCaseData.getCaseSolutionSchedule

        For i As Integer = 1 To colCase.Count
            objCaseSolution = colCase(i)
            objLogFile.WriteLine(Now() & ", CaseSolutionSchedule, Caption:" & objCaseSolution.Caption)

            objCase = objCaseData.createCase(objCaseSolution)
            objLogFile.WriteLine(Now() & ", CaseSolutionSchedule, CaseNumber:" & objCase.Casenumber)

            Dim isAboutObj As ComputerUser = getIsAboutData(objCaseSolution)
            objCaseData.saveCaseIsAbout(objCase.Id, isAboutObj)

            Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

            objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

            objUser = objUserData.getUserById(objCase.Performer_User_Id)

            If objCaseSolution.Log.Count > 0 Then
                If objCaseSolution.Log(0).Text_Internal <> "" Or objCaseSolution.Log(0).Text_External <> "" Then
                    objLogData.createLog(objCase.Id, objCase.Persons_EMail, objCaseSolution.Log(0).Text_Internal, objCaseSolution.Log(0).Text_External, 0, "DH Helpdesk", iCaseHistory_Id, 0)
                    objLog.Text_External = objCaseSolution.Log(0).Text_External
                    objLog.Text_Internal = objCaseSolution.Log(0).Text_Internal
                End If
            End If

            If objCase.Performer_User_Id <> 0 And objCase.PerformerEMail <> "" Then
                objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailAssignCasePerformer, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                If objMailTemplate IsNot Nothing Then
                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                    Dim sSendTime As DateTime = Date.Now()
                    Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                    Dim objMail As New Mail
                    Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                    objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, SharedFunctions.EMailType.EMailAssignCasePerformer, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                End If
            End If


            If objCaseSolution.ExtendedCaseFormId IsNot Nothing Then
                If objCaseSolution.ExtendedCaseFormId.HasValue And objCaseSolution.ExtendedCaseFormId.Value > 0 Then
                    Dim extendedCaseDataId = objExtendedCaseService.CreateExtendedCaseData(objCaseSolution.ExtendedCaseFormId.Value)
                    objCaseData.CreateExtendedCaseConnection(objCase.Id, objCaseSolution.ExtendedCaseFormId.Value, extendedCaseDataId)
                End If
            End If


        Next
        'End If

    End Sub

    Private Function getIsAboutData(objCaseSolution As CCase) As ComputerUser
        Dim isAboutData As ComputerUser = New ComputerUser()
        isAboutData.UserId = objCaseSolution.IsAbout_ReportedBy
        isAboutData.SurName = objCaseSolution.IsAbout_PersonsName
        isAboutData.EMail = objCaseSolution.IsAbout_PersonsEMail
        isAboutData.Phone = objCaseSolution.IsAbout_PersonsPhone
        isAboutData.CellPhone = objCaseSolution.IsAbout_PersonsCellPhone
        isAboutData.Region_Id = objCaseSolution.IsAbout_Region_Id
        isAboutData.Department_Id = objCaseSolution.IsAbout_Department_Id
        isAboutData.OU_Id = objCaseSolution.IsAbout_OU_Id
        isAboutData.Location = objCaseSolution.IsAbout_Place
        isAboutData.CostCentre = objCaseSolution.IsAbout_CostCentre
        isAboutData.UserCode = objCaseSolution.IsAbout_UserCode
        Return isAboutData
    End Function

    Private Sub caseReminder(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData
        Dim objLog As New Log
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        ' Hämta ärenden
        Dim colCase As Collection = objCaseData.getCaseReminder

        For i As Integer = 1 To colCase.Count
            objCase = colCase(i)

            objLogFile.WriteLine(Now() & ", caseReminder, CaseNumber:" & objCase.Casenumber)

            Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

            objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

            If objCase.Persons_EMail <> "" Then
                objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailReminderNotifier, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                If objMailTemplate IsNot Nothing Then
                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                    Dim sSendTime As DateTime = Date.Now()
                    Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                    Dim objMail As New Mail
                    Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                    objLogFile.WriteLine(Now() & ", caseReminder, Case:" & objCase.Casenumber.ToString & ", to: " & objCase.Persons_EMail)

                    objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, SharedFunctions.EMailType.EMailReminderNotifier, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                End If
            End If

        Next
        'End If

    End Sub

    Private Sub caseAutoClose(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData
        Dim objLog As New Log
        Dim sMessageId As String
        Dim sEmailList As String = ""

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        ' Hämta ärenden
        Dim colCase As Collection = objCaseData.getCaseAutoClose
        objLogFile.WriteLine(Now() & ", running caseAutoClose, found " & colCase.Count & " cases to autoclose")

        'For debug only - remove
        'Dim debugCase As CCase = objCaseData.getCase(39360)
        'colCase.Add(debugCase)

        For Each objCase In colCase
            If objCase.StateSecondary_FinishingCause_Id IsNot Nothing Then

                objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                ' Save Logs (Logga händelsen)
                Dim iLog_Id As Integer = objLogData.createLog(objCase.Id, objCase.Persons_EMail, "", "", 0, "DH Helpdesk", iCaseHistory_Id, objCase.StateSecondary_FinishingCause_Id)
                objCaseData.closeCase(objCase)
                objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Closed case")
                objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailCaseClosed, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)


                If objMailTemplate IsNot Nothing Then

                    'Get surveyfields
                    Dim caseService As New DH.Helpdesk.VBCSharpBridge.CaseExposure
                    Dim bodyWithSurvey As String = caseService.GetSurveyBodyString(objCase.Id, objMailTemplate.Id, objCase.Persons_EMail, objCustomer.HelpdeskEMail, objGlobalSettings.ServerPort, objGlobalSettings.ServerName, objMailTemplate.Body)
                    'Replace surveyfields and add it to body
                    objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Got mailtemplate with survey fields")
                    objMailTemplate.Body = bodyWithSurvey
                    Dim caseEmailer As New DH.Helpdesk.VBCSharpBridge.CaseEmailExposure
                    If objMailTemplate.IncludeLogExternal Then
                        Dim extraBody As String = caseEmailer.GetExternalLogTextHistory(objCase.Id, iLog_Id, objCustomer.HelpdeskEMail)
                        objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Got external case history")
                        objMailTemplate.Body += extraBody
                    End If

                    If objCase.Persons_EMail <> "" Then
                        If objMailTemplate.SendMethod = "1" Then
                            sEmailList = objCase.Persons_EMail
                        Else
                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                            Dim sSendTime As DateTime = Date.Now()
                            Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                            Dim objMail As New Mail
                            Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)
                            objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Sent AutoClose email to " & objCase.Persons_EMail)
                            objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                            objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Created emailLog alt 1")

                        End If

                    End If


                    Dim followers As List(Of String) = objCaseData.getCaseExtraFollowers(objCase.Id)

                    If followers.Count > 0 Then
                        For Each follower As String In followers
                            If objMailTemplate.SendMethod = "1" Then
                                If sEmailList = "" Then
                                    sEmailList = follower
                                Else
                                    sEmailList = sEmailList & ";" & follower
                                End If

                            Else
                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                                Dim sSendTime As DateTime = Date.Now()
                                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                                Dim objMail As New Mail
                                Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, follower, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)
                                objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Sent AutoClose email to follower " & follower)
                                objLogData.createEMailLog(iCaseHistory_Id, follower, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Created emailLog alt 2")

                            End If
                        Next

                    End If

                    If sEmailList <> "" Then
                        sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                        Dim sSendTime As DateTime = Date.Now()
                        Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                        Dim objMail As New Mail
                        Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, sEmailList, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)
                        objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Sent AutoClose email to " & sEmailList)
                        objLogData.createEMailLog(iCaseHistory_Id, sEmailList, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                        objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber & " Created emailLog alt 3")

                    End If

                End If

            End If
        Next
        'End If

    End Sub

    Private Sub approveCase(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objMailTemplateData As New MailTemplateData
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        ' Hämta ärenden som ska godkännas automatiskt
        Dim colCase As Collection = objCaseData.getCaseForAutomaticApprove()

        For i As Integer = 1 To colCase.Count
            objCase = colCase(i)

            objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

            If DateDiff(DateInterval.Hour, objCase.FinishingDate, Now()) >= objCase.AutomaticApproveTime Then
                objCaseData.approveCase(objCase)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")
            End If
        Next
    End Sub

    Private Sub getContractInNoticeOfRemoval(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objTextTranslationData As New TextTranslationData
        Dim objContractData As New ContractData
        Dim objContract As Contract
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objLogData As New LogData
        Dim objUserData As New UserData
        Dim objUser As User
        Dim objComputerUserData As New ComputerUserData
        'Dim objComputerUser As ComputerUser
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        Dim colContract As Collection = objContractData.getContractsForNoticeOfRemoval()

        If colContract.Count > 0 Then
            For i As Integer = 1 To colContract.Count
                objContract = colContract(i)

                objCase = New CCase
                objCase.RegistrationSource = 5

                objLogFile.WriteLine(Now() & ", getContractInNoticeOfRemoval, ContractNumber:" & objContract.ContractNumber)

                objCase.Caption = objTextTranslationData.getTextTranslation("För uppsägning", objContract.Language_Id) & " " & objContract.ContractNumber
                objCase.Description = objCase.Caption
                objCase.Customer_Id = objContract.Customer_Id
                objCase.CaseType_Id = objContract.CaseType_Id
                objCase.Form_Id = objContract.Form_Id

                If objContract.InNoticeOfRemoval_StateSecondary_Id <> 0 Then
                    objCase.StateSecondary_Id = objContract.InNoticeOfRemoval_StateSecondary_Id
                End If

                ' Kontrollera om användaren är aktiv
                If objContract.FollowUpResponsibleUser_Id <> 0 Then
                    objUser = objUserData.getUserById(objContract.FollowUpResponsibleUser_Id)

                    If objUser.Status = 1 Then
                        objCase.Performer_User_Id = objContract.FollowUpResponsibleUser_Id
                    End If
                End If

                If objCase.Performer_User_Id = 0 And objContract.ResponsibleUser_Id <> 0 Then
                    ' Kontrollera om ansvarig är aktiv
                    objUser = objUserData.getUserById(objContract.ResponsibleUser_Id)

                    If objUser.Status = 1 Then
                        objCase.Performer_User_Id = objContract.ResponsibleUser_Id
                    End If
                End If

                objCase.RegLanguage_Id = objContract.Language_Id

                'Set notifier from contract
                If Not String.IsNullOrEmpty(objContract.CreateCase_UserId) Then
                    setInitiator(objContract.CreateCase_UserId, objContract.Customer_Id, objCase)
                End If

                If objContract.Department_Id <> 0 Then
                    objCase.Department_Id = objContract.Department_Id
                End If

                objCase = objCaseData.createCase(objCase)

                ' Logga i avtalsloggen
                objContractData.createLog(objContract.Id, 3, objCase.PerformerEMail, objCase.Id)

                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                If Len(objCase.PerformerEMail) > 6 Then

                    objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailAssignCasePerformer, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                    If Not objMailTemplate Is Nothing Then
                        sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                        Dim sSendTime As DateTime = Date.Now()
                        Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                        Dim objMail As New Mail
                        Dim sRet_SendMail As String = objMail.sendMail(objCase, Nothing, objCustomer, objCase.PerformerEMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                        objLogData.createEMailLog(iCaseHistory_Id, objCase.PerformerEMail, SharedFunctions.EMailType.EMailAssignCasePerformer, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub getContractForFollowUp(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objTextTranslationData As New TextTranslationData
        Dim objContractData As New ContractData
        Dim objContract As Contract
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objLogData As New LogData
        Dim objUserData As New UserData
        Dim objUser As User
        'Dim objComputerUser As ComputerUser
        Dim objComputerUserData As New ComputerUserData
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        Dim colContract As Collection = objContractData.getContractsForFollowUp()

        If colContract.Count > 0 Then
            For i As Integer = 1 To colContract.Count
                objContract = colContract(i)

                objCase = New CCase
                objCase.RegistrationSource = 5

                objCase.Caption = objTextTranslationData.getTextTranslation("För uppföljning", objContract.Language_Id) & " " & objContract.ContractNumber
                objCase.Description = objCase.Caption
                objCase.Customer_Id = objContract.Customer_Id
                objCase.CaseType_Id = objContract.CaseType_Id
                objCase.Form_Id = objContract.Form_Id

                If objContract.FollowUp_StateSecondary_Id <> 0 Then
                    objCase.StateSecondary_Id = objContract.FollowUp_StateSecondary_Id
                End If

                ' Kontrollera om användaren är aktiv
                objUser = objUserData.getUserById(objContract.FollowUpResponsibleUser_Id)

                If objUser.Status = 1 Then
                    objCase.Performer_User_Id = objContract.FollowUpResponsibleUser_Id
                Else
                    ' Kontrollera om ansvarig är aktiv
                    objUser = objUserData.getUserById(objContract.ResponsibleUser_Id)

                    If objUser.Status = 1 Then
                        objCase.Performer_User_Id = objContract.ResponsibleUser_Id
                    End If
                End If

                objCase.RegLanguage_Id = objContract.Language_Id

                'Set notifier from contract
                If Not String.IsNullOrEmpty(objContract.CreateCase_UserId) Then
                    setInitiator(objContract.CreateCase_UserId, objContract.Customer_Id, objCase)
                End If

                If objContract.Department_Id <> 0 Then
                    objCase.Department_Id = objContract.Department_Id
                End If

                objCase = objCaseData.createCase(objCase)

                ' Logga i avtalsloggen
                objContractData.createLog(objContract.Id, 2, objCase.PerformerEMail, objCase.Id)

                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                If Len(objCase.PerformerEMail) > 6 Then

                    objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailAssignCasePerformer, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                    If Not objMailTemplate Is Nothing Then
                        sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                        Dim sSendTime As DateTime = Date.Now()
                        Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                        Dim objMail As New Mail
                        Dim sRet_SendMail As String = objMail.sendMail(objCase, Nothing, objCustomer, objCase.PerformerEMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                        objLogData.createEMailLog(iCaseHistory_Id, objCase.PerformerEMail, SharedFunctions.EMailType.EMailAssignCasePerformer, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub setInitiator(userId As String, customerId As Integer, ByRef objCase As CCase)
        If Not String.IsNullOrEmpty(userId) Then

            Dim objComputerUser = New ComputerUserData().getComputerUserByUserId(userId, customerId)

            If Not objComputerUser Is Nothing Then
                objCase.ReportedBy = objComputerUser.UserId
                objCase.Persons_Name = objComputerUser.FirstName & " " & objComputerUser.SurName
                objCase.Persons_EMail = objComputerUser.EMail
                objCase.Persons_Phone = objComputerUser.Phone

                If objComputerUser.Department_Id <> 0 Then
                    objCase.Department_Id = objComputerUser.Department_Id
                End If

                If objComputerUser.Region_Id <> 0 Then
                    objCase.Region_Id = objComputerUser.Region_Id
                End If
            End If
        End If
    End Sub

    Private Sub sendCaseInfoMail(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData
        Dim objCase As CCase
        Dim objMailData As New Mail
        Dim objUserData As New UserData
        Dim objUser As User
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer

        Dim sSubject As String = "Ärenden per handläggare"
        Dim sMessage As String = ""
        Dim sCustomerName As String = ""
        Dim sLink As String
        Dim sProtocol As String
        Dim bSwitch As Integer
        Dim sStyle As String = ""
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        If objGlobalSettings.ServerPort = 443 Then
            sProtocol = "https"
        Else
            sProtocol = "http"
        End If

        giDBType = objGlobalSettings.DBType

        ' Hämta handläggare som ska informeras
        Dim colUser As Collection = objUserData.getCaseInfoMailUsers()

        For j = 1 To colUser.Count
            objUser = colUser(j)

            objLogFile.WriteLine(Now() & ", sendCaseInfoMail:" & objUser.EMail.ToString & ", " & objUser.CaseInfoMail)

            If objUser.CaseInfoMail = 127 Or (Today.DayOfWeek = DayOfWeek.Monday And objUser.CaseInfoMail = 1) Then
                objCustomer = objCustomerData.getCustomerById(objUser.DefaultCustomer_Id)

                Dim colCase As Collection = objCaseData.getCasePerPerformer(objUser.Id)

                If colCase.Count > 0 Then
                    sMessage = "<span style=""font-family:arial;font-size:14px"">Ärenden per handläggare</span><br><br><table style=""font-family:arial;font-size:12px;"" cellpadding=3 cellspacing=0><tr style=""background-color:#E1E1E1""><td style=""WIDTH:150px"">Kund</td><td style=""WIDTH:150px"">Avdelning</td><td style=""WIDTH:100px"">Ärende</td><td style=""WIDTH:150px"">Registreringsdatum</td><td>Rubrik</td><td style=""WIDTH:100px"">Understatus</td></tr>"

                    For i As Integer = 1 To colCase.Count
                        If bSwitch = 1 Then
                            bSwitch = 0
                        Else
                            bSwitch = 1
                        End If

                        sStyle = "style=""border-bottom:#F0F0F0 1px solid"""
                        objCase = colCase(i)

                        sMessage = sMessage & "<tr>"

                        If sCustomerName <> objCase.CustomerName Then
                            sMessage = sMessage & "<td " & sStyle & ">" & objCase.CustomerName & "</td>"
                        Else
                            sMessage = sMessage & "<td " & sStyle & ">&nbsp;</td>"
                        End If

                        sMessage = sMessage & "<td " & sStyle & ">" & objCase.Department & "</td>"

                        If objGlobalSettings.DBVersion > "5" Then
                            Dim editCasePath As String
                            If objGlobalSettings.UseMobileRouting Then
                                editCasePath = CasePaths.EDIT_CASE_MOBILEROUTE
                            Else
                                editCasePath = CasePaths.EDIT_CASE_DESKTOP
                            End If
                            sLink = "<a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & editCasePath & objCase.Id & """>" & objCase.Casenumber & "</a>"
                        Else
                            sLink = "<a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & """>" & objCase.Casenumber & "</a>"
                        End If

                        sMessage = sMessage & "<td " & sStyle & ">" & sLink & "</td>"
                        sMessage = sMessage & "<td " & sStyle & ">" & objCase.RegTime.ToShortDateString & "</td>"
                        sMessage = sMessage & "<td " & sStyle & ">" & objCase.Caption & "</td>"
                        sMessage = sMessage & "<td " & sStyle & ">" & objCase.StateSecondary & "</td>"
                        sMessage = sMessage & "</tr>"


                        sCustomerName = objCase.CustomerName
                    Next

                    sMessage = sMessage & "</table>"

                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                    objMailData.Send(objCustomer, objCustomer.HelpdeskEMail, objUser.EMail, sSubject, sMessage, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)

                End If
            End If
        Next
    End Sub

    Private Sub checklistSchedule(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objChecklistData As New ChecklistData
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType

        ' Hämta checklistor 
        Dim checklists As IList(Of Checklist) = objChecklistData.getChecklistSchedule

        For Each cl As Checklist In checklists
            objCustomer = objCustomerData.getCustomerById(cl.Customer_Id)

            objChecklistData.createChecklistDate(cl.Id, cl.Customer_Id)

            sMessageId = createMessageId(objCustomer.HelpdeskEMail)

            Dim objMail As New Mail
            objMail.Send(objCustomer, objCustomer.HelpdeskEMail, cl.Recipients, "Checklista: " & cl.ChecklistName, cl.ChecklistMailBody, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)
        Next


    End Sub

    Private Sub clearFileViewLog(ByVal sConnectionString As String)
        Dim objCaseData As New CaseData

        gsConnectionString = sConnectionString

        objCaseData.clearFileViewLog()
    End Sub

    Private Sub caseCleanUp(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCaseData As New CaseData

        gsConnectionString = sConnectionString

        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        gsAttachedFileFolder = objGlobalSettings.AttachedFileFolder

        objCaseData.CaseCleanUp()
    End Sub

    Private Sub sendQuestionnaire(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objQuestionnaireData As New QuestionnaireData
        Dim objQuestionnaire As Questionnaire
        Dim objCaseData As New CaseData
        Dim objQuestionnaireCircular As QuestionnaireCircular
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objMailTemplateData As New MailTemplateData
        Dim objLogData As New LogData
        Dim objTextTranslationData As New TextTranslationData

        gsConnectionString = sConnectionString
        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        Dim colQuestionnaire As Collection = objQuestionnaireData.getQuestionnaires()

        For i As Integer = 1 To colQuestionnaire.Count
            objQuestionnaire = colQuestionnaire(i)
            objCustomer = objCustomerData.getCustomerById(objQuestionnaire.Customer_Id)


            objQuestionnaireCircular = objQuestionnaireData.createQuestionnaireCircular(objQuestionnaire, objCustomer)

            Dim objMailTemplate As MailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailQuestionnaire, objCustomer.Id, objCustomer.Language_Id, objGlobalSettings.DBVersion)

            For Each qcp In objQuestionnaireCircular.QuestionnaireCircularPart
                If giSendMail = 1 Then
                    If Not objMailTemplate Is Nothing Then
                        Dim sMessageId As String = createMessageId(objCustomer.HelpdeskEMail)

                        Dim objMail As New Mail
                        objMail.sendQuestionnaireMail(qcp, objCustomer, objMailTemplate, objGlobalSettings, sMessageId)

                        objQuestionnaireData.updateQuestionnaireCircularPartStatus(qcp.Id)
                    End If
                End If

            Next

            objQuestionnaireData.updateQuestionnaireCircularStatus(objQuestionnaireCircular.Id)
        Next
    End Sub

    Private Sub sendCaseStatistics(ByVal sConnectionString As String)
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer
        Dim objPriorityData As New PriorityData
        Dim objPriority As Priority
        Dim objCaseData As New CaseData
        Dim objCase As CCase

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        Dim colCustomer = objCustomerData.getCustomers()

        For i = 0 To colCustomer.Count - 1
            objCustomer = colCustomer(i)

            If objCustomer.CaseStatisticsEMailList <> "" Then
                Dim sStatistics As String
                Dim iWorkingDayStart As Integer = objCustomer.WorkingDayStart
                Dim iWorkingDayEnd = objCustomer.WorkingDayEnd
                Dim iWorkingDayLength As Integer = CInt(iWorkingDayEnd) - CInt(iWorkingDayStart)
                Dim vCaseInfo As List(Of KeyValuePair(Of String, Integer)) = New List(Of KeyValuePair(Of String, Integer))
                Dim vPriority As List(Of KeyValuePair(Of String, Integer)) = New List(Of KeyValuePair(Of String, Integer))

                Dim iMaxSolutionTime As Integer = 0
                Dim iNumberOfPrioritiesLessThan1WorkingDay As Integer = 0

                ' Hämta in prioriteter
                Dim colPriority As Collection = objPriorityData.GetPriorityByCustomerId(objCustomer.Id)

                If colPriority IsNot Nothing Then
                    For j = 1 To colPriority.Count
                        objPriority = colPriority(j)

                        If objPriority.SolutionTime > iMaxSolutionTime Then
                            iMaxSolutionTime = objPriority.SolutionTime
                        End If

                        ' Lägg in prioriteter som har mindre än 1 arbetsdags lösningstid
                        If objPriority.SolutionTime <> 0 And objPriority.SolutionTime < iWorkingDayLength Then
                            iNumberOfPrioritiesLessThan1WorkingDay = iNumberOfPrioritiesLessThan1WorkingDay + 1

                            vPriority.Add(New KeyValuePair(Of String, Integer)(objPriority.Name, objPriority.SolutionTime))
                        End If
                    Next
                End If

                ' Skapa array som ska innehålla statistikinformation
                Dim vStatistics As List(Of KeyValuePair(Of String, Integer)) = New List(Of KeyValuePair(Of String, Integer))

                vStatistics.Add(New KeyValuePair(Of String, Integer)("Ärenden", 0))
                vStatistics.Add(New KeyValuePair(Of String, Integer)("Aktiva", 0))
                vStatistics.Add(New KeyValuePair(Of String, Integer)("Vilande", 0))
                vStatistics.Add(New KeyValuePair(Of String, Integer)("Varnade", 0))
                vStatistics.Add(New KeyValuePair(Of String, Integer)("Bevakade", 0))

                ' Skapa array med prioriteter
                If iMaxSolutionTime <> 0 Then
                    vCaseInfo.Add(New KeyValuePair(Of String, Integer)("0 (Akut)", 0))

                    For Each pair As KeyValuePair(Of String, Integer) In vPriority
                        vCaseInfo.Add(New KeyValuePair(Of String, Integer)(pair.Key, 0))
                    Next

                    'For i = 1 + iNumberOfPrioritiesLessThan1WorkingDay To iMaxSolutionTime + iNumberOfPrioritiesLessThan1WorkingDay
                    '    vCaseInfo(i, 0) = i - iNumberOfPrioritiesLessThan1WorkingDay
                    '    vCaseInfo(i, 1) = 0
                    'Next
                End If



                Dim colCases = objCaseData.getCasesByCustomer(objCustomer.Id)

                If colCases IsNot Nothing Then
                    For j = 1 To colCases.Count
                        objCase = colCases(j)

                        Dim iSolutionTime As Integer = 0

                        If CLng(iMaxSolutionTime) <> 0 Then
                            If Not IsDate(objCase.WatchDate) And Not IsDate(objCase.FinishingDate) And (objCase.StateSecondary_Id = 0 Or objCase.IncludeInCaseStatistics = 1) And Not objCase.Priority_Id = 0 And iSolutionTime <> 0 Then
                                Dim iLeadTime As Integer = objCaseData.LeadTimeMinutes(DateAdd("n", objCase.ExternalTime, objCase.RegTime), Now(), iWorkingDayStart, iWorkingDayEnd, objCase.HolidayHeader_Id)


                                If iLeadTime >= iSolutionTime Then

                                    vCaseInfo(0) = New KeyValuePair(Of String, Integer)(vCaseInfo(0).Key, vCaseInfo(0).Value + 1)

                                    ' Lägg till i akutlistan
                                    'If iNrOfUrgentCases = 0 Then
                                    '    ReDim vUrgent(0)
                                    'Else
                                    '    ReDim Preserve vUrgent(iNrOfUrgentCases)
                                    'End If

                                    'vUrgent(iNrOfUrgentCases) = rsStatistics("Id")

                                    'iNrOfUrgentCases = iNrOfUrgentCases + 1
                                Else
                                    For k As Integer = 0 To vCaseInfo.Count - 1
                                        If i <= CInt(iNumberOfPrioritiesLessThan1WorkingDay) Then
                                            If iSolutionTime - iLeadTime <= vCaseInfo(k).Key Then
                                                vCaseInfo(k) = New KeyValuePair(Of String, Integer)(vCaseInfo(k).Key, vCaseInfo(k).Value + 1)

                                                Exit For
                                            End If
                                        Else

                                            If iSolutionTime - iLeadTime <= CInt((CInt(vCaseInfo(k).Key) + 1) * iWorkingDayLength) Then
                                                vCaseInfo(k) = New KeyValuePair(Of String, Integer)(vCaseInfo(k).Key, vCaseInfo(k).Value + 1)

                                                Exit For
                                            End If
                                        End If
                                    Next

                                End If
                            End If
                        End If

                        If IsDate(objCase.WatchDate) And Not IsDate(objCase.FinishingDate) Then
                            ' Bevakning
                            vStatistics(4) = New KeyValuePair(Of String, Integer)(vStatistics(4).Key, vStatistics(4).Value + 1)

                        ElseIf objCase.StateSecondary_Id <> 0 And objCase.IncludeInCaseStatistics <> 1 And Not IsDate(objCase.FinishingDate) Then
                            ' Vilande
                            vStatistics(2) = New KeyValuePair(Of String, Integer)(vStatistics(2).Key, vStatistics(2).Value + 1)
                        End If

                        If objCase.Status <> 0 Then
                            If objCase.Status > 1 And Not IsDate(objCase.FinishingDate) Then
                                ' Varnade
                                vStatistics(3) = New KeyValuePair(Of String, Integer)(vStatistics(3).Key, vStatistics(3).Value + 1)
                            End If
                        End If

                        If (objCase.StateSecondary_Id = 0 Or objCase.IncludeInCaseStatistics = 1) And Not IsDate(objCase.FinishingDate) And Not IsDate(objCase.WatchDate) Then
                            vStatistics(1) = New KeyValuePair(Of String, Integer)(vStatistics(1).Key, vStatistics(1).Value + 1)
                        End If

                        ' Totalt
                        vStatistics(0) = New KeyValuePair(Of String, Integer)(vStatistics(0).Key, vStatistics(0).Value + 1)

                    Next

                    sStatistics = "<TABLE><TR><TD Class=""Celltext"">"
                    sStatistics = sStatistics & "Aktiva ärenden:</TD><TD Class=""Celltext"">" & vStatistics(0).Value & "</TD></TR>"
                    sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Vilande:</TD><TD Class=""Celltext"">" & vStatistics(1).Value & "</TD></TR>"
                    sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Varnade:</TD><TD Class=""Celltext"">" & vStatistics(2).Value & "</TD></TR>"
                    sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Bevakade:</TD><TD Class=""Celltext"">" & vStatistics(4).Value & "</TD></TR>"
                    sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Totalt:</TD><TD Class=""Celltext"">" & vStatistics(0).Value & "</TD></TR></TABLE>"

                    sStatistics = sStatistics & "<BR><B>Antal ärenden per återstående åtgärdstid (dagar)</B><BR>"

                    sStatistics = sStatistics & "<TABLE>"

                    'For k = 0 To CLng(iMaxSolutionTime)
                    '    sStatistics = sStatistics & "<TR><TD Class=""Celltext"">" & vCaseInfo(k).Key & "</TD><TD Class=""Celltext"">" & vCaseInfo(k).Value & "</TD></TR>"
                    'Next

                    sStatistics = sStatistics & "</TABLE>"


                    If IsValidEmailAddress(objCustomer.CaseStatisticsEMailList) = True Then
                        Dim objMail As New Mail

                        Dim sMessageId As String = createMessageId(objCustomer.HelpdeskEMail)

                        Dim sRet_SendMail As String = objMail.Send(objCustomer, objCustomer.HelpdeskEMail, Replace(objCustomer.CaseStatisticsEMailList, vbCrLf, ""), "Ärendestatistik - " & objCustomer.Name & " - " & DateTime.Now.Year & "-" & DateTime.Now.Month & "-" & DateTime.Now.Day, "<FONT face=""MS Sans Serif"" size=2>" & sStatistics & "</FONT>", objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)
                    End If
                End If

                '    sSQLCase = "Select tblCase.Id, " &
                '                "CaseNumber, " &
                '                "Priority_Id, " &
                '                "FinishingDate, " &
                '                "StateSecondary_Id, " &
                '                "ExternalTime, " &
                '                "RegTime, " &
                '                "IsNull(SolutionTime, 0) As SolutionTime, " &
                '                "tblCase.Status, " &
                '                "IncludeInCaseStatistics, " &
                '                "Watchdate " &

                '"FROM tblCase " &
                '    "LEFT OUTER JOIN tblPriority On tblCase.Priority_Id = tblPriority.Id " &
                '    "LEFT OUTER JOIN tblStateSecondary On tblCase.StateSecondary_Id = tblStateSecondary.Id " &
                '"WHERE tblCase.Customer_Id=" & rsCustomer("Id") & " " &
                '    "And tblCase.FinishingDate Is NULL And tblCase.Deleted=0 "

                '    rsStatistics.Open(sSQLCase, conDH_Support)



            End If
        Next


    End Sub

    Private Sub openLogFile()
        Dim sFileName As String
        Dim sTemp As String

        Dim sPath As String = Environment.CurrentDirectory & "\log\"

        ' JWE 201901-15 added safe meassures for log folder
        Directory.CreateDirectory(sPath)

        sFileName = sPath & "DH_Helpdesk_Schedule_" & DatePart(DateInterval.Year, Now())

        sTemp = DatePart(DateInterval.Month, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Day, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp & ".log"

        objLogFile = New StreamWriter(sFileName, True)

    End Sub

    Private Sub closeLogFile()
        If objLogFile Is Nothing Then
            Return
        End If
        objLogFile.Close()
    End Sub

End Module
