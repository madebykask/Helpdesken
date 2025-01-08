Imports System.Configuration
Imports System.Data.SqlClient
Imports DH.Helpdesk.Library
Imports System.IO
Imports DH.Helpdesk.Library.SharedFunctions
Imports DH.Helpdesk.Common.Constants

Module DH_Helpdesk_CaseSolutionSchedule
    'Private objLogFile As StreamWriter

    Public Const ConnectionStringName As String = "Helpdesk"

    Public Sub Main()

        Dim args As String() = Environment.GetCommandLineArgs()

        ' Första argumentet är vanligtvis exe-filen, så börja från index 1
        ' Default values for the optional arguments

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
            caseSolutionSchedule(sConnectionstring, dateAndTime, workMode)
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

    Private Sub caseSolutionSchedule(ByVal sConnectionString As String, ByVal dateAndTime As DateTime, ByVal workMode As Integer)
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

        objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
        giDBType = objGlobalSettings.DBType
        Dim colCase As Collection = objCaseData.getCaseSolutionSchedule(dateAndTime)

        LogToFile(dateAndTime & " Number of CaseSolutions found at this time and date:" & colCase.Count)
        For i As Integer = 1 To colCase.Count
            Try
                objCaseSolution = colCase(i)
                Dim cslId = objCaseSolution.CaseSolution_Id
                If (workMode = 1) Then
                    LogToFile(dateAndTime & " " & objCaseSolution.Caption & ": CaseSolution_Id: " & cslId)
                    Continue For
                End If

                objCase = objCaseData.createCaseFromCaseSolutionScedule(objCaseSolution)
                LogToFile(dateAndTime & " " & objCaseSolution.Caption & ": CaseSolution_Id: " & cslId & " - Created CaseNumber:" & objCase.Casenumber & ", Case_Id: " & objCase.Id)

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
            Catch ex As Exception
                LogError(ex.ToString())
            End Try

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
