Imports System.IO
Imports System.Linq
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Mail
Imports System.Text
Imports DH.Helpdesk.Common.Constants
Imports DH.Helpdesk.Dal.Infrastructure
Imports DH.Helpdesk.Dal.Repositories
Imports DH.Helpdesk.Domain
Imports DH.Helpdesk.Library.SharedFunctions
Imports iTextSharp.text.log
Imports Newtonsoft.Json

Public Class Mail
    Private _logger As ILogger ' Assuming ILogger is defined elsewhere


    Public Function sendMail(objCase As CCase,
                             objLog As Log,
                             objCustomer As Customer,
                             sEmailTo As String,
                             objmailTemplate As MailTemplate,
                             objGlobalSettings As GlobalSettings,
                             sMessageId As String,
                             sEMailLogGUID As String,
                             connectionString As String,
                             Optional files As List(Of MailFile) = Nothing
                             ) As String
        ' Skicka mail
        Dim sSubject As String
        Dim sBody As String
        Dim sLink As String
        Dim sLinkSelfService As String
        Dim sProtocol As String
        Dim sRet As String = ""

        Try

            sSubject = objmailTemplate.Subject
            sBody = objmailTemplate.Body

            Dim setting As Setting
            Using factory As DatabaseFactory = New DatabaseFactory(connectionString)

                Dim settingsRepository As New SettingRepository(factory)
                setting = settingsRepository.Get(Function(x) x.Customer_Id = objCustomer.Id)

            End Using

            '[#1]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("CaseNumber"), objCase.Casenumber)
            sBody = Replace(sBody, getMailTemplateIdentifier("CaseNumber"), objCase.Casenumber)

            '[#2]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("CustomerName"), objCase.CustomerName)
            sBody = Replace(sBody, getMailTemplateIdentifier("CustomerName"), objCase.CustomerName)

            '[#3]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Persons_Name"), objCase.Persons_Name)
            sBody = Replace(sBody, getMailTemplateIdentifier("Persons_Name"), objCase.Persons_Name)

            '[#4]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Caption"), objCase.Caption)
            sBody = Replace(sBody, getMailTemplateIdentifier("Caption"), objCase.Caption)

            '[#5]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Description"), objCase.Description)
            sBody = Replace(sBody, getMailTemplateIdentifier("Description"), Replace(objCase.Description, vbCrLf, "<br>"))

            '[#6]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("FirstName"), objCase.PerformerFirstName)
            sBody = Replace(sBody, getMailTemplateIdentifier("FirstName"), objCase.PerformerFirstName)

            '[#7]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("SurName"), objCase.PerformerSurName)
            sBody = Replace(sBody, getMailTemplateIdentifier("SurName"), objCase.PerformerSurName)

            '[#8]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Persons_EMail"), objCase.Persons_EMail)
            sBody = Replace(sBody, getMailTemplateIdentifier("Persons_EMail"), objCase.Persons_EMail)

            '[#9]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Persons_Phone"), objCase.Persons_Phone)
            sBody = Replace(sBody, getMailTemplateIdentifier("Persons_Phone"), objCase.Persons_Phone)

            Dim textExternal = If(Not objLog Is Nothing, objLog.Text_External, "")
            Dim textInternal = If(Not objLog Is Nothing, objLog.Text_Internal, "")
            '[#10]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Text_External"), textExternal)
            sBody = Replace(sBody, getMailTemplateIdentifier("Text_External"), textExternal)

            '[#11]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Text_Internal"), textInternal)
            sBody = Replace(sBody, getMailTemplateIdentifier("Text_Internal"), textInternal)

            '[#12]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("PriorityName"), objCase.PriorityName)
            sBody = Replace(sBody, getMailTemplateIdentifier("PriorityName"), objCase.PriorityName)

            '[#13]
            If objCustomer.CaseWorkingGroupSource = 0 Then
                sSubject = Replace(sSubject, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.PerformerWorkingGroupEMail)
                sBody = Replace(sBody, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.PerformerWorkingGroupEMail)
            Else
                sSubject = Replace(sSubject, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.WorkingGroupEMail)
                sBody = Replace(sBody, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.WorkingGroupEMail)
            End If

            '[#14]
            Dim bAttachExternalFiles = False
            If (sBody.Contains("[#14]")) Then
                bAttachExternalFiles = True
                sBody = sBody.Replace("[#14]", String.Empty)
            End If

            '[#30] - Internal Log files (2attachments mode)
            Dim bAttachInternalFiles = False
            If (sBody.Contains("[#30]")) Then
                bAttachInternalFiles = True
                sBody = sBody.Replace("[#30]", String.Empty)
            End If

            '[#15]
            If objCustomer.CaseWorkingGroupSource = 0 Then
                sSubject = Replace(sSubject, getMailTemplateIdentifier("WorkingGroup"), objCase.PerformerWorkingGroup)
                sBody = Replace(sBody, getMailTemplateIdentifier("WorkingGroup"), objCase.PerformerWorkingGroup)
            Else
                sSubject = Replace(sSubject, getMailTemplateIdentifier("WorkingGroup"), objCase.CaseWorkingGroup)
                sBody = Replace(sBody, getMailTemplateIdentifier("WorkingGroup"), objCase.CaseWorkingGroup)
            End If

            '[#16]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("RegTime"), objCase.RegTime)
            sBody = Replace(sBody, getMailTemplateIdentifier("RegTime"), objCase.RegTime)

            '[#17]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("InventoryNumber"), objCase.InventoryNumber)
            sBody = Replace(sBody, getMailTemplateIdentifier("InventoryNumber"), objCase.InventoryNumber)

            '[#18]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Persons_CellPhone"), objCase.Persons_CellPhone)
            sBody = Replace(sBody, getMailTemplateIdentifier("Persons_CellPhone"), objCase.Persons_CellPhone)

            '[#19]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Available"), objCase.Available)
            sBody = Replace(sBody, getMailTemplateIdentifier("Available"), objCase.Available)

            '[#20]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Priority_Description"), objCase.PriorityDescription)
            sBody = Replace(sBody, getMailTemplateIdentifier("Priority_Description"), objCase.PriorityDescription)

            '[#21]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("WatchDate"), objCase.WatchDate.ToString())
            sBody = Replace(sBody, getMailTemplateIdentifier("WatchDate"), objCase.WatchDate.ToString())

            '[#22]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("LastChangedByUser"), objCase.ChangedName + " " + objCase.ChangedSurName)
            sBody = Replace(sBody, getMailTemplateIdentifier("LastChangedByUser"), objCase.ChangedName + " " + objCase.ChangedSurName)

            '[#23]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Miscellaneous"), objCase.Miscellaneous)
            sBody = Replace(sBody, getMailTemplateIdentifier("Miscellaneous"), objCase.Miscellaneous)

            '[#24]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Place"), objCase.Place)
            sBody = Replace(sBody, getMailTemplateIdentifier("Place"), objCase.Place)

            '[#25]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("CaseType"), objCase.CaseTypeName)
            sBody = Replace(sBody, getMailTemplateIdentifier("CaseType"), objCase.CaseTypeName)

            '[#26]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Category"), objCase.CategoryName)
            sBody = Replace(sBody, getMailTemplateIdentifier("Category"), objCase.CategoryName)

            '[#27]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("ProductArea"), objCase.ProductAreaName)
            sBody = Replace(sBody, getMailTemplateIdentifier("ProductArea"), objCase.ProductAreaName)

            '[#28]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("ReportedBy"), objCase.ReportedBy)
            sBody = Replace(sBody, getMailTemplateIdentifier("ReportedBy"), objCase.ReportedBy)

            '[#29]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("RegUser"), objCase.RegUserName)
            sBody = Replace(sBody, getMailTemplateIdentifier("RegUser"), objCase.RegUserName)

            '[#70]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Performer_Phone"), objCase.PerformerPhone)
            sBody = Replace(sBody, getMailTemplateIdentifier("Performer_Phone"), objCase.PerformerPhone)

            '[#71]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Performer_CellPhone"), objCase.PerformerCellPhone)
            sBody = Replace(sBody, getMailTemplateIdentifier("Performer_CellPhone"), objCase.PerformerCellPhone)

            '[#72]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Performer_Email"), objCase.PerformerEMail)
            sBody = Replace(sBody, getMailTemplateIdentifier("Performer_Email"), objCase.PerformerEMail)


            '[#73]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("IsAbout_PersonsName"), objCase.IsAbout_PersonsName)
            sBody = Replace(sBody, getMailTemplateIdentifier("IsAbout_PersonsName"), objCase.IsAbout_PersonsName)

            '[#80]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("AutoCloseDays"), objCase.AutoCloseDays)
            sBody = Replace(sBody, getMailTemplateIdentifier("AutoCloseDays"), objCase.AutoCloseDays)

            If objGlobalSettings.ServerPort = 443 Then
                sProtocol = "https"
            Else
                sProtocol = "http"
            End If

            While InStr(1, sBody, "[#98]", 1)
                If InStr(1, sBody, "[#98]", 1) <> 0 And InStr(1, sBody, "[/#98]", 1) <> 0 Then
                    Dim iPos1 As Integer = InStr(1, sBody, "[#98]", 1)
                    Dim iPos2 As Integer = InStr(1, sBody, "[/#98]", 1)

                    Dim sTextToReplace As String = Mid(sBody, iPos1, iPos2 - iPos1 + 6)

                    Dim sLinkTextSelfService As String = Mid(sBody, iPos1 + 5, iPos2 - iPos1 - 5)

                    If objGlobalSettings.DBVersion > "5" Then
                        'sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sLinkTextSelfService & "</a>"
                        sLinkSelfService = "<a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sLinkTextSelfService & "</a>"
                    Else
                        'sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sLinkTextSelfService & "</a>"
                        sLinkSelfService = "<a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sLinkTextSelfService & "</a>"
                    End If

                    sBody = Replace(sBody, sTextToReplace, sLinkSelfService, 1, 1)
                Else
                    If objGlobalSettings.DBVersion > "5" Then
                        'sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & "</a>"
                        sLinkSelfService = "<a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & "</a>"
                    Else
                        'sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & "</a>"
                        sLinkSelfService = "<a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & "</a>"
                    End If

                    sBody = Replace(sBody, "[#98]", sLinkSelfService, 1, 1)
                End If
            End While


            'If InStr(1, sBody, "[#98]", 1) <> 0 Then
            '    ' Ersätt med länk
            '    sBody = Replace(sBody, "[#98]", sLinkSelfService)
            'End If

            While InStr(1, sBody, "[#99]", 1)
                If InStr(1, sBody, "[#99]", 1) <> 0 And InStr(1, sBody, "[/#99]", 1) <> 0 Then
                    Dim iPos1 As Integer = InStr(1, sBody, "[#99]", 1)
                    Dim iPos2 As Integer = InStr(1, sBody, "[/#99]", 1)

                    Dim sTextToReplace As String = Mid(sBody, iPos1, iPos2 - iPos1 + 6)

                    Dim sLinkText As String = Mid(sBody, iPos1 + 5, iPos2 - iPos1 - 5)

                    If objGlobalSettings.DBVersion > "5" Then

                        Dim editCasePath As String
                        If objGlobalSettings.UseMobileRouting Then
                            editCasePath = CasePaths.EDIT_CASE_MOBILEROUTE
                        Else
                            editCasePath = CasePaths.EDIT_CASE_DESKTOP
                        End If
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & editCasePath & objCase.Id & """>" & sLinkText & "</a>"
                    Else
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & """>" & sLinkText & "</a>"
                    End If

                    sBody = Replace(sBody, sTextToReplace, sLink, 1, 1)
                Else
                    If objGlobalSettings.DBVersion > "5" Then
                        Dim editCasePath As String
                        If objGlobalSettings.UseMobileRouting Then
                            editCasePath = CasePaths.EDIT_CASE_MOBILEROUTE
                        Else
                            editCasePath = CasePaths.EDIT_CASE_DESKTOP
                        End If
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & editCasePath & objCase.Id & """>" & sProtocol & "://" & objGlobalSettings.ServerName & editCasePath & objCase.Id & "</a>"
                    Else
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & "</a>"
                    End If

                    sBody = Replace(sBody, "[#99]", sLink, 1, 1)
                End If
            End While

            sBody = sBody.Replace(vbCrLf, "<br>")



            If objCustomer.UseGraphSendingEmail = True Then
                Dim filesToAttach As String
                If files IsNot Nothing AndAlso files.Any() AndAlso (bAttachExternalFiles OrElse bAttachInternalFiles) Then

                    For Each attachedFile As String In files.Where(Function(f) bAttachExternalFiles AndAlso f.IsInternal = False OrElse bAttachInternalFiles AndAlso f.IsInternal = True).Select(Function(f) f.FilePath).ToList()

                        If filesToAttach = String.Empty Then
                            filesToAttach = attachedFile
                        Else
                            filesToAttach = filesToAttach & ", " & attachedFile
                        End If
                    Next
                End If
                Dim email As New EmailLog
                email = SetEmailLog(objCustomer.HelpdeskEMail, sEmailTo, sSubject, sBody, "Enqueued", EmailSendStatus.Pending, DateTime.Now,
                                    Nothing, String.Empty, String.Empty, 0, Nothing, Nothing, filesToAttach,
                                    String.Empty, False, 0, Nothing, 0, 0, sMessageId)



                SendGraphMail(email, objCustomer)



            Else

                Dim filesToAttach As List(Of String) = New List(Of String)
                'Prepare files to attach. Check internal/external flag
                If files IsNot Nothing AndAlso files.Any() AndAlso (bAttachExternalFiles OrElse bAttachInternalFiles) Then

                    For Each attachedFile As String In files.Where(Function(f) bAttachExternalFiles AndAlso f.IsInternal = False OrElse bAttachInternalFiles AndAlso f.IsInternal = True).Select(Function(f) f.FilePath).ToList()
                        filesToAttach.Add(attachedFile)
                    Next
                End If

                If giLoglevel > 0 Then
                    objLogFile.WriteLine(Now() & ", sendMail, From:" & objCustomer.HelpdeskEMail & ", To: " & sEmailTo & ". Attached files: " & If(filesToAttach IsNot Nothing, String.Join(";", filesToAttach) & "", "None"))
                    'objLogFile.WriteLine(Now() & ", sendMail, Body:" & sBody)
                End If


                If Not IsNullOrEmpty(setting.SMTPServer) Then
                    sRet = Send(objCustomer.HelpdeskEMail, sEmailTo, sSubject, sBody, objGlobalSettings.EMailBodyEncoding, setting.SMTPServer, setting.SMTPPort, setting.IsSMTPSecured, setting.SMTPUserName, setting.SMTPPassWord, sMessageId, filesToAttach)
                Else
                    sRet = Send(objCustomer.HelpdeskEMail, sEmailTo, sSubject, sBody, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId, filesToAttach)
                End If

            End If
            ' Log sRet result!

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR sendMail " & ex.Message.ToString)
            End If
        End Try

        Return sRet
    End Function

    Public Function sendQuestionnaireMail(ByVal qcp As QuestionnaireCircularPart, ByVal objCustomer As Customer, ByVal objmailTemplate As MailTemplate, ByVal objGlobalSettings As GlobalSettings, ByVal sMessageId As String) As Long
        ' Skicka mail
        Dim sSubject As String
        Dim sBody As String
        Dim sLink As String
        Dim sProtocol As String

        Try
            sSubject = objmailTemplate.Subject
            sBody = objmailTemplate.Body


            ' [#1]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("CaseNumber"), qcp.Casenumber)
            sBody = Replace(sBody, getMailTemplateIdentifier("CaseNumber"), qcp.Casenumber)

            ' [#2]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("CustomerName"), objCustomer.Name)
            sBody = Replace(sBody, getMailTemplateIdentifier("CustomerName"), objCustomer.Name)

            '[#4]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Caption"), qcp.Caption)
            sBody = Replace(sBody, getMailTemplateIdentifier("Caption"), qcp.Caption)

            '[#5]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("Description"), qcp.Description)
            sBody = Replace(sBody, getMailTemplateIdentifier("Description"), Replace(qcp.Description, vbCrLf, "<br>"))

            If objGlobalSettings.ServerPort = 443 Then
                sProtocol = "https"
            Else
                sProtocol = "http"
            End If

            sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Q.asp?GUID=" & qcp.GUID.ToString() & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/Q.asp?GUID=" & qcp.GUID.ToString() & "</a>"

            If InStr(1, sBody, "[#99]", 1) <> 0 Then
                ' Ersätt med länk
                sBody = Replace(sBody, "[#99]", sLink)
            End If


            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", sendMail, From:" & objCustomer.HelpdeskEMail & ", To: " & qcp.Persons_EMail)
            End If

            Send(objCustomer.HelpdeskEMail, qcp.Persons_EMail, sSubject, sBody, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR sendQuestionnaireMail " & ex.Message.ToString)
            End If
        End Try

    End Function

    Public Function Send(objCustomer As Customer, sFrom As String,
                         sTo As String,
                         sSubject As String,
                         sBody As String,
                         sEMailBodyEncoding As String,
                         sSMTPServer As String,
                         sMessageId As String,
                         Optional filesToAttach As List(Of String) = Nothing) As String

        Dim smtpServer As String = Nothing
        Dim smtpUsername As String = Nothing
        Dim smtpPassword As String = Nothing
        Dim smtpPort As Integer = Nothing
        Dim smtpSecure As Boolean = Nothing

        If Not IsNullOrEmpty(sSMTPServer) Then
            Dim aConfiguration() As String = Split(sSMTPServer, ";")

            smtpServer = aConfiguration(0)

            If aConfiguration.Length > 2 Then
                smtpUsername = aConfiguration(1)
                smtpPassword = aConfiguration(2)
            End If

            If aConfiguration.Length > 3 Then
                smtpPort = CType(aConfiguration(3), Integer)
            End If


            If aConfiguration.Length > 4 Then
                smtpSecure = CType(aConfiguration(4), Boolean)
            End If
        End If

        Return Send(sFrom, sTo, sSubject, sBody, sEMailBodyEncoding, smtpServer, smtpPort, smtpSecure, smtpUsername, smtpPassword, sMessageId, filesToAttach)
    End Function

    Private Function IsValidEmail(email As String) As Boolean
        If String.IsNullOrWhiteSpace(email) Then
            Return False
        End If

        Try
            Dim addr As New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Function IsBlockedRecipient(sEmail As String, sBlockedEmailRecipients As String) As Boolean
        If String.IsNullOrWhiteSpace(sEmail) OrElse String.IsNullOrWhiteSpace(sBlockedEmailRecipients) Then
            Return False
        End If

        Dim emails() As String = sBlockedEmailRecipients.Split(";"c)
        If emails.Length = 0 Then
            Return False
        End If

        For Each pattern As String In emails
            If Not String.IsNullOrWhiteSpace(pattern) Then
                If sEmail.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0 Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Function GetOAuthToken(tenantId As String, clientId As String, clientSecret As String) As String
        Using client As New HttpClient()
            Dim tokenEndpoint As String = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"
            Dim content As New FormUrlEncodedContent(New Dictionary(Of String, String) From {
                {"client_id", clientId},
                {"scope", "https://graph.microsoft.com/.default"},
                {"client_secret", clientSecret},
                {"grant_type", "client_credentials"}
            })

            Dim response As HttpResponseMessage = client.PostAsync(tokenEndpoint, content).Result
            Dim responseString As String = response.Content.ReadAsStringAsync().Result

            If Not response.IsSuccessStatusCode Then
                ' Throw New Exception($"Failed to get access token: {responseString}")
                Return Nothing
            End If

            Dim responseObject As Object = JsonConvert.DeserializeObject(Of Object)(responseString)
            Return responseObject("access_token").ToString()
        End Using
    End Function

    Public Function SendGraphMail(email As EmailLog, objCustomer As Customer
                         ) As String

        Try
            Dim token As String = GetOAuthToken(objCustomer.GraphTenantId, objCustomer.GraphClientId, objCustomer.GraphClientSecret)

            If token IsNot Nothing Then
                Using client As New HttpClient()
                    client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", token)

                    Dim blockedEmails As String = objCustomer.BlockedEmailRecipients

                    Dim toRecipients = email.EmailAddress.Split(","c) _
                        .Select(Function(address) address.Trim()) _
                        .Where(Function(address) IsValidEmail(address) AndAlso Not IsBlockedRecipient(address, blockedEmails)) _
                        .Select(Function(address) New With {.emailAddress = New With {.address = address}}) _
                        .ToArray()

                    Dim ccRecipients = email.Cc.Split(","c) _
                        .Select(Function(address) address.Trim()) _
                        .Where(Function(address) IsValidEmail(address) AndAlso Not IsBlockedRecipient(address, blockedEmails)) _
                        .Select(Function(address) New With {.emailAddress = New With {.address = address}}) _
                        .ToArray()

                    Dim bccRecipients = email.Bcc.Split(","c) _
                        .Select(Function(address) address.Trim()) _
                        .Where(Function(address) IsValidEmail(address) AndAlso Not IsBlockedRecipient(address, blockedEmails)) _
                        .Select(Function(address) New With {.emailAddress = New With {.address = address}}) _
                        .ToArray()

                    Dim attachments As List(Of Object) = If(Not String.IsNullOrWhiteSpace(email.Files),
                        email.Files.Split("|"c) _
                        .Where(Function(filePath) File.Exists(filePath)) _
                        .Select(Function(filePath) New Dictionary(Of String, Object) From {
                            {"@odata.type", "#microsoft.graph.fileAttachment"},
                            {"name", Path.GetFileName(filePath)},
                            {"contentBytes", Convert.ToBase64String(File.ReadAllBytes(filePath))}
                        }).Cast(Of Object)().ToList(), New List(Of Object)())

                    Dim attachments1 As List(Of Object) = If(Not String.IsNullOrWhiteSpace(email.FilesInternal),
                        email.FilesInternal.Split("|"c) _
                        .Where(Function(filePath) File.Exists(filePath)) _
                        .Select(Function(filePath) New Dictionary(Of String, Object) From {
                            {"@odata.type", "#microsoft.graph.fileAttachment"},
                            {"name", Path.GetFileName(filePath)},
                            {"contentBytes", Convert.ToBase64String(File.ReadAllBytes(filePath))}
                        }).Cast(Of Object)().ToList(), New List(Of Object)())

                    Dim combinedAttachments = attachments.Concat(attachments1).ToArray()

                    Dim importance As String = If(email.HighPriority, "high", "normal")

                    Dim message As Object
                    Dim customHeaders As New List(Of Object)()
                    If Not String.IsNullOrWhiteSpace(email.MessageId) Then
                        Dim replyToAddress As String = objCustomer.HelpdeskEMail.Split(","c).FirstOrDefault(Function(addr) IsValidEmail(addr.Trim()))

                        customHeaders.Add(New With {.name = "x-message-id", .value = email.MessageId})
                        message = New With {
                        .message = New With {
                            .replyTo = If(Not String.IsNullOrWhiteSpace(replyToAddress),
                            New Object() {New With {.emailAddress = New With {.address = replyToAddress}}},
                            Nothing),
                            .subject = email.Subject,
                            .body = New With {.contentType = "HTML", .content = email.Body},
                            .toRecipients = toRecipients,
                            .ccRecipients = ccRecipients,
                            .bccRecipients = bccRecipients,
                            .attachments = combinedAttachments,
                            .importance = importance,
                            .internetMessageHeaders = If(customHeaders.Any(), customHeaders, Nothing) ' Only include headers if any exist
                        }
                    }
                    Else

                        Dim replyToAddress As String = objCustomer.HelpdeskEMail.Split(","c).FirstOrDefault(Function(addr) IsValidEmail(addr.Trim()))



                        message = New With {
                        .message = New With {
                            .replyTo = If(Not String.IsNullOrWhiteSpace(replyToAddress),
                            New Object() {New With {.emailAddress = New With {.address = replyToAddress}}},
                            Nothing),
                            .subject = email.Subject,
                            .body = New With {.contentType = "HTML", .content = email.Body},
                            .toRecipients = toRecipients,
                            .ccRecipients = ccRecipients,
                            .bccRecipients = bccRecipients,
                            .attachments = combinedAttachments,
                            .importance = importance
                        }
                        }
                    End If




                    Dim jsonMessage As String = JsonConvert.SerializeObject(message)
                    Dim content As New StringContent(jsonMessage, Encoding.UTF8, "application/json")

                    Dim usr As String = objCustomer.GraphUserName
                    Dim emailEndpoint As String = $"https://graph.microsoft.com/v1.0/users/{usr}/sendMail"

                    Dim response As HttpResponseMessage = client.PostAsync(emailEndpoint, content).Result

                    If Not response.IsSuccessStatusCode Then
                        Dim errorMsg As String = response.Content.ReadAsStringAsync().Result
                        Throw New Exception($"Failed to send email via Graph: {errorMsg}")
                    End If

                    'email.SendTime = sendTime
                    'email.SendStatus = EmailSendStatus.Sent
                End Using
            Else
                _logger.Error($"Error sending email. EmailLog Id: {email.MessageId}. ", Nothing)
                'attempt.Message = "No token acquired"
            End If
        Catch ex As Exception
            _logger.Error($"Error sending email. EmailLog Id: {email.MessageId}. ", ex)
            Dim errorMsg As New StringBuilder(ex.Message)
            Dim inner As Exception = ex.InnerException
            While inner IsNot Nothing
                errorMsg.AppendLine(inner.Message)
                inner = inner.InnerException
            End While
            'attempt.Message = errorMsg.ToString()
        End Try
    End Function
    Public Function Send(objCustomer As Customer, sFrom As String,
                         sTo As String,
                         sSubject As String,
                         sBody As String,
                         sEMailBodyEncoding As String,
                         smtpServer As String,
                         smtpPort As Integer,
                         smtpSecure As Boolean,
                         smtpUsername As String,
                         smtpPassword As String,
                         sMessageId As String,
                         Optional filesToAttach As List(Of String) = Nothing) As String
        ' Create Mail
        Dim msg As New MailMessage()
        Dim sRet As String = ""

        With msg
            .From = New MailAddress(sFrom)

            sTo = sTo.Replace(";", ",")

            .To.Add(sTo)
            .IsBodyHtml = True
            .Subject = sSubject
            .Body = sBody
            .Headers.Add("Message-ID", sMessageId)

            If sEMailBodyEncoding <> "" Then
                .BodyEncoding = System.Text.Encoding.GetEncoding(sEMailBodyEncoding)
            End If
        End With

        Dim smtp As New SmtpClient()

        If smtpServer = "" Then
            smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
        Else
            smtp.Host = smtpServer

            If Not IsNullOrEmpty(smtpUsername) Then
                Dim credentials = New Net.NetworkCredential(smtpUsername, smtpPassword)
                smtp.Credentials = credentials
            End If

            If smtpPort > 0 Then
                smtp.Port = smtpPort
            End If

            smtp.EnableSsl = smtpSecure
        End If

        'Attach files to message if any
        If (filesToAttach IsNot Nothing AndAlso filesToAttach.Any()) Then

            For Each file As String In filesToAttach
                If (IO.File.Exists(file)) Then
                    msg.Attachments.Add(New Attachment(file))
                End If
            Next
        End If

        Try
            smtp.Send(msg)
        Catch ex As Exception
            sRet = ex.Message.ToString()
            objLogFile.WriteLine("Smtp error: {0}, Send message. EmailTo: {1}, Message-ID: {2}", sRet, msg.To, sMessageId)
        End Try

        Return sRet
    End Function

    Public Function SetEmailLog(sFrom As String, sEmailAddress As String, sSubject As String, sBody As String,
                                sResponseMessage As String, SendStatus As EmailSendStatus, SendTime As DateTime, Attempts As Integer,
                                sCC As String, sBCC As String, CaseHistory_Id As Integer, ChangedDate As DateTime, CreatedDate As DateTime,
                                sFiles As String, sFilesInternal As String, HighPriority As Boolean, Id As Integer, LastAttempt As DateTime,
                                LogId As Integer, MailId As Integer, MessageId As String) As EmailLog

        Dim el As New EmailLog

        el.From = sFrom
        el.EmailAddress = sEmailAddress
        el.Subject = sSubject
        el.Body = sBody
        el.ResponseMessage = "Enqueued"
        el.SendStatus = SendStatus
        el.SendTime = SendTime
        el.Attempts = Attempts
        el.Bcc = sBCC
        el.Cc = sBCC
        el.CaseHistory_Id = CaseHistory_Id
        el.ChangedDate = ChangedDate
        el.CreatedDate = CreatedDate
        el.Files = sFiles
        el.FilesInternal = sFilesInternal
        el.HighPriority = HighPriority
        el.Id = Id
        el.LastAttempt = LastAttempt
        el.Log_Id = LogId
        el.MailId = MailId
        el.MessageId = MessageId



        Return el

    End Function

    Public Function SendErrorMail(sFrom As String,
                         sTo As String,
                         sSubject As String,
                         sBody As String,
                         connectionString As String,
                         smtpServer As String, objCustomer As Customer)
        ' Create Mail
        'Dim setting As Setting
        'Using factory As DatabaseFactory = New DatabaseFactory(connectionString)

        '    Dim settingsRepository As New SettingRepository(factory)
        '    setting = settingsRepository.Get(Function(x) x.Customer_Id = objCustomer.Id)

        'End Using
        If objCustomer.UseGraphSendingEmail = True Then
            Dim email As New EmailLog
            email = SetEmailLog(sFrom, sTo, sSubject, sBody, "Enqueued", EmailSendStatus.Pending, DateTime.Now,
                                Nothing, String.Empty, String.Empty, 0, Nothing, Nothing, String.Empty,
                                String.Empty, False, 0, Nothing, 0, 0, String.Empty)



            SendGraphMail(email, objCustomer)
        Else
            Dim msg As New MailMessage()
            Dim sRet As String = ""

            With msg
                .From = New MailAddress(sFrom)

                sTo = sTo.Replace(";", ",")

                .To.Add(sTo)
                .IsBodyHtml = True
                .Subject = sSubject
                .Body = sBody

            End With

            Dim smtp As New SmtpClient()

            smtp.Host = smtpServer


            Try
                smtp.Send(msg)
            Catch ex As Exception
                sRet = ex.Message.ToString()
                objLogFile.WriteLine("Smtp error: {0}, Send message. EmailTo: {1}, Message-ID: {2}", sRet, msg.To)
            End Try

            Return sRet
        End If

    End Function
End Class
