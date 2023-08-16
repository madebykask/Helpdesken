Imports System.Configuration
Imports System.Data.SqlClient
Imports DH.Helpdesk.Library
Imports System.IO
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.Common.Constants
Imports DH.Helpdesk.Common.Enums

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
        'ReDim aArguments(1)
        'aArguments(0) = 1
        'aArguments(1) = "Data Source=ITSEELM-NT2014.ikea.com; Initial Catalog=ITSQL0099; User Id=dhsschr; Password=dhsschr321!;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=DHUTVSQL2; Initial Catalog=DH_Support; User Id=sa; Password=;Network Library=dbmssocn"

        If aArguments.Length > 0 Then

            ' parse command line args
            Dim workMode = GetWorkMode(aArguments)
            Dim sConnectionstring = GetConnectionString(aArguments)
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

            If Not objMailTemplate Is Nothing Then
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

            If Not objMailTemplate Is Nothing Then
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

                If Not objMailTemplate Is Nothing Then
                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                    Dim sSendTime As DateTime = Date.Now()
                    Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                    Dim objMail As New Mail
                    Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                    objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, SharedFunctions.EMailType.EMailAssignCasePerformer, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                End If
            End If


            If Not objCaseSolution.ExtendedCaseFormId Is Nothing Then
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

                If Not objMailTemplate Is Nothing Then
                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                    Dim sSendTime As DateTime = Date.Now()
                    Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                    Dim objMail As New Mail
                    Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

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

        For i As Integer = 1 To colCase.Count
            objCase = colCase(i)

            If objCase.StateSecondary_FinishingCause_Id IsNot Nothing Then

                objLogFile.WriteLine(Now() & ", caseAutoClose, CaseNumber:" & objCase.Casenumber)

                Dim iCaseHistory_Id As Integer = objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk")

                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)

                ' Save Logs (Logga händelsen)
                Dim iLog_Id As Integer = objLogData.createLog(objCase.Id, objCase.Persons_EMail, "", "", 0, "DH Helpdesk", iCaseHistory_Id, objCase.StateSecondary_FinishingCause_Id)
                objCaseData.closeCase(objCase)

                objMailTemplate = objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailCaseClosed, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                If objMailTemplate IsNot Nothing Then
                    If objCase.Persons_EMail <> "" Then
                        If objMailTemplate.SendMethod = "1" Then
                            sEmailList = objCase.Persons_EMail
                        Else
                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                            Dim sSendTime As DateTime = Date.Now()
                            Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                            Dim objMail As New Mail
                            Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                            objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

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

                                objLogData.createEMailLog(iCaseHistory_Id, follower, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

                            End If
                        Next

                    End If

                    If sEmailList <> "" Then
                        sMessageId = createMessageId(objCustomer.HelpdeskEMail)

                        Dim sSendTime As DateTime = Date.Now()
                        Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                        Dim objMail As New Mail
                        Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, sEmailList, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)

                        objLogData.createEMailLog(iCaseHistory_Id, sEmailList, SharedFunctions.EMailType.EMailCaseClosed, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

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

                    objMailData.Send(objCustomer.HelpdeskEMail, objUser.EMail, sSubject, sMessage, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)

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
            objMail.Send(objCustomer.HelpdeskEMail, cl.Recipients, "Checklista: " & cl.ChecklistName, cl.ChecklistMailBody, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)
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

        gsConnectionString = sConnectionString

        ' Hämta globala inställningar
        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

        giDBType = objGlobalSettings.DBType

        Dim colCustomer = objCustomerData.getCustomers()

        For i = 1 To colCustomer.Count
            objCustomer = colCustomer(i)

            If objCustomer.CaseStatisticsEMailList <> "" Then

            End If
        Next


        '   If Not isEmptyRecordset(rsCustomer) Then
        '       Do Until rsCustomer.EOF
        '           iWorkingDayStart = rsCustomer("WorkingDayStart")
        '           iWorkingDayEnd = rsCustomer("WorkingDayEnd")
        '           iWorkingDayLength = CInt(iWorkingDayEnd) - CInt(iWorkingDayStart)
        '           vCaseInfo = Empty
        '           vPriority = Empty
        '           iMaxSolutionTime = 0
        '           iNumberOfPrioritiesLessThan1WorkingDay = 0

        '           ' Hämta in prioriteter
        '           sSQL = "SELECT  MAX(SolutionTime) AS SolutionTime FROM tblPriority"
        '           sSQL = sSQL & " WHERE Customer_Id=" & rsCustomer("Id")

        '           rsStatistics.Open(sSQL, conDH_Support)

        '           If Not IsNull(rsStatistics("SolutionTime")) Then
        '               iMaxSolutionTime = CLng(CLng(rsStatistics("SolutionTime")) / iWorkingDayLength)
        '           End If
        '           rsStatistics.Close()

        '           ' Hämta in antalet prioriteter som har mindre än 1 arbetsdags lösningstid
        '           sSQL = "SELECT SolutionTime " & _
        '                   "FROM tblPriority "

        '           If iCustomer_Id <> "" Then
        '               sSQL = sSQL & " WHERE Customer_Id=" & rsCustomer("Id")
        '           End If

        '           sSQL = sSQL & " GROUP BY SolutionTime " & _
        '                           "HAVING (SolutionTime <> 0) AND (SolutionTime < " & iWorkingDayLength & ") " & _
        '                           "ORDER BY SolutionTime "

        '           rsStatistics.Open(sSQL, conDH_Support)

        '           If Not IsEmptyRecordset(rsStatistics) Then
        '               i = 0
        '               iMinSolutionTime = rsStatistics("SolutionTime")
        '               Do Until rsStatistics.EOF
        '                   If i = 0 Then
        '                       ReDim vPriority(0, 0)
        '                   Else
        '                       ReDim Preserve vPriority(0, i)
        '                   End If

        '                   vPriority(0, i) = rsStatistics("SolutionTime")

        '                   i = i + 1
        '                   rsStatistics.MoveNext()
        '               Loop

        '               iNumberOfPrioritiesLessThan1WorkingDay = i
        '           End If

        '           rsStatistics.Close()

        '           ' Skapa array som ska innehålla statistikinformation
        '           ReDim vStatistics(4, 1)

        '           vStatistics(0, 0) = "Ärenden"
        '           vStatistics(1, 0) = "Aktiva"
        '           vStatistics(2, 0) = "Vilande"
        '           vStatistics(3, 0) = "Varnade"
        '           vStatistics(4, 0) = "Bevakade"
        '           vStatistics(0, 1) = 0
        '           vStatistics(1, 1) = 0
        '           vStatistics(2, 1) = 0
        '           vStatistics(3, 1) = 0
        '           vStatistics(4, 1) = 0

        '           ' Skapa array med prioriteter
        '           If CLng(iMaxSolutionTime) <> 0 Then
        '               ReDim vCaseInfo(iMaxSolutionTime + iNumberOfPrioritiesLessThan1WorkingDay, 1)

        '               vCaseInfo(0, 0) = "0 (Akut)"
        '               vCaseInfo(0, 1) = 0

        '               For i = 1 To iNumberOfPrioritiesLessThan1WorkingDay
        '                   vCaseInfo(i, 0) = vPriority(0, i - 1)
        '                   vCaseInfo(i, 1) = 0
        '               Next

        '               For i = 1 + iNumberOfPrioritiesLessThan1WorkingDay To iMaxSolutionTime + iNumberOfPrioritiesLessThan1WorkingDay
        '                   vCaseInfo(i, 0) = i - iNumberOfPrioritiesLessThan1WorkingDay
        '                   vCaseInfo(i, 1) = 0
        '               Next
        '           End If

        '           sSQLCase = "SELECT tblCase.Id, " & _
        '                               "CaseNumber, " & _
        '                               "Priority_Id, " & _
        '                               "FinishingDate, " & _
        '                               "StateSecondary_Id, " & _
        '                               "ExternalTime, " & _
        '                               "RegTime, " & _
        '                               "IsNull(SolutionTime, 0) AS SolutionTime, " & _
        '                               "tblCase.Status, " & _
        '                               "IncludeInCaseStatistics, " & _
        '                               "Watchdate " & _
        '               "FROM tblCase " & _
        '                   "LEFT OUTER JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id " & _
        '                   "LEFT OUTER JOIN tblStateSecondary ON tblCase.StateSecondary_Id = tblStateSecondary.Id " & _
        '               "WHERE tblCase.Customer_Id=" & rsCustomer("Id") & " " & _
        '                   "AND tblCase.FinishingDate IS NULL AND tblCase.Deleted=0 "

        '           rsStatistics.Open(sSQLCase, conDH_Support)

        '           If Not isEmptyRecordset(rsStatistics) Then
        '               Do Until rsStatistics.EOF

        '                   If CLng(iMaxSolutionTime) <> 0 Then
        '                       If Not IsDate(rsStatistics("Watchdate")) And Not IsDate(rsStatistics("FinishingDate")) And (isNull(rsStatistics("StateSecondary_Id")) Or rsStatistics("IncludeInCaseStatistics") = "1") And Not IsNull(rsStatistics("Priority_Id")) And rsStatistics("SolutionTime") <> 0 Then
        '                           iLeadTime = LeadTime(DateAdd("n", rsStatistics("ExternalTime"), rsStatistics("RegTime")), Now(), iWorkingDayStart, iWorkingDayEnd)

        '                           If CInt(iLeadTime) >= CInt(rsStatistics("SolutionTime")) Then

        '                               vCaseInfo(0, 1) = vCaseInfo(0, 1) + 1

        '                               ' Lägg till i akutlistan
        '                               If iNrOfUrgentCases = 0 Then
        '                                   ReDim vUrgent(0)
        '                               Else
        '                                   ReDim Preserve vUrgent(iNrOfUrgentCases)
        '                               End If

        '                               vUrgent(iNrOfUrgentCases) = rsStatistics("Id")

        '                               iNrOfUrgentCases = iNrOfUrgentCases + 1
        '                           Else
        '                               For i = 1 To UBound(vCaseInfo)
        '                                   If i <= CInt(iNumberOfPrioritiesLessThan1WorkingDay) Then
        '                                       If CInt(rsStatistics("SolutionTime") - iLeadTime) <= CInt(vCaseInfo(i, 0)) Then
        '                                           vCaseInfo(i, 1) = vCaseInfo(i, 1) + 1

        '                                           Exit For
        '                                       End If
        '                                   Else

        '                                       If CInt(rsStatistics("SolutionTime") - iLeadTime) <= CInt((CInt(vCaseInfo(i, 0)) + 1) * iWorkingDayLength) Then
        '                                           vCaseInfo(i, 1) = vCaseInfo(i, 1) + 1

        '                                           Exit For
        '                                       End If
        '                                   End If
        '                               Next

        '                           End If
        '                       End If
        '                   End If

        '                   If IsDate(rsStatistics("Watchdate")) And Not IsDate(rsStatistics("Finishingdate")) Then
        '                       ' Bevakning
        '                       vStatistics(4, 1) = vStatistics(4, 1) + 1
        '                   ElseIf Not IsNull(rsStatistics("StateSecondary_Id")) And rsStatistics("IncludeInCaseStatistics") <> "1" And Not IsDate(rsStatistics("Finishingdate")) Then
        '                       ' Vilande
        '                       vStatistics(2, 1) = CLng(vStatistics(2, 1)) + 1
        '                   End If

        '                   If Not IsNull(rsStatistics("Status")) Then
        '                       If CLng(rsStatistics("Status")) > 1 And Not IsDate(rsStatistics("Finishingdate")) Then
        '                           ' Varnade
        '                           vStatistics(3, 1) = CLng(vStatistics(3, 1)) + 1
        '                       End If
        '                   End If

        '                   If (IsNull(rsStatistics("StateSecondary_Id")) Or rsStatistics("IncludeInCaseStatistics") = "1") And Not (IsDate(rsStatistics("Finishingdate"))) And Not IsDate(rsStatistics("Watchdate")) Then
        '                       vStatistics(1, 1) = CLng(vStatistics(1, 1)) + 1
        '                   End If

        '                   ' Totalt
        '                   vStatistics(0, 1) = CLng(vStatistics(0, 1)) + 1

        '                   rsStatistics.MoveNext()
        '               Loop

        '               sStatistics = "<TABLE><TR><TD Class=""Celltext"">"
        '               sStatistics = sStatistics & "Aktiva ärenden:</TD><TD Class=""Celltext"">" & vStatistics(1, 1) & "</TD></TR>"
        '               sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Vilande:</TD><TD Class=""Celltext"">" & vStatistics(2, 1) & "</TD></TR>"
        '               sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Varnade:</TD><TD Class=""Celltext"">" & vStatistics(3, 1) & "</TD></TR>"
        '               sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Bevakade:</TD><TD Class=""Celltext"">" & vStatistics(4, 1) & "</TD></TR>"
        '               sStatistics = sStatistics & "<TR><TD Class=""Celltext"">Totalt:</TD><TD Class=""Celltext"">" & vStatistics(0, 1) & "</TD></TR></TABLE>"

        '               If Not IsEmpty(vCaseInfo) Then
        '                   sStatistics = sStatistics & "<BR><B>Antal ärenden per återstående åtgärdstid (dagar)</B><BR>"

        '                   sStatistics = sStatistics & "<TABLE>"

        '                   For i = 0 To CLng(iMaxSolutionTime)
        '                       sStatistics = sStatistics & "<TR><TD Class=""Celltext"">" & vCaseInfo(i, 0) & "</TD><TD Class=""Celltext"">" & vCaseInfo(i, 1) & "</TD></TR>"
        '                   Next

        '                   sStatistics = sStatistics & "</TABLE>"
        '               End If

        '               If isValidEMailAddress(rsCustomer("CaseStatisticsEMailList")) = True Then

        'sendMail_CDOSYS rsCustomer("HelpdeskEMail"), Replace(rsCustomer("CaseStatisticsEMailList"), vbCrLf, ""), "Ärendestatistik - " & rsCustomer("Name") & " - " & DatePart("yyyy", Date) & "-" & Lpad(DatePart("m", Date), "0", 2) & "-" & Lpad(DatePart("d", Date), "0", 2), "<FONT face=""MS Sans Serif"" size=2>" & sStatistics & "</FONT>"
        '               End If
        '           End If

        '           rsStatistics.Close()

        '           ' Logga aktiviteten
        '           sSQL = "INSERT INTO tblLogProgram (Log_Type, Customer_Id, LogText, RegTime) " & _
        '                   "VALUES(4, " & rsCustomer("Id") & ", '" & sStatistics & "', getDate())"

        '           conDH_Support.Execute sSQL

        '           rsCustomer.MoveNext()
        '       Loop
        '   End If
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
        objLogFile.Close()
    End Sub
End Module
