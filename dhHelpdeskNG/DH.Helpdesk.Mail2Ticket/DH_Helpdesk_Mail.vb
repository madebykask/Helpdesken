﻿Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Windows.Forms
Imports DH.Helpdesk.BusinessData.Enums.Users
Imports DH.Helpdesk.BusinessData.OldComponents.GlobalEnums
Imports DH.Helpdesk.Library
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.VBCSharpBridge.Models
Imports HtmlAgilityPack
Imports Microsoft.Exchange.WebServices.Data
Imports Microsoft.Identity.Client
Imports Rebex
Imports Rebex.Mail
Imports Rebex.Mime
Imports Rebex.Mime.Headers
Imports Rebex.Net
Imports Winnovative
Imports System.IO.Compression
Imports System.IO.Compression.FileSystem
Imports AttachmentCollection = Microsoft.Exchange.WebServices.Data.AttachmentCollection


Module DH_Helpdesk_Mail
    Private Const InboxMailFolderName As String = "Inbox"
    'Dim msDeniedHtmlBodyString As String
    Dim iSequenceNumber As ImapMessageSet
    Dim msDeniedHtmlBodyString As String

    Dim itemattach() As String
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
    Dim tempFolder = "\temp"
    Dim workingModeNumber As Integer


    Public Sub Main()
        Dim processId As Integer = Process.GetCurrentProcess().Id

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


        If aArguments IsNot Nothing Then
            If aArguments.Length > 0 Then
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

        workingModeNumber = CType(workingMode, Integer)

        tempFolder = workingModeNumber.ToString() & "_temp_" & processId.ToString()

        If Not IsNullOrEmpty(logFolderArg) Then
            gsLogPath = logFolderArg
        End If

        If Not IsNullOrEmpty(logIdentifierArg) Then
            gsInternalLogIdentifier = logIdentifierArg.ToString().Trim()
        End If

        If Not IsNullOrEmpty(productAreaSepArg) Then
            gsProductAreaSeperator = productAreaSepArg
        End If

        'Log cmd line args
        Try

            If IsNullOrEmpty(sConnectionstring) Then
                LogError("Connectionstring can not be empty", Nothing)
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
            ReadMailBox(sConnectionstring, workingMode, workingModeArg)

        Catch ex As Exception
            LogError("Error ReadMailBox " & workingModeArg & " " & ex.StackTrace() & " " & ex.ToString(), Nothing)
        Finally

            Try
                Dim objCustomerData As New CustomerData
                Dim iSyncType As SyncType = If(workingMode = SyncType.SyncByWorkingGroup, SyncType.SyncByWorkingGroup, SyncType.SyncByCustomer)
                Dim customers As List(Of Customer)
                Dim objGlobalSettingsData As New GlobalSettingsData
                Dim objGlobalSettings As GlobalSettings = objGlobalSettingsData.getGlobalSettings()
                If iSyncType = SyncType.SyncByWorkingGroup Then
                    customers = objCustomerData.getCustomersByWorkingGroup()
                Else
                    customers = objCustomerData.getCustomers().Where(Function(x) x.Status = 1).ToList()
                End If
                For Each objCustomer As Customer In customers
                    If objCustomer.PhysicalFilePath = "" Then
                        objCustomer.PhysicalFilePath = objGlobalSettings.AttachedFileFolder
                    End If

                    'Make sure to delete old temp folders (if any error occurred in session) 
                    Try
                        Dim tempFolders As String() = Directory.GetDirectories(objCustomer.PhysicalFilePath, workingModeNumber.ToString() & "_temp_" & processId)
                        For Each folder As String In tempFolders
                            Dim di As DirectoryInfo = New DirectoryInfo(folder)
                            di.Delete(True) ' True to delete recursively
                        Next
                    Catch ex As Exception
                        ' Log or handle the exception if necessary
                        'LogError("Error deleting tempfolders " & ex.ToString(), Nothing)
                    End Try
                Next

                ' This is old ugly stuff
                If itemattach IsNot Nothing Then
                    For i As Integer = 0 To itemattach.Count - 1
                        If itemattach(i) IsNot Nothing Then
                            Dim strFile As String = itemattach(i).ToString()
                            If System.IO.File.Exists(strFile) Then
                                System.IO.File.Delete(strFile)
                            End If
                        End If

                    Next
                End If
            Catch ex As Exception
                LogToFile("Error in finally clause " & ex.ToString(), Nothing)
            End Try
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
            LogError("Error ToggleConfigEncryption " & ex.ToString(), Nothing)
        End Try
    End Sub

    Private Function FormatConnectionString(connectionString As String) As String
        Dim builder As SqlConnectionStringBuilder = New SqlConnectionStringBuilder(connectionString)
        Return String.Format("Data Source={0}; Initial Catalog={1};Network Library={2}", builder.DataSource, builder.InitialCatalog, builder.NetworkLibrary)
    End Function


    Public Function ReadMailBox(ByVal sConnectionstring As String, ByVal iSyncType As SyncType, ByVal workingModeArg As String) As Integer
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings
        Dim objCustomerData As New CustomerData
        Dim objPriorityData As New PriorityData
        Dim objCustomer As Customer
        Dim objMovedFromCustomer As Customer
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
        Dim IMAPlist As ImapMessageCollection = Nothing
        Dim message As MailMessage = Nothing
        Dim sFromEMailAddress As String = ""
        Dim sToEMailAddress As String = ""
        Dim sNewCaseToEmailAddress As String = ""
        Dim sSubject As String
        Dim sBodyText As String = ""
        Dim iLog_Id As Integer
        Dim iCaseNumber As Integer
        Dim sExternalCaseNumber As String = ""
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
        Dim isHtml As Boolean = False
        Dim emailBox As String = ""

        Try

            ' Hämta globala inställningar
            gsConnectionString = sConnectionstring

            ' Hämta globala inställningar
            objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

            giDBType = objGlobalSettings.DBType
            gsDBVersion = objGlobalSettings.DBVersion

            Dim customers As List(Of Customer)

            If iSyncType = SyncType.SyncByWorkingGroup Then
                customers = objCustomerData.getCustomersByWorkingGroup()
            Else
                customers = objCustomerData.getCustomers().Where(Function(x) x.Status = 1).ToList()
            End If

            Dim iCustomerCount = 0

            For Each objCustomer In customers

                iCustomerCount += 1
                objMovedFromCustomer = Nothing

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

                    emailBox = objCustomer.POP3UserName

                    LogToFile($"M2T for mailbox: {emailBox}, " & objCustomer.Name & ", Nr: " & iCustomerCount & "(" & customers.Count & "), appVersion: " & CurrentAssemblyInfo.Version, iPop3DebugLevel)

                    Dim mails As List(Of MailMessage) = Nothing
                    ' Denna try fångar exception
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

                        If eMailConnectionType = MailConnectionType.Ews Then
                            If String.IsNullOrEmpty(objCustomer.EMailFolder) Then
                                objCustomer.EMailFolder = InboxMailFolderName 'Set Default To inbox If NULL
                            End If

                            'Dim maxAttempt As Integer = 5
                            'Dim maxAttempt As Integer = 5 ' GetAppSettingValue("MaxAttempt")
                            Dim imax As String = GetAppSettingValue("MaxConnectionAttempts")
                            Dim maxAttempt As Integer
                            If IsNumeric(imax) Then
                                maxAttempt = Convert.ToInt32(imax)
                            Else
                                maxAttempt = 1
                            End If
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
                                                                                              $"{objCustomer.PhysicalFilePath}\{tempFolder}")

                                    'Successful Quit
                                    task.Wait()
                                    mails = task.Result
                                    Exit For

                                Catch ex As Exception
                                    icount = icount + 1
                                    If icount = maxAttempt Then
                                        'LogError("Error readMailBox: " & ex.Message.ToString(), Nothing)
                                        'rethrow
                                        Throw
                                    End If
                                    Thread.Sleep(2000)

                                End Try
                            Next

                            iListCount = mails.Count()
#Region "IMAP"
                        Else
                            eMailConnectionType = MailConnectionType.Imap
                            IMAPclient = New Imap()

                            ' Enable IMAPI Client Logging
                            If bEnableIMAPIClientLog AndAlso Not IsNullOrEmpty(sIMAPIClientLogPath) Then
                                IMAPclient.LogWriter = New FileLogWriter(sIMAPIClientLogPath, Rebex.LogLevel.Debug)
                            End If

                            Dim host As IPHostEntry = Dns.GetHostEntry(objCustomer.POP3Server)

                            If host IsNot Nothing Then
                                ip = host.AddressList(0).ToString()
                            End If

                            'LogToFile("Connecting to " & objCustomer.POP3Server & " (" & ip & "):" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName, iPop3DebugLevel)

                            If objCustomer.POP3Port = 993 Then
                                IMAPclient.Connect(objCustomer.POP3Server.ToString(), SslMode.Implicit)
                            Else
                                'Added last argument for SSL SslMode.Explicit 20230622 for Osby 
                                IMAPclient.Connect(objCustomer.POP3Server, objCustomer.POP3Port, SslMode.Explicit)
                            End If

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

                        LogToFile("Connecting to " & objCustomer.POP3Server & " (" & ip & "):" & objCustomer.POP3Port & ", " & objCustomer.POP3UserName & ", EWS Mode: " & MailConnectionType.Ews, iPop3DebugLevel)

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
                            '
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
#End Region
                        Dim caseismerged As Integer = 0
                        Dim dFinDate As Date
                        Dim casefinsihed As Boolean = False
                        Dim caseisactivated As Boolean = False

                        If iListCount > 0 Then
                            For iListIndex As Integer = 0 To iListCount - 1

                                caseismerged = 0
                                dFinDate = Date.MinValue
                                casefinsihed = False
                                caseisactivated = False

                                iFinishingCause_Id = 0
                                sBodyText = ""
                                iLog_Id = 0
                                sExternalCaseNumber = ""


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
                                    uniqueMessageId = message.MessageId.ToString()
                                End If

                                LogToFile("Read Mail From " & message.From.ToString() & ", To " & message.To.ToString() & ", MessageID: " & uniqueMessageId & ", HasBodyText: " & message.HasBodyText & ", HasBodyHtml: " & message.HasBodyHtml & ", IsSigned: " & message.IsSigned & ", Silent: " & message.Silent, iPop3DebugLevel)

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

                                If bOrder Then
                                    ' Ta fram användaren
                                    Dim iPos As Integer = InStr(1, sSubject, " ", vbTextCompare)
                                    Dim sUserId As String = ""

                                    If iPos > 0 Then
                                        sUserId = Trim(Left(sSubject, iPos - 1))
                                    End If

                                    LogToFile("Beställning från " & sUserId, iPop3DebugLevel)

                                    If sUserId <> "" Then
                                        Dim objCU As ComputerUser = objComputerUserData.getComputerUserByUserId(sUserId, objCustomer.Id)

                                        If objCU IsNot Nothing Then
                                            sFromEMailAddress = objCU.EMail
                                        End If

                                    End If
                                End If

                                iMailID = 0

                                If bEnableNewEmailProcessing Then
                                    'New logic to find existing case by email
                                    Dim messageIds As List(Of String) = ExtractMessageIds(message)
                                    LogToFile(String.Format("EnableNewEmailProcessing True. MessageIds found: {0}", String.Join(",", messageIds)), iPop3DebugLevel)

                                    ' Iterate over all UniqueMessageIds from the email to find matching case
                                    For Each messageId As String In messageIds
                                        ' Find objCase
                                        objCase = FindCaseByUniqueMessageId(messageId, objCustomer)

                                        If (objCase IsNot Nothing) Then
                                            ' Kontrollera vilket mailID detta är ett svar på | Check which mailID this is an answer to
                                            iMailID = objCaseData.getMailIDByMessageID(messageId)
                                            If objCase.Customer_Id <> objCustomer.Id AndAlso objCase.MovedFromCustomer_Id = objCustomer.Id Then
                                                'This case has been moved from this customer
                                                'Keep the old customer
                                                objMovedFromCustomer = objCustomerData.getCustomerById(objCase.MovedFromCustomer_Id)
                                                'Get the new correct customer
                                                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                                                LogToFile("Found existing moved case with bEnableNewEmailProcessing. Old Customer: " & messageId & " MovedFromCustomer: " & objCase.MovedFromCustomer_Id & " new customer:" & objCase.Customer_Id, iPop3DebugLevel)
                                            End If
                                            LogToFile("EnableNewEmailProcessing Found case by getMailIDByMessageID: " & iMailID.ToString() & " CaseNumber: " & objCase.Casenumber, iPop3DebugLevel)
                                            Exit For
                                        Else
                                            LogToFile("EnableNewEmailProcessing Did not find case by getMailIDByMessageID: " & messageId, iPop3DebugLevel)
                                        End If
                                    Next
                                Else
                                    'old logic 
                                    ' Check if the message is a reply by verifying InReplyTo count
                                    If message.InReplyTo.Count > 0 Then
                                        Dim replyToId As String = message.InReplyTo(0).ToString()
                                        LogToFile("Old logic. Reply From: " & replyToId, iPop3DebugLevel)

                                        ' Attempt to find a case by the replyToId using getCaseByMessageID
                                        objCase = objCaseData.getCaseByMessageID(replyToId)

                                        ' No case found by InReplyTo; try with getCaseByOrderMessageID if ModuleOrder is 1
                                        If objCase Is Nothing AndAlso objCustomer.ModuleOrder = 1 Then
                                            LogToFile("Did not find case by replyToId: " & replyToId & " Trying with getCaseByOrderMessageID", iPop3DebugLevel)

                                            ' Attempt to find a case by OrderMessageID
                                            objCase = objCaseData.getCaseByOrderMessageID(replyToId)

                                            If objCase IsNot Nothing Then
                                                ' Check if the case was moved from the current customer
                                                If objCase.Customer_Id <> objCustomer.Id AndAlso objCase.MovedFromCustomer_Id = objCustomer.Id Then
                                                    'Keep the old customer
                                                    objMovedFromCustomer = objCustomerData.getCustomerById(objCase.MovedFromCustomer_Id)
                                                    ' Update to the new correct customer if the case was moved
                                                    objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                                                    LogToFile("Found existing moved case with orderMessageId. Old Customer: " & objCase.MovedFromCustomer_Id & " new customer:" & objCase.Customer_Id, iPop3DebugLevel)
                                                End If
                                                LogToFile("Found case by getCaseByOrderMessageID: " & replyToId & " CaseNumber: " & objCase.Casenumber, iPop3DebugLevel)
                                            Else
                                                LogToFile("Did not find case by getCaseByOrderMessageID: " & replyToId, iPop3DebugLevel)
                                            End If
                                        ElseIf objCase IsNot Nothing Then
                                            ' Case found by getCaseByMessageID, check for customer transfer
                                            If objCase.Customer_Id <> objCustomer.Id AndAlso objCase.MovedFromCustomer_Id = objCustomer.Id Then
                                                ' Update to the new correct customer if the case was moved
                                                'Keep the old customer
                                                objMovedFromCustomer = objCustomerData.getCustomerById(objCase.MovedFromCustomer_Id)
                                                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                                                LogToFile("Found existing moved case with getCaseByOrderMessageID. Old Customer: " & objCase.MovedFromCustomer_Id & " new customer:" & objCase.Customer_Id, iPop3DebugLevel)
                                            End If
                                            LogToFile("Found case by getCaseByMessageID: " & replyToId & " CaseNumber: " & objCase.Casenumber, iPop3DebugLevel)
                                        End If

                                        ' Retrieve the associated email ID for this case
                                        iMailID = objCaseData.getMailIDByMessageID(replyToId)
                                    Else
                                        ' If not a reply, set iMailID to 0
                                        iMailID = 0
                                    End If

                                End If

                                If objCase Is Nothing AndAlso objCustomer.ExternalEMailSubjectPattern <> "" Then
                                    sExternalCaseNumber = ExtractExternalCaseNumberFromSubject(sSubject, objCustomer.ExternalEMailSubjectPattern)

                                    If sExternalCaseNumber <> "" Then
                                        LogToFile("Found ExternalEMailSubjectPattern :" & objCustomer.ExternalEMailSubjectPattern & " and ExternalCaseNumber:  " & sExternalCaseNumber & " Subject: " & sSubject, iPop3DebugLevel)

                                        objCase = objCaseData.GetCaseByExternalCaseNumber(sExternalCaseNumber)

                                        If objCase IsNot Nothing Then
                                            LogToFile("Found existing case by ExternalCaseNumber: " & sExternalCaseNumber, iPop3DebugLevel)
                                        Else
                                            LogToFile("Did not find existing case by ExternalCaseNumber: " & sExternalCaseNumber, iPop3DebugLevel)
                                        End If
                                    End If
                                End If
                                ' Kontrollera om det är svar på ett befintligt ärende | Check if there is an answer to an existing case
                                ' denna är ju lurig - kunden som ligger i loopen måste ha emailsubjectpattern för att det ska bli kontroll även om ärendet blivit flyttat till annan kund
                                ' This one is tricky - the customer in the loop must have an email subject pattern for the check to be performed even if the case has been moved from this customer
                                If objCustomer.EMailSubjectPattern <> "" AndAlso objCase Is Nothing Then
                                    LogToFile("Found EmailSubjectPattern: " & objCustomer.EMailSubjectPattern & " on customerId: " & objCustomer.Id & ". Mailsubject: " & sSubject, iPop3DebugLevel)
                                    iCaseNumber = extractCaseNumberFromSubject(sSubject, objCustomer.EMailSubjectPattern)

                                    If iCaseNumber <> 0 Then
                                        LogToFile("Found CaseNumber from EmailSubjectPattern: " & iCaseNumber, iPop3DebugLevel)
                                        objCase = objCaseData.getCaseByCaseNumber(iCaseNumber)
                                        If objCase IsNot Nothing AndAlso objCase.Customer_Id <> objCustomer.Id Then
                                            If objCase.MovedFromCustomer_Id = objCustomer.Id Then
                                                'This case has been moved from this customer
                                                'Keep the old customer
                                                objMovedFromCustomer = objCustomerData.getCustomerById(objCase.MovedFromCustomer_Id)
                                                'Get the new correct customer
                                                objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                                                LogToFile("Found existing moved case with EmailSubjectPattern and CaseNumber. Old Customer: " & objCase.MovedFromCustomer_Id & " CaseNumber " & iCaseNumber & " MovedFromCustomer: " & objCustomer.Id, iPop3DebugLevel)
                                            Else
                                                objCase = Nothing
                                                LogToFile("Did not find existing moved case with EmailSubjectPattern and CaseNumber: " & iCaseNumber, iPop3DebugLevel)
                                            End If
                                        Else
                                            LogToFile("Did not find existing case with EmailSubjectPattern and CaseNumber: " & iCaseNumber, iPop3DebugLevel)
                                        End If

                                    End If

                                End If

                                objComputerUser = objComputerUserData.getComputerUserByEMail(sFromEMailAddress, objCustomer.Id)


                                If message.HasBodyHtml Then
                                    LogToFile("HasBodyHtml ", 1)
                                    sBodyText = getInnerHtml(message.BodyHtml)
                                    Try
                                        sBodyText = CleanHtml(sBodyText)
                                        sBodyText = CleanStyles(sBodyText)
                                    Catch ex As Exception
                                        'LogError("Error Cleanstyles " & ex.ToString(), Nothing)
                                    End Try
                                    Try

                                        sBodyText = CreateBase64Images(objCustomer, message, $"{objCustomer.PhysicalFilePath}\{tempFolder}", sBodyText)
                                    Catch ex As Exception
                                        'LogError("Error CreateBase64Images " & ex.ToString(), Nothing)
                                    End Try

                                    isHtml = True

                                End If

                                If message.HasBodyText AndAlso Not message.HasBodyHtml Then
                                    LogToFile("HasBodyText only", 1)
                                    sBodyText = Replace(message.BodyText.ToString(), Chr(10), vbCrLf, 1, -1, CompareMethod.Text)

                                End If

                                '//hämta användare baserat på userid/reportedBy | Get user based on userid/reportedBy
                                Dim fields As New Dictionary(Of String, String)
                                If fieldsToUpdate.Count > 0 Then
                                    fields = GetParsedMailBody(fieldsToUpdate, sBodyText)
                                    'Fix
                                    Dim strUserId As String = GetValueFromEmailtext(fields, "reportedby")
                                    If Not IsNullOrEmpty(strUserId) Then
                                        objComputerUser = objComputerUserData.getComputerUserByUserId(strUserId, objCustomer.Id)
                                    End If
                                End If
                                '' New case
#Region "New Case"
                                If objCase Is Nothing Then
                                    objCase = New CCase With {
                                        .ExternalCasenumber = sExternalCaseNumber,
                                        .Caption = Left(message.Subject.ToString(), 100),
                                        .Description = sBodyText,
                                        .CaseType_Id = objCustomer.EMailDefaultCaseType_Id,
                                        .Category_Id = objCustomer.EMailDefaultCategory_Id,
                                        .ProductArea_Id = objCustomer.EMailDefaultProductArea_Id,
                                        .Priority_Id = objCustomer.EMailDefaultPriority_Id,
                                        .Status_Id = objCustomer.DefaultStatus_Id,
                                        .StateSecondary_Id = objCustomer.DefaultStateSecondary_Id,
                                        .Customer_Id = objCustomer.Id,
                                        .WorkingGroup_Id = objCustomer.DefaultWorkingGroup_Id,
                                        .RegLanguage_Id = objCustomer.Language_Id,
                                        .RegistrationSourceCustomer_Id = objCustomer.RegistrationSourceCustomer_Id,
                                        .Performer_User_Id = objCustomer.DefaultAdministratorExternalUser_Id,
                                        .RegUserName = sFromEMailAddress
                                    }

                                    If objComputerUser IsNot Nothing Then
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

                                        If ct IsNot Nothing Then
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

                                    ' hämta uppgifter från e-post till ärende objektet | Get email details to the case object
                                    If fieldsToUpdate.Count > 0 AndAlso fields.Count > 0 Then
                                        objCase = UpdateCaseFieldsFromEmail(objCase, fields, colDepartment, objCustomer.Id, colPrio)
                                    End If

                                    ' Kontrollera om watchdate ska sättas | Check if watchdate should be set
                                    If objCase.Priority_Id <> 0 Then
                                        Dim pd As New PriorityData

                                        Dim p As Priority = pd.getPriorityById(objCase.Priority_Id)

                                        If p IsNot Nothing Then
                                            sPriorityEMailList = p.EMailList
                                        End If

                                        If p.SolutionTime = 0 AndAlso colDepartment IsNot Nothing AndAlso colDepartment.Count > 0 Then
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
                                    Try
                                        Dim caseProcessor As New DH.Helpdesk.VBCSharpBridge.CaseExposure

                                        Dim caseBridge As New CaseBridge With {
                                            .Customer_Id = objCase.Customer_Id,
                                            .FromEmail = sFromEMailAddress
                                        }

                                        ' Call the ProcessCase method
                                        Dim result As CaseBridge = caseProcessor.RunBusinessRules(caseBridge)

                                        If result.Performer_User_Id IsNot Nothing Then
                                            objCase.Performer_User_Id = result.Performer_User_Id
                                        End If

                                        If result.WorkingGroup_Id IsNot Nothing Then
                                            objCase.WorkingGroup_Id = result.WorkingGroup_Id
                                        End If

                                        objCase = objCaseData.createCase(objCase)
                                    Catch ex As Exception
                                        LogError("Error creating Case in database: " & ex.Message.ToString(), objCustomer)
                                        Continue For
                                    End Try

                                    ' save caseisabout - Advanced
                                    If fieldsToUpdate.Count > 0 AndAlso fields.Count > 0 Then
                                        Dim objCaseIsAbout As ComputerUser = CreateCaseIsAbout(objCustomer.Id, fields)
                                        If objCaseIsAbout IsNot Nothing Then
                                            objCaseData.saveCaseIsAbout(objCase.Id, objCaseIsAbout)
                                        End If
                                    End If

                                    LogToFile("Create Case:" & objCase.Casenumber & ", Attachments:" & message.Attachments.Count, iPop3DebugLevel)

                                    'Save files to disk
                                    Dim sHTMLFileName As String = ""
                                    Dim sPDFFileName As String = ""
                                    Try
                                        sHTMLFileName = CreateHtmlFileFromMail(objCustomer, message, objCustomer.PhysicalFilePath & "\" & objCase.Casenumber, objCase.Casenumber, "")
                                        sPDFFileName = CreatePdfFileFromMail(objCustomer, message, objCustomer.PhysicalFilePath & "\" & objCase.Casenumber, objCase.Casenumber)
                                    Catch ex As Exception
                                        LogError("Error creating Html / pdf " & ex.Message, Nothing)
                                    End Try


                                    If Not IsNullOrEmpty(sPDFFileName) Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen  | Add to database                                    
                                        objCaseData.saveFileInfo(objCase.Id, "Mail/" & sPDFFileName)
                                    End If



                                    iCaseHistory_Id = objCaseData.saveCaseHistory(objCase.Id, sFromEMailAddress)

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

                                    Dim newcaseEmailTo As String = objCase.Persons_EMail
                                    If objCustomer.NewCaseMailTo = 1 Then
                                        newcaseEmailTo = sNewCaseToEmailAddress
                                    End If

                                    ' Check if the recipient is blocked
                                    If Not isBlockedRecipient(newcaseEmailTo, objCustomer.BlockedEmailRecipients) Then
                                        If isValidRecipient(newcaseEmailTo, objCustomer.AllowedEMailRecipients) Then
                                            If objCustomer.EMailRegistrationMailID <> 0 AndAlso Not bOrder AndAlso (message.From.ToString() <> message.To.ToString()) Then
                                                objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.NewCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                                If objMailTemplate IsNot Nothing Then
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

                                                    sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, newcaseEmailTo, objMailTemplate, objGlobalSettings,
                                                                     sMessageId, sEMailLogGUID, sConnectionstring)

                                                    objLogData.createEMailLog(iCaseHistory_Id, newcaseEmailTo, MailTemplates.NewCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                                End If
                                            End If
                                        Else
                                            LogToFile("readMailBox, isValidRecipient false: " & objCase.Persons_EMail & ", " & objCustomer.AllowedEMailRecipients, iPop3DebugLevel)
                                        End If
                                    Else
                                        LogToFile("readMailBox, isBlockedRecipient true: " & objCase.Persons_EMail & ", " & objCustomer.BlockedEmailRecipients, iPop3DebugLevel)
                                    End If

                                    If objCustomer.EMailRegistrationMailID <> 0 AndAlso objCustomer.NewCaseEMailList <> "" Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.NewCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If objMailTemplate IsNot Nothing Then
                                            Dim vNewCaseEmailList As String() = objCustomer.NewCaseEMailList.Split(";")

                                            For index As Integer = 0 To vNewCaseEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

                                                sRet_SendMail =
                                                objMail.sendMail(objCase, Nothing, objCustomer, vNewCaseEmailList(index), objMailTemplate, objGlobalSettings,
                                                                 sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vNewCaseEmailList(index), MailTemplates.NewCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If

                                    If objCase.Performer_User_Id <> 0 Then
                                        Dim objUser As User = objUserData.getUserById(objCase.Performer_User_Id)

                                        If objUser IsNot Nothing AndAlso objUser.Status = 1 AndAlso (objUser.AllocateCaseMail = 1 AndAlso Len(objUser.EMail) > 6) Then
                                            objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.AssignedCaseToUser, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                            If objMailTemplate IsNot Nothing Then
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString()
                                                sRet_SendMail =
                                                objMail.sendMail(objCase, Nothing, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId,
                                                                 sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, MailTemplates.AssignedCaseToUser, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
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

                                    If workingGroupId <> 0 AndAlso workingGroupAllocateCaseMail = 1 Then
                                        Dim emailsList As List(Of String) = New List(Of String)()

                                        If Not IsNullOrEmpty(workingGroupEMail) Then
                                            emailsList = workingGroupEMail.Split(New Char() {";"c, ","c}).ToList()
                                        Else
                                            Dim users As List(Of WorkingGroupUser) = objUserWGData.getWorkgroupUsers(workingGroupId)
                                            Dim usersDepartments As List(Of KeyValuePair(Of Integer, Integer)) = New List(Of KeyValuePair(Of Integer, Integer))
                                            If objCase.Department_Id <> 0 Then
                                                usersDepartments = objDepartmentData.getUserDepartmentsIds(users.Select(Function(x) x.Id).ToArray())
                                            End If

                                            For Each user As WorkingGroupUser In users
                                                If user.AllocateCaseMail = 1 AndAlso Not String.IsNullOrWhiteSpace(user.EMail) And
                                               user.Status = 1 AndAlso user.WorkingGroupUserRole = WorkingGroupUserPermission.ADMINSTRATOR Then
                                                    If objCase.Department_Id <> 0 AndAlso usersDepartments.Exists(Function(ud) ud.Key = user.Id) Then
                                                        If usersDepartments.Exists(Function(ud) ud.Key = user.Id AndAlso ud.Value = objCase.Department_Id) Then
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
                                            If objMailTemplate IsNot Nothing Then
                                                For Each recipient As String In emailsList.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).Distinct()
                                                    Dim objMail As New Mail

                                                    sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                    sSendTime = Date.Now()

                                                    Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

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

                                        If objMailTemplate IsNot Nothing Then
                                            Dim vPriorityEmailList As String() = sPriorityEMailList.Split(";")

                                            For index As Integer = 0 To vPriorityEmailList.Length - 1
                                                Dim objMail As New Mail

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

                                                sRet_SendMail =
                                                    objMail.sendMail(objCase, Nothing, objCustomer, vPriorityEmailList(index), objMailTemplate, objGlobalSettings,
                                                                     sMessageId, sEMailLogGUID, sConnectionstring)

                                                objLogData.createEMailLog(iCaseHistory_Id, vPriorityEmailList(index), MailTemplates.AssignedCaseToPriority, sMessageId,
                                                                          sSendTime, sEMailLogGUID, sRet_SendMail)
                                            Next

                                        End If
                                    End If

                                    If Not IsNullOrEmpty(sHTMLFileName) Then
                                        Try
                                            DeleteFilesInsideFolder($"{objCustomer.PhysicalFilePath}\{objCase.Casenumber}\html", True)
                                            DeleteFilesInsideFolder($"{objCustomer.PhysicalFilePath}\{tempFolder}", True)

                                        Catch ex As Exception
                                            'LogError("Error deleting files: " & ex.Message, Nothing)
                                        End Try
                                    End If
#End Region
#Region "Existing Case"
                                Else ' Existing case 

                                    ' Check if it has been moved to another customer
                                    If (objCase.MovedFromCustomer_Id <> 0 AndAlso objCase.Customer_Id <> objCustomer.Id) Then
                                        LogToFile("Case has been moved to another customer", iPop3DebugLevel)
                                        'Keep the old customer
                                        objMovedFromCustomer = objCustomerData.getCustomerById(objCase.MovedFromCustomer_Id)
                                        'Get the new customer
                                        objCustomer = objCustomerData.getCustomerById(objCase.Customer_Id)
                                        LogToFile("Found existing moved case MovedFromCustomer: " & objCase.MovedFromCustomer_Id & " new customer:" & objCase.Customer_Id, iPop3DebugLevel)
                                    End If
                                    ' Save answer as a log post 
                                    ' Only answer 
                                    sBodyText = extractAnswerFromBody(sBodyText, objCustomer.EMailAnswerSeparator)
                                    ' Mark as unread
                                    objCaseData.markCaseUnread(objCase)
                                    ' check if its merged
                                    caseismerged = objCaseData.checkIfCaseIsMerged(objCase.Id)

                                    dFinDate = objCase.FinishingDate
                                    If caseismerged > 0 Then
                                        'objCaseData.updateChangeTime(objCase.Id)

                                        objCaseData.updateChangeTime(caseismerged)
                                        objCase = Nothing
                                        objCase = objCaseData.getCase(caseismerged)
                                        If objCase Is Nothing Then
                                            Continue For
                                        End If
                                        If IsDate(objCase.FinishingDate) Then
                                            If objCase.FinishingDate <> Date.MinValue AndAlso objCase.FinishingDate <> Date.MaxValue Then
                                                casefinsihed = True
                                            End If
                                        End If

                                    End If

                                    'If objCase.FinishingDate <> Date.MinValue Then
                                    If dFinDate <> Date.MinValue Then
                                        ' Aktivera ärendet

                                        If caseismerged = 0 Or casefinsihed = True Then
                                            objCaseData.activateCase(objCase, objCustomer.OpenCase_StateSecondary_Id, objCustomer.WorkingDayStart, objCustomer.WorkingDayEnd, objCustomer.TimeZone_offset)
                                            caseisactivated = True
                                        End If
                                    Else
                                        If objCustomer.ModuleAccount = 1 Then
                                            ' Check if ther is a connected order
                                            Dim ad As New AccountData

                                            Dim a As Account = ad.getAccountByCaseNumber(objCase.Casenumber)

                                            If a IsNot Nothing AndAlso InStr(a.AccountActivity.CloseCase_M2T_Sender, sFromEMailAddress, CompareMethod.Text) <> 0 Then
                                                iFinishingCause_Id = a.AccountActivity.CloseCase_FinishingCause_Id
                                            End If

                                            If iFinishingCause_Id <> 0 Then
                                                ' Close case
                                                objCaseData.closeCase(objCase)
                                            End If
                                        End If

                                    End If

                                    If objCase.ResetOnExternalUpdate = 1 Then
                                        objCaseData.resetStateSecondary(objCase, objCustomer.WorkingDayStart, objCustomer.WorkingDayEnd, objCustomer.TimeZone_offset)
                                    End If

                                    iCaseHistory_Id = objCaseData.saveCaseHistory(objCase.Id, objCase.Persons_EMail)


                                    Dim isInternalLogUsed As Boolean = CheckInternalLogConditions(iMailID, objCustomer, sFromEMailAddress, sToEMailAddress)

                                    If sExternalCaseNumber <> "" Then
                                        isInternalLogUsed = False
                                    End If

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

                                    'Creating htmlfile to use for pdf-creating
                                    Dim sHTMLFileName As String = ""
                                    Dim sPDFFileName As String = ""
                                    Try
                                        sHTMLFileName = CreateHtmlFileFromMail(objCustomer, message, Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id), objCase.Casenumber, "")
                                        sPDFFileName = CreatePdfFileFromMail(objCustomer, message, Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id), objCase.Casenumber)
                                    Catch ex As Exception
                                        LogError("Error creating Html / pdf " & ex.Message, Nothing)
                                    End Try


                                    If Not IsNullOrEmpty(sPDFFileName) Then
                                        iHTMLFile = 1

                                        ' Lägg in i databasen
                                        objLogData.saveFileInfo(iLog_Id, "Mail/" & sPDFFileName, bIsInternalLogFile)
                                    End If

                                    If Not IsNullOrEmpty(sHTMLFileName) Then
                                        Try
                                            DeleteFilesInsideFolder(Path.Combine(objCustomer.PhysicalFilePath, logSubFolderPrefix & iLog_Id) & "\html", True)
                                            DeleteFilesInsideFolder($"{objCustomer.PhysicalFilePath}\{tempFolder}", True)
                                        Catch ex As Exception
                                            'LogError("Error deleting files: " & ex.Message, Nothing)
                                        End Try

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
                                    If objCase.ExternalUpdateMail = 1 AndAlso Len(objCase.PerformerEMail) > 6 Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.CaseIsUpdated, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If objMailTemplate IsNot Nothing Then
                                            Dim objUser As User = objUserData.getUserById(objCase.Performer_User_Id)
                                            If objUser IsNot Nothing AndAlso objUser.Status = 1 AndAlso Not String.IsNullOrWhiteSpace(objUser.EMail) Then
                                                Dim objMail As New Mail
                                                Dim objLog As New Log

                                                ' Set appropriate log text property
                                                objLog.Text_External = If(Not isInternalLogUsed, sBodyText, String.Empty)
                                                objLog.Text_Internal = If(isInternalLogUsed, sBodyText, String.Empty)

                                                sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                                sSendTime = Date.Now()

                                                Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

                                                sRet_SendMail =
                                                objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings,
                                                                 sMessageId, sEMailLogGUID, sConnectionstring, attachedFiles)

                                                objLogData.createEMailLog(iCaseHistory_Id, objCase.PerformerEMail, MailTemplates.CaseIsUpdated, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)
                                            End If
                                        End If
                                    ElseIf iFinishingCause_Id <> 0 AndAlso Len(objCase.Persons_EMail) > 6 AndAlso Not isBlockedRecipient(objCase.Persons_EMail, objCustomer.BlockedEmailRecipients) Then
                                        objMailTemplate = objMailTemplateData.getMailTemplateById(MailTemplates.ClosedCase, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion)

                                        If objMailTemplate IsNot Nothing Then
                                            Dim objMail As New Mail
                                            Dim objLog As New Log

                                            ' Set appropriate log text property
                                            objLog.Text_External = If(Not isInternalLogUsed, sBodyText, String.Empty)
                                            objLog.Text_Internal = If(isInternalLogUsed, sBodyText, String.Empty)

                                            sMessageId = createMessageId(objCustomer.HelpdeskEMail)
                                            sSendTime = Date.Now()

                                            Dim sEMailLogGUID As String = Guid.NewGuid().ToString()

                                            sRet_SendMail =
                                            objMail.sendMail(objCase, objLog, objCustomer, objCase.Persons_EMail, objMailTemplate, objGlobalSettings,
                                                             sMessageId, sEMailLogGUID, sConnectionstring, attachedFiles)

                                            objLogData.createEMailLog(iCaseHistory_Id, objCase.Persons_EMail, MailTemplates.ClosedCase, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail)

                                        End If

                                    End If
                                End If
#End Region
                                ' spara e-post adresser
                                If message IsNot Nothing AndAlso objCase IsNot Nothing Then
                                    Dim messageId As String = message.MessageId.ToString()
                                    objMailTicket.Save(objCase.Id, iLog_Id, "to", message.To.ToString(), message.Subject, messageId) ' Saving subject only for to address...
                                    objMailTicket.Save(objCase.Id, iLog_Id, "cc", message.CC.ToString(), Nothing, messageId)
                                    objMailTicket.Save(objCase.Id, iLog_Id, "bcc", message.Bcc.ToString(), Nothing, messageId)
                                End If
                                'Be sure we use the right emailbox to delete messages
                                If (objCustomer.POP3UserName <> emailBox) Then
                                    'Get properties for deletion of email
                                    Dim originalCustomer As Customer = objCustomer
                                    LogToFile($"Reading {emailBox} thats not the same as current customers Pop3UserName: {objCustomer.POP3UserName} in the loop. Fetching the right properties for deleting emails ", iPop3DebugLevel)
                                    If (iSyncType = SyncType.SyncByWorkingGroup) Then
                                        originalCustomer = objCustomerData.GetCustomerDataFromEmailBoxInUse(emailBox, 5)
                                    Else
                                        originalCustomer = objCustomerData.GetCustomerDataFromEmailBoxInUse(emailBox, 0)
                                    End If
                                    objCustomer.Id = originalCustomer.Id
                                    objCustomer.POP3Server = originalCustomer.POP3Server
                                    objCustomer.POP3Port = originalCustomer.POP3Port
                                    objCustomer.POP3UserName = originalCustomer.POP3UserName
                                    objCustomer.EMailFolder = originalCustomer.EMailFolder
                                    objCustomer.EMailFolderArchive = originalCustomer.EMailFolderArchive
                                    objCustomer.EwsApplicationId = originalCustomer.EwsApplicationId
                                    objCustomer.EwsClientSecret = originalCustomer.EwsClientSecret
                                    objCustomer.EwsTenantId = originalCustomer.EwsTenantId
                                End If

                                If eMailConnectionType = MailConnectionType.Pop3 Then
                                    ' Inget stöd för POP3 längre
                                ElseIf eMailConnectionType = MailConnectionType.Imap Then

                                    If Not IsNullOrEmpty(objCustomer.EMailFolderArchive) Then
                                        LogToFile("Move Message To: " & objCustomer.EMailFolderArchive, iPop3DebugLevel)
                                        IMAPclient.CopyMessage(sUniqueID, objCustomer.EMailFolderArchive)
                                    End If

                                    LogToFile("Deleting Message", iPop3DebugLevel)
                                    IMAPclient.DeleteMessage(sUniqueID)

                                    ' Purge to apply message delete otherwise the message will stay in Inbox
                                    IMAPclient.Purge()
                                ElseIf eMailConnectionType = MailConnectionType.Ews Then
                                    LogToFile("Trying to delete " & message.Subject & " from Inbox for customer id: " & objCustomer.Id, iPop3DebugLevel)
                                    ' Copy mail if archieve, ' delete mail
                                    DeleteEwsMail(message, objCustomer.Id,
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
                        LogError("Error readMailBox." & objCustomer.POP3UserName & "  Error: " & ex.ToString() & " Error message: " & ex.Message, objCustomer)
                        Continue For
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
            'Shall we keep this?
            LogError("Error readMailBox: " & ex.Message.ToString(), Nothing)
            'rethrow
            'Throw
        End Try
    End Function


    Private Async Function DeleteEwsMail(message As EwsMailMessage, iCustomerId As Integer, server As String, port As Integer, userName As String, emailFolder As String, emailArchiveFolder As String, applicationId As String, clientSecret As String, tenantId As String) As System.Threading.Tasks.Task
        'emailFolder = "Inbox"
        'emailArchiveFolder = "Archive/M2T_test"
        Dim ewsScopes As String() = New String() {"https://outlook.office.com/.default"}
        Dim app As IConfidentialClientApplication = ConfidentialClientApplicationBuilder.Create(applicationId).WithAuthority(AzureCloudInstance.AzurePublic, tenantId).WithClientSecret(clientSecret).Build()

        Dim task As Task(Of AuthenticationResult) = app.AcquireTokenForClient(ewsScopes).ExecuteAsync()
        Dim result As AuthenticationResult = Await task

        'Dim service As ExchangeService = New ExchangeService()
        Dim service As ExchangeService = getExchangeService()
        ' TODO: Port? Maybe not
        service.Url = New Uri(server)
        service.Credentials = New OAuthCredentials(result.AccessToken)
        service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, userName)
        service.HttpHeaders.Add("X-AnchorMailbox", userName)

        Dim tokenStatus As String = If(String.IsNullOrEmpty(result.AccessToken), "No token", "Token present")

        LogToFile($"Trying to delete message for customer: {iCustomerId} with server: {server}, ImpersonatedUserId {service.ImpersonatedUserId.Id}, token status: {tokenStatus}, username {userName} ", 1)

        If (Not String.IsNullOrWhiteSpace(emailArchiveFolder)) Then
            Dim archive As Folder = FindEwsFolder(iCustomerId, emailArchiveFolder, service)
            If Not archive Is Nothing Then
                Dim copyIds = New List(Of ItemId)
                copyIds.Add(message.EwsID)
                service.CopyItems(copyIds, archive.Id, False)
                LogToFile($"EmailArchiveFolder found Coying email to: {emailArchiveFolder}.", iCustomerId)
            End If
        End If

        Dim deleteIds As List(Of ItemId) = New List(Of ItemId)()
        deleteIds.Add(message.EwsID)
        service.DeleteItems(deleteIds,
                            DeleteMode.HardDelete,
                            SendCancellationsMode.SendToNone,
                            AffectedTaskOccurrence.AllOccurrences)

    End Function



    Private Function getCorrectTimeZone() As TimeZoneInfo
        Return TimeZoneInfo.CreateCustomTimeZone("Time zone to workaround a bug", TimeZoneInfo.Local.BaseUtcOffset, "Time zone to workaround a bug", "Time zone to workaround a bug")
    End Function

    Private Function getExchangeService() As ExchangeService
        Dim service = New ExchangeService(ExchangeVersion.Exchange2010_SP2, getCorrectTimeZone())
        Return service
    End Function

    Private Async Function ReadEwsFolderAsync(objCustomer As Customer, server As String, port As Integer, userName As String, emailFolder As String, emailArchiveFolder As String,
                                              applicationId As String, clientSecret As String, tenantId As String, temppath As String) As Task(Of List(Of MailMessage))
        Dim ewsScopes As String() = New String() {"https://outlook.office.com/.default"}
        Dim app As IConfidentialClientApplication = ConfidentialClientApplicationBuilder.Create(applicationId).WithAuthority(AzureCloudInstance.AzurePublic, tenantId).WithClientSecret(clientSecret).Build()

        Dim task As Task(Of AuthenticationResult) = app.AcquireTokenForClient(ewsScopes).ExecuteAsync()
        Dim result As AuthenticationResult = Await task


        Dim service As ExchangeService = getExchangeService()
        ' TODO: Port? Maybe not
        service.Url = New Uri(server)
        service.Credentials = New OAuthCredentials(result.AccessToken)
        service.ImpersonatedUserId = New ImpersonatedUserId(ConnectingIdType.SmtpAddress, userName)
        service.HttpHeaders.Add("X-AnchorMailbox", userName)

        Dim inbox As Folder
        If emailFolder.Equals(InboxMailFolderName, StringComparison.InvariantCultureIgnoreCase) Then
            'Dim folderId As FolderId = New FolderId(WellKnownFolderName.MsgFolderRoot, userName)
            inbox = Folder.Bind(service, WellKnownFolderName.Inbox)
        Else
            inbox = FindEwsFolder(objCustomer.Id, emailFolder, service)
        End If

        If inbox Is Nothing Then
            LogError($"EmailFolder '{emailFolder}' doesn't exist.", objCustomer)
            Return Nothing
        End If

        ' Här smäller det eftersom arkivmapp tydligen inte är obligatorisk
        'If Not String.IsNullOrWhiteSpace(emailArchiveFolder) Then
        '    If FindEwsFolder(objCustomer, emailArchiveFolder, service) Is Nothing Then
        '        LogError($"EmailFolderArchive '{emailArchiveFolder}' doesn't exist.", objCustomer)
        '        'Return Nothing
        '    End If
        'End If

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

        ReDim itemattach(items.Count)
        Dim intArray As Integer = 0


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

                If mail.Attachments.Any() Then
                    For Each attach As Microsoft.Exchange.WebServices.Data.Attachment In mail.Attachments
                        If attach.GetType() = GetType(FileAttachment) Then
                            Try
                                attach.Load()
                            Catch ex As Exception
                                LogError($"Error loading FileAttachment: {ex.Message} Customer_Id: {objCustomer.Id}", objCustomer)
                                Continue For
                            End Try
                            'New
                            If Not Directory.Exists(temppath) Then
                                Directory.CreateDirectory(temppath)
                            End If
                            Dim fileAttach As FileAttachment = attach
                            Dim retval As String
                            Dim strName As String = SanitizeFileName(fileAttach.Name)
                            If Not attach.IsInline Then
                                retval = Path.Combine(temppath, strName)
                                fileAttach.Load(retval)

                                Dim bData As Byte()
                                Dim br As BinaryReader = New BinaryReader(System.IO.File.OpenRead(retval))
                                bData = br.ReadBytes(br.BaseStream.Length)

                                Dim newAttachment As Rebex.Mail.Attachment = New Rebex.Mail.Attachment(New MemoryStream(bData, 0, bData.Length), strName)
                                message.Attachments.Add(newAttachment)
                                newAttachment = Nothing
                                bData = Nothing
                                br = Nothing
                                bData = Nothing

                                ' Ensure intArray does not exceed the bounds of the array.
                                If intArray >= items.Count Then
                                    ReDim Preserve itemattach(intArray)  ' Optionally expand the array as needed
                                End If
                                itemattach(intArray) = retval
                                intArray = intArray + 1

                            Else
                                Try
                                    Dim newResource As LinkedResource = New LinkedResource(New MemoryStream(fileAttach.Content), strName, fileAttach.ContentType)
                                    If fileAttach.ContentId IsNot Nothing Then
                                        newResource.ContentId = fileAttach.ContentId
                                    End If
                                    If fileAttach.ContentLocation IsNot Nothing Then
                                        newResource.ContentLocation = fileAttach.ContentLocation
                                    End If
                                    message.Resources.Add(newResource)
                                Catch ex As Exception
                                    LogError($"Error loading Not Inline Attachments: {ex.Message} Customer_Id: {objCustomer.Id}", objCustomer)
                                    Continue For
                                End Try

                            End If
                        End If

                        ' Handling ItemAttachment
                        If attach.GetType() = GetType(ItemAttachment) Then
                            Try
                                Dim itemAttachment As ItemAttachment = attach
                                itemAttachment.Load(ItemSchema.MimeContent)

                                If Not Directory.Exists(temppath) Then
                                    Directory.CreateDirectory(temppath)
                                End If
                                ' Sanitize file name
                                Dim fileName As String = Path.Combine(temppath, SanitizeFileName(itemAttachment.Item.Subject) & ".eml")

                                ' Save to file and add to message attachments
                                File.WriteAllBytes(fileName, itemAttachment.Item.MimeContent.Content)
                                message.Attachments.Add(New Rebex.Mail.Attachment(fileName))

                                ' Delete the temporary file
                                System.IO.File.Delete(fileName)
                            Catch ex As Exception
                                LogError($"Error loading ItemAttachment: {ex.Message} Customer_Id: {objCustomer.Id}", objCustomer)
                                Continue For
                            End Try
                        End If
                    Next
                End If

                messages.Add(message)
            End If

        Next

        Return messages
    End Function
    ' Helper method to sanitize file names
    Private Function SanitizeFileName(fileName As String) As String
        ' Replace invalid characters with an underscore
        Dim invalidChars As Char() = System.IO.Path.GetInvalidFileNameChars().Concat(System.IO.Path.GetInvalidPathChars()).ToArray()
        Dim sanitizedFileName As String = New String(fileName.Select(Function(c) If(invalidChars.Contains(c), "_"c, c)).ToArray())

        Try
            ' Extract file extension from the sanitized file name
            Dim extension As String = System.IO.Path.GetExtension(sanitizedFileName)

            Dim fileNameWithoutExtension As String = System.IO.Path.GetFileNameWithoutExtension(sanitizedFileName)

            ' Truncate the name if it's too long
            If fileNameWithoutExtension.Length > 100 - extension.Length Then
                fileNameWithoutExtension = fileNameWithoutExtension.Substring(0, 100 - extension.Length)
            End If

            ' Append timestamp in HHmmssfff format and combine with the extension
            sanitizedFileName = $"{fileNameWithoutExtension}_{DateTime.Now:HHmmssfff}{extension}"

            Return sanitizedFileName
        Catch ex As Exception
            ' Fallback in case path operations still fail
            LogToFile("Could not sanitize filename: " & fileName, 1)
            Dim extension As String = System.IO.Path.GetExtension(sanitizedFileName)
            Return $"UnknownFile_{DateTime.Now:HHmmssfff}{extension}"
        End Try

    End Function

    Private Function FindEwsFolder(iCustomerId As Integer, emailFolder As String, service As ExchangeService) As Folder
        Dim emailFolders As String()
        If emailFolder.Contains("/") Then
            emailFolders = emailFolder.Split("/"c)
        Else
            emailFolders = New String() {emailFolder}
        End If

        Dim folder As Folder = Nothing
        LogToFile("Starting folder search for: " & emailFolder & " (Customer: " & iCustomerId & ")", 1)

        For Each currentFolderName As String In emailFolders
            Dim folders As FindFoldersResults

            Try
                If folder Is Nothing Then
                    LogToFile($"Searching for root folder: {currentFolderName} (Customer: {iCustomerId})", 1)
                    folders = service.FindFolders(WellKnownFolderName.MsgFolderRoot, New FolderView(100))
                ElseIf folder.Id IsNot Nothing Then
                    LogToFile($"Searching for subfolder: {currentFolderName} under parent {folder.DisplayName} (Customer: {iCustomerId})", 1)
                    folders = service.FindFolders(folder.Id, New FolderView(100))
                Else
                    LogToFile($"Invalid folder ID for: {currentFolderName} (Customer: {iCustomerId})", 1)
                    Return Nothing ' Stop processing if folder ID is invalid
                End If
            Catch ex As Exception
                LogToFile($"Exception in FindFolders: {ex.Message} - {ex.StackTrace} - Customer ID: {iCustomerId} - Service URL: {service.Url}", 1)
                Return Nothing ' Exit if a critical error occurs
            End Try

            ' Log results of the folder search
            If folders IsNot Nothing AndAlso folders.Folders.Any() Then
                LogToFile($"Found {folders.Folders.Count} folders under {If(folder?.DisplayName, "Root")} (Customer: {iCustomerId})", 1)
            Else
                LogToFile($"No folders found under {If(folder?.DisplayName, "Root")} (Customer: {iCustomerId})", 1)
                Return Nothing ' Exit if no folders found
            End If

            ' Find the folder by name
            folder = folders.FirstOrDefault(Function(f) f.DisplayName.Equals(currentFolderName, StringComparison.InvariantCultureIgnoreCase))

            If folder Is Nothing Then
                LogToFile($"Can't find folder: {currentFolderName} (Customer: {iCustomerId})", 1)
                Return Nothing ' Exit immediately
            Else
                LogToFile($"Folder found: {folder.DisplayName} (Customer: {iCustomerId})", 1)
            End If
        Next


        LogToFile("Folder search completed successfully for: " & emailFolder & " (Customer: " & iCustomerId & ")", 1)
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
                                           objectId As String,
                                           prefix As String,
                                           iPop3DebugLevel As Integer,
                                           globalSettings As GlobalSettings) As List(Of String)

        Dim files As New List(Of String)
        Dim deniedFiles As New List(Of String)
        Dim whiteList As List(Of String) = GetFileUploadWhiteList(globalSettings)

        Dim tempDirPath As String = BuildFilePath(objCustomer.PhysicalFilePath, tempFolder, objectId)
        Dim saveDirPath As String = BuildFilePath(objCustomer.PhysicalFilePath, If(IsNullOrEmpty(prefix), objectId, prefix & objectId))

        If message.Attachments.Count > 6 - iHtmlFile Then
            EnsureDirectoryExists(tempDirPath)
            EnsureDirectoryExists(saveDirPath)

            For Each msgAttachment As Rebex.Mail.Attachment In message.Attachments
                ProcessSingleAttachment(msgAttachment, tempDirPath, whiteList, deniedFiles, files, iPop3DebugLevel, addToSavedFilesList:=False)
            Next

            ' Skapa zip-fil
            Dim zipFilePath As String = Path.Combine(saveDirPath, objectId & ".zip")
            createZipFile(tempDirPath, zipFilePath)
            LogToFile("Attached file path: " & zipFilePath, iPop3DebugLevel)

            ' Vänta för säkerhets skull
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Threading.Thread.Sleep(250)

            SafeDeleteDirectory(tempDirPath)
            files.Add(zipFilePath)

        ElseIf message.Attachments.Count > 0 Then
            EnsureDirectoryExists(saveDirPath)

            For Each msgAttachment As Rebex.Mail.Attachment In message.Attachments
                ProcessSingleAttachment(msgAttachment, saveDirPath, whiteList, deniedFiles, files, iPop3DebugLevel)
            Next
        End If

        ' Spara blockerade filer
        If deniedFiles.Any() Then
            Dim deniedFilesContent As String = String.Join(Environment.NewLine, deniedFiles)
            Dim filePath As String = Path.Combine(saveDirPath, "blocked files.txt")
            File.WriteAllText(filePath, deniedFilesContent)
            files.Add(filePath)
        End If

        Return files
    End Function

    Private Sub EnsureDirectoryExists(path As String)
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
    End Sub

    Private Sub ProcessSingleAttachment(msgAttachment As Rebex.Mail.Attachment,
                                    targetDirectory As String,
                                    whiteList As List(Of String),
                                    deniedFiles As List(Of String),
                                    savedFiles As List(Of String),
                                    debugLevel As Integer,
                                    Optional addToSavedFilesList As Boolean = True)

        Dim sanitizedFileName As String = SanitizeFileName(msgAttachment.FileName)
        Dim extension As String = Path.GetExtension(sanitizedFileName).Replace(".", "").ToLower()

        If IsExtensionInWhiteList(extension, whiteList) Then
            Dim targetPath As String = Path.Combine(targetDirectory, sanitizedFileName)
            Dim i As Integer = 1

            While File.Exists(targetPath)
                Dim nameWithoutExt = Path.GetFileNameWithoutExtension(sanitizedFileName)
                sanitizedFileName = $"{nameWithoutExt}_{i}.{extension}"
                targetPath = Path.Combine(targetDirectory, sanitizedFileName)
                i += 1
            End While

            msgAttachment.Save(targetPath)

            'Using fs As FileStream = File.Open(targetPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            'End Using

            LogToFile("Saved file: " & targetPath, debugLevel)
            If addToSavedFilesList Then savedFiles.Add(targetPath)
        Else
            deniedFiles.Add(sanitizedFileName)
            LogToFile("Blocked file: " & sanitizedFileName, debugLevel)
        End If
    End Sub

    Private Sub SafeDeleteDirectory(path As String)
        Const maxAttempts As Integer = 5
        Const delayMs As Integer = 500

        For attempt As Integer = 1 To maxAttempts
            Try
                If Directory.Exists(path) Then
                    Directory.Delete(path, True)
                End If
                Exit For
            Catch ex As IOException
                If attempt = maxAttempts Then
                    LogToFile("Failed to delete temp folder after retries: " & path, 1)
                    Throw
                End If
                Threading.Thread.Sleep(delayMs)
            End Try
        Next
    End Sub


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
                        ret.UserId = Left(d.Value, 200)
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

    Private Function CreateBase64Images(objCustomer As Customer, ByVal message As MailMessage,
                                            ByVal sFolder As String, ByVal smallBody As String) As String

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
                    If Not IsNullOrEmpty(smallBody) Then
                        sBodyHtml = smallBody
                    End If

                    If Directory.Exists(sFolder) = False Then
                        Directory.CreateDirectory(sFolder)
                    End If
                    ' Kontrollera om det finns några resurser
                    Dim resCol As LinkedResourceCollection = message.Resources
                    If Not resCol Is Nothing Then
                        For k As Integer = 0 To resCol.Count - 1
                            Dim res As LinkedResource = resCol(k)

                            sMediaType = res.MediaType
                            'LogToFile("Image mediaType: " & sMediaType, 1)
                            If sMediaType.Contains("image") Then

                                If Not res.ContentId Is Nothing Then
                                    ' Byt ut cid: i htmlbody
                                    'LogToFile("ContentId: " & res.ContentId.ToString(), 1)
                                    sContentId = res.ContentId.ToString()
                                    sContentId = sContentId.Replace("<", "")
                                    sContentId = sContentId.Replace(">", "")
                                    sContentId = sContentId.Replace("[", "")
                                    sContentId = sContentId.Replace("]", "")

                                    sContentId = "cid:" & sContentId

                                    sFileExtension = sMediaType.Replace("image/", "")
                                    iFileCount = iFileCount + 1

                                    sContentLocation = sFolder & "\" & iFileCount & "." & sFileExtension
                                    res.Save(sContentLocation)
                                    'LogToFile("Saved file: " & sContentLocation, 1)
                                    Dim imgHref As String = ""
                                    Dim bData As Byte()


                                    Dim newImagePath = ResizeImage(sContentLocation, sFolder, iFileCount, sFileExtension)


                                    ' Properly handling the file read operation
                                    Using fs As FileStream = File.OpenRead(newImagePath)
                                        ReDim bData(CInt(fs.Length) - 1)
                                        fs.Read(bData, 0, bData.Length)
                                    End Using ' FileStream is disposed of here, releasing the file

                                    Dim base64String As String = Convert.ToBase64String(bData)
                                    imgHref = "data:image/png;base64," & base64String

                                    sBodyHtml = sBodyHtml.Replace(sContentId, imgHref)

                                End If
                            End If
                        Next
                    End If


                End If
            End If

        Catch ex As Exception
            LogError("Error createBase&4Images MediaType: " & sMediaType & ", " & ex.Message.ToString(), objCustomer)

            'Rethrow
            'Throw
        End Try

        Return sBodyHtml
    End Function

    Private Function CreateHtmlFileFromMail(objCustomer As Customer, ByVal message As MailMessage,
                                            ByVal sFolder As String,
                                            ByVal sCaseNumber As String, ByVal smallBody As String) As String

        Dim sBodyHtml As String = ""
        Dim sFileName As String = ""
        Dim sMediaType As String = ""
        Dim sContentId As String = ""
        Dim sContentLocation As String = ""
        Dim iFileCount As Integer = 0
        Dim sFileExtension As String = ""

        Try
            If Not message Is Nothing Then

                If Directory.Exists(sFolder) = False Then
                    Directory.CreateDirectory(sFolder)
                End If

                If Directory.Exists(sFolder & "\html") = False Then
                    Directory.CreateDirectory(sFolder & "\html")
                End If

                If message.HasBodyHtml Then

                    sBodyHtml = message.BodyHtml
                    If Not IsNullOrEmpty(smallBody) Then
                        sBodyHtml = smallBody
                    End If
                    'Dim apa As String = "/<body[^>]*>((.|[\n\r])*)<\/body>/im"

                    sBodyHtml = Regex.Replace(sBodyHtml, "<base.*?>", "")

                    ' Skapa fil
                    sFileName = sCaseNumber & ".htm"

                    ' Kontrollera om det finns några resurser
                    Dim resCol As LinkedResourceCollection = message.Resources
                    If Not resCol Is Nothing Then


                        For k As Integer = 0 To resCol.Count - 1
                            Dim res As LinkedResource = resCol(k)

                            sMediaType = res.MediaType

                            If sMediaType.Contains("image") Then

                                If Not res.ContentId Is Nothing Then
                                    ' Byt ut cid: i htmlbody
                                    sContentId = res.ContentId.ToString()
                                    sContentId = sContentId.Replace("<", "")
                                    sContentId = sContentId.Replace(">", "")

                                    sContentId = "cid:" & sContentId

                                    sFileExtension = sMediaType.Replace("image/", "")
                                    iFileCount = iFileCount + 1

                                    sContentLocation = sFolder & "\html\" & iFileCount & "." & sFileExtension
                                    res.Save(sFolder & "\html\" & iFileCount & "." & sFileExtension)
                                    Dim imgHref As String = ""
                                    Dim bData As Byte()
                                    Dim br As BinaryReader = New BinaryReader(System.IO.File.OpenRead(sContentLocation))
                                    bData = br.ReadBytes(br.BaseStream.Length)
                                    Dim base64String As String = Convert.ToBase64String(bData, 0, bData.Length)
                                    imgHref = "data:image/png;base64," & base64String

                                    br.Close()
                                    sBodyHtml = sBodyHtml.Replace(sContentId, imgHref)
                                Else
                                    sFileExtension = sMediaType.Replace("image/", "")
                                    iFileCount = iFileCount + 1

                                    ' Spara filen
                                    res.Save(sFolder & "\html\" & iFileCount & "." & sFileExtension)

                                    sContentLocation = IIf(String.IsNullOrWhiteSpace(res.ContentLocation), String.Empty, res.ContentLocation)
                                    'sBodyHtml = sBodyHtml.Replace(sContentLocation, iFileCount & "." & sFileExtension)
                                End If
                            End If
                        Next

                    End If

                    Dim objFile As StreamWriter
                    'Dim objHeaderFile As StreamWriter

                    objFile = New StreamWriter(sFolder & "\html\" & sFileName, False, UnicodeEncoding.UTF8)
                    objFile.Write(sBodyHtml)
                    objFile.Close()

                Else
                    ' Skapa fil
                    Dim objFile As StreamWriter
                    sFileName = sCaseNumber & ".htm"
                    Dim htmlString As String = System.Net.WebUtility.HtmlEncode(message.BodyText).Replace(vbLf, "<br />")
                    htmlString = "<font face=verdana>" & htmlString & "</font>"
                    objFile = New StreamWriter(sFolder & "\html\" & sFileName, False, UnicodeEncoding.UTF8)
                    objFile.Write(htmlString)
                    objFile.Close()

                End If


            End If

        Catch ex As Exception
            LogError("Error createHtmlFileFromMail MediaType: " & sMediaType & ", " & ex.Message.ToString(), objCustomer)

            'Rethrow
            'Throw New Exception
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


            End If

        Catch ex As Exception
            LogError("Error createPDFFileFromMail MediaType: " & sMediaType & ", " & ex.Message.ToString(), objCustomer)

            'Rethrow
            'Throw
        End Try

        'delete html Directory htm file
        'DeleteFilesInsideFolder(sFolder & "\html", True)

        Return pdfFileName
    End Function
    Sub DeleteFilesInsideFolder(ByVal target_folder_path As String, ByVal also_delete_sub_folders As Boolean, Optional ByVal max_retries As Integer = 3, Optional ByVal retry_delay As Integer = 1000)

        ' Check if the target folder exists before proceeding
        If Not Directory.Exists(target_folder_path) Then Exit Sub
        ' Delete each file inside the target folder with retries
        For Each file_path As String In Directory.GetFiles(target_folder_path)
            Dim success As Boolean = False

            For attempt As Integer = 1 To max_retries
                Try
                    If File.Exists(file_path) Then
                        File.SetAttributes(file_path, FileAttributes.Normal) ' Remove read-only flag if set
                        File.Delete(file_path)
                    End If
                    success = True
                    Exit For
                Catch ex As IOException
                    Thread.Sleep(retry_delay)
                End Try
            Next
        Next

        ' If sub-folders should be deleted
        If also_delete_sub_folders Then

            ' Delete each sub-folder recursively with retries
            For Each sub_folder_path As String In Directory.GetDirectories(target_folder_path)
                Dim folderSuccess As Boolean = False

                For attempt As Integer = 1 To max_retries
                    Try
                        If Directory.Exists(sub_folder_path) Then
                            Directory.Delete(sub_folder_path, True)
                        End If
                        folderSuccess = True
                        Exit For
                    Catch ex As IOException
                        Thread.Sleep(retry_delay)
                    End Try
                Next
            Next

            ' Garbage collect to release any lingering file handles before deleting the target folder
            GC.Collect()
            GC.WaitForPendingFinalizers()

            ' Delete the target folder itself with retries
            Dim finalSuccess As Boolean = False
            For attempt As Integer = 1 To max_retries
                Try
                    If Directory.Exists(target_folder_path) Then
                        Directory.Delete(target_folder_path, True)
                    End If
                    finalSuccess = True
                    Exit For
                Catch ex As IOException
                    Thread.Sleep(retry_delay)
                End Try
            Next

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

    Private Sub createZipFile(sourceDir As String, zipFilePath As String)
        Try
            ' Ta bort tidigare zip om den finns
            If File.Exists(zipFilePath) Then
                File.Delete(zipFilePath)
            End If

            ' Skapa zip från mappen
            ZipFile.CreateFromDirectory(sourceDir, zipFilePath, CompressionLevel.Optimal, includeBaseDirectory:=False)
        Catch ex As Exception
            LogToFile("Zip creation failed: " & ex.Message, 1)
            Throw
        End Try
    End Sub

    Private Function convertHTMLtoText(ByVal sHTML As String) As String
        Dim startTime As DateTime
        Dim MyWebBrowser As New WebBrowser
        sHTML = Encoding.UTF8.GetString(Encoding.Default.GetBytes(sHTML))

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

    Private Function getInnerHtml(ByVal sHTML As String) As String
        Dim doc As New HtmlAgilityPack.HtmlDocument()
        doc.LoadHtml(sHTML)

        Dim bodyNode As HtmlNode = doc.DocumentNode.SelectSingleNode("//body")
        If bodyNode IsNot Nothing Then
            'Return bodyNode.InnerHtml.Replace(vbCrLf, "").Replace("'", "''")
            Return bodyNode.InnerHtml

        Else
            'Return sHTML.Replace(vbCrLf, "").Replace("'", "''")
            Return sHTML
        End If

    End Function


    Private Function isBlockedRecipient(sEMail As String, sBlockedEMailRecipents As String) As Boolean
        ' Return False if sEMail or sBlockedEMailRecipients are empty or contain invalid characters
        If String.IsNullOrWhiteSpace(sEMail) Or String.IsNullOrWhiteSpace(sBlockedEMailRecipents) Then
            Return False
        End If
        ' Split sBlockedEMailRecipients into an array of strings using the semicolon as a delimiter
        Dim aEMails() As String = sBlockedEMailRecipents.Split(";")
        If aEMails.Length = 0 Then
            Return False
        End If

        For Each pattern As String In aEMails
            If Not String.IsNullOrWhiteSpace(pattern) Then
                ' Check if sEMail contains the pattern using a case-insensitive comparison
                If sEMail.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0 Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Function isValidRecipient(sEMail As String, sAllowedEMailRecipents As String) As Boolean
        isValidRecipient = False

        If sAllowedEMailRecipents = "" Then
            isValidRecipient = True
        Else
            Dim aEMails() As String = sAllowedEMailRecipents.Split(";")

            For i As Integer = 0 To aEMails.Length - 1
                If InStr(sEMail, aEMails(i).ToString(), CompareMethod.Text) <> 0 Then

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
                        Dim endTag As String
                        endTag = "<"
                        toPos = mailMessage.ToLower().IndexOf(endTag, fromPos)

                        If (toPos < 1) Then
                            endTag = "</"
                            toPos = mailMessage.IndexOf(endTag, fromPos)
                        End If

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
                Dim fieldValue As String = GetValueFromKeyValuePair(fields, d.Key)

                fieldValue = System.Web.HttpUtility.HtmlDecode(fieldValue).Trim()

                Select Case d.Key.ToLower
                    Case "reportedby"
                        c.ReportedBy = Left(fieldValue, 200)
                    Case "persons_name"
                        c.Persons_Name = Left(fieldValue, 50)
                    Case "persons_email"
                        c.Persons_EMail = Left(fieldValue, 100)
                    Case "persons_phone"
                        c.Persons_Phone = Left(fieldValue, 50)
                    Case "persons_cellphone"
                        c.Persons_CellPhone = Left(fieldValue, 50)
                    Case "place"
                        c.Place = Left(fieldValue, 100)
                    Case "description"
                        c.Description = d.Value
                    Case "miscellaneous"
                        c.Miscellaneous = Left(fieldValue, 1000)
                    Case "available"
                        c.Available = Left(fieldValue, 100)
                    Case "invoicenumber"
                        c.InvoiceNumber = Left(fieldValue, 50)
                    Case "referencenumber"
                        c.ReferenceNumber = Left(fieldValue, 200)
                    Case "usercode"
                        c.UserCode = Left(fieldValue, 20)
                    Case "inventorynumber"
                        c.InventoryNumber = Left(fieldValue, 20)
                    Case "inventorylocation"
                        ' TODO
                    Case "department_id"
                        If departments IsNot Nothing Then
                            If departments.Count > 0 Then
                                For Each dp As Department In departments
                                    If ReturnStringForCompare(dp.Department) = ReturnStringForCompare(fieldValue) Then
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
                                    If ReturnStringForCompare(p.Name) = ReturnStringForCompare(fieldValue) Then
                                        c.Priority_Id = p.Id
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Case "productarea_id"
                        Dim pa As ProductArea = ReturnProductArea(customerid, fieldValue)
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
    Private Function StripTags(ByVal inputString As String) As String
        Dim result As String = System.Text.RegularExpressions.Regex.Replace(inputString, "(?i)<(?!br)[^>]*>", "") ' Remove all HTML tags except <br>

        result = result.Split({"<br>"}, StringSplitOptions.None)(0).Trim()
        result = result.Split({"<br />"}, StringSplitOptions.None)(0).Trim()

        Return result
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

    Private Function GetValueFromKeyValuePair(fields As Dictionary(Of String, String), key As String) As String
        Dim ret As String = String.Empty

        If fields IsNot Nothing Then
            For Each d As KeyValuePair(Of String, String) In fields
                If Not IsNullOrEmpty(d.Value) Then
                    If d.Key.ToLower = key.ToLower() Then
                        ret = StripTags(d.Value.Trim())
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
        'check if objCustomer not is null
        If objCustomer IsNot Nothing Then
            SendErrorMail(msg, objCustomer)
        End If

    End Sub

    Private Sub SendErrorMail(msg As String, objCustomer As Customer)
        Try
            Dim sConnectionstring As String = ConfigurationManager.ConnectionStrings("Helpdesk")?.ConnectionString
            Dim objGlobalSettingsData As New GlobalSettingsData
            MGlobal.gsConnectionString = sConnectionstring
            Dim objGlobalSettings As GlobalSettings
            objGlobalSettings = objGlobalSettingsData.getGlobalSettings()

            Dim toAddress As String = objCustomer.ErrorMailTo
            Dim fromAddress As String = objCustomer.HelpdeskEMail
            If (Not IsNullOrEmpty(objGlobalSettings.SMTPServer) And Not IsNullOrEmpty(toAddress) And Not IsNullOrEmpty(fromAddress)) Then

                Try
                    Dim objMail As New Mail
                    objMail.SendErrorMail(fromAddress, toAddress, "Error in M2T", msg, sConnectionstring, objGlobalSettings.SMTPServer, objCustomer)

                Catch ex As Exception
                    If objErrorLogFile IsNot Nothing Then
                        objErrorLogFile.WriteLine("{0}: {1}", Now(), ex.Message)
                    End If
                End Try
            End If
        Catch ex As Exception
            LogError("Error in SendErrorMail" & ex.Message.ToString(), Nothing)
        End Try


    End Sub
    Public Function CleanHtml(ByVal input As String) As String
        Dim doc As New HtmlAgilityPack.HtmlDocument()
        doc.LoadHtml(input)
        ' Iterate through all nodes in the HTML document
        For Each node As HtmlNode In doc.DocumentNode.DescendantsAndSelf()
            ' Check if the node is an element node
            If node.NodeType = HtmlNodeType.Element Then
                ' Iterate through all attributes of the node
                For Each attribute As HtmlAttribute In node.Attributes
                    ' Replace single quotes with &quote; in attribute values
                    attribute.Value = attribute.Value.Replace("'", "&quot;")
                Next
            ElseIf node.NodeType = HtmlNodeType.Text Then
                ' Replace single quotes with &quote; in text content
                node.InnerHtml = node.InnerHtml.Replace("'", "&quot;")
            End If
        Next
        ' Remove divs that only contain other divs Or br tags
        Dim divNodes As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//div[not(*) or (*[self::div or self::br]) and not(text()[normalize-space()])]")
        If divNodes IsNot Nothing Then
            For Each emptydiv As HtmlNode In divNodes
                If emptydiv.ParentNode IsNot Nothing Then
                    For Each child As HtmlNode In emptydiv.ChildNodes
                        emptydiv.ParentNode.InsertBefore(child, emptydiv)
                    Next
                    emptydiv.Remove()
                End If
            Next
        End If

        ' Remove <br> at the very start of the document
        Dim firstNode As HtmlNode = doc.DocumentNode.ChildNodes.FirstOrDefault()
        While firstNode IsNot Nothing AndAlso firstNode.Name = "br"
            firstNode.Remove()
            firstNode = doc.DocumentNode.ChildNodes.FirstOrDefault()
        End While

        ' Remove <br> at the very end of the document
        Dim lastNode As HtmlNode = doc.DocumentNode.ChildNodes.LastOrDefault()
        While lastNode IsNot Nothing AndAlso lastNode.Name = "br"
            lastNode.Remove()
            lastNode = doc.DocumentNode.ChildNodes.LastOrDefault()
        End While

        ' Remove <p><br></p> sequences
        Dim pNodesWithBr As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//p[br and not(node()[2])]")
        If pNodesWithBr IsNot Nothing Then
            For Each p As HtmlNode In pNodesWithBr
                p.Remove()
            Next
        End If

        ' Remove empty p tags
        Dim pNodes As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//p[not(node())]")
        If pNodes IsNot Nothing Then
            For Each p As HtmlNode In pNodes
                p.Remove()
            Next
        End If

        Return doc.DocumentNode.OuterHtml
    End Function
    Public Function CleanStyles(ByVal input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return input
        End If

        Dim doc As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
        doc.LoadHtml(input)


        Dim imgs As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//img")

        If imgs IsNot Nothing Then

            For Each img As HtmlNode In imgs
                If img.Attributes("style") IsNot Nothing Then
                    img.Attributes("style").Remove()
                End If
                If img.Attributes("id") IsNot Nothing Then
                    img.Attributes("id").Remove()
                End If

            Next
        End If

        Dim par As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//p")

        If par IsNot Nothing Then

            For Each p As HtmlNode In par
                If p.Attributes("class") IsNot Nothing Then

                    p.Attributes("class").Remove()
                End If
            Next
        End If

        Dim div As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//div")

        If div IsNot Nothing Then

            For Each d As HtmlNode In div
                If d.Attributes("class") IsNot Nothing Then
                    d.Attributes("class").Remove()
                End If
                If d.Attributes("style") IsNot Nothing Then
                    d.Attributes("style").Remove()
                End If
            Next
        End If

        Dim form As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//form")
        If form IsNot Nothing Then
            For Each f As HtmlNode In form
                f.Remove()
            Next
        End If

        Dim inputTags As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//input")
        If inputTags IsNot Nothing Then
            For Each i As HtmlNode In inputTags
                i.Remove()
            Next
        End If

        Dim buttons As HtmlNodeCollection = doc.DocumentNode.SelectNodes("//button")
        If buttons IsNot Nothing Then
            For Each b As HtmlNode In buttons
                b.Remove()
            Next
        End If

        Dim scriptTags As IEnumerable(Of HtmlNode) = doc.DocumentNode.Descendants("script")

        For Each scriptTag As HtmlNode In scriptTags.ToList()
            scriptTag.Remove()
        Next

        Dim baseTags As IEnumerable(Of HtmlNode) = doc.DocumentNode.Descendants("base")

        For Each base As HtmlNode In baseTags.ToList()
            base.Remove()
        Next

        Dim nodesToRemove As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//comment()[starts-with(normalize-space(.),'<?xml:')]")
        If nodesToRemove IsNot Nothing Then
            For Each node As HtmlAgilityPack.HtmlNode In nodesToRemove
                node.Remove()
            Next
        End If
        'Todo - Check this out
        Dim xmlTagsToRemove As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//comment()[starts-with(normalize-space(.),'<!--?xml:')]")
        If xmlTagsToRemove IsNot Nothing Then
            For Each node As HtmlAgilityPack.HtmlNode In xmlTagsToRemove
                node.Remove()
            Next
        End If

        Dim allNodes As HtmlNode = doc.DocumentNode
        Dim ret = allNodes.InnerHtml
        Return ret
    End Function

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
    Private Function ResizeImage(contentLocation As String, folder As String, iFileCount As Integer, fileExtension As String) As String
        Dim newFilePath As String = System.IO.Path.Combine(folder, $"{iFileCount}_small.{fileExtension}")

        ' The 'Using' block ensures that IDisposable objects are correctly disposed, releasing resources
        Using originalImage As Bitmap = New Bitmap(contentLocation) ' Load the original image

            Dim newWidth As Integer = 500
            Dim newHeight As Integer = originalImage.Height

            ' Calculate the new dimensions
            If originalImage.Width > newWidth Then
                newHeight = originalImage.Height * newWidth / originalImage.Width
            Else
                newWidth = originalImage.Width
            End If

            ' Fallback to prevent zero dimensions
            newHeight = If(newHeight = 0, 1, newHeight)
            newWidth = If(newWidth = 0, 1, newWidth)

            ' Using statement ensures proper disposal of newImage instance after usage
            Using newImage As New Bitmap(newWidth, newHeight)

                ' Using statement for proper disposal of graphics object
                Using graphics As Graphics = Graphics.FromImage(newImage)

                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic

                    ' Draw the original image on the new image using the specified width and height
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight)
                End Using ' Graphics object is disposed here

                newImage.Save(newFilePath) ' Save the new image
            End Using ' New Image object is disposed here

        End Using ' Original Bitmap object is disposed here

        Return newFilePath
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


