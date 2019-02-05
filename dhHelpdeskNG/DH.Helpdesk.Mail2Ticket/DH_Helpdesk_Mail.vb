Imports System.Configuration
Imports DH.Helpdesk.Mail2Ticket.Library
Imports Rebex.Net
Imports System.IO
Imports System.Linq
Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports Rebex.Mail
Imports Rebex.Mime

Module DH_Helpdesk_Mail
    'Dim msDeniedHtmlBodyString As String
    Dim iSequenceNumber As ImapMessageSet
    Dim msDeniedHtmlBodyString As String
    Dim bEnableNewEmailProcessing = False

    Public Sub Main()
        Dim fileName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        ToggleConfigEncryption(fileName)
        ' 5: SyncByWorkingGroup

        Dim sCommand As String = Command()
        Dim aArguments() As String = sCommand.Split(",")
        If (aArguments.Length = 1 And aArguments(0) = "") Then
            aArguments = Nothing
        End If

        Dim appVal As String = ConfigurationManager.AppSettings("enableNewEmailProcessing")
        If (Not String.IsNullOrEmpty(appVal) AndAlso appVal.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) Then
            bEnableNewEmailProcessing = True
        End If
        Dim sConnectionstring As String = ConfigurationManager.ConnectionStrings("Helpdesk")?.ConnectionString
        Dim workingModeArg As String = ConfigurationManager.AppSettings("WorkingMode")
        Dim logFolderArg As String = ConfigurationManager.AppSettings("LogFolder")
        Dim logIdentifierArg As String = ConfigurationManager.AppSettings("LogIdentifier")
        Dim productAreaSepArg As String = ConfigurationManager.AppSettings("ProductAreaSeparator")
        Dim newModeArg As String = ""

        'NOTE: USE Command Line Arguements in Project Properties under Debug tab instead of hardcoding values
        '      Example: 5,Data Source=DHUTVSQL2; Initial Catalog=DH_Support; User Id=sa; Password=;Network Library=dbmssocn;,,,,1

        'msDeniedHtmlBodyString = ConfigurationManager.AppSettings("DeniedHtmlBodyString").ToString()

        'ReDim aArguments(2)
        'aArguments(0) = "5"
        'aArguments(1) = "Data Source=ITSEELM-NT2014.ikea.com; Initial Catalog=ITSQL0099; User Id=dhsschr; Password=dhsschr321!;Network Library=dbmssocn"

        'aArguments(1) = "Data Source=IK2T4021.wmikea.com; Initial Catalog=CHSBTPRE; User Id=IKEARetail_DH_Helpdesk; Password=kgk270;Network Library=dbmssocn"

        'aArguments(1) = "Data Source=Ik2p4042.wmikea.com; Initial Catalog=CHSBT; User Id=CHSBT_appl_user; Password=Sperl25#;Network Library=dbmssocn"

        'aArguments(1) = "Data Source=10.230.6.81; Initial Catalog=PTSQL0036; User Id=DHBSCHR; Password=dhbschr123;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=PPSEELM-NT2002.ikeadt.com; Initial Catalog=PPSQL0049; User Id=DBA_PPECHS; Password=Ikea!6712;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=DHWEBSQL1; Initial Catalog=DH_Helpdesk; User Id=sa; Password=sa4semlor;Network Library=dbmssocn;"
        'aArguments(1) = "Data Source=ITSEELM-NT2017.ikea.com; Initial Catalog=DHSEHR; User Id=dhsehr; Password=chs456;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=ITSEELM-NT2014.ikea.com; Initial Catalog=ITSQL0098;User Id=dhsefm; Password=dhsefm321!;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=ITSEELM-NT2014.ikea.com; Initial Catalog=ITSQL0112; User Id=dhbscfin; Password=dhbscfin321!;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=DHUTVSQL2; Initial Catalog=DH_Support; User Id=sa; Password=;Network Library=dbmssocn;"
        'aArguments(1) = "Data Source=PTPSEELM-NT2008.ikeadt.com;Initial Catalog=PTSQL0036;User Id=DBA_PTECHS;Password=Ikea!6712;Network Library=dbmssocn"
        'aArguments(1) = "Data Source=DHUTVSQL3; Initial Catalog=dhHelpdeskNG_Test_Preconal; User Id=dhHelpdesk_Test_Preconal; Password=kgk277;Network Library=dbmssocn;"
        'aArguments(2) = "c:\temp"
        'aArguments(3) = "datahalland"
        'aArguments(4) = ";"
        If aArguments IsNot Nothing Then
            If (aArguments.Length > 0) Then
                workingModeArg = GetCmdArg(aArguments, 0)
                sConnectionstring = GetCmdArg(aArguments, 1)
                logFolderArg = GetCmdArg(aArguments, 2)
                logIdentifierArg = GetCmdArg(aArguments, 3)
                productAreaSepArg = GetCmdArg(aArguments, 4)
                newModeArg = GetCmdArg(aArguments, 5)
                bEnableNewEmailProcessing = IIf(newModeArg = "1", True, False)
            End If
        End If

        Dim workingMode = IIf(workingModeArg = "5", SyncType.SyncByWorkingGroup, SyncType.SyncByCustomer)

        If Not String.IsNullOrEmpty(logFolderArg) Then
            gsLogPath = logFolderArg
        End If

        If Not String.IsNullOrEmpty(logIdentifierArg) Then
            gsInternalLogIdentifier = logIdentifierArg.ToString.Trim
        End If

        If Not String.IsNullOrEmpty(productAreaSepArg) Then
            gsProductAreaSeperator = productAreaSepArg
        End If

        'Log cmd line args
        Try
            openLogFile()

            'Log input params
            LogToFile(String.Format(
                "Cmd Line Args:" & vbCrLf & vbTab &
                "- WorkingMode: {0}" & vbCrLf & vbTab &
                "- ConnectionString: {1}" & vbCrLf & vbTab &
                "- Log folder: {2}" & vbCrLf & vbTab &
                "- Log identifier: {3}" & vbCrLf & vbTab &
                "- ProductArea Separator: {4}" & vbCrLf & vbTab &
                "- New email processing: {5}",
                workingModeArg, sConnectionstring, logFolderArg, logIdentifierArg, productAreaSepArg, newModeArg), 1)

            'start processing
            readMailBox(sConnectionstring, workingMode)

        Catch ex As Exception
            LogError(ex.ToString())
        Finally
            closeLogFile()
        End Try
    End Sub
    Private Function GetCmdArg(args As String(), index As Int32) As String
        Dim val As String = ""
        If args.Length > index Then
            val = args(index)
        End If
        Return val
    End Function

    Private Function ToggleConfigEncryption(exeConfigName As String)
        ' Takes the executable file name without the
        ' .config extension.
        Try
            ' Open the configuration file And retrieve 
            ' the connectionStrings section.
            Dim config = ConfigurationManager.OpenExeConfiguration(exeConfigName)

            Dim section = CType(config.GetSection("connectionStrings"), ConnectionStringsSection)

            If (section.SectionInformation.IsProtected) Then
                ' Remove encryption.
                ' section.SectionInformation.UnprotectSection()
            Else
                ' Encrypt the section.
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
            End If

            ' Save the current configuration.
            config.Save()

            LogToFile("app.config connection string is protected={0}", section.SectionInformation.IsProtected)

        Catch ex As Exception
            LogError(ex.ToString())
        End Try
    End Function

    Public Function readMailBox(ByVal sConnectionstring As String, ByVal iSyncType As SyncType) As Integer
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCustomerData As New CustomerData
        Dim objPriorityData As New PriorityData
        Dim objCustomer As Customer
        Dim objCaseData As New CaseData
        Dim objDepartmentData As New DepartmentData
        Dim objComputerUserData As New ComputerUserData
        Dim objMailTemplateData As New MailTemplateData
        Dim objMailTemplate As MailTemplate
        Dim objTextTranslationData As New TextTranslationData
        Dim objMailTicket As New Mail2TicketData
        Dim objLogData As New LogData
        Dim IMAPclient As Imap = Nothing
        Dim IMAPlist As ImapMessageCollection = Nothing ' TODO IMAP
        Dim message As MailMessage = Nothing
        Dim sFromEMailAddress As String = ""
        Dim sToEMailAddress As String = ""
        Dim sNewCaseToEmailAddress As String = ""
        Dim sSubject As String
        Dim sBodyText As String = ""
        Dim j As Integer
        Dim iLog_Id As Integer
        Dim iCaseNumber As Integer
        Dim iListCount As Integer = 0
        Dim objComputerUser As ComputerUser
        Dim bOrder As Boolean = False
        Dim iHTMLFile As Integer = 0
        Dim iCaseHistory_Id As Integer
        Dim iFinishingCause_Id As Integer
        Dim sUniqueID As String
        Dim iMailID As Integer = 0
        Dim sMessageId As String = ""
        Dim sRet_SendMail As String = ""
        Dim sSendTime As DateTime
        Dim sPriorityEMailList As String = ""

        Try
            gsConnectionString = sConnectionstring

            ' Hämta globala inställningar
            objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

            giDBType = objGlobalSettings.DBType
            gsDBVersion = objGlobalSettings.DBVersion

            Dim colCustomer As Collection

            If iSyncType = SyncType.SyncByWorkingGroup Then
                colCustomer = objCustomerData.getCustomersByWorkingGroup()
            Else
                colCustomer = objCustomerData.getCustomers()
            End If

            For i As Integer = 1 To colCustomer.Count

                objCustomer = colCustomer(i)

                If objCustomer.PhysicalFilePath = "" Then
                    objCustomer.PhysicalFilePath = objGlobalSettings.AttachedFileFolder
                End If

                Dim iPop3DebugLevel = objCustomer.POP3DebugLevel
                
                giLoglevel = iPop3DebugLevel

                If Not String.IsNullOrEmpty(objCustomer.POP3Server) AndAlso Not String.IsNullOrEmpty(objCustomer.POP3UserName) Then

                    LogToFile("M2T for " & objCustomer.Name & ", Nr: " & i & "(" & colCustomer.Count & "), ver " & objGlobalSettings.DBVersion, iPop3DebugLevel)

                    Try
                        IMAPclient = New Imap()

                        Dim host As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(objCustomer.POP3Server)
                        Dim ip As String = ""

                        If Not host Is Nothing Then
                            ip = host.AddressList(0).ToString()
                        End If

                        LogToFile("Connecting to " & objCustomer.POP3Server & " (" + ip + "):" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName, iPop3DebugLevel)

                        If objCustomer.POP3Port = 993 Then
                            IMAPclient.Connect(objCustomer.POP3Server.ToString(), objCustomer.POP3Port, Nothing, ImapSecurity.Implicit)
                        Else
                            IMAPclient.Connect(objCustomer.POP3Server, objCustomer.POP3Port)
                        End If
                        
                        ' hämta inställningar om e-post texten ska översättas till fält på ärendet
                        Dim fieldsToUpdate As Dictionary(Of String, String)
                        fieldsToUpdate = objCustomerData.GetCaseFieldsSettings(objCustomer.Id)

                        ' hämta alla avdelningar på kunden
                        Dim colDepartment As Collection
                        colDepartment = objDepartmentData.getDepartments(objCustomer.Id)

                        ' hämta alla prioroteter på kunden
                        Dim colPrio As Collection
                        colPrio = objPriorityData.GetPriorityByCustomerId(objCustomer.Id)

                        'If objCustomer.MailServerProtocol = 0 Then
                        '    ' Inget stöd för POP3 längre

                        'ElseIf objCustomer.MailServerProtocol = 1 Then
                        '    IMAPclient = New Imap()

                        '    If objCustomer.POP3DebugLevel > 0 Then
                        '        objLogFile.WriteLine(Now() & ", Connecting to " & objCustomer.POP3Server & ":" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName)
                        '    End If

                        '    If objCustomer.POP3Port = 993 Then
                        '        IMAPclient.Connect(objCustomer.POP3Server.ToString(), objCustomer.POP3Port, Nothing, ImapSecurity.Implicit)
                        '    Else
                        '        IMAPclient.Connect(objCustomer.POP3Server, objCustomer.POP3Port)
                        '    End If

                        'End If

                        If String.IsNullOrEmpty(objCustomer.POP3UserName) Or String.IsNullOrEmpty(objCustomer.POP3Password) Then
                            LogError("Missing UserName Or Password")
                            Exit For
                        ElseIf objCustomer.EMailDefaultCaseType_Id = 0 Then
                            LogError("Missing Default Case Type")
                            Exit For
                        End If
                        
                        If objCustomer.MailServerProtocol = 0 Then
                            ' Inget stöd för POP3 längre
                            LogToFile("Pop3 Is Not supported.", iPop3DebugLevel)
                        Else
                            LogToFile("Login " & objCustomer.POP3UserName, iPop3DebugLevel)
                            IMAPclient.Login(objCustomer.POP3UserName, objCustomer.POP3Password)
                        End If

                        If objCustomer.MailServerProtocol = 0 Then
                            ' Inget stöd för POP3 längre
                        ElseIf objCustomer.MailServerProtocol = 1 Then

                            Dim emailFolder As String = "Inbox" ' Default email folder
                            
                            'Validate email folders
                            If Not String.IsNullOrEmpty(objCustomer.EMailFolder) Then
                                If Not CheckEmailFolderExists(IMAPclient, objCustomer.EMailFolder)
                                    LogError($"EmailFolder '{objCustomer.EMailFolder}' doesn't exist.")
                                    Exit For
                                Else 
                                    emailFolder = objCustomer.EMailFolder
                                End If
                            End If

                            If Not String.IsNullOrEmpty(objCustomer.EMailFolderArchive) Then
                                If Not CheckEmailFolderExists(IMAPclient, objCustomer.EMailFolderArchive)
                                    LogError($"EmailFolderArchive '{objCustomer.EMailFolderArchive}' doesn't exist.")
                                    Exit For
                                End If
                            End If

                            LogToFile("Connecting to '" & emailFolder & "' email folder.", iPop3DebugLevel)
                            IMAPclient.SelectFolder(emailFolder)

                            If Not IMAPclient.CurrentFolder.Name.Equals(emailFolder, StringComparison.OrdinalIgnoreCase) Then
                                LogError($"Failed to connect to '{emailFolder}' email folder")
                                Exit For
                            End If

                            IMAPlist = IMAPclient.GetMessageList()

                            iListCount = IMAPlist.Count()
                            LogToFile("IMAPlist.Count: " & iListCount, iPop3DebugLevel)
                        End If

                        If iListCount > 0 Then
                            For iListIndex As Integer = 0 To iListCount - 1
                                iFinishingCause_Id = 0
                                sBodyText = ""
                                iLog_Id = 0
                                sUniqueID = IMAPlist(iListIndex).UniqueId

                                Dim objCase As CCase = Nothing
                                
                                If objCustomer.MailServerProtocol = 0 Then
                                    ' Inget stöd för POP3 längre
                                Else
                                    message = IMAPclient.GetMailMessage(sUniqueID)
                                    message.Silent = True
                                End If

                                Dim uniqueMessageId As String = ""
                                If message.MessageId IsNot Nothing Then
                                    uniqueMessageId = message.MessageId.ToString
                                End If

                                LogToFile("Read Mail From " & message.From.ToString & ", To " & message.To.ToString & ", MessageID: " & uniqueMessageId & ", HasBodyText: " & message.HasBodyText & ", " & message.HasBodyHtml & ", IsSigned: " & message.IsSigned & ", Silent: " & message.Silent, iPop3DebugLevel)

                                sFromEMailAddress = parseEMailAddress(message.From.ToString())
                                sToEMailAddress = parseEMailAddress(message.To.ToString())

                                If objCustomer.POP3EMailPrefix <> "" Then
                                    sFromEMailAddress = Replace(sFromEMailAddress, objCustomer.POP3EMailPrefix, "")
                                End If

                                sNewCaseToEmailAddress = sFromEMailAddress
                                sSubject = message.Subject.ToString()

                                ' Kontrollera om det är ett beställningsmail
                                If InStr(1, sSubject, "beställning ordernummer", vbTextCompare) > 0 Then
                                    bOrder = True
                                Else
                                    bOrder = False
                                End If

                                If bOrder = True Then
                                    ' Ta fram användaren
                                    Dim iPos As Integer = InStr(1, sSubject, " ", vbTextCompare)
                                    Dim sUserId As String = ""

                                    If iPos > 0 Then
                                        sUserId = Trim(Left(sSubject, iPos - 1))
                                    End If
                                    
                                    LogToFile("Beställning från " & sUserId, iPop3DebugLevel)

                                    If sUserId <> "" Then
                                        Dim objCU As ComputerUser = objComputerUserData.getComputerUserByUserId(sUserId, objCustomer.Id)

                                        If Not objCU Is Nothing Then
                                            sFromEMailAddress = objCU.EMail
                                        End If

                                        'Dim objUserData As New UserData
                                        'Dim objUser As User = objUserData.getUserByUserId(sUserId)

                                        'If Not objUser Is Nothing Then
                                        '    sFromEMailAddress = objUser.EMail
                                        'End If
                                    End If
                                End If
                                
                                iMailID = 0

                                If bEnableNewEmailProcessing
                                    'New logic to find existing case by email
                                    Dim messageIds as List(Of String) = ExtractMessageIds(message)
                                    LogToFile(String.Format("MessageIds found: {0}", String.Join(",", messageIds)), objCustomer.POP3DebugLevel)

                                    ' Iterate over all UniqueMessageIds from the email to find matching case
                                    For Each messageId as String In messageIds
                                        ' Find objCase
                                        objCase = FindCaseByUniqueMessageId(messageId, objCustomer)

                                        If (objCase IsNot Nothing) Then
                                            ' Kontrollera vilket mailID detta är ett svar på | Check which mailID this is an answer to
                                            iMailID = objCaseData.getMailIDByMessageID(messageId)
                                            LogToFile(Now() & "getMailIDByMessageID: " & iMailID.ToString(), objCustomer.POP3DebugLevel)
                                            Exit For
                                        End If
                                    Next
                                Else 
                                    'old logic 
                                    If message.InReplyTo.Count > 0 Then
                                        Dim replyToId As String = message.InReplyTo(0).ToString
                                        LogToFile(Now() & ", Reply From: " & replyToId, iPop3DebugLevel)
                                        
                                        LogToFile(Now() & ", getCaseByMessageID. InReplyTo: " & replyToId, iPop3DebugLevel)
                                        objCase = objCaseData.getCaseByMessageID(replyToId)

                                        If objCase Is Nothing And objCustomer.ModuleOrder = 1 Then
                                            LogToFile(Now() & ", getCaseByOrderMessageID. InReplyTo: " & replyToId, iPop3DebugLevel)
                                            objCase = objCaseData.getCaseByOrderMessageID(replyToId)
                                        End If

                                        ' Kontrollera vilket mailID detta ar ett svar pa
                                        iMailID = objCaseData.getMailIDByMessageID(replyToId)
                                    Else
                                        iMailID = 0
                                    End If
                                End If
                                
                                If objCase Is Nothing Then
                                    ' Kontrollera om det är svar på ett befintligt ärende | Check if there is an answer to an existing case
                                    If objCustomer.EMailSubjectPattern <> "" Then
                                        
                                        LogToFile("Subject: " & sSubject, iPop3DebugLevel)

                                        iCaseNumber = extractCaseNumberFromSubject(sSubject, objCustomer.EMailSubjectPattern)
                                        LogToFile("CaseNumber: " & iCaseNumber, iPop3DebugLevel)

                                        If iCaseNumber <> 0 Then
                                            objCase = objCaseData.getCaseByCaseNumber(iCaseNumber)
                                        End If

                                    End If
                                End If

                                objComputerUser = objComputerUserData.getComputerUserByEMail(sFromEMailAddress, objCustomer.Id)

                                If message.HasBodyText = True Then
                                    sBodyText = Replace(message.BodyText.ToString(), Chr(10), vbCrLf, 1, -1, CompareMethod.Text)
                                ElseIf message.HasBodyHtml = True Then
                                    sBodyText = message.BodyHtml.ToString()
                                    sBodyText = convertHTMLtoText(sBodyText)
                                End If

                                '//hämta användare baserat på userid/reportedBy
                                Dim fields As New Dictionary(Of String, String)
                                If fieldsToUpdate.Count > 0 Then
                                    fields = GetParsedMailBody(fieldsToUpdate, sBodyText)
                                    Dim strUserId As String = GetValueFromEmailtext(fields, "reportedby")
                                    If Not String.IsNullOrWhiteSpace(strUserId) Then
                                        objComputerUser = objComputerUserData.getComputerUserByUserId(strUserId, objCustomer.Id)
                                    End If
                                End If

                                If objCase Is Nothing Then
                                    objCase = New CCase
                                    objCase.Caption = Left(message.Subject.ToString(), 100)
                                    objCase.Description = sBodyText
                                    objCase.CaseType_Id = objCustomer.EMailDefaultCaseType_Id
                                    objCase.Category_Id = objCustomer.EMailDefaultCategory_Id
                                    objCase.ProductArea_Id = objCustomer.EMailDefaultProductArea_Id
                                    objCase.Priority_Id = objCustomer.EMailDefaultPriority_Id
                                    objCase.Status_Id = objCustomer.DefaultStatus_Id
                                    objCase.StateSecondary_Id = objCustomer.DefaultStateSecondary_Id
                                    objCase.Customer_Id = objCustomer.Id
                                    objCase.WorkingGroup_Id = objCustomer.DefaultWorkingGroup_Id
                                    objCase.RegLanguage_Id = objCustomer.Language_Id
                                    objCase.RegistrationSourceCustomer_Id = objCustomer.RegistrationSourceCustomer_Id
                                    objCase.Performer_User_Id = objCustomer.DefaultAdministratorExternalUser_Id
                                    objCase.RegUserName = sFromEMailAddress

                                    If Not objComputerUser Is Nothing Then
                                        objCase.ReportedBy = objComputerUser.UserId
                                        objCase.Persons_Name = objComputerUser.FirstName & " " & objComputerUser.SurName
                                        objCase.Persons_EMail = objComputerUser.EMail
                                        objCase.Persons_Phone = objComputerUser.Phone
                                        objCase.Persons_CellPhone = objComputerUser.CellPhone
                                        objCase.Place = objComputerUser.Location
                                        objCase.UserCode = objComputerUser.UserCode
                                        objCase.CostCentre = objComputerUser.CostCentre

                                        If objComputerUser.OU_Id <> 0 Then
                                            objCase.OU_Id = objComputerUser.OU_Id
                                        End If

                                        If objComputerUser.Department_Id = 0 Then
                                            objCase.Department_Id = objCustomer.EMailDefaultDepartment_Id
                                        Else
                                            objCase.Department_Id = objComputerUser.Department_Id
                                        End If

                                        If objComputerUser.Region_Id <> 0 Then
                                            objCase.Region_Id = objComputerUser.Region_Id
                                        End If

                                    Else
                                        objCase.Persons_EMail = sFromEMailAddress
                                        objCase.Department_Id = objCustomer.EMailDefaultDepartment_Id
                                    End If

                                    If objCase.CaseType_Id <> 0 Then
                                        Dim ctd As New CaseTypeData

                                        Dim ct As CaseType = ctd.getCaseTypeById(objCase.CaseType_Id)

                                        If Not ct Is Nothing Then
                                            If ct.User_Id <> 0 Then
                                                objCase.Performer_User_Id = ct.User_Id
                                            End If

                                            If iSyncType <> SyncType.SyncByWorkingGroup Then
                                                If ct.WorkingGroup_Id <> 0 Then
                                                    objCase.WorkingGroup_Id = ct.WorkingGroup_Id
                                                End If
                                            End If
                                        End If
                                    End If

                                    ' hämta uppgifter från e-post till ärende objektet
                                    If fieldsToUpdate.Count > 0 Then
                                        If fields.Count > 0 Then
                                            objCase = UpdateCaseFieldsFromEmail(objCase, fields, colDepartment, objCustomer.Id, colPrio)
                                        End If
                                    End If

                                    ' Kontrollera om watchdate ska sättas
                                    If objCase.Priority_Id <> 0 Then
                                        Dim pd As New PriorityData

                                        Dim p As Priority = pd.getPriorityById(objCase.Priority_Id)

                                        If Not p Is Nothing Then
                                            sPriorityEMailList = p.EMailList
                                        End If

                                        If p.SolutionTime = 0 Then
                                            If colDepartment IsNot Nothing Then
                                                If colDepartment.Count > 0 Then
                                                    For Each d As Department In colDepartment
                                                        If objCase.Department_Id = d.Id Then
                                                            If d.WatchDate <> DateTime.MinValue Then
                                                                objCase.WatchDate = d.WatchDate
                                                            End If

                                                            Exit For
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        End If

                                    End If

                                    objCase = objCaseData.createCase(objCase)
                                    'dhal i CaseHistory skall orginal från adressen hamna i CreatedByUser 
                                    'iCaseHistory_Id = objCaseData.saveCaseHistory(objCase.Id, objCase.Persons_EMail.ToString)
                                    iCaseHistory_Id = objCaseData.saveCaseHistory(objCase.Id, sFromEMailAddress)

                                    ' save caseisabout
                                    If fieldsToUpdate.Count > 0 Then
                                        If fields.Count > 0 Then
                                            Dim objCaseIsAbout As ComputerUser = CreateCaseIsAbout(objCustomer.Id, fields)
                                            If objCaseIsAbout IsNot Nothing Then
                                                objCaseData.saveCaseIsAbout(objCase.Id, objCaseIsAbout)
                                            End If
                                        End If
                                    End If

                                    LogToFile("Create Case:" & objCase.Casenumber & ", Attachments:" & message.Attachments.Count, iPop3DebugLevel)

                                    'Save 
                                    Dim sHTMLFileName As String = createHtmlFileFromMail(message, objCustomer.PhysicalFilePath & "\" & objCase.Casenumber, objCase.Casenumber)

                                    If Not String.IsNullOrEmpty(sHTMLFileName) Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen
                                        objCaseData.saveFileInfo(objCase.Id, "html/" & sHTMLFileName)
                                    End If

                                    If message.Attachments.Count > 6 - iHTMLFile Then

                                        ' Kontrollera om folder existerar
                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber)
                                        End If

                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber)
                                        End If

                                        For j = 0 To message.Attachments.Count - 1
                                            Dim sFileNameTemp As String = message.Attachments.Item(j).FileName

                                            sFileNameTemp = sFileNameTemp.Replace(":", "")
                                            sFileNameTemp = URLDecode(sFileNameTemp)

                                            message.Attachments.Item(j).Save(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber & "\" & sFileNameTemp)
                                        Next

                                        Dim sFileName As String = objCustomer.PhysicalFilePath & "\" & objCase.Casenumber & "\" & objCase.Casenumber & ".zip"

                                        sFileName = createZipFile(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber, sFileName)

                                        If sFileName <> "" Then
                                            ' Ta bort tempkatalogen
                                            If Directory.Exists(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber) = True Then
                                                Directory.Delete(objCustomer.PhysicalFilePath & "\Temp" & objCase.Casenumber, True)
                                            End If

                                            ' Lägg in i databasen
                                            objCaseData.saveFileInfo(objCase.Id, objCase.Casenumber & ".zip")
                                        End If
                                    ElseIf message.Attachments.Count > 0 Then

                                        ' Kontrollera om folder existerar
                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber)
                                        End If

                                        For j = 0 To message.Attachments.Count - 1
                                            Dim sFileName As String = message.Attachments.Item(j).FileName

                                            sFileName = sFileName.Replace(":", "")
                                            sFileName = URLDecode(sFileName)

                                            LogToFile("Filename:" & sFileName, iPop3DebugLevel)

                                            message.Attachments.Item(j).Save(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber & "\" & sFileName)

                                            ' Lägg in i databasen
                                            objCaseData.saveFileInfo(objCase.Id, sFileName)
                                        Next

                                    End If
                                    '#65030
                                    Dim newcaseEmailTo As String = objCase.Persons_EMail
                                    If objCustomer.NewCaseMailTo = 1 Then
                                        newcaseEmailTo = sNewCaseToEmailAddress
                                    End If
                                    '#65030

                                    If isValidRecipient(newcaseEmailTo, objCustomer.AllowedEMailRecipients) = True Then
                                        If objCustomer.EMailRegistrationMailID <> 0 And bOrder = False And (message.From.ToString <> message.To.ToString) Then
                                            'If Len(objCase.Persons_EMail) > 6 Then  (objCase.Persons_EMail can be empty) #65030
                                            objMailTemplate = objMailTemplateData.getMailTemplateById(1, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                            If Not objMailTemplate Is Nothing Then
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                                                'helpdesk case 58782, #65030
                                                'Dim newcaseEmailTo As String = objCase.Persons_EMail
                                                'If objCustomer.NewCaseMailTo = 1 Then
                                                '    newcaseEmailTo = sNewCaseToEmailAddress
                                                'End If
                                                'helpdesk case 58782
                                                sRet_SendMail = objMail.sendMail(objCase, Nothing, objCustomer, newcaseEmailTo, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)
                                                objLogData.createEMailLog(iCaseHistory_Id, newcaseEmailTo, 1, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            End If
                                            'End If  #65030
                                        End If
                                    Else
                                        LogToFile("readMailBox, isValidRecipient " & objCase.Persons_EMail & ", " & objCustomer.AllowedEMailRecipients, iPop3DebugLevel)
                                    End If

                                    If objCustomer.EMailRegistrationMailID <> 0 And objCustomer.NewCaseEMailList <> "" Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(1, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim vNewCaseEmailList() As String = objCustomer.NewCaseEMailList.Split(";")

                                            For Index As Integer = 0 To vNewCaseEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                                sRet_SendMail = objMail.sendMail(objCase, Nothing, objCustomer, vNewCaseEmailList(Index), objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vNewCaseEmailList(Index), 1, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If

                                    If objCase.Performer_User_Id <> 0 Then
                                        Dim objUserData As New UserData
                                        Dim objUser As User = objUserData.getUserById(objCase.Performer_User_Id)

                                        If Not objUser Is Nothing Then
                                            If objUser.AllocateCaseMail = 1 And Len(objUser.EMail) > 6 Then
                                                objMailTemplate = objMailTemplateData.getMailTemplateById(2, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                                If Not objMailTemplate Is Nothing Then
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                                    sRet_SendMail = objMail.sendMail(objCase, Nothing, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                                    objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, 2, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                                End If
                                            End If
                                        End If
                                    End If

                                    Dim workingGroup_Id As Integer = objCase.WorkingGroup_Id
                                    Dim workingGroupEMail As String = objCase.WorkingGroupEMail
                                    Dim workingGroupAllocateCaseMail As Integer = objCase.WorkingGroupAllocateCaseMail
                                    If objCustomer.CaseWorkingGroupSource = 0 Then
                                        workingGroup_Id = objCase.PerformerWorkingGroup_Id
                                        workingGroupEMail = objCase.PerformerWorkingGroupEMail
                                        workingGroupAllocateCaseMail = objCase.PerformerWorkingGroupAllocateCaseMail
                                    End If

                                    If workingGroup_Id <> 0 Then

                                        If Not workingGroupEMail Is Nothing And workingGroupAllocateCaseMail = 1 Then
                                            objMailTemplate = objMailTemplateData.getMailTemplateById(7, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                            If Not objMailTemplate Is Nothing Then
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                                sRet_SendMail = objMail.sendMail(objCase, Nothing, objCustomer, workingGroupEMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, workingGroupEMail, 7, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            End If
                                        End If
                                    End If

                                    If sPriorityEMailList <> "" Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(13, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim vPriorityEmailList() As String = sPriorityEMailList.Split(";")

                                            For Index As Integer = 0 To vPriorityEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                                sRet_SendMail = objMail.sendMail(objCase, Nothing, objCustomer, vPriorityEmailList(Index), objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vPriorityEmailList(Index), 13, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If
                                Else

                                    ' Spara svaret som en loggpost på aktuellt ärende
                                    ' Ta endast med svaret
                                    sBodyText = extractAnswer(sBodyText, objCustomer.EMailAnswerSeparator)

                                    ' Markera ärendet som oläst
                                    objCaseData.markCaseUnread(objCase)

                                    ' Uppdatera ärendet och aktivera om det är avslutat
                                    If objCase.FinishingDate <> Date.MinValue Then
                                        ' Aktivera ärendet
                                        objCaseData.activateCase(objCase, objCustomer.OpenCase_StateSecondary_Id, objCustomer.WorkingDayStart, objCustomer.WorkingDayEnd, objCustomer.TimeZone_offset)
                                    Else
                                        If objCustomer.ModuleAccount = 1 Then
                                            ' Kontrollera om det finns en kopplad beställning
                                            Dim ad As New AccountData

                                            Dim a As Account = ad.getAccountByCaseNumber(objCase.Casenumber)

                                            If Not a Is Nothing Then
                                                If InStr(a.AccountActivity.CloseCase_M2T_Sender, sFromEMailAddress, CompareMethod.Text) <> 0 Then
                                                    iFinishingCause_Id = a.AccountActivity.CloseCase_FinishingCause_Id
                                                End If
                                            End If

                                            If iFinishingCause_Id <> 0 Then
                                                ' Avsluta ärendet
                                                objCaseData.closeCase(objCase)
                                            End If
                                        End If

                                    End If

                                    If objCase.ResetOnExternalUpdate = 1 Then
                                        objCaseData.resetStateSecondary(objCase, objCustomer.WorkingDayStart, objCustomer.WorkingDayEnd, objCustomer.TimeZone_offset)
                                    End If

                                    iCaseHistory_Id = objCaseData.saveCaseHistory(objCase.Id, objCase.Persons_EMail)

                                    ' Logga händelsen
                                    If gsInternalLogIdentifier <> "" Then

                                        If InStr(sFromEMailAddress, gsInternalLogIdentifier) > 0 Or InStr(sToEMailAddress, gsInternalLogIdentifier) > 0 Then
                                            ' Lägg in som intern loggpost
                                            iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                        Else
                                            If iMailID = 2 Or iMailID = 5 Or iMailID = 7 Or iMailID = 10 Then
                                                iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                            Else
                                                If objCustomer.DefaultEmailLogDestination = 1 And iMailID = 0 Then
                                                    iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                                Else
                                                    iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, "", sBodyText, 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                                End If

                                            End If

                                        End If
                                    Else
                                        If iMailID = 2 Or iMailID = 5 Or iMailID = 7 Or iMailID = 10 Then
                                            ' Svar på skicka intern loggpost eller om handläggaren svarar
                                            iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                        Else
                                            If objCustomer.DefaultEmailLogDestination = 1 And iMailID = 0 Then
                                                iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                            Else
                                                iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, "", sBodyText, 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                            End If

                                        End If

                                    End If

                                    Dim sHTMLFileName As String = createHtmlFileFromMail(message, objCustomer.PhysicalFilePath & "\L" & iLog_Id, objCase.Casenumber)

                                    If sHTMLFileName <> "" Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen
                                        objLogData.saveFileInfo(iLog_Id, "html/" & sHTMLFileName)
                                    End If

                                    If message.Attachments.Count > 6 + iHTMLFile Then

                                        ' Kontrollera om folder existerar
                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id)
                                        End If

                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\L" & iLog_Id) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\L" & iLog_Id)
                                        End If

                                        For j = 0 To message.Attachments.Count - 1
                                            Dim sFileNameTemp As String = message.Attachments.Item(j).FileName

                                            sFileNameTemp = sFileNameTemp.Replace(":", "")
                                            sFileNameTemp = URLDecode(sFileNameTemp)

                                            message.Attachments.Item(j).Save(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id & "\" & sFileNameTemp)
                                        Next

                                        Dim sFileName As String = objCustomer.PhysicalFilePath & "\L" & iLog_Id & "\" & iLog_Id & ".zip"

                                        sFileName = createZipFile(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id, sFileName)

                                        If sFileName <> "" Then
                                            ' Ta bort tempkatalogen
                                            If Directory.Exists(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id) = True Then
                                                Directory.Delete(objCustomer.PhysicalFilePath & "\Temp" & iLog_Id, True)
                                            End If

                                            ' Lägg in i databasen
                                            objLogData.saveFileInfo(iLog_Id, iLog_Id & ".zip")
                                        End If
                                    ElseIf message.Attachments.Count > 0 Then

                                        ' Kontrollera om folder existerar
                                        If Directory.Exists(objCustomer.PhysicalFilePath & "\L" & iLog_Id) = False Then
                                            Directory.CreateDirectory(objCustomer.PhysicalFilePath & "\L" & iLog_Id)
                                        End If

                                        For j = 0 To message.Attachments.Count - 1
                                            Dim sFileName As String = message.Attachments.Item(j).FileName

                                            sFileName = sFileName.Replace(":", "")
                                            sFileName = URLDecode(sFileName)

                                            LogToFile("Filename:" & sFileName, iPop3DebugLevel)

                                            message.Attachments.Item(j).Save(objCustomer.PhysicalFilePath & "\L" & iLog_Id & "\" & sFileName)

                                            ' Lägg in i databasen
                                            objLogData.saveFileInfo(iLog_Id, sFileName)
                                        Next

                                    End If

                                    ' Meddela handläggaren att ärendet uppdaterat
                                    If objCase.ExternalUpdateMail = 1 And Len(objCase.PerformerEMail) > 6 Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(10, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim objMail As New Mail
                                            Dim objLog As New Log

                                            objLog.Text_External = sBodyText

                                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                            sSendTime = Date.Now()

                                            Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                            sRet_SendMail = objMail.sendMail(objCase, objLog, objCustomer, objCase.PerformerEMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                            objLogData.createEMailLog(iCaseHistory_Id, objCase.PerformerEMail, 10, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                        End If
                                    ElseIf iFinishingCause_Id <> 0 And Len(objCase.Persons_EMail) > 6 Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(3, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim objMail As New Mail
                                            Dim objLog As New Log

                                            objLog.Text_External = sBodyText

                                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                            sSendTime = Date.Now()

                                            Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString

                                            sRet_SendMail = objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, sConnectionstring)

                                            objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, 3, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

                                        End If
                                    End If
                                End If

                                'here tit
                                ' spara e-post adresser
                                If message IsNot Nothing AndAlso objCase IsNot Nothing Then
                                    Dim messageId As String = message.MessageId.ToString()
                                    objMailTicket.Save(objCase.Id, iLog_Id, "to", message.To.ToString(), messageId)
                                    objMailTicket.Save(objCase.Id, iLog_Id, "cc", message.CC.ToString(), messageId)
                                    objMailTicket.Save(objCase.Id, iLog_Id, "bcc", message.Bcc.ToString(), messageId)
                                End If

                                 If objCustomer.MailServerProtocol = 0 Then
                                    ' Inget stöd för POP3 längre
                                Else
                                    If Not String.IsNullOrEmpty(objCustomer.EMailFolderArchive) Then
                                        LogToFile("Move Message to: " & objCustomer.EMailFolderArchive, iPop3DebugLevel)
                                        IMAPclient.CopyMessage(sUniqueID, objCustomer.EMailFolderArchive)
                                    End If

                                    LogToFile("Deleting Message", iPop3DebugLevel)
                                    IMAPclient.DeleteMessage(sUniqueID)
                                    
                                    ' Purge to apply message delete otherwise the message will stay in Inbox
                                    IMAPclient.Purge() 
                                End If
                            Next
                        End If 'Messages.Count
                        
                    Catch ex As Exception
                         LogError("Error readMailBox. CurstomerId  Error: " & ex.Message)
                    Finally
                        If objCustomer.MailServerProtocol = 0 Then
                            ' Inget stöd för POP3 längre
                        Else
                            
                            LogToFile("Disconnecting", iPop3DebugLevel)
                            
                            If IMAPclient IsNot Nothing Then
                                IMAPclient.Disconnect()
                                IMAPclient.Dispose()
                                IMAPclient = Nothing
                            End If
                            
                        End If
                    End Try

                End If

            Next

        Catch ex As Exception
            LogError("Error readMailBox: " & ex.Message.ToString)
            'rethrow
            Throw
        End Try
    End Function

    Private Function CheckEmailFolderExists(imapClient As IMAP, emailFolder As String) As Boolean
        Dim isFolderExists as Boolean =  False
        Try 
            isFolderExists = imapClient.FolderExists(emailFolder)
        Catch ex As Exception
            isFolderExists = False
        End Try
        Return isFolderExists
    End Function

    Private Function ExtractMessageIds(message As MailMessage) As IList(of string)
        Dim items as List(Of string) = New List(Of string)

        '1. message's messageId
        items.Add(message.MessageId.ToString())
        
        '2 replyTo's messageId 
        If message.InReplyTo IsNot Nothing AndAlso message.InReplyTo.Count > 0
            Dim messageId = message.InReplyTo(0).ToString()
            If Not String.IsNullOrEmpty(messageId)
                items.Add(messageId)
            End If
        End If 

        ' 3. References messageId
        Dim refHeader as String = GetMessageHeaderValue(message, "References")
        If Not String.IsNullOrEmpty(refHeader)

            Dim refMessageIds As String() = refHeader.Split(New Char() { " "c }, StringSplitOptions.RemoveEmptyEntries)
            If (refMessageIds.Any())
                items.AddRange(refMessageIds)
            End If

        End If

        Return items.Distinct().ToList()

    End Function

    Private Function FindCaseByUniqueMessageId(uniqueMessageId as String, customer As Customer) As CCase

        Dim objCase as CCase = Nothing
        Dim objCaseData as CaseData = new CaseData()
        Dim objMailTicket = new Mail2TicketData()

        LogToFile("Find case by messageId: " & uniqueMessageId, customer.POP3DebugLevel)

        'first check if Mail2Ticket has message record 
        Dim logData as Mail2TicketEntity  = objMailTicket.GetByMessageId(uniqueMessageId)
        If (logData IsNot Nothing AndAlso logData.CaseId > 0)
            objCase = objCaseData.getCase(logData.CaseId)
            If objCase IsNot Nothing
                LogToFile(String.Format("Message was found in Mail2Ticket table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)
                Return objCase
            End If
        End If

        'then try to find case by messageId in tblEmailLog
        If objCase Is Nothing Then
            objCase = objCaseData.getCaseByMessageID(uniqueMessageId)
            If objCase IsNot Nothing
                LogToFile(String.Format("Message was found in tblEmailLog table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)
                Return objCase
            End If
        End If

        'then try to find case by messageId among order messageIds
        If objCase Is Nothing AndAlso customer.ModuleOrder = 1 Then
            objCase = objCaseData.getCaseByOrderMessageID(uniqueMessageId)
            If objCase IsNot Nothing
                LogToFile(String.Format("Message was found in tblOrderEmailLog table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)
                Return objCase
            End If
        End If

    End Function



    Private Function GetMessageHeaderValue(message As MailMessage, name as String) As String
        Dim val as String = ""
        Dim mimeHeader as MimeHeader = message.Headers.FirstOrDefault(Function(x) x.Name = name)
        If mimeHeader IsNot Nothing
            val = mimeHeader.Raw
        End If    
        Return val
    End Function



    Private Function CreateCaseIsAbout(iCustomer_Id As Integer, fields As Dictionary(Of String, String)) As ComputerUser

        Dim objComputerUserData As New ComputerUserData
        Dim ret As ComputerUser = Nothing
        Dim strUserId As String = GetValueFromEmailtext(fields, "isabout_reportedby")
        Dim strEmail As String = GetValueFromEmailtext(fields, "isabout_persons_email")
        Dim NoOfIsAboutFields As Integer = 0

        If Not String.IsNullOrWhiteSpace(strUserId) Then
            ret = objComputerUserData.getComputerUserByUserId(strUserId, iCustomer_Id)
        Else
            If Not String.IsNullOrWhiteSpace(strEmail) Then
                ret = objComputerUserData.getComputerUserByEMail(strEmail, iCustomer_Id)
            End If
        End If

        If ret Is Nothing Then
            ret = New ComputerUser()
        End If

        For Each d As KeyValuePair(Of String, String) In fields

            If Not String.IsNullOrWhiteSpace(d.Value) Then

                Select Case d.Key.ToLower
                    Case "isabout_reportedby"
                        ret.UserId = Left(d.Value, 40)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_persons_name"
                        ret.FirstName = Left(d.Value, 50)
                        ret.SurName = String.Empty
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_persons_email"
                        ret.EMail = Left(d.Value, 100)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_persons_phone"
                        ret.Phone = Left(d.Value, 50)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_persons_cellphone"
                        ret.CellPhone = Left(d.Value, 50)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_place"
                        ret.Location = Left(d.Value, 100)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_usercode"
                        ret.UserCode = Left(d.Value, 20)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                    Case "isabout_costcentre"
                        ret.CostCentre = Left(d.Value, 50)
                        NoOfIsAboutFields = NoOfIsAboutFields + 1
                End Select
            End If
        Next

        If NoOfIsAboutFields = 0 Then
            Return Nothing
        Else
            Return ret
        End If
        Return ret

    End Function

    Private Function createHtmlFileFromMail(ByVal message As Rebex.Mail.MailMessage, _
                                            ByVal sFolder As String, _
                                            ByVal sCaseNumber As String) As String

        Dim sBodyHtml As String = ""
        Dim sFileName As String = ""
        Dim sMediaType As String = ""
        Dim sContentId As String = ""
        Dim sContentLocation As String = ""
        Dim iFileCount As Integer = 0
        Dim sFileExtension As String = ""

        Try
            If Not message Is Nothing Then
                If message.HasBodyHtml Then

                    sBodyHtml = message.BodyHtml
                    sBodyHtml = Regex.Replace(sBodyHtml, "<base.*?>", "")

                    If Directory.Exists(sFolder) = False Then
                        Directory.CreateDirectory(sFolder)
                    End If

                    If Directory.Exists(sFolder & "\html") = False Then
                        Directory.CreateDirectory(sFolder & "\html")
                    End If

                    ' Skapa fil
                    sFileName = sCaseNumber & ".htm"

                    ' Kontrollera om det finns några resurser
                    Dim resCol As Rebex.Mail.LinkedResourceCollection = message.Resources

                    For k As Integer = 0 To resCol.Count - 1
                        Dim res As Rebex.Mail.LinkedResource = resCol(k)

                        sMediaType = res.MediaType

                        If sMediaType.Contains("image") Then

                            If Not res.ContentId Is Nothing Then
                                ' Byt ut cid: i htmlbody
                                sContentId = res.ContentId.ToString
                                sContentId = sContentId.Replace("<", "")
                                sContentId = sContentId.Replace(">", "")

                                sContentId = "cid:" & sContentId

                                sFileExtension = sMediaType.Replace("image/", "")
                                iFileCount = iFileCount + 1

                                'sContentLocation = res.ContentLocation.ToString
                                ' Spara filen
                                res.Save(sFolder & "\html\" & iFileCount & "." & sFileExtension)

                                sBodyHtml = sBodyHtml.Replace(sContentId, iFileCount & "." & sFileExtension)
                            Else
                                sFileExtension = sMediaType.Replace("image/", "")
                                iFileCount = iFileCount + 1

                                ' Spara filen
                                res.Save(sFolder & "\html\" & iFileCount & "." & sFileExtension)

                                sContentLocation = res.ContentLocation.ToString
                                sBodyHtml = sBodyHtml.Replace(sContentLocation, iFileCount & "." & sFileExtension)
                            End If
                        End If
                    Next

                    Dim objFile As StreamWriter

                    objFile = New StreamWriter(sFolder & "\html\" & sFileName, False, System.Text.UnicodeEncoding.Unicode)
                    objFile.Write(sBodyHtml)
                    objFile.Close()

                End If
            End If

        Catch ex As Exception
            LogError("Error createHtmlFileFromMail MediaType: " & sMediaType & ", " & ex.Message.ToString)
            
            'Rethrow
            Throw
        End Try

        Return sFileName
    End Function

    Private Sub openLogFile()

        If objLogFile IsNot Nothing
            Return
        End If

        Dim sLogFolderPath as String

        If Not String.IsNullOrEmpty(gsLogPath) Then
            sLogFolderPath = gsLogPath 
        Else
            sLogFolderPath = Path.Combine(Environment.CurrentDirectory, "log")
        End If
        
        If Not Directory.Exists(sLogFolderPath) Then 
            Directory.CreateDirectory(sLogFolderPath)
        End If

        Dim sFileName = "DH_Helpdesk_Mail_" & DatePart(DateInterval.Year, Now()) & 
                        DatePart(DateInterval.Month, Now()).ToString().PadLeft(2, "0") & 
                        DatePart(DateInterval.Day, Now()).ToString().PadLeft(2, "0") &
                        ".log"
        Dim sFilePath = Path.Combine(sLogFolderPath, sFileName)
        objLogFile = New StreamWriter(sFilePath, True)

    End Sub
    Private Sub closeLogFile()
        If objLogFile IsNot Nothing Then
            Try
                objLogFile.Close()
            Catch ex As Exception
            Finally
                objLogFile = Nothing
            End Try 
        End If
    End Sub

    Private Function createZipFile(ByVal sSourceDir As String, ByVal sFileName As String) As String
        Dim fz As New ICSharpCode.SharpZipLib.Zip.FastZip


        fz.CreateZip(sFileName, sSourceDir, True, "", "")

        fz = Nothing

        Return sFileName
    End Function

    Private Function extractAnswer(ByVal sBodyText As String, ByVal sEMailAnswerSeparator As String) As String
        Dim aEMailAnswerSeparator() As String
        Dim iPos As Integer = 0
        Dim iPos_new As Integer = 0

        aEMailAnswerSeparator = Split(sEMailAnswerSeparator, ";")

        For i As Integer = 0 To aEMailAnswerSeparator.Length - 1
            iPos = InStr(2, sBodyText, aEMailAnswerSeparator(i).ToString, CompareMethod.Binary)

            If iPos > 0 Then
                If iPos_new = 0 Then
                    iPos_new = iPos
                Else
                    If iPos < iPos_new Then
                        iPos_new = iPos
                    End If
                End If

            End If
        Next

        If iPos_new = 0 Then
            Return sBodyText
        Else
            Return Left(sBodyText, iPos_new - 1)
        End If
    End Function

    Private Function convertHTMLtoText(ByVal sHTML As String) As String
        Dim startTime As DateTime
        Dim MyWebBrowser As New System.Windows.Forms.WebBrowser

        startTime = DateTime.Now()

        MyWebBrowser.DocumentText = sHTML

        MyWebBrowser.ScriptErrorsSuppressed = True

        While (MyWebBrowser.ReadyState <> System.Windows.Forms.WebBrowserReadyState.Complete Or MyWebBrowser.IsBusy = True) And DateDiff(DateInterval.Second, startTime, DateTime.Now()) < 10
            System.Windows.Forms.Application.DoEvents()
        End While

        Try
            Return (MyWebBrowser.Document.Body.InnerText)
        Catch ex As Exception
            Return sHTML
        End Try
    End Function

    Private Function isValidRecipient(sEMail As String, sAllowedEMailRecipents As String) As Boolean
        isValidRecipient = False

        If sAllowedEMailRecipents = "" Then
            isValidRecipient = True
        Else
            Dim aEMails() As String = sAllowedEMailRecipents.Split(";")

            For i As Integer = 0 To aEMails.Length - 1
                If InStr(sEMail, aEMails(i).ToString, CompareMethod.Text) <> 0 Then

                    isValidRecipient = True
                    Exit For
                End If
            Next
        End If
    End Function

    Private Function GetParsedMailBody(fields As Dictionary(Of String, String), ByVal mailMessage As String) As Dictionary(Of String, String)

        Dim ret As New Dictionary(Of String, String)

        If fields IsNot Nothing Then
            If Not String.IsNullOrEmpty(mailMessage) Then

                For Each d As KeyValuePair(Of String, String) In fields

                    Dim fromPos As Integer
                    fromPos = mailMessage.IndexOf(d.Value)

                    If fromPos > -1 Then
                        Dim toPos As Integer

                        ' försök först hitta en sluttag
                        Dim endTag As String = "/" & d.Value
                        toPos = mailMessage.IndexOf(endTag, fromPos)

                        If (toPos < 1) Then
                            ' använd enter om den inte hittas
                            toPos = mailMessage.IndexOf(vbNewLine, fromPos)
                        End If

                        If toPos > 0 Then
                            ret.Add(d.Key, mailMessage.Substring(fromPos, toPos - fromPos).Replace(d.Value, String.Empty).Trim())
                        End If
                    End If

                Next

            End If
        End If

        Return ret

    End Function

    Private Function UpdateCaseFieldsFromEmail(ByRef c As CCase, fields As Dictionary(Of String, String), departments As Collection, customerid As Integer, priorities As Collection) As CCase
        Dim ret As CCase = c

        For Each d As KeyValuePair(Of String, String) In fields

            If Not String.IsNullOrWhiteSpace(d.Value) Then
                Select Case d.Key.ToLower
                    Case "reportedby"
                        c.ReportedBy = Left(d.Value, 40)
                    Case "persons_name"
                        c.Persons_Name = Left(d.Value, 50)
                    Case "persons_email"
                        c.Persons_EMail = Left(d.Value, 100)
                    Case "persons_phone"
                        c.Persons_Phone = Left(d.Value, 50)
                    Case "persons_cellphone"
                        c.Persons_CellPhone = Left(d.Value, 50)
                    Case "place"
                        c.Place = Left(d.Value, 100)
                    Case "description"
                        c.Description = d.Value
                    Case "miscellaneous"
                        c.Miscellaneous = Left(d.Value, 1000)
                    Case "available"
                        c.Available = Left(d.Value, 100)
                    Case "invoicenumber"
                        c.InvoiceNumber = Left(d.Value, 50)
                    Case "referencenumber"
                        c.ReferenceNumber = Left(d.Value, 200)
                    Case "usercode"
                        c.UserCode = Left(d.Value, 20)
                    Case "inventorynumber"
                        c.InventoryNumber = Left(d.Value, 20)
                    Case "inventorylocation"
                        ' TODO
                    Case "department_id"
                        If departments IsNot Nothing Then
                            If departments.Count > 0 Then
                                For Each dp As Department In departments
                                    If ReturnStringForCompare(dp.Department) = ReturnStringForCompare(d.Value) Then
                                        c.Department_Id = dp.Id
                                        If dp.Region_Id > 0 Then
                                            c.Region_Id = dp.Region_Id
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Case "priority_id"
                        If priorities IsNot Nothing Then
                            If priorities.Count > 0 Then
                                For Each p As Priority In priorities
                                    If ReturnStringForCompare(p.Name) = ReturnStringForCompare(d.Value) Then
                                        c.Priority_Id = p.Id
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Case "productarea_id"
                        Dim pa As ProductArea = ReturnProductArea(customerid, d.Value)
                        If pa IsNot Nothing Then
                            c.ProductArea_Id = pa.Id
                            If pa.Priority_Id <> 0 Then
                                c.Priority_Id = pa.Priority_Id
                            End If
                            If pa.WorkingGroup_Id <> 0 Then
                                c.WorkingGroup_Id = pa.WorkingGroup_Id
                            End If
                            If pa.SubState_Id <> 0 Then
                                c.StateSecondary_Id = pa.SubState_Id
                            End If
                        End If
                End Select
            End If
        Next

        Return ret

    End Function

    Private Function GetValueFromEmailtext(fields As Dictionary(Of String, String), key As String) As String
        Dim ret As String = String.Empty

        If fields IsNot Nothing Then
            For Each d As KeyValuePair(Of String, String) In fields
                If Not String.IsNullOrWhiteSpace(d.Value) Then
                    If d.Key.ToLower = key.ToLower() Then
                        ret = d.Value.Trim()
                        Exit For
                    End If
                End If
            Next
        End If

        Return ret

    End Function

    Private Function ReturnStringForCompare(value As String) As String
        Return value.Replace(" ", "").ToLower
    End Function

    Private Sub LogToFile(msg As String, level As Integer) 
        If level > 0 Then
            If objLogFile IsNot Nothing 
                objLogFile.WriteLine("{0}: {1}", Now(),  msg)
            End If
        End If
    End Sub

    Private Sub LogError(msg As String) 
        If objLogFile IsNot Nothing 
            objLogFile.WriteLine("{0}: {1}", Now(),  msg)
        End If
    End Sub


    Private Function ReturnProductArea(customerid As Integer, value As String) As ProductArea
        Dim ret As ProductArea = Nothing
        Dim areas As String() = Split(value, gsProductAreaSeperator)
        Dim parentid As Integer = 0
        Dim pa As New ProductAreaData()

        If IsArray(areas) Then
            For i As Integer = 0 To (areas.Length - 1) Step 1

                Dim name As String = areas(i).Trim()
                Dim p As ProductArea = pa.GetProductArea(customerid, name, parentid)
                If p IsNot Nothing Then
                    ret = p
                    parentid = p.Id
                End If

            Next
        End If

        Return ret

    End Function

End Module
