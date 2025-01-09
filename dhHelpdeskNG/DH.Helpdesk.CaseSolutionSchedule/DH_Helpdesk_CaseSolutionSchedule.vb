Imports System.Configuration
Imports System.Data.SqlClient
Imports DH.Helpdesk.Library
Imports System.IO
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.Common.Constants
Imports System.Threading.Tasks
Imports System.Net.Mail

Module DH_Helpdesk_CaseSolutionSchedule
    'Private objLogFile As StreamWriter

    Public Const ConnectionStringName As String = "Helpdesk"

    Public Sub Main()

        Dim args As String() = Environment.GetCommandLineArgs()
        Dim delaySecondsSetting As String = ConfigurationManager.AppSettings("DelaySeconds")
        Dim delaySequence As List(Of Integer) = delaySecondsSetting.Split(","c).Select(Function(s) Integer.Parse(s.Trim())).ToList()

        ' You can send in two arguments.
        ' First argument should be a date like "2025-01-01 07:05:00" or the specific time you want to run the CaseSolutionSchedule.
        ' Second argument should be a workmode. If workmode is 1, the CaseSolutionSchedule will only list the CaseSolutions that would have been created.
        ' If workmode is 0, the CaseSolutions will be created.
        ' If you don't send in any arguments, the CaseSolutionSchedule will run with the current date and time and workmode 0.

        Dim defaultDateAndTime As DateTime = DateTime.Now
        Dim defaultWorkMode As Integer = 0

        Dim dateAndTime As DateTime
        Dim workMode As Integer

        If args.Length > 1 AndAlso DateTime.TryParse(args(1), dateAndTime) Then
            ' Use the provided dateAndTime argument
        Else
            dateAndTime = defaultDateAndTime
        End If

        If args.Length > 2 AndAlso Integer.TryParse(args(2), workMode) Then
            ' Use the provided workMode argument
        Else
            workMode = defaultWorkMode
        End If

        ' encrypt connection string if exists
        Dim secureConnectionString = ConfigurationManager.AppSettings("SecureConnectionString")
        If (Not String.IsNullOrEmpty(secureConnectionString) AndAlso Boolean.Parse(secureConnectionString)) Then
            Dim fileName = Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location)
            SecureConnectionStringSection(fileName)
        End If

        Dim sConnectionstring = GetConnectionString()

        Try
            giLoglevel = 0
            createLogFile()
            LogToFile(dateAndTime & " Starting CaseSolutionSchedule")
            caseSolutionSchedule(sConnectionstring, dateAndTime, workMode, delaySequence)
            LogToFile(dateAndTime & " End of CaseSolutionSchedule")
            closeLogFile()
        Catch ex As Exception
            LogError(ex.ToString())
        Finally
            closeLogFile()
            closeErrorLogFile()
        End Try

    End Sub

    Private Function GetConnectionString() As String
        Return ConfigurationManager.ConnectionStrings(ConnectionStringName)?.ConnectionString
    End Function
    Private Sub LogToFile(msg As String)
        If objLogFile IsNot Nothing Then
            objLogFile.WriteLine("{0}: {1}", Now(), msg)
        End If
    End Sub
    Private Sub LogError(msg As String)
        If objErrorLogFile Is Nothing Then
            createErrorLogFile()
        End If
        If objErrorLogFile IsNot Nothing Then
            objErrorLogFile.WriteLine("{0}: {1}", Now(), msg)
        End If
    End Sub
    Private Sub SecureConnectionStringSection(exeConfigName As String)
        createLogFile()
        Try
            EncryptSection(Of ConnectionStringsSection)(exeConfigName, "connectionStrings")
            LogToFile("app.config section is protected")
        Catch ex As Exception
            LogError(ex.ToString())
        Finally
            closeLogFile()
        End Try

    End Sub

    Private Sub EncryptSection(Of TSection As ConfigurationSection)(exeConfigName As String, sectionName As String)

        Dim config = ConfigurationManager.OpenExeConfiguration(exeConfigName)

        Dim section = CType(config.GetSection(sectionName), TSection)
        If section IsNot Nothing Then
            If (Not section.SectionInformation.IsProtected) Then
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
            End If

            config.Save()
        End If
    End Sub

    Private Sub caseSolutionSchedule(ByVal sConnectionString As String, ByVal dateAndTime As DateTime, ByVal workMode As Integer, ByVal delaySequence As List(Of Integer))
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
        Dim objLog As New Log
        Dim sMessageId As String

        gsConnectionString = sConnectionString

        objGlobalSettings = RetryDatabaseOperationWithDelays(Function() objGlobalSettingsData.getGlobalSettings(), delaySequence, "getGlobalSettings")
        giDBType = objGlobalSettings.DBType

        Dim colCase As Collection = RetryDatabaseOperationWithDelays(Function() objCaseData.getCaseSolutionSchedule(dateAndTime), delaySequence, "getCaseSolutionSchedule")


        LogToFile(dateAndTime & " Number of CaseSolutions found at this time and date:" & colCase.Count)
        For i As Integer = 1 To colCase.Count
            Try
                objCaseSolution = colCase(i)
                Dim cslId = objCaseSolution.CaseSolution_Id
                If (workMode = 1) Then
                    LogToFile(dateAndTime & " " & objCaseSolution.Caption & ": CaseSolution_Id: " & cslId)
                    Continue For
                End If

                objCase = RetryDatabaseOperationWithDelays(Function() objCaseData.createCaseFromCaseSolutionScedule(objCaseSolution), delaySequence, "createCaseFromCaseSolutionScedule")

                LogToFile(dateAndTime & " " & objCaseSolution.Caption & ": CaseSolution_Id: " & cslId & " - Created CaseNumber:" & objCase.Casenumber & ", Case_Id: " & objCase.Id)

                Dim isAboutObj As ComputerUser = RetryDatabaseOperationWithDelays(Function() getIsAboutData(objCaseSolution), delaySequence, "getIsAboutData")
                objCaseData.saveCaseIsAbout(objCase.Id, isAboutObj)

                Dim iCaseHistory_Id As Integer = RetryDatabaseOperationWithDelays(Function() objCaseData.saveCaseHistory(objCase.Id, "DH Helpdesk"), delaySequence, "saveCaseHistory")

                objCustomer = RetryDatabaseOperationWithDelays(Function() objCustomerData.getCustomerById(objCase.Customer_Id), delaySequence, "getCustomerById")

                objUser = RetryDatabaseOperationWithDelays(Function() objUserData.getUserById(objCase.Performer_User_Id), delaySequence, "getUserById")

                If objCaseSolution.Log.Count > 0 Then
                    If objCaseSolution.Log(0).Text_Internal <> "" Or objCaseSolution.Log(0).Text_External <> "" Then
                        RetryDatabaseOperationWithDelays(Function() objLogData.createLog(objCase.Id, objCase.Persons_EMail, objCaseSolution.Log(0).Text_Internal, objCaseSolution.Log(0).Text_External, 0, "DH Helpdesk", iCaseHistory_Id, 0), delaySequence, "createLog")
                        objLog.Text_External = objCaseSolution.Log(0).Text_External
                        objLog.Text_Internal = objCaseSolution.Log(0).Text_Internal
                    End If
                End If

                If objCase.Performer_User_Id <> 0 And objCase.PerformerEMail <> "" Then
                    objMailTemplate = RetryDatabaseOperationWithDelays(Function() objMailTemplateData.getMailTemplateById(SharedFunctions.EMailType.EMailAssignCasePerformer, objCase.Customer_Id, objCase.RegLanguage_Id, objGlobalSettings.DBVersion), delaySequence, "getMailTemplateById")

                    If objMailTemplate IsNot Nothing Then
                        sMessageId = RetryDatabaseOperationWithDelays(Function() createMessageId(objCustomer.HelpdeskEMail), delaySequence, "createMessageId")

                        Dim sSendTime As DateTime = Date.Now()
                        Dim sEMailLogGUID As String = System.Guid.NewGuid().ToString
                        Dim objMail As New Mail
                        Dim sRet_SendMail As String = objMail.sendMail(objCase, objLog, objCustomer, objUser.EMail, objMailTemplate, objGlobalSettings, sMessageId, sEMailLogGUID, gsConnectionString)
                        RetryDatabaseOperationWithDelays(Sub() objLogData.createEMailLog(iCaseHistory_Id, objUser.EMail, SharedFunctions.EMailType.EMailAssignCasePerformer, sMessageId, sSendTime, sEMailLogGUID, sRet_SendMail), delaySequence, "createEMailLog")
                    End If
                End If

                If objCaseSolution.ExtendedCaseFormId IsNot Nothing Then
                    If objCaseSolution.ExtendedCaseFormId.HasValue And objCaseSolution.ExtendedCaseFormId.Value > 0 Then
                        Dim extendedCaseDataId = RetryDatabaseOperationWithDelays(Function() objExtendedCaseService.CreateExtendedCaseData(objCaseSolution.ExtendedCaseFormId.Value), delaySequence, "CreateExtendedCaseData")
                        RetryDatabaseOperationWithDelays(Sub() objCaseData.CreateExtendedCaseConnection(objCase.Id, objCaseSolution.ExtendedCaseFormId.Value, extendedCaseDataId), delaySequence, "CreateExtendedCaseConnection")
                    End If
                End If
            Catch ex As Exception
                LogError(ex.ToString())
            End Try

        Next
        'End If

    End Sub
    Public Function RetryDatabaseOperationWithDelays(Of T)(
    operation As Func(Of T),
    delaySequence As List(Of Integer),
    operationName As String) As T

        Dim attempt As Integer = 0

        Do While attempt <= delaySequence.Count
            Try
                ' Attempt the database operation
                Return operation()
            Catch ex As Exception
                attempt += 1
                ' If maximum retries reached, send an error email and rethrow the exception
                If attempt > delaySequence.Count Then
                    LogError($"[{operationName}] Max retry attempts reached. Sending error email.")
                    SendErrorEmail($"Error in caseSolutionSchedule: {operationName}", $"Operation: {operationName} failed after {delaySequence.Count} attempts. Error: {ex.Message}")
                    Throw
                End If
                LogError($"[{operationName}] failed on attempt {attempt}. Error: {ex.Message}")
                ' Wait for the specified delay for this attempt
                Dim delayMs As Integer = delaySequence(attempt - 1) * 1000 ' Convert seconds to milliseconds
                LogError($"[{operationName}] Retrying after {delayMs / 1000} seconds...")
                Threading.Thread.Sleep(delayMs)
            End Try
        Loop

        ' This line should never be reached
        Throw New InvalidOperationException("Retry logic failed unexpectedly.")
    End Function

    Public Sub RetryDatabaseOperationWithDelays(
    operation As Action,
    delaySequence As List(Of Integer),
    operationName As String)

        Dim attempt As Integer = 0

        Do While attempt < delaySequence.Count
            Try
                ' Attempt the database operation
                operation()
                Return ' Exit after successful execution
            Catch ex As Exception
                attempt += 1
                LogError($"[{operationName}] failed on attempt {attempt}. Error: {ex.Message}")

                ' If maximum retries reached, send an error email and rethrow the exception
                If attempt >= delaySequence.Count Then
                    LogError($"[{operationName}] Max retry attempts reached. Sending error email.")
                    SendErrorEmail($"Error in caseSolutionSchedule: {operationName}", $"Operation: {operationName} failed after {attempt} attempts. Error: {ex.Message}")
                    Throw
                End If

                ' Wait for the specified delay for this attempt
                Dim delayMs As Integer = delaySequence(attempt - 1) * 1000 ' Convert seconds to milliseconds
                LogError($"[{operationName}] Retrying after {delayMs / 1000} seconds...")
                Threading.Thread.Sleep(delayMs)
            End Try
        Loop

        ' This line should never be reached
        Throw New InvalidOperationException("Retry logic failed unexpectedly.")
    End Sub

    Public Sub SendErrorEmail(subject As String, body As String)
        Try
            ' Get email configuration from app.config
            Dim smtpServer As String = ConfigurationManager.AppSettings("SMTPServer")
            Dim smtpPort As Integer = Integer.Parse(ConfigurationManager.AppSettings("SMTPPort"))
            Dim emailRecipient As String = ConfigurationManager.AppSettings("ErrorEmailRecipient")
            Dim emailSender As String = ConfigurationManager.AppSettings("ErrorEmailSender")

            ' Configure the SMTP client
            Dim smtpClient As New SmtpClient(smtpServer, smtpPort) With {
            .EnableSsl = False ' Disable SSL if required for anonymous sending
        }

            ' Create the email message
            Dim mailMessage As New MailMessage(emailSender, emailRecipient) With {
            .Subject = subject,
            .Body = body
        }

            ' Send the email anonymously
            smtpClient.Send(mailMessage)

        Catch ex As Exception
            LogError(Now() & "Error sending Error mail from caseSolutionSchedule." & ex.Message)
        End Try
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

    Private Sub createLogFile()
        Dim sFileName As String
        Dim sTemp As String

        Dim sPath As String = Environment.CurrentDirectory & "\log\"

        Directory.CreateDirectory(sPath)

        sFileName = sPath & "DH_Helpdesk_CaseSolutionSchedule_" & DatePart(DateInterval.Year, Now())

        sTemp = DatePart(DateInterval.Month, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Day, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp & ".log"

        objLogFile = New StreamWriter(sFileName, True)

    End Sub
    Private Sub createErrorLogFile()
        Dim sFileName As String
        Dim sTemp As String

        Dim sPath As String = Environment.CurrentDirectory & "\log\"

        Directory.CreateDirectory(sPath)

        sFileName = sPath & "DH_Helpdesk_CaseSolutionSchedule_Error_" & DatePart(DateInterval.Year, Now())

        sTemp = DatePart(DateInterval.Month, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Day, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp & ".log"

        objErrorLogFile = New StreamWriter(sFileName, True)

    End Sub

    Private Sub closeLogFile()
        objLogFile.Close()
    End Sub

    Private Sub closeErrorLogFile()
        If objErrorLogFile IsNot Nothing Then
            objErrorLogFile.Close()
        End If
    End Sub

End Module
