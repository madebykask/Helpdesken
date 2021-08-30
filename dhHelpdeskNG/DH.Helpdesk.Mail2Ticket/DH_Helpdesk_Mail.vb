﻿Imports System.Configuration
Imports System.Data.SqlClient
Imports DH.Helpdesk.Library
Imports Rebex.Net
Imports System.IO
Imports System.Linq
Imports System.Text
Imports DH.Helpdesk.Library.SharedFunctions
Imports System.Text.RegularExpressions
Imports DH.Helpdesk.BusinessData.Enums.Users
Imports Rebex.Mail
Imports Rebex.Mime
Imports DH.Helpdesk.BusinessData.OldComponents.GlobalEnums
Imports Microsoft.Identity.Client
Imports System.Threading.Tasks
Imports Rebex.Mime.Headers
Imports System.Collections.Generic
Imports System.Net
Imports System.Reflection
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Windows.Forms
Imports ICSharpCode.SharpZipLib.Zip
Imports Microsoft.Exchange.WebServices.Data
Imports Microsoft.VisualBasic
Imports Rebex
Imports Winnovative
Imports System.Threading

Module DH_Helpdesk_Mail
    Private Const InboxMailFolderName As String = "Inbox"
    'Dim msDeniedHtmlBodyString As String
    Dim iSequenceNumber As ImapMessageSet
    Dim msDeniedHtmlBodyString As String

    ' Optional Parameters set from config
    Dim bEnableNewEmailProcessing = False
    Dim customersFilter As List(Of Integer) = New List(Of Integer)
    Dim workingGroupsFilter As List(Of Integer) = New List(Of Integer)
    Dim sEmailForlderOverride As String
    Dim sEmailForlderArchiveOverride As String
    Dim iLogLevelOverride As Integer? = Nothing
    Dim bEnableIMAPIClientLog As Boolean = False
    Dim sIMAPIClientLogPath As String
    Public Enum MailConnectionType
        Pop3 = 0
        Imap = 1
        Ews = 2
    End Enum

    Dim eMailConnectionType As MailConnectionType


    Public Sub Main()

        Dim secureConnectionString As String = GetAppSettingValue("SecureConnectionString")
        If (Not IsNullOrEmpty(secureConnectionString) AndAlso secureConnectionString.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) Then
            Dim fileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location)
            ToggleConfigEncryption(fileName)
        End If
        ' 5: SyncByWorkingGroup

        Dim sCommand As String = Command()
        Dim aArguments() As String = sCommand.Split(",")
        If (aArguments.Length = 1 And aArguments(0) = "") Then
            aArguments = Nothing
        End If

        Dim appVal As String = GetAppSettingValue("enableNewEmailProcessing")
        If (Not IsNullOrEmpty(appVal) AndAlso appVal.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) Then
            bEnableNewEmailProcessing = True
        End If
        Dim sConnectionstring As String = ConfigurationManager.ConnectionStrings("Helpdesk")?.ConnectionString
        Dim workingModeArg As String = GetAppSettingValue("WorkingMode")
        Dim logFolderArg As String = GetAppSettingValue("LogFolder")
        Dim logIdentifierArg As String = GetAppSettingValue("LogIdentifier")
        Dim productAreaSepArg As String = GetAppSettingValue("ProductAreaSeparator")
        Dim newModeArg As String = ""

#Region "Optional params for diagnostic purposes"

        Dim sWorkingGroupsFilter = GetAppSettingValue("workingGroupsFilter")
        Dim sCustomersFilter = GetAppSettingValue("customersFilter")
        sEmailForlderOverride = GetAppSettingValue("emailFolderOverride")
        sEmailForlderArchiveOverride = GetAppSettingValue("emailFolderArchiveOverride")

        Dim sLogLevelOverride As String = GetAppSettingValue("logLevelOverride")
        iLogLevelOverride = If(IsNullOrEmpty(sLogLevelOverride), 0, Int32.Parse(sLogLevelOverride))

        Dim sEnableIMAPIClientLog = GetAppSettingValue("enableIMAPIClientLog")
        bEnableIMAPIClientLog = Not IsNullOrEmpty(sEnableIMAPIClientLog) AndAlso Boolean.Parse(sEnableIMAPIClientLog)

        sIMAPIClientLogPath = GetAppSettingValue("IMAPIClientLogPath")

        If Not IsNullOrEmpty(sCustomersFilter) Then
            Dim elements() As String = sCustomersFilter.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
            customersFilter = elements.Select(Function(x) Int32.Parse(x)).ToList()
        End If

        If Not IsNullOrEmpty(sWorkingGroupsFilter) Then
            Dim elements() As String = sWorkingGroupsFilter.Split(New Char() {";"c}, StringSplitOptions.RemoveEmptyEntries)
            workingGroupsFilter = elements.Select(Function(x) Int32.Parse(x)).ToList()
        End If

#End Region

        'NOTE: USE Command Line Arguements in Project Properties under Debug tab instead of hardcoding values
        '      Example: 5,Data Source=DHUTVSQL2; Initial Catalog=DH_Support; User Id=sa; Password=;Network Library=dbmssocn;,,,,1

        'msDeniedHtmlBodyString = GetConfigVal("DeniedHtmlBodyString").ToString()

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
                workingModeArg = GetCmdArg(aArguments, 0, workingModeArg)
                sConnectionstring = GetCmdArg(aArguments, 1, sConnectionstring)
                logFolderArg = GetCmdArg(aArguments, 2, logFolderArg)
                logIdentifierArg = GetCmdArg(aArguments, 3, logIdentifierArg)
                productAreaSepArg = GetCmdArg(aArguments, 4, productAreaSepArg)
                newModeArg = GetCmdArg(aArguments, 5, newModeArg)
                bEnableNewEmailProcessing = newModeArg = "1"
            End If
        End If

        Dim workingMode = If(workingModeArg = "5", SyncType.SyncByWorkingGroup, SyncType.SyncByCustomer)
        ' For testing purposes only
        ' Dim workingMode = SyncType.SyncByWorkingGroup

        If Not IsNullOrEmpty(logFolderArg) Then
            gsLogPath = logFolderArg
        End If

        If Not IsNullOrEmpty(logIdentifierArg) Then
            gsInternalLogIdentifier = logIdentifierArg.ToString.Trim
        End If

        If Not IsNullOrEmpty(productAreaSepArg) Then
            gsProductAreaSeperator = productAreaSepArg
        End If

        'Log cmd line args
        Try

            'openLogFile()
            If IsNullOrEmpty(sConnectionstring) Then
                Throw New ArgumentNullException("connection string")
            End If

            Dim logConnectionString As String = FormatConnectionString(sConnectionstring)
            'Log input params
            LogToFile(String.Format(
                "Cmd Line Args:" & vbCrLf & vbTab &
                "- WorkingMode: {0}" & vbCrLf & vbTab &
                "- ConnectionString: {1}" & vbCrLf & vbTab &
                "- Log folder: {2}" & vbCrLf & vbTab &
                "- Log identifier: {3}" & vbCrLf & vbTab &
                "- ProductArea Separator: {4}" & vbCrLf & vbTab &
                "- New email processing: {5}",
                workingModeArg, logConnectionString, logFolderArg, logIdentifierArg, productAreaSepArg, bEnableNewEmailProcessing), 1)

            'start processing
            ReadMailBox(sConnectionstring, workingMode)

        Catch ex As Exception
            LogError(ex.ToString(), Nothing)
        Finally
            closeLogFiles()
        End Try

    End Sub

    Private Function GetCmdArg(args As String(), index As Int32, defaultValue As String) As String
        Dim val As String = defaultValue
        If args.Length > index AndAlso Not IsNullOrEmpty(args(index)) Then
            val = args(index)
        End If
        Return If(String.IsNullOrEmpty(val), "", val)
    End Function

    Private Sub ToggleConfigEncryption(exeConfigName As String)
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
            LogError(ex.ToString(), Nothing)
        End Try
    End Sub

    Private Function FormatConnectionString(connectionString As String) As String
        Dim builder As SqlConnectionStringBuilder = New SqlConnectionStringBuilder(connectionString)
        Return String.Format("Data Source={0}; Initial Catalog={1};Network Library={2}", builder.DataSource, builder.InitialCatalog, builder.NetworkLibrary)
    End Function


    Public Function ReadMailBox(ByVal sConnectionstring As String, ByVal iSyncType As SyncType) As Integer
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCustomerData As New CustomerData
        Dim objPriorityData As New PriorityData
        Dim objCustomer As Customer
        Dim objCaseData As New CaseData
        Dim objUserData As New UserData
        Dim objUserWGData As New WorkingGroupUserData
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

            Dim customers As List(Of Customer)

            If iSyncType = SyncType.SyncByWorkingGroup Then
                customers = objCustomerData.getCustomersByWorkingGroup()
            Else
                customers = objCustomerData.getCustomers()
            End If

            Dim iCustomerCount = 0

            For Each objCustomer In customers

                iCustomerCount += 1

                ' Filter customers based on diagnostic param
                If (customersFilter.Any() AndAlso Not customersFilter.Contains(objCustomer.Id)) Then
                    Continue For
                End If

                ' Filter workging groups based on diagnostic param
                If (iSyncType = SyncType.SyncByWorkingGroup AndAlso workingGroupsFilter.Any() AndAlso Not workingGroupsFilter.Contains(objCustomer.DefaultWorkingGroup_Id)) Then
                    Continue For
                End If

                Dim iPop3DebugLevel = objCustomer.POP3DebugLevel

                ' Override log level with diagnostic param
                If iLogLevelOverride IsNot Nothing AndAlso iLogLevelOverride > 0 Then
                    iPop3DebugLevel = iLogLevelOverride
                End If

                giLoglevel = iPop3DebugLevel

                If objCustomer.PhysicalFilePath = "" Then
                    objCustomer.PhysicalFilePath = objGlobalSettings.AttachedFileFolder
                End If

                If Not IsNullOrEmpty(objCustomer.POP3Server) AndAlso Not IsNullOrEmpty(objCustomer.POP3UserName) Then

                    LogToFile("M2T for " & objCustomer.Name & ", Nr: " & iCustomerCount & "(" & customers.Count & "), appVersion: " & CurrentAssemblyInfo.Version, iPop3DebugLevel)

                    Dim mails As List(Of MailMessage) = Nothing
                    Try
                        ' hämta inställningar om e-post texten ska översättas till fält på ärendet
                        Dim fieldsToUpdate As Dictionary(Of String, String)
                        fieldsToUpdate = objCustomerData.GetCaseFieldsSettings(objCustomer.Id)

                        ' hämta alla avdelningar på kunden
                        Dim colDepartment As Collection
                        colDepartment = objDepartmentData.getDepartments(objCustomer.Id)

                        ' hämta alla prioroteter på kunden
                        Dim colPrio As Collection
                        colPrio = objPriorityData.GetPriorityByCustomerId(objCustomer.Id)

                        eMailConnectionType = objCustomer.MailServerProtocol

                        Dim ip As String = ""

                        If objCustomer.UseEws Then
                            If IsNullOrEmpty(objCustomer.EMailFolder) Then
                                objCustomer.EMailFolder = InboxMailFolderName 'Set Default To inbox If NULL
                            End If

                            'Dim maxAttempt As Integer = 5
                            Dim maxAttempt As Integer = 5 ' GetAppSettingValue("MaxAttempt")
                            Dim icount As Integer = 0
                            Dim task As Task(Of List(Of MailMessage))

                            For i As Integer = maxAttempt To 0 Step -1

                                Try

                                    task = ReadEwsFolderAsync(objCustomer, objCustomer.POP3Server,
                                                                                              objCustomer.POP3Port,
                                                                                              objCustomer.POP3UserName,
                                                                                              objCustomer.EMailFolder,
                                                                                              objCustomer.EMailFolderArchive,
                                                                                              objCustomer.EwsApplicationId,
                                                                                              objCustomer.EwsClientSecret,
                                                                                              objCustomer.EwsTenantId,
                                                                                              objCustomer.PhysicalFilePath)

                                    'Successful Quit
                                    task.Wait()
                                    mails = task.Result
                                    Exit For

                                Catch ex As Exception
                                    icount = icount + 1
                                    If icount = maxAttempt Then
                                        'LogError("Error readMailBox: " & ex.Message.ToString, Nothing)
                                        'rethrow
                                        Throw
                                    End If
                                    Thread.Sleep(2000)

                                End Try
                            Next


                            'Dim task As Task(Of List(Of MailMessage)) = ReadEwsFolderAsync(objCustomer, objCustomer.POP3Server,
                            '                                                          objCustomer.POP3Port,
                            '                                                          objCustomer.POP3UserName,
                            '                                                          objCustomer.EMailFolder,
                            '                                                          objCustomer.EMailFolderArchive,
                            '                                                          objCustomer.EwsApplicationId,
                            '                                                          objCustomer.EwsClientSecret,
                            '                                                          objCustomer.EwsTenantId,
                            '                                                          objCustomer.PhysicalFilePath)

                            'task.Wait()

                            'mails = task.Result
                            iListCount = mails.Count()
                        Else
                            eMailConnectionType = MailConnectionType.Imap
                            IMAPclient = New Imap()

                            ' Enable IMAPI Client Logging
                            If bEnableIMAPIClientLog AndAlso Not IsNullOrEmpty(sIMAPIClientLogPath) Then
                                IMAPclient.LogWriter = New FileLogWriter(sIMAPIClientLogPath, Rebex.LogLevel.Debug)
                            End If

                            Dim host As IPHostEntry = Dns.GetHostEntry(objCustomer.POP3Server)

                            If Not host Is Nothing Then
                                ip = host.AddressList(0).ToString()
                            End If

                            'LogToFile("Connecting to " & objCustomer.POP3Server & " (" & ip & "):" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName, iPop3DebugLevel)

                            If objCustomer.POP3Port = 993 Then
                                IMAPclient.Connect(objCustomer.POP3Server.ToString(), SslMode.Implicit)
                            Else
                                IMAPclient.Connect(objCustomer.POP3Server, objCustomer.POP3Port)
                            End If



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

                            If IsNullOrEmpty(objCustomer.POP3UserName) Or IsNullOrEmpty(objCustomer.POP3Password) Then
                                LogError("Missing UserName Or Password", objCustomer)
                                Exit For
                            ElseIf objCustomer.EMailDefaultCaseType_Id = 0 Then
                                LogError("Missing Default Case Type", objCustomer)
                                Exit For
                            End If

                            If eMailConnectionType = MailConnectionType.Pop3 Then
                                ' Inget stöd för POP3 längre
                                LogError("Pop3 Is Not supported. Mailserver protocol 0", objCustomer)

                                ' ELSE CHECK EWS
                            ElseIf eMailConnectionType = MailConnectionType.Imap Then
                                LogToFile("Login " & objCustomer.POP3UserName, iPop3DebugLevel)
                                IMAPclient.Login(objCustomer.POP3UserName, objCustomer.POP3Password)
                            ElseIf eMailConnectionType = MailConnectionType.Ews Then
                                LogToFile("Login " & objCustomer.POP3UserName, iPop3DebugLevel)
                            End If

                        End If

                        LogToFile("Connecting to " & objCustomer.POP3Server & " (" & ip & "):" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName & ", EWS Mode: " & objCustomer.UseEws, iPop3DebugLevel)


                        If eMailConnectionType = MailConnectionType.Pop3 Then
                            ' Inget stöd för POP3 längre
                            LogError("Pop3 Is Not supported. Mailserver protocol 0", objCustomer)
                        ElseIf eMailConnectionType = MailConnectionType.Imap Then

                            Dim emailFolder As String = "Inbox" ' Default email folder

                            'Override with diagnostic values if exist
                            If Not IsNullOrEmpty(sEmailForlderOverride) Then
                                objCustomer.EMailFolder = sEmailForlderOverride
                                LogToFile($"Email Folder has been overrided to {sEmailForlderOverride}", iPop3DebugLevel)
                            End If

                            If Not IsNullOrEmpty(sEmailForlderArchiveOverride) Then
                                objCustomer.EMailFolderArchive = sEmailForlderArchiveOverride
                                LogToFile($"Email Folder Archive has been overrided to {sEmailForlderArchiveOverride}", iPop3DebugLevel)
                            End If


                            'Validate email folders
                            If Not IsNullOrEmpty(objCustomer.EMailFolder) Then
                                If Not CheckEmailFolderExists(IMAPclient, objCustomer.EMailFolder) Then
                                    LogError($"EmailFolder '{objCustomer.EMailFolder}' doesn't exist.", objCustomer)
                                    Exit For
                                Else
                                    emailFolder = objCustomer.EMailFolder
                                End If
                            End If

                            If Not IsNullOrEmpty(objCustomer.EMailFolderArchive) Then
                                If Not CheckEmailFolderExists(IMAPclient, objCustomer.EMailFolderArchive) Then
                                    LogError($"EmailFolderArchive '{objCustomer.EMailFolderArchive}' doesn't exist.", objCustomer)
                                    Exit For
                                End If
                            End If

                            LogToFile("Connecting to '" & emailFolder & "' email folder.", iPop3DebugLevel)
                            IMAPclient.SelectFolder(emailFolder)

                            If Not IMAPclient.CurrentFolder.Name.Equals(emailFolder, StringComparison.OrdinalIgnoreCase) Then
                                LogError($"Failed to connect to '{emailFolder}' email folder", objCustomer)
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


                                Dim objCase As CCase = Nothing

                                If eMailConnectionType = MailConnectionType.Ews Then
                                    message = mails(iListIndex)
                                ElseIf eMailConnectionType = MailConnectionType.Imap Then
                                    sUniqueID = IMAPlist(iListIndex).UniqueId
                                    message = IMAPclient.GetMailMessage(sUniqueID)
                                    message.Silent = True
                                Else
                                    LogError("Pop3 Is Not supported. Mailserver protocol 0", objCustomer)
                                    'Throw New ArgumentException("Email type not supported")
                                    Continue For
                                End If

                                Dim attachedFiles As List(Of MailFile) = New List(Of MailFile)()

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

                                If bEnableNewEmailProcessing Then
                                    'New logic to find existing case by email
                                    Dim messageIds As List(Of String) = ExtractMessageIds(message)
                                    LogToFile(String.Format("MessageIds found: {0}", String.Join(",", messageIds)), objCustomer.POP3DebugLevel)

                                    ' Iterate over all UniqueMessageIds from the email to find matching case
                                    For Each messageId As String In messageIds
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
                                    If Not IsNullOrEmpty(strUserId) Then
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
                                    Try
                                        objCase = objCaseData.createCase(objCase)
                                    Catch ex As Exception
                                        LogError("Error creating Case in database: " & ex.Message.ToString, objCustomer)
                                        Continue For
                                    End Try

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
                                    Dim sHTMLFileName As String = CreateHtmlFileFromMail(objCustomer, message, objCustomer.PhysicalFilePath & "\" & objCase.Casenumber, objCase.Casenumber)
                                    Dim sPDFFileName As String = CreatePdfFileFromMail(objCustomer, message, objCustomer.PhysicalFilePath & "\" & objCase.Casenumber, objCase.Casenumber)

                                    If Not IsNullOrEmpty(sPDFFileName) Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen                                       
                                        objCaseData.saveFileInfo(objCase.Id, "Mail/" & sPDFFileName)
                                    End If

                                    If Not IsNullOrEmpty(sHTMLFileName) Then
                                        'iHTMLFile = 1

                                        ' Lägg in i databasen
                                        'objCaseData.saveFileInfo(objCase.Id, "html/" & sHTMLFileName)

                                        DeleteFilesInsideFolder(objCustomer.PhysicalFilePath & "\" & objCase.Casenumber & "\html", True)
                                    End If



                                    'Attached files processing for Case
                                    Dim caseFiles As List(Of String) = ProcessMessageAttachments(message, iHTMLFile, objCustomer, objCase.Casenumber.ToString(), Nothing, iPop3DebugLevel, objGlobalSettings)
                                    If (caseFiles IsNot Nothing AndAlso caseFiles.Any()) Then



                                        For Each caseFilePath As String In caseFiles
                                            Dim sFileName As String = Path.GetFileName(caseFilePath)

                                            objCaseData.saveFileInfo(objCase.Id, sFileName)

                                            'Add to files to attach list
                                            attachedFiles.Add(New MailFile(sFileName, caseFilePath, False))

                                        Next
                                    End If

                                    '#65030
                                    Dim newcaseEmailTo As String = objCase.Persons_EMail
                                    If objCustomer.NewCaseMailTo = 1 Then
                                        newcaseEmailTo = sNewCaseToEmailAddress
                                    End If
                                    '#65030

                                    If isBlockedRecipient(newcaseEmailTo, objCustomer.BlockedEmailRecipients) = False Then
                                        If isValidRecipient(newcaseEmailTo, objCustomer.AllowedEMailRecipients) = True Then
                                            If objCustomer.EMailRegistrationMailID <> 0 And bOrder = False And (message.From.ToString <> message.To.ToString) Then
                                                'If Len(objCase.Persons_EMail) > 6 Then  (objCase.Persons_EMail can be empty) #65030
                                                objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.NewCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                                If Not objMailTemplate Is Nothing Then
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = Guid.NewGuid().ToString
                                                    'helpdesk case 58782, #65030
                                                    'Dim newcaseEmailTo As String = objCase.Persons_EMail
                                                    'If objCustomer.NewCaseMailTo = 1 Then
                                                    '    newcaseEmailTo = sNewCaseToEmailAddress
                                                    'End If
                                                    'helpdesk case 58782
                                                    sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, newcaseEmailTo, objMailTemplate, objGlobalSettings,
                                                                     sMessageId, sEMailLogGUID, sConnectionstring)

                                                    objLogData.createEMailLog(iCaseHistory_Id, newcaseEmailTo, MailTemplates.NewCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                                End If
                                                'End If  #65030
                                            End If
                                        Else
                                            LogToFile("readMailBox, isValidRecipient " & objCase.Persons_EMail & ", " & objCustomer.AllowedEMailRecipients, iPop3DebugLevel)
                                        End If
                                    Else
                                        LogToFile("readMailBox, isBlockedRecipient " & objCase.Persons_EMail & ", " & objCustomer.BlockedEmailRecipients, iPop3DebugLevel)
                                    End If

                                    If objCustomer.EMailRegistrationMailID <> 0 And objCustomer.NewCaseEMailList <> "" Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.NewCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim vNewCaseEmailList() As String = objCustomer.NewCaseEMailList.Split(";")

                                            For Index As Integer = 0 To vNewCaseEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                                sRet_SendMail =
                                                objMail.sendMail(objCase, Nothing, objCustomer, vNewCaseEmailList(Index), objMailTemplate, objGlobalSettings,
                                                                 sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vNewCaseEmailList(Index), MailTemplates.NewCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If

                                    If objCase.Performer_User_Id <> 0 Then
                                        Dim objUser As User = objUserData.getUserById(objCase.Performer_User_Id)

                                        If Not objUser Is Nothing Then
                                            If objUser.AllocateCaseMail = 1 And Len(objUser.EMail) > 6 Then
                                                objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.AssignedCaseToUser, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                                If Not objMailTemplate Is Nothing Then
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                                    sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId,
                                                                     sEMailLogGUID, sConnectionstring)

                                                    objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, MailTemplates.AssignedCaseToUser, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                                End If
                                            End If
                                        End If
                                    End If

                                    Dim workingGroupId As Integer = objCase.WorkingGroup_Id
                                    Dim workingGroupEMail As String = objCase.WorkingGroupEMail
                                    Dim workingGroupAllocateCaseMail As Integer = objCase.WorkingGroupAllocateCaseMail

                                    If objCustomer.CaseWorkingGroupSource = 0 Then
                                        workingGroupId = objCase.PerformerWorkingGroup_Id
                                        workingGroupEMail = objCase.PerformerWorkingGroupEMail
                                        workingGroupAllocateCaseMail = objCase.PerformerWorkingGroupAllocateCaseMail
                                    End If

                                    If workingGroupId <> 0 And workingGroupAllocateCaseMail = 1 Then
                                        Dim emailsList As List(Of String) = New List(Of String)()

                                        If Not IsNullOrEmpty(workingGroupEMail) Then
                                            emailsList = workingGroupEMail.Split(New Char() {";"c, ","c}).ToList()
                                        Else
                                            Dim users As List(Of WorkingGroupUser) = objUserWGData.getWorkgroupUsers(workingGroupId)
                                            Dim usersDepartments As List(Of KeyValuePair(Of Integer, Integer)) = New List(Of KeyValuePair(Of Integer, Integer))
                                            If Not objCase.Department_Id = 0 Then
                                                usersDepartments = objDepartmentData.getUserDepartmentsIds(users.Select(Function(x) x.Id).ToArray())
                                            End If

                                            For Each user As WorkingGroupUser In users
                                                If user.AllocateCaseMail = 1 And Not String.IsNullOrWhiteSpace(user.EMail) And
                                               user.Status = 1 And user.WorkingGroupUserRole = WorkingGroupUserPermission.ADMINSTRATOR Then
                                                    If Not objCase.Department_Id = 0 And usersDepartments.Any(Function(ud) ud.Key = user.Id) Then
                                                        If usersDepartments.Any(Function(ud) ud.Key = user.Id And ud.Value = objCase.Department_Id) Then
                                                            emailsList.Add(user.EMail)
                                                        End If
                                                    Else
                                                        emailsList.Add(user.EMail)
                                                    End If
                                                End If
                                            Next
                                        End If

                                        If emailsList.Any() Then
                                            objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.AssignedCaseToWorkinggroup, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)
                                            If Not objMailTemplate Is Nothing Then
                                                For Each recipient As String In emailsList.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).Distinct()
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                                    sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, recipient, objMailTemplate, objGlobalSettings,
                                                                     sMessageId, sEMailLogGUID, sConnectionstring)

                                                    objLogData.createEMailLog(iCaseHistory_Id, recipient, MailTemplates.AssignedCaseToWorkinggroup,
                                                                          sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                                Next
                                            End If
                                        End If
                                    End If

                                    If sPriorityEMailList <> "" Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.AssignedCaseToPriority, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim vPriorityEmailList() As String = sPriorityEMailList.Split(";")

                                            For Index As Integer = 0 To vPriorityEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                                sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, vPriorityEmailList(Index), objMailTemplate, objGlobalSettings,
                                                                     sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vPriorityEmailList(Index), MailTemplates.AssignedCaseToPriority, sMessageId,
                                                                          sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If
                                Else ' Existing case 

                                    ' Spara svaret som en loggpost på aktuellt ärende
                                    ' Ta endast med svaret
                                    sBodyText = extractAnswerFromBody(sBodyText, objCustomer.EMailAnswerSeparator)

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

                                    Dim isInternalLogUsed As Boolean = CheckInternalLogConditions(iMailID, objCustomer, sFromEMailAddress, sToEMailAddress)

                                    ' Save Logs (Logga händelsen)
                                    If isInternalLogUsed Then
                                        ' Save as Internal Log (Lägg in som intern loggpost)
                                        iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, sBodyText, "", 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                    Else
                                        iLog_Id = objLogData.createLog(objCase.Id, objCase.Persons_EMail, "", sBodyText, 0, sFromEMailAddress, iCaseHistory_Id, iFinishingCause_Id)
                                    End If

                                    Dim isTwoAttachmentsActive As Boolean = CheckIfTwoAttachmentsModeEnabled(objCaseData, objCustomer.Id)
                                    Dim bIsInternalLogFile = isInternalLogUsed AndAlso isTwoAttachmentsActive ' Mark file as internal only 2attachments is enabled
                                    Dim logSubFolderPrefix = If(bIsInternalLogFile, "LL", "L") ' LL - Internal log subfolder, L - external log subfolder

                                    Dim sHTMLFileName As String = CreateHtmlFileFromMail(objCustomer, message, Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id), objCase.Casenumber)
                                    Dim sPDFFileName As String = CreatePdfFileFromMail(objCustomer, message, Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id), objCase.Casenumber)

                                    If Not IsNullOrEmpty(sPDFFileName) Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen
                                        objLogData.saveFileInfo(iLog_Id, "Mail/" & sPDFFileName, bIsInternalLogFile)
                                    End If

                                    If Not IsNullOrEmpty(sHTMLFileName) Then
                                        'iHTMLFile = 1

                                        ' Lägg in i databasen
                                        'objCaseData.saveFileInfo(objCase.Id, "html/" & sHTMLFileName)

                                        DeleteFilesInsideFolder(Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id) & "\html", True)
                                    End If

                                    ' Process attached log files 
                                    Dim logFiles As List(Of String) = ProcessMessageAttachments(message, iHTMLFile, objCustomer, iLog_Id.ToString(), logSubFolderPrefix, iPop3DebugLevel, objGlobalSettings)
                                    If (logFiles IsNot Nothing AndAlso logFiles.Any()) Then
                                        For Each logFilePath As String In logFiles
                                            Dim sFileName = Path.GetFileName(logFilePath)
                                            objLogData.saveFileInfo(iLog_Id, sFileName, bIsInternalLogFile)

                                            'add file to files attachment list
                                            attachedFiles.Add(New MailFile(sFileName, logFilePath, bIsInternalLogFile))
                                        Next
                                    End If

                                    ' Meddela handläggaren att ärendet uppdaterat / send case was updated notification 
                                    If objCase.ExternalUpdateMail = 1 And Len(objCase.PerformerEMail) > 6 Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.CaseIsUpdated, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim objMail As New Mail
                                            Dim objLog As New Log

                                            ' Set appropriate log text property
                                            objLog.Text_External = If(Not isInternalLogUsed, sBodyText, String.Empty)
                                            objLog.Text_Internal = If(isInternalLogUsed, sBodyText, String.Empty)

                                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                            sSendTime = Date.Now()

                                            Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                            sRet_SendMail =
                                                objMail.sendMail(objCase, objLog, objCustomer, objCase.PerformerEMail, objMailTemplate, objGlobalSettings,
                                                                 sMessageId, sEMailLogGUID, sConnectionstring, attachedFiles)

                                            objLogData.createEMailLog(iCaseHistory_Id, objCase.PerformerEMail, MailTemplates.CaseIsUpdated, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                        End If
                                    ElseIf iFinishingCause_Id <> 0 And Len(objCase.Persons_EMail) > 6 Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.ClosedCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If Not objMailTemplate Is Nothing Then
                                            Dim objMail As New Mail
                                            Dim objLog As New Log

                                            ' Set appropriate log text property
                                            objLog.Text_External = If(Not isInternalLogUsed, sBodyText, String.Empty)
                                            objLog.Text_Internal = If(isInternalLogUsed, sBodyText, String.Empty)

                                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                            sSendTime = Date.Now()

                                            Dim sEMailLogGUID As String = Guid.NewGuid().ToString

                                            sRet_SendMail =
                                                objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings,
                                                                 sMessageId, sEMailLogGUID, sConnectionstring, attachedFiles)

                                            objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, MailTemplates.ClosedCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

                                        End If
                                    End If
                                End If

                                'here tit
                                ' spara e-post adresser
                                If message IsNot Nothing AndAlso objCase IsNot Nothing Then
                                    Dim messageId As String = message.MessageId.ToString()
                                    objMailTicket.Save(objCase.Id, iLog_Id, "to", message.To.ToString(), message.Subject, messageId) ' Saving subject only for to address...
                                    objMailTicket.Save(objCase.Id, iLog_Id, "cc", message.CC.ToString(), Nothing, messageId)
                                    objMailTicket.Save(objCase.Id, iLog_Id, "bcc", message.Bcc.ToString(), Nothing, messageId)
                                End If

                                If eMailConnectionType = MailConnectionType.Pop3 Then
                                    ' Inget stöd för POP3 längre
                                ElseIf eMailConnectionType = MailConnectionType.Imap Then

                                    If Not IsNullOrEmpty(objCustomer.EMailFolderArchive) Then
                                        LogToFile("Move Message to: " & objCustomer.EMailFolderArchive, iPop3DebugLevel)
                                        IMAPclient.CopyMessage(sUniqueID, objCustomer.EMailFolderArchive)
                                    End If

                                    LogToFile("Deleting Message", iPop3DebugLevel)
                                    IMAPclient.DeleteMessage(sUniqueID)

                                    ' Purge to apply message delete otherwise the message will stay in Inbox
                                    IMAPclient.Purge()
                                ElseIf eMailConnectionType = MailConnectionType.Ews Then
                                    ' Copy mail if archieve, ' delete mail
                                    DeleteEwsMail(message, objCustomer,
                                          objCustomer.POP3Server,
                                          objCustomer.POP3Port,
                                          objCustomer.POP3UserName,
                                          objCustomer.EMailFolder,
                                          objCustomer.EMailFolderArchive,
                                          objCustomer.EwsApplicationId,
                                          objCustomer.EwsClientSecret,
                                          objCustomer.EwsTenantId).Wait()
                                End If
                            Next
                        End If


                    Catch ex As Exception
                        LogError("Error readMailBox." & objCustomer.POP3UserName & "  Error: " & ex.ToString(), objCustomer)
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
            LogError("Error readMailBox: " & ex.Message.ToString, Nothing)
            'rethrow
            Throw
        End Try
    End Function


    Private Async Function DeleteEwsMail(message As EwsMailMessage, objCustomer As Customer, server As String, port As Integer, userName As String, emailFolder As String, emailArchiveFolder As String, applicationId As String, clientSecret As String, tenantId As String) As System.Threading.Tasks.Task
        'emailFolder = "Inbox"
        'emailArchiveFolder = "Archive/M2T_test"
        Dim ewsScopes As String() = New String() {"https://outlook.office.com/.default"}
        Dim app As IConfidentialClientApplication = ConfidentialClientApplicationBuilder.Create(applicationId).WithAuthority(AzureCloudInstance.AzurePublic, tenantId).WithClientSecret(clientSecret).Build()

        Dim task As Task(Of AuthenticationResult) = app.AcquireTokenForClient(ewsScopes).ExecuteAsync()
        Dim result As AuthenticationResult = Await task

        Dim service As ExchangeService = New ExchangeService()
        ' TODO: Port? Maybe not
        service.Url = New Uri(server)
        service.Credentials = New OAuthCredentials(result.AccessToken)
        service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, userName)

        If (Not String.IsNullOrWhiteSpace(emailArchiveFolder)) Then

            Dim archive As Folder = FindEwsFolder(objCustomer, emailArchiveFolder, service)

            If (archive Is Nothing) Then
                LogError("Error coping to archive folder. Archive folder not found: " & emailArchiveFolder, objCustomer)
            Else
                Dim ids = New List(Of ItemId)
                ids.Add(message.EwsID)
                service.CopyItems(ids, archive.Id, False)
            End If
        End If

        Dim deleteIds As List(Of ItemId) = New List(Of ItemId)()
        deleteIds.Add(message.EwsID)
        service.DeleteItems(deleteIds,
                            DeleteMode.HardDelete,
                            SendCancellationsMode.SendToNone,
                            AffectedTaskOccurrence.AllOccurrences)

        'service.CopyItems()
    End Function


    Private Async Function ReadEwsFolderAsync(objCustomer As Customer, server As String, port As Integer, userName As String, emailFolder As String, emailArchiveFolder As String,
                                              applicationId As String, clientSecret As String, tenantId As String, temppath As String) As Task(Of List(Of MailMessage))
        'emailFolder = "Inkorg/M2T_test"
        'emailArchiveFolder = "Arkiv/M2T_test"
        Dim ewsScopes As String() = New String() {"https://outlook.office.com/.default"}
        Dim app As IConfidentialClientApplication = ConfidentialClientApplicationBuilder.Create(applicationId).WithAuthority(AzureCloudInstance.AzurePublic, tenantId).WithClientSecret(clientSecret).Build()

        Dim task As Task(Of AuthenticationResult) = app.AcquireTokenForClient(ewsScopes).ExecuteAsync()
        Dim result As AuthenticationResult = Await task

        Dim service As ExchangeService = New ExchangeService()
        ' TODO: Port? Maybe not
        service.Url = New Uri(server)
        service.Credentials = New OAuthCredentials(result.AccessToken)
        service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, userName)

        Dim inbox As Folder
        If emailFolder.Equals(InboxMailFolderName, StringComparison.InvariantCultureIgnoreCase) Then
            'Dim folderId As FolderId = New FolderId(WellKnownFolderName.MsgFolderRoot, userName)
            inbox = Folder.Bind(service, WellKnownFolderName.Inbox)
        Else
            inbox = FindEwsFolder(objCustomer, emailFolder, service)
        End If

        If inbox Is Nothing Then
            LogError($"EmailFolder '{emailFolder}' doesn't exist.", objCustomer)
            Return Nothing
        End If

        If Not String.IsNullOrWhiteSpace(emailArchiveFolder) Then
            If FindEwsFolder(objCustomer, emailArchiveFolder, service) Is Nothing Then
                LogError($"EmailFolderArchive '{emailArchiveFolder}' doesn't exist.", objCustomer)
                Return Nothing
            End If
        End If

        Dim items As FindItemsResults(Of Item) = inbox.FindItems(New ItemView(10000))
        Dim messages As List(Of MailMessage) = New List(Of MailMessage)()

        If items Is Nothing Or Not items.Any() Then
            Return messages
        End If

        Dim response = service.LoadPropertiesForItems(items, New PropertySet(EmailMessageSchema.From,
                                                                                                  EmailMessageSchema.ToRecipients,
                                                                                                  EmailMessageSchema.CcRecipients,
                                                                                                  EmailMessageSchema.BccRecipients,
                                                                                                  ItemSchema.Body,
                                                                                                  ItemSchema.Subject,
                                                                                                  ItemSchema.Attachments,
                                                                                                  EmailMessageSchema.InternetMessageId,
                                                                                                  EmailMessageSchema.InReplyTo,
                                                                                                  ItemSchema.DateTimeReceived))

        For Each item As Item In items
            If item.GetType() Is GetType(EmailMessage) Then
                Dim mail As EmailMessage = item
                If String.IsNullOrWhiteSpace(mail.Subject) Then
                    mail.Subject = ""
                    'Continue For
                End If
                If String.IsNullOrWhiteSpace(mail.Body) Then
                    mail.Body = ""
                    'Continue For
                End If
                Dim message As EwsMailMessage = New EwsMailMessage()
                message.MessageId = New MessageId(mail.InternetMessageId)
                message.EwsID = mail.Id
                message.From.Add(New MailAddress(mail.From.Address, mail.From.Name))
                If (mail.InReplyTo IsNot Nothing) Then
                    message.InReplyTo.Add(New MessageId(mail.InReplyTo))
                End If

                For Each recpt As MailAddress In (From r As EmailAddress In mail.ToRecipients
                                                  Select New MailAddress(r.Address, r.Name))
                    message.To.Add(recpt)
                Next

                For Each recpt As MailAddress In (From r As EmailAddress In mail.CcRecipients
                                                  Select New MailAddress(r.Address, r.Name))
                    message.CC.Add(recpt)
                Next

                message.Subject = mail.Subject
                If mail.Body.BodyType = BodyType.HTML Then
                    message.BodyHtml = mail.Body.Text
                Else
                    message.BodyText = mail.Body.Text
                End If
                'For Each attach As Microsoft.Exchange.WebServices.Data.Attachment In mail.Attachments
                '    Dim byteArray(attach.Size) As Byte
                '    Dim newAttachment As Rebex.Mail.Attachment = New Rebex.Mail.Attachment(attach.Size, attach.Name, attach.ContentType.ToString())
                '    Console.WriteLine(newAttachment.GetType())

                'Next
                If mail.Attachments.Any() Then
                    For Each attach As Microsoft.Exchange.WebServices.Data.Attachment In mail.Attachments
                        If attach.GetType() = GetType(FileAttachment) Then
                            Try
                                attach.Load()
                            Catch ex As Exception
                                LogError("Error loading attachment: " & ex.Message.ToString, objCustomer)
                                Continue For
                            End Try

                            Dim fileAttach As FileAttachment = attach
                            If Not attach.IsInline Then
                                Dim newAttachment As Rebex.Mail.Attachment = New Rebex.Mail.Attachment(New MemoryStream(fileAttach.Content), fileAttach.Name)
                                message.Attachments.Add(newAttachment)
                            Else
                                Dim newResource As LinkedResource = New LinkedResource(New MemoryStream(fileAttach.Content), fileAttach.Name, fileAttach.ContentType)
                                If Not fileAttach.ContentId Is Nothing Then
                                    newResource.ContentId = fileAttach.ContentId
                                End If
                                If Not fileAttach.ContentLocation Is Nothing Then
                                    newResource.ContentLocation = fileAttach.ContentLocation
                                End If
                                message.Resources.Add(newResource)
                            End If

                        End If
                        If attach.GetType() = GetType(ItemAttachment) Then
                            Try


                                Dim itemAttachment As ItemAttachment = attach
                                itemAttachment.Load(ItemSchema.MimeContent)
                                'Dim fileName As String = "C:\\Temp\\" + itemAttachment.Item.Subject + ".eml"
                                Dim fileName As String = temppath & "\" & itemAttachment.Item.Subject.Replace(":", "").Replace(",", "").Replace("?", "").Replace(" ", "").Replace("/", "-") + ".eml"
                                '// Write the bytes of the attachment into a file.
                                File.WriteAllBytes(fileName, itemAttachment.Item.MimeContent.Content)
                                'message.Attachments.Add(New Rebex.Mail.Attachment("C:\\Temp\\" + itemAttachment.Item.Subject + ".eml"))
                                message.Attachments.Add(New Rebex.Mail.Attachment(fileName))

                                System.IO.File.Delete(fileName)




                                'attach.Load()
                                ''Todo - Save itemAttach?




                                'Dim itemAttach As ItemAttachment = attach
                                'Dim byteArray(itemAttach.Size) As Byte
                                'Dim newAttachment As Rebex.Mail.Attachment = New Rebex.Mail.Attachment(New MemoryStream(byteArray), itemAttach.Name.Replace(":", "-").Replace("vbCrLf", " "))
                                ''newAttachment.FileName = newAttachment.FileName & ".eml"


                                ''Dim attach1 As Microsoft.Exchange.WebServices.Data.Attachment = attach
                                'message.Attachments.Add(newAttachment)
                            Catch ex As Exception
                                LogError("Error loading attachment: " & ex.Message.ToString, objCustomer)
                                Continue For

                            End Try


                            'TODO - save And add it to the message
                            ' Dim newResource As LinkedResource = New LinkedResource(New MemoryStream(itemAttach.ContentType, itemAttach.Name))


                        End If
                    Next
                End If

                messages.Add(message)
            End If
        Next

        Return messages
    End Function

    Private Function FindEwsFolder(objCustomer As Customer, emailFolder As String, service As ExchangeService) As Folder
        Dim emailFolders As String()
        If emailFolder.IndexOf("/"c) >= 0 Then
            emailFolders = emailFolder.Split("/"c)
        Else
            emailFolders = New String() {emailFolder}
        End If

        Dim folders As FindFoldersResults
        Dim folder As Folder = Nothing
        For Each currentFolderName As String In emailFolders
            If folder Is Nothing Then
                folders = service.FindFolders(WellKnownFolderName.MsgFolderRoot, New FolderView(100))
            Else
                folders = service.FindFolders(folder.Id, New FolderView(100))
            End If
            folder = folders.FirstOrDefault(Function(f) f.DisplayName.Equals(currentFolderName, StringComparison.InvariantCultureIgnoreCase))
            If folder Is Nothing Then
                LogError("Can't find folder: " & currentFolderName, objCustomer)
                Exit For
            End If
        Next

        If folder Is Nothing Then
            LogError("Can't find folder: " & emailFolder, objCustomer)
            Return Nothing
        End If

        Return folder
    End Function

    Private Function CheckIfTwoAttachmentsModeEnabled(objCaseData As CaseData, iCustomerID As Integer) As Boolean
        Dim sFieldName = "tblLog.FileName_Internal"
        Dim res = objCaseData.CheckCaseField(iCustomerID, sFieldName)
        Return res
    End Function

    Private Function CheckInternalLogConditions(iMailID As Int32, objCustomer As Customer, sFromEmail As String, sToEmail As String)

        If Not IsNullOrEmpty(gsInternalLogIdentifier) AndAlso (InStr(sFromEmail, gsInternalLogIdentifier) > 0 OrElse InStr(sToEmail, gsInternalLogIdentifier) > 0) Then
            Return True
        End If

        If objCustomer.DefaultEmailLogDestination = 1 AndAlso iMailID = 0 Then
            Return True
        End If

        If iMailID = MailTemplates.AssignedCaseToUser OrElse iMailID = MailTemplates.InternalLogNote OrElse
           iMailID = MailTemplates.AssignedCaseToWorkinggroup OrElse iMailID = MailTemplates.CaseIsUpdated Then
            Return True
        End If

        Return False

    End Function


    Function BuildFilePath(ParamArray args() As String) As String
        Dim filePath As String = ""
        For Each token As String In args
            If Not IsNullOrEmpty(token) Then
                filePath = Path.Combine(filePath, token.TrimEnd("\")) ' token may start with network path - should keep it. ex: \\machine\
            End If
        Next
        Return filePath
    End Function

    Private Function GetFileUploadWhiteList(globalSettings As GlobalSettings) As List(Of String)
        Dim whiteListStr As String = globalSettings.FileUploadExtensionWhitelist
        Dim whiteList As List(Of String) = Nothing
        If (whiteListStr IsNot Nothing) Then
            whiteList = whiteListStr.Split(";").ToList()
        End If

        Return whiteList
    End Function

    Private Function IsExtensionInWhiteList(extension As String, whiteList As List(Of String)) As Boolean
        Dim okFile As Boolean = True
        If (whiteList IsNot Nothing) Then
            If (Not whiteList.Contains(extension)) Then
                okFile = False
            End If

        End If
        Return okFile
    End Function


    Private Function ProcessMessageAttachments(message As MailMessage,
                                               iHtmlFile As Integer,
                                               objCustomer As Customer,
                                               objectId As String,  'Case.Id or Log.Id
                                               prefix As String, '"<CaseNumber>" for Case file, "L<LogId>" for Log file
                                               iPop3DebugLevel As Integer,
                                               globalSettings As GlobalSettings) As List(Of String)

        Dim files As List(Of String) = New List(Of String)
        Dim deniedFiles As List(Of String) = New List(Of String)

        Dim whiteList As List(Of String) = GetFileUploadWhiteList(globalSettings)

        Dim tempDirPath As String = BuildFilePath(objCustomer.PhysicalFilePath, "Temp", objectId)
        Dim saveDirPath As String = BuildFilePath(objCustomer.PhysicalFilePath, If(IsNullOrEmpty(prefix), objectId, prefix & objectId))

        If message.Attachments.Count > 6 - iHtmlFile Then

            ' Kontrollera om folder existerar
            If Directory.Exists(tempDirPath) = False Then
                Directory.CreateDirectory(tempDirPath)
            End If

            If Directory.Exists(saveDirPath) = False Then
                Directory.CreateDirectory(saveDirPath)
            End If

            'Save to temp dir
            For Each msgAttachment As Rebex.Mail.Attachment In message.Attachments
                Dim sFileNameTemp As String = msgAttachment.FileName
                sFileNameTemp = sFileNameTemp.Replace(":", "")
                sFileNameTemp = URLDecode(sFileNameTemp)

                Dim extension As String = Path.GetExtension(sFileNameTemp).Replace(".", "").ToLower()
                If (IsExtensionInWhiteList(extension, whiteList)) Then
                    ' save temp file
                    Dim sTempFilePath = Path.Combine(tempDirPath, sFileNameTemp)
                    msgAttachment.Save(sTempFilePath)
                Else
                    deniedFiles.Add(sFileNameTemp)
                    LogToFile("Blocked file: " & sFileNameTemp, iPop3DebugLevel)
                End If

            Next

            'zip files 
            Dim sFilePath As String = Path.Combine(saveDirPath, objectId & ".zip")
            createZipFile(tempDirPath, sFilePath)
            LogToFile("Attached file path: " & sFilePath, iPop3DebugLevel)

            If Not IsNullOrEmpty(sFilePath) Then
                'Ta bort tempkatalogen / delete temp dir
                If Directory.Exists(tempDirPath) = True Then
                    Directory.Delete(tempDirPath, True)
                End If
                files.Add(sFilePath)
            End If
        ElseIf message.Attachments.Count > 0 Then

            ' Kontrollera om folder existerar
            If Directory.Exists(saveDirPath) = False Then
                Directory.CreateDirectory(saveDirPath)
            End If

            'Save to temp dir
            For Each msgAttachment As Rebex.Mail.Attachment In message.Attachments
                Dim sFileName As String = msgAttachment.FileName
                sFileName = sFileName.Replace(":", "")
                sFileName = URLDecode(sFileName)

                Dim extension As String = Path.GetExtension(sFileName).Replace(".", "").ToLower()
                If (IsExtensionInWhiteList(extension, whiteList)) Then

                    Dim sFilePath = Path.Combine(saveDirPath, sFileName)
                    LogToFile("Attached file path: " & sFilePath, iPop3DebugLevel)
                    msgAttachment.Save(sFilePath)

                    files.Add(sFilePath)
                Else
                    deniedFiles.Add(sFileName)
                    LogToFile("Blocked file: " & sFileName, iPop3DebugLevel)
                End If

            Next
        End If
        If (deniedFiles.Any()) Then
            Dim deniedFilesContent As String = deniedFiles.Aggregate(Function(o As String, p As String) o & Environment.NewLine & p)
            Dim filePath As String = Path.Combine(saveDirPath, "blocked files.txt")
            File.WriteAllText(filePath, deniedFilesContent)

            files.Add(filePath)

            ' TODO: Save blocked files.txt
        End If
        Return files
    End Function

    Private Function CheckEmailFolderExists(imapClient As Imap, emailFolder As String) As Boolean
        Dim isFolderExists As Boolean = False
        Try
            isFolderExists = imapClient.FolderExists(emailFolder)
        Catch ex As Exception
            isFolderExists = False
        End Try
        Return isFolderExists
    End Function

    Private Function ExtractMessageIds(message As MailMessage) As IList(Of String)
        Dim items As List(Of String) = New List(Of String)

        '1. message's messageId
        items.Add(message.MessageId.ToString())

        '2 replyTo's messageId 
        If message.InReplyTo IsNot Nothing AndAlso message.InReplyTo.Count > 0 Then
            Dim messageId = message.InReplyTo(0).ToString()
            If Not IsNullOrEmpty(messageId) Then
                items.Add(messageId)
            End If
        End If

        ' 3. References messageId
        Dim refHeader As String = GetMessageHeaderValue(message, "References")
        If Not IsNullOrEmpty(refHeader) Then

            Dim refMessageIds As String() = refHeader.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            If (refMessageIds.Any()) Then
                items.AddRange(refMessageIds)
            End If

        End If

        Return items.Distinct().ToList()

    End Function

    Private Function FindCaseByUniqueMessageId(uniqueMessageId As String, customer As Customer) As CCase

        Dim objCase As CCase = Nothing
        Dim objCaseData As CaseData = New CaseData()
        Dim objMailTicket = New Mail2TicketData()

        LogToFile("Find case by messageId: " & uniqueMessageId, customer.POP3DebugLevel)

        'first check if Mail2Ticket has message record 
        Dim logData As Mail2TicketEntity = objMailTicket.GetByMessageId(uniqueMessageId)
        If (logData IsNot Nothing AndAlso logData.CaseId > 0) Then
            objCase = objCaseData.getCase(logData.CaseId)
            If objCase IsNot Nothing Then
                LogToFile(String.Format("Message was found in Mail2Ticket table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)

            End If
        End If

        'then try to find case by messageId in tblEmailLog
        If objCase Is Nothing Then
            objCase = objCaseData.getCaseByMessageID(uniqueMessageId)
            If objCase IsNot Nothing Then
                LogToFile(String.Format("Message was found in tblEmailLog table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)

            End If
        End If

        'then try to find case by messageId among order messageIds
        If objCase Is Nothing AndAlso customer.ModuleOrder = 1 Then
            objCase = objCaseData.getCaseByOrderMessageID(uniqueMessageId)
            If objCase IsNot Nothing Then
                LogToFile(String.Format("Message was found in tblOrderEmailLog table. CaseId: {0}, MessageId: {1}", objCase.Id, uniqueMessageId), customer.POP3DebugLevel)

            End If
        End If
        Return objCase
    End Function



    Private Function GetMessageHeaderValue(message As MailMessage, name As String) As String
        Dim val As String = ""
        Dim mimeHeader As MimeHeader = message.Headers.FirstOrDefault(Function(x) x.Name = name)
        If mimeHeader IsNot Nothing Then
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

        If Not IsNullOrEmpty(strUserId) Then
            ret = objComputerUserData.getComputerUserByUserId(strUserId, iCustomer_Id)
        Else
            If Not IsNullOrEmpty(strEmail) Then
                ret = objComputerUserData.getComputerUserByEMail(strEmail, iCustomer_Id)
            End If
        End If

        If ret Is Nothing Then
            ret = New ComputerUser()
        End If

        For Each d As KeyValuePair(Of String, String) In fields

            If Not IsNullOrEmpty(d.Value) Then

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

    Private Function CreateHtmlFileFromMail(objCustomer As Customer, ByVal message As MailMessage,
                                            ByVal sFolder As String,
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

                'Dim from As String = message.From.ToString
                'Dim fromEmail As String = parseEMailAddress(message.From.ToString())
                'Dim sent As String = message.Date.ToString
                'Dim reciepent As String = message.To.ToString
                'Dim reciepentEmail As String = parseEMailAddress(message.To.ToString())
                'Dim Cc As String = message.CC.ToString
                'Dim subject As String = message.Subject
                'Dim mh As MimeHeaderCollection = message.Headers


                'Dim headerHtml As String = "<!DOCTYPE html><html><body style=" & "font-family: 'Times New Roman'; font-size: 18px" & ">" &
                '                            "<p><strong>From:</strong> " & from & "&#60;" & fromEmail & "&#62;" & "</p>" &
                '                            "<p><strong>Sent:</strong> " & sent & "</p>" &
                '                            "<p><strong>To:</strong> " & reciepent & "&#60;" & reciepentEmail & "&#62;" & "</p>" &
                '                            "<p><strong>Cc:</strong> " & Cc & "</p>" &
                '                            "<p><strong>Subject:</strong> " & subject & "</p>" &
                '                           "</body></html>"

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
                    Dim resCol As LinkedResourceCollection = message.Resources

                    For k As Integer = 0 To resCol.Count - 1
                        Dim res As LinkedResource = resCol(k)

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

                                sContentLocation = IIf(String.IsNullOrWhiteSpace(res.ContentLocation), String.Empty, res.ContentLocation)
                                sBodyHtml = sBodyHtml.Replace(sContentLocation, iFileCount & "." & sFileExtension)
                            End If
                        End If
                    Next

                    Dim objFile As StreamWriter
                    'Dim objHeaderFile As StreamWriter

                    objFile = New StreamWriter(sFolder & "\html\" & sFileName, False, UnicodeEncoding.Unicode)
                    objFile.Write(sBodyHtml)
                    objFile.Close()

                    'objHeaderFile = New StreamWriter(sFolder & "\html\" & "HeaderFile.htm", False, System.Text.UnicodeEncoding.Unicode)
                    'objHeaderFile.Write(headerHtml)
                    'objHeaderFile.Close()

                End If
            End If

        Catch ex As Exception
            LogError("Error createHtmlFileFromMail MediaType: " & sMediaType & ", " & ex.Message.ToString, objCustomer)

            'Rethrow
            Throw
        End Try

        Return sFileName
    End Function

    Private Function CreatePdfFileFromMail(objCustomer As Customer, ByVal message As MailMessage,
                                            ByVal sFolder As String,
                                            ByVal sCaseNumber As String) As String

        Dim sBodyHtml As String = ""
        Dim sFileName As String = ""
        Dim pdfFileName As String = ""
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

                    If Directory.Exists(sFolder & "\Mail") = False Then
                        Directory.CreateDirectory(sFolder & "\Mail")
                    End If

                    ' Skapa fil                   
                    pdfFileName = sCaseNumber & ".pdf"

                    'Winovative

                    Dim htmlToPdfConverter As New HtmlToPdfConverter()

                    ' Set license key received after purchase to use the converter in licensed mode
                    ' Leave it not set to use the converter in demo mode
                    'htmlToPdfConverter.LicenseKey = "xUtbSltKWkpbW0RaSllbRFtYRFNTU1M="
                    htmlToPdfConverter.LicenseKey = "K6W2pLWksra3tqS1saq0pLe1qrW2qr29vb2ktA=="

                    ' Set PDF page size which can be a predefined size like A4 or a custom size in points 
                    ' Leave it not set to have a default A4 PDF page
                    'htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = SelectedPdfPageSize()
                    ' Enable header in the generated PDF document
                    'htmlToPdfConverter.PdfDocumentOptions.ShowHeader = True

                    ' Optionally add a space between header and the page body
                    ' The spacing for first page and the subsequent pages can be set independently
                    ' Leave this option not set for no spacing
                    'htmlToPdfConverter.PdfDocumentOptions.Y = Single.Parse(5)
                    'htmlToPdfConverter.PdfDocumentOptions.TopSpacing = Single.Parse(0)


                    'DrawHeader(sFolder & "\html\" & "HeaderFile.htm", htmlToPdfConverter, True)

                    ' Convert HTML to PDF using the settings above
                    Dim outPdfFile As String = sFolder & "\Mail\" & sCaseNumber & ".pdf"
                    Try

                        Dim url As String = sFolder & "\html\" & sCaseNumber & ".htm"

                        ' Convert the HTML page given by an URL to a PDF document in a memory buffer
                        Dim outPdfBuffer() As Byte = htmlToPdfConverter.ConvertUrl(url)

                        ' Write the memory buffer in a PDF file
                        File.WriteAllBytes(outPdfFile, outPdfBuffer)

                    Catch ex As Exception
                        ' The HTML to PDF conversion failed                       
                        LogError(String.Format("HTML to PDF Error. {0}", ex.Message), objCustomer)
                    End Try

                    ' Open the created PDF document in default PDF viewer
                    'Try
                    '    Process.Start(outPdfFile)
                    'Catch ex As Exception
                    '    LogError(String.Format("Cannot open created PDF file '{0}'. {1}", outPdfFile, ex.Message))
                    'End Try
                End If
            End If

        Catch ex As Exception
            LogError("Error createPDFFileFromMail MediaType: " & sMediaType & ", " & ex.Message.ToString, objCustomer)

            'Rethrow
            Throw
        End Try

        'delete html Directory htm file
        'DeleteFilesInsideFolder(sFolder & "\html", True)

        Return pdfFileName
    End Function
    Sub DeleteFilesInsideFolder(ByVal target_folder_path As String, ByVal also_delete_sub_folders As Boolean)

        ' loop through each file in the target directory
        For Each file_path As String In Directory.GetFiles(target_folder_path)

            ' delete the file if possible...otherwise skip it
            Try
                File.Delete(file_path)
            Catch ex As Exception

            End Try

        Next


        ' if sub-folders should be deleted
        If also_delete_sub_folders Then

            ' loop through each file in the target directory
            For Each sub_folder_path As String In Directory.GetDirectories(target_folder_path)

                ' delete the sub-folder if possible...otherwise skip it
                Try
                    Directory.Delete(sub_folder_path, also_delete_sub_folders)
                Catch ex As Exception

                End Try

            Next
            ' delete the Target-folder if possible...otherwise skip it
            Try
                Directory.Delete(target_folder_path, also_delete_sub_folders)
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub openLogFile()

        If objLogFile IsNot Nothing Then
            Return
        End If

        Dim sLogFolderPath As String

        If Not IsNullOrEmpty(gsLogPath) Then
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
    Private Sub openErrorLogFile()

        If objErrorLogFile IsNot Nothing Then
            Return
        End If

        Dim sLogFolderPath As String

        If Not IsNullOrEmpty(gsLogPath) Then
            sLogFolderPath = gsLogPath
        Else
            sLogFolderPath = Path.Combine(Environment.CurrentDirectory, "log")
        End If

        If Not Directory.Exists(sLogFolderPath) Then
            Directory.CreateDirectory(sLogFolderPath)
        End If

        Dim sFileName = "DH_Helpdesk_Mail_Error_" & DatePart(DateInterval.Year, Now()) &
                        DatePart(DateInterval.Month, Now()).ToString().PadLeft(2, "0") &
                        DatePart(DateInterval.Day, Now()).ToString().PadLeft(2, "0") &
                        ".log"
        Dim sFilePath = Path.Combine(sLogFolderPath, sFileName)
        objErrorLogFile = New StreamWriter(sFilePath, True)

    End Sub
    Private Sub DrawHeader(htmlHeader As String, ByVal htmlToPdfConverter As HtmlToPdfConverter, ByVal drawHeaderLine As Boolean)
        Dim headerHtmlUrl As String = htmlHeader

        ' Set the header height in points
        htmlToPdfConverter.PdfHeaderOptions.HeaderHeight = 60

        ' Set header background color
        'htmlToPdfConverter.PdfHeaderOptions.HeaderBackColor = Color.White

        ' Create a HTML element to be added in header
        Dim headerHtml As New HtmlToPdfElement(headerHtmlUrl)

        ' Set the HTML element to fit the container height
        headerHtml.FitHeight = True

        ' Add HTML element to header
        htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtml)

        If drawHeaderLine Then
            ' Calculate the header width based on PDF page size and margins
            Dim headerWidth As Single = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width - htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin

            ' Calculate header height
            Dim headerHeight As Single = htmlToPdfConverter.PdfHeaderOptions.HeaderHeight

            ' Create a line element for the bottom of the header
            Dim headerLine As New LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1)

            ' Set line color
            'headerLine.ForeColor = Color.Gray

            ' Add line element to the bottom of the header
            htmlToPdfConverter.PdfHeaderOptions.AddElement(headerLine)
        End If
    End Sub


    Private Sub closeLogFiles()
        If objLogFile IsNot Nothing Then
            Try
                objLogFile.Close()
            Catch ex As Exception
            Finally
                objLogFile = Nothing
            End Try
        End If
        'New Error logfile
        If objErrorLogFile IsNot Nothing Then
            Try
                objErrorLogFile.Close()
            Catch ex As Exception
            Finally
                objErrorLogFile = Nothing
            End Try
        End If
    End Sub

    Private Sub createZipFile(ByVal sSourceDir As String, ByVal sFileName As String)
        Dim fz As New FastZip
        fz.CreateZip(sFileName, sSourceDir, True, "", "")
        fz = Nothing
    End Sub

    Private Function convertHTMLtoText(ByVal sHTML As String) As String
        Dim startTime As DateTime
        Dim MyWebBrowser As New WebBrowser

        startTime = DateTime.Now()

        MyWebBrowser.DocumentText = sHTML

        MyWebBrowser.ScriptErrorsSuppressed = True

        While (MyWebBrowser.ReadyState <> WebBrowserReadyState.Complete Or MyWebBrowser.IsBusy = True) And DateDiff(DateInterval.Second, startTime, DateTime.Now()) < 10
            Application.DoEvents()
        End While

        Try
            Return (MyWebBrowser.Document.Body.InnerText)
        Catch ex As Exception
            Return sHTML
        End Try
    End Function
    Private Function isBlockedRecipient(sEMail As String, sBlockedEMailRecipents As String) As Boolean
        isBlockedRecipient = False

        If sBlockedEMailRecipents = "" Then
            isBlockedRecipient = False
        Else
            Dim aEMails() As String = sBlockedEMailRecipents.Split(";")

            For i As Integer = 0 To aEMails.Length - 1
                If InStr(sEMail, aEMails(i).ToString, CompareMethod.Text) <> 0 Then

                    isBlockedRecipient = True
                    Exit For
                End If
            Next
        End If
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
            If Not IsNullOrEmpty(mailMessage) Then

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

            If Not IsNullOrEmpty(d.Value) Then
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
                If Not IsNullOrEmpty(d.Value) Then
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
        openLogFile()
        If level > 0 Then
            If objLogFile IsNot Nothing Then
                objLogFile.WriteLine("{0}: {1}", Now(), msg)
            End If
        End If
    End Sub

    Private Sub LogError(msg As String, objCustomer As Customer)
        openErrorLogFile()
        If objErrorLogFile IsNot Nothing Then
            objErrorLogFile.WriteLine("{0}: {1}", Now(), msg)
        End If
        SendErrorMail(msg, objCustomer)
    End Sub

    Private Sub SendErrorMail(msg As String, objCustomer As Customer)
        Try
            Dim smtpServer As String = GetAppSettingValue("DefaultSmtpServer")
            Dim sConnectionstring As String = ConfigurationManager.ConnectionStrings("Helpdesk")?.ConnectionString
            Dim toAddress As String = GetAppSettingValue("ErrorMailTo")
            Dim fromAddress As String = GetAppSettingValue("ErrorMailFrom")
            If (Not IsNullOrEmpty(smtpServer) And objCustomer IsNot Nothing) Then

                Try
                    Dim objMail As New Mail
                    objMail.SendErrorMail(fromAddress, toAddress, "Error in M2T", msg, objCustomer, sConnectionstring, smtpServer)

                Catch ex As Exception
                    If objErrorLogFile IsNot Nothing Then
                        objErrorLogFile.WriteLine("{0}: {1}", Now(), ex.Message)
                    End If
                End Try
            End If
        Catch ex As Exception

        End Try


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

Public Class EwsMailMessage
    Inherits MailMessage

    Private miId As ItemId
    Public Property EwsID() As ItemId
        Get
            Return miId
        End Get
        Set(ByVal value As ItemId)
            miId = value
        End Set
    End Property

End Class


