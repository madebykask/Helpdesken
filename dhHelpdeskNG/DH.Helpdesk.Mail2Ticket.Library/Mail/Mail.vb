Imports System.Net.Mail
Imports DH.Helpdesk.Dal.Infrastructure
Imports DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel
Imports DH.Helpdesk.Dal.Repositories
Imports DH.Helpdesk.Domain
Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions

Public Class Mail
    Public Function sendMail(ByVal objCase As CCase, ByVal objLog As Log, ByVal objCustomer As Customer, ByVal sEmailTo As String, ByVal objmailTemplate As MailTemplate, ByVal objGlobalSettings As GlobalSettings, ByVal sMessageId As String, ByVal sEMailLogGUID As String, Optional ByVal connectionString As String = Nothing) As String
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

                Dim settingsRepository As New SettingRepository(New DatabaseFactory(connectionString))
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

            If Not objLog Is Nothing Then
                '[#10]
                sSubject = Replace(sSubject, getMailTemplateIdentifier("Text_External"), objLog.Text_External)
                sBody = Replace(sBody, getMailTemplateIdentifier("Text_External"), objLog.Text_External)

                '[#11]
                sSubject = Replace(sSubject, getMailTemplateIdentifier("Text_Internal"), objLog.Text_Internal)
                sBody = Replace(sBody, getMailTemplateIdentifier("Text_Internal"), objLog.Text_Internal)
            End If

            '[#12]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("PriorityName"), objCase.PriorityName)
            sBody = Replace(sBody, getMailTemplateIdentifier("PriorityName"), objCase.PriorityName)

            '[#13]
            sSubject = Replace(sSubject, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.WorkingGroupEMail)
            sBody = Replace(sBody, getMailTemplateIdentifier("WorkingGroupEMail"), objCase.WorkingGroupEMail)

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
                        sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sLinkTextSelfService & "</a>"
                    Else
                        sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sLinkTextSelfService & "</a>"
                    End If

                    sBody = Replace(sBody, sTextToReplace, sLinkSelfService, 1, 1)
                Else
                    If objGlobalSettings.DBVersion > "5" Then
                        sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & """>" & sProtocol & "://" & objGlobalSettings.ExternalSite & "/case/index/" & sEMailLogGUID & "</a>"
                    Else
                        sLinkSelfService = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/CI.asp?Id=" & objCase.CaseGUID & "</a>"
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
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/cases/edit/" & objCase.Id & """>" & sLinkText & "</a>"
                    Else
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & """>" & sLinkText & "</a>"
                    End If

                    sBody = Replace(sBody, sTextToReplace, sLink, 1, 1)
                Else
                    If objGlobalSettings.DBVersion > "5" Then
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/cases/edit/" & objCase.Id & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/cases/edit/" & objCase.Id & "</a>"
                    Else
                        sLink = "<br><a href=""" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & """>" & sProtocol & "://" & objGlobalSettings.ServerName & "/Default.asp?GUID=" & objCase.CaseGUID & "</a>"
                    End If

                    sBody = Replace(sBody, "[#99]", sLink, 1, 1)
                End If
            End While

            sBody = sBody.Replace(vbCrLf, "<br>")

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", sendMail, From:" & objCustomer.HelpdeskEMail & ", To: " & sEmailTo)

                'objLogFile.WriteLine(Now() & ", sendMail, Body:" & sBody)
            End If

            If Not String.IsNullOrEmpty(setting.SMTPServer) Then
                sRet = Send(objCustomer.HelpdeskEMail, sEmailTo, sSubject, sBody, objGlobalSettings.EMailBodyEncoding, setting.SMTPServer, setting.SMTPPort, setting.IsSMTPSecured, setting.SMTPUserName, setting.SMTPPassWord, sMessageId)
            Else
                sRet = Send(objCustomer.HelpdeskEMail, sEmailTo, sSubject, sBody, objGlobalSettings.EMailBodyEncoding, objGlobalSettings.SMTPServer, sMessageId)
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

    Public Function Send(ByVal sFrom As String, ByVal sTo As String, ByVal sSubject As String, ByVal sBody As String, ByVal sEMailBodyEncoding As String, ByVal sSMTPServer As String, ByVal sMessageId As String) As String

        Dim smtpServer As String = Nothing
        Dim smtpUsername As String = Nothing
        Dim smtpPassword As String = Nothing
        Dim smtpPort As Integer = Nothing
        Dim smtpSecure As Boolean = Nothing

        If Not String.IsNullOrEmpty(sSMTPServer) Then
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

        Return Send(sFrom, sTo, sSubject, sBody, sEMailBodyEncoding, smtpServer, smtpPort, smtpSecure, smtpUsername, smtpPassword, sMessageId)
    End Function

    Public Function Send(ByVal sFrom As String, ByVal sTo As String, ByVal sSubject As String, ByVal sBody As String, ByVal sEMailBodyEncoding As String, ByVal smtpServer As String, ByVal smtpPort As Integer, ByVal smtpSecure As Boolean, ByVal smtpUsername As String, ByVal smtpPassword As String, ByVal sMessageId As String) As String
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

            If Not String.IsNullOrEmpty(smtpUsername) Then
                Dim credentials = New System.Net.NetworkCredential(smtpUsername, smtpPassword)

                smtp.Credentials = credentials
            End If

            If smtpPort > 0 Then
                smtp.Port = smtpPort
            End If

            smtp.EnableSsl = smtpSecure

        End If

        Try
            smtp.Send(msg)
        Catch ex As Exception
            sRet = ex.Message.ToString()
            objLogFile.WriteLine("Smtp error: {0}, Send message. EmailTo: {1}, Message-ID: {2}", sRet, msg.To, sMessageId)            
        End Try

        Return sRet
    End Function
End Class
