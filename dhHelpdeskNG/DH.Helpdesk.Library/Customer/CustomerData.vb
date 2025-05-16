Imports DH.Helpdesk.Library.SharedFunctions
Imports System.Net
Imports System.IO
Imports System.Data.SqlClient

Public Class CustomerData
    Public Function getCustomerById(ByVal Id As Integer) As Customer
        Return getCustomer(Id:=Id)
    End Function

    Public Function getCustomerByDomain(ByVal Domain_Id As Integer) As Customer
        Return getCustomer(Domain_Id:=Domain_Id)
    End Function

    Public Function getCustomerByWorkingGroup() As Customer
        Return getCustomer(iWorkingGroup:=1)
    End Function
    Private Function getCustomer(Optional ByVal Id As Integer = 0, Optional ByVal Domain_Id As Integer = 0, Optional ByVal iWorkingGroup As Integer = 0) As Customer
        Dim sSQL As String
        Dim dt As DataTable

        Try

            If gsURL = "" Then
                sSQL = "SELECT tblCustomer.Id, tblCustomer.CustomerNumber, tblCustomer.Name, tblCustomer.HelpdeskEMail, CaseWorkingGroupSource, Language_Id, " &
                           "POP3Server, POP3Port, POP3DebugLevel, POP3EMailPrefix, WorkingDayStart, WorkingDayEnd, TimeZone_offset, LDAPCreateOrganization, "

                If iWorkingGroup = 1 Then
                    sSQL = sSQL & "tblWorkingGroup.POP3UserName, tblWorkingGroup.POP3Password, tblWorkingGroup.CreateCase_ProductArea_Id AS EMailDefaultProductArea_Id, tblWorkingGroup.WorkingGroupEMail, tblWorkingGroup.EMailCaseType_Id AS WorkingGroupDefaultCaseType_Id, "
                Else
                    sSQL = sSQL & "tblSettings.POP3UserName, tblSettings.POP3Password, tblProductArea.Id AS EMailDefaultProductArea_Id, Null AS WorkingGroupEMail, Null AS WorkingGroupDefaultCaseType_Id, "
                End If

                sSQL = sSQL & "tblCaseType.Id AS EMailDefaultCaseType_Id, tblPriority.Id AS EMailDefaultPriority_Id, tblCategory.Id AS EMailDefaultCategory_Id, " &
                            "tblStatus.Id AS DefaultStatus_Id, tblStateSecondary.Id AS DefaultStateSecondary_Id, tblStateSecondary2.Id AS DefaultStateSecondary_Id2, " &
                           "Null AS EMailDefaultDepartment_Id, tblSettings.EmailSubjectPattern, tblSettings.ExternalEmailSubjectPattern, tblSettings.EMailAnswerSeparator, " &
                           "tblSettings.XMLLogLevel, tblSettings.XMLAllFiles, MailServerProtocol, EMailRegistrationMailID, tblSettings.EMailImportType, " &
                           "tblCustomer.OverWriteFromMasterDirectory, " &
                           "tblSettings.LDAPLogLevel, tblSettings.OpenCase_StateSecondary_Id, " &
                           "tblSettings.LDAPSyncType, tblCustomer.Days2WaitBeforeDelete, tblSettings.LDAPAuthenticationType, 0 AS WorkingGroup_Id, " &
                           "tblSettings.InventoryDays2WaitBeforeDelete, tblSettings.LDAPAllUsers, tblSettings.EMailAnswerDestination, tblSettings.ModuleOrder, tblSettings.ModuleAccount, tblSettings.PhysicalFilePath, " &
                           "tblSettings.InventoryCreate, tblSettings.AllowedEMailRecipients, tblCustomer.CaseStatisticsEMailList, tblSettings.EMailFolder, tblSettings.EMailFolderArchive, tblSettings.EwsApplicationId, tblSettings.EwsClientSecret, tblSettings.EwsTenantId, tblSettings.UseEws, " &
                           "tblSettings.DefaultAdministratorExternal, tblCustomer.NewCaseEMailList, tblRegistrationSourceCustomer.Id AS RegistrationSourceCustomer_Id, tblSettings.DefaultEmailLogDestination, tblSettings.BlockedEmailRecipients, tblSettings.ErrorMailTo, " &
                           "tblSettings.UseGraphSendingEmail, tblSettings.GraphClientId, tblSettings.GraphTenantId, tblSettings.GraphClientSecret, tblSettings.GraphClientSecretExpireDate, tblSettings.GraphUserName, tblSettings.EwsClientSecretExpireDate "


                If Id <> 0 Then
                    sSQL = sSQL & ", tblSettings.XMLFileFolder, tblCustomer.NDSPath AS LDAPServerName, tblSettings.LDAPUserName, tblSettings.LDAPPassword " &
                                    ", tblSettings.LDAPBase, tblSettings.LDAPFilter "
                ElseIf Domain_Id <> 0 Then
                    sSQL = sSQL & ", tblDomain.XMLFileFolder, tblDomain.LDAPServerName, tblDomain.LDAPUserName, tblDomain.LDAPPassword " &
                                    ", tblDomain.LDAPBase, tblDomain.LDAPFilter "
                End If

                sSQL = sSQL & ", tblSettings.M2TNewCaseMailTo, tblCustomer.Status "
                sSQL = sSQL & "FROM tblCustomer " &
                                "INNER JOIN tblSettings ON tblCustomer.Id = tblSettings.Customer_Id " &
                                "LEFT JOIN tblCaseType ON tblCustomer.Id = tblCaseType.Customer_Id AND tblCaseType.isEMailDefault=1 " &
                                "LEFT JOIN tblCategory ON tblCustomer.Id = tblCategory.Customer_Id AND tblCategory.isEMailDefault=1 " &
                                "LEFT JOIN tblProductArea ON tblCustomer.Id = tblProductArea.Customer_Id AND tblProductArea.isEMailDefault=1 " &
                                "LEFT JOIN tblPriority ON tblCustomer.Id = tblPriority.Customer_Id AND tblPriority.isEMailDefault=1 " &
                                "LEFT JOIN tblStatus ON tblCustomer.Id = tblStatus.Customer_Id AND tblStatus.isDefault=1 " &
                                "LEFT JOIN tblStateSecondary ON tblCustomer.Id = tblStateSecondary.Customer_Id AND tblStateSecondary.isDefault=1 " &
                                "LEFT JOIN tblStateSecondary tblStateSecondary2 ON tblStatus.StateSecondary_Id = tblStateSecondary2.Id " &
                                "LEFT JOIN tblRegistrationSourceCustomer ON tblCustomer.Id = tblRegistrationSourceCustomer.Customer_Id AND tblRegistrationSourceCustomer.SystemCode=3 "

                If Id <> 0 Then
                    sSQL = sSQL & "WHERE tblCustomer.Id = " & Id
                ElseIf Domain_Id <> 0 Then
                    sSQL = sSQL & "INNER JOIN tblDomain on tblCustomer.Id=tblDomain.Customer_Id " &
                                    "WHERE tblDomain.Id = " & Domain_Id
                ElseIf iWorkingGroup = 1 Then
                    sSQL = sSQL & "INNER JOIN tblWorkingGroup ON tblCustomer.Id = tblWorkingGroup.Customer_Id " &
                                  "WHERE tblWorkingGroup.POP3Username IS NOT NULL "
                End If

                sSQL = sSQL & " ORDER BY tblCustomer.Name"

                dt = getDataTable(gsConnectionString, sSQL)

                Dim c As Customer

                c = New Customer(dt.Rows(0))

                Return c
            Else
                Return getCustomerHTTP(Id, Domain_Id)
            End If
        Catch ex As Exception
            'MsgBox(Err.Description)

            Throw ex
        End Try
    End Function

    Private Function getCustomerHTTP(ByVal Id As Integer, ByVal Domain_Id As Integer) As Customer
        Dim sURL As String
        Dim sXML As String
        Dim xmlNode As System.Xml.XmlNode

        Try
            Dim c As Customer = New Customer()

            If Domain_Id = 0 Then
                sURL = gsURL & "/getInfo.asp?TableName=tblCustomer"

                If Id <> 0 Then
                    sURL = gsURL & "&FieldName=tblCustomer.Id&Id=" & Id
                End If
            Else
                sURL = gsURL & "/getInfo.asp?TableName=tblDomain&Id=" & Domain_Id
            End If

            Dim request As WebRequest = WebRequest.Create(sURL)
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            Dim objStream As New StreamReader(response.GetResponseStream())

            sXML = objStream.ReadToEnd()

            response.Close()

            Dim xmlDocument As New System.Xml.XmlDocument
            xmlDocument.LoadXml(sXML)

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Id")

            If Not xmlNode Is Nothing Then
                c.Id = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/CustomerNumber")

            If Not xmlNode Is Nothing Then
                c.CustomerNumber = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/HelpdeskEMail")

            If Not xmlNode Is Nothing Then
                c.HelpdeskEMail = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/CaseWorkingGroupSource")

            If Not xmlNode Is Nothing Then
                c.CaseWorkingGroupSource = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Language_Id")

            If Not xmlNode Is Nothing Then
                c.Language_Id = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/MailServerProtocol")

            If Not xmlNode Is Nothing Then
                c.MailServerProtocol = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3Server")

            If Not xmlNode Is Nothing Then
                c.POP3Server = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3UserName")

            If Not xmlNode Is Nothing Then
                c.POP3UserName = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3Password")

            If Not xmlNode Is Nothing Then
                c.POP3Password = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3Port")

            If Not xmlNode Is Nothing Then
                c.POP3Port = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3DebugLevel")

            If Not xmlNode Is Nothing Then
                c.POP3DebugLevel = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/POP3EMailPrefix")

            If Not xmlNode Is Nothing Then
                c.POP3EMailPrefix = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailDefaultCaseType_Id")

            If Not xmlNode Is Nothing Then
                If Trim(xmlNode.InnerText) <> "" Then
                    c.EMailDefaultCaseType_Id = Trim(xmlNode.InnerText)
                End If
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailDefaultPriority_Id")

            If Not xmlNode Is Nothing Then
                If Trim(xmlNode.InnerText) <> "" Then
                    c.EMailDefaultPriority_Id = Trim(xmlNode.InnerText)
                End If
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailDefaultDepartment_Id")

            If Not xmlNode Is Nothing Then
                If Trim(xmlNode.InnerText) <> "" Then
                    c.EMailDefaultDepartment_Id = Trim(xmlNode.InnerText)
                End If
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailSubjectPattern")

            If Not xmlNode Is Nothing Then
                c.EMailSubjectPattern = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailAnswerSeparator")

            If Not xmlNode Is Nothing Then
                c.EMailAnswerSeparator = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMailRegistrationMailID")

            If Not xmlNode Is Nothing Then
                c.EMailRegistrationMailID = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/XMLFileFolder")

            If Not xmlNode Is Nothing Then
                c.XMLFileFolder = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/XMLLogLevel")

            If Not xmlNode Is Nothing Then
                c.XMLLogLevel = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/XMLAllFiles")

            If Not xmlNode Is Nothing Then
                c.XMLAllFiles = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/OverwriteFromMasterDirectory")

            If Not xmlNode Is Nothing Then
                c.XMLAllFiles = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/LDAPLogLevel")

            If Not xmlNode Is Nothing Then
                c.LDAPLogLevel = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/LDAPSyncType")

            If Not xmlNode Is Nothing Then
                c.LDAPSyncType = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Days2WaitBeforeDelete")

            If Not xmlNode Is Nothing Then
                c.Days2WaitBeforeDelete = Trim(xmlNode.InnerText)
            End If

            'xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/M2TNewCaseMailTo")

            'If Not xmlNode Is Nothing Then
            '    c.NewCaseMailTo = Trim(xmlNode.InnerText)
            'End If

            Return c
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCustomers() As List(Of Customer)
        Dim colCustomer As New List(Of Customer)
        Dim sSQL As String
        Dim dt As DataTable
        Dim dr As DataRow

        Try
            sSQL = "SELECT tblCustomer.Id, tblCustomer.CustomerNumber, tblCustomer.Name, tblCustomer.HelpdeskEMail, CaseWorkingGroupSource, Language_Id, " &
                        "POP3Server, POP3UserName, POP3Password, POP3Port, POP3DebugLevel, POP3EMailPrefix, WorkingDayStart, WorkingDayEnd, " &
                        "tblCaseType.Id AS EMailDefaultCaseType_Id, tblCategory.Id AS EMailDefaultCategory_Id, tblProductArea.Id AS EMailDefaultProductArea_Id, " &
                        "tblPriority.Id AS EMailDefaultPriority_Id, tblStatus.Id AS DefaultStatus_Id, tblStateSecondary.Id AS DefaultStateSecondary_Id, " &
                        "tblStateSecondary2.Id AS DefaultStateSecondary_Id2, " &
                        "tblDepartment.Id AS EMailDefaultDepartment_Id, tblSettings.EmailSubjectPattern, tblSettings.ExternalEmailSubjectPattern, tblSettings.EMailAnswerSeparator, " &
                        "tblSettings.XMLFileFolder, tblSettings.XMLLogLevel, tblSettings.XMLAllFiles, MailServerProtocol, EMailRegistrationMailID, tblSettings.EMailImportType, " &
                        "tblCustomer.OverWriteFromMasterDirectory, tblSettings.LDAPLogLevel, tblSettings.OpenCase_StateSecondary_Id, " &
                        "tblSettings.LDAPUserName, tblSettings.LDAPPassword, tblSettings.LDAPBase, tblSettings.LDAPFilter, " &
                        "tblSettings.LDAPSyncType, tblCustomer.Days2WaitBeforeDelete, " &
                        "tblSettings.XMLFileFolder, tblCustomer.NDSPath AS LDAPServerName, tblSettings.LDAPAuthenticationType, 0 AS WorkingGroup_Id, " &
                        "tblSettings.InventoryDays2WaitBeforeDelete, tblSettings.LDAPAllUsers, tblSettings.EMailAnswerDestination, tblSettings.ModuleOrder, tblSettings.ModuleAccount, tblSettings.PhysicalFilePath, " &
                        "tblSettings.InventoryCreate, tblSettings.AllowedEMailRecipients, Null AS WorkingGroupEMail, tblCustomer.CaseStatisticsEMailList, " &
                        "tblSettings.EMailFolder, tblSettings.EMailFolderArchive, tblSettings.DefaultAdministratorExternal, Null AS WorkingGroupDefaultCaseType_Id, tblCustomer.NewCaseEMailList, " &
                        "tblRegistrationSourceCustomer.Id AS RegistrationSourceCustomer_Id, tblSettings.DefaultEmailLogDestination, TimeZone_offset, LDAPCreateOrganization,  tblSettings.EwsApplicationId, tblSettings.EwsClientSecret, tblSettings.EwsTenantId, tblSettings.UseEws, tblSettings.BlockedEmailRecipients, tblSettings.ErrorMailTo, tblCustomer.Status, " &
                        "tblSettings.UseGraphSendingEmail, tblSettings.GraphClientId, tblSettings.GraphTenantId, tblSettings.GraphClientSecret, tblSettings.GraphClientSecretExpireDate, tblSettings.GraphUserName, tblSettings.EwsClientSecretExpireDate "

            sSQL = sSQL & ", tblSettings.M2TNewCaseMailTo "
            sSQL = sSQL & "FROM tblCustomer " &
"INNER JOIN tblSettings ON tblCustomer.Id = tblSettings.Customer_Id " &
"LEFT JOIN tblCaseType ON tblCustomer.Id = tblCaseType.Customer_Id AND tblCaseType.isEMailDefault=1 " &
"LEFT JOIN tblCategory ON tblCustomer.Id = tblCategory.Customer_Id AND tblCategory.isEMailDefault=1 " &
"LEFT JOIN tblProductArea ON tblCustomer.Id = tblProductArea.Customer_Id AND tblProductArea.isEMailDefault=1 " &
"LEFT JOIN tblPriority ON tblCustomer.Id = tblPriority.Customer_Id AND tblPriority.isEMailDefault=1 " &
"LEFT JOIN tblStatus ON tblCustomer.Id = tblStatus.Customer_Id AND tblStatus.isDefault=1 " &
"LEFT JOIN tblStateSecondary ON tblCustomer.Id = tblStateSecondary.Customer_Id AND tblStateSecondary.isDefault=1 " &
"LEFT JOIN tblStateSecondary tblStateSecondary2 ON tblStatus.StateSecondary_Id = tblStateSecondary2.Id " &
                        "LEFT JOIN tblDepartment ON tblCustomer.Id = tblDepartment.Customer_Id AND tblDepartment.isEMailDefault=1 " &
                        "LEFT JOIN tblRegistrationSourceCustomer ON tblCustomer.Id = tblRegistrationSourceCustomer.Customer_Id AND tblRegistrationSourceCustomer.SystemCode=3 "


            dt = getDataTable(gsConnectionString, sSQL)

            Dim c As Customer

            For Each dr In dt.Rows
                c = New Customer(dr)
                colCustomer.Add(c)
            Next

            Return colCustomer
        Catch ex As Exception
            Throw 
        End Try
    End Function

    Public Function getCustomersByWorkingGroup() As List(Of Customer)
        Dim colCustomer As New List(Of Customer)
        Dim sSQL As String
        Dim dt As DataTable
        Dim dr As DataRow

        Try
            sSQL = "SELECT tblCustomer.Id, tblCustomer.CustomerNumber, tblCustomer.Name, tblCustomer.HelpdeskEMail, CaseWorkingGroupSource, Language_Id, " &
                        "POP3Server, tblWorkingGroup.POP3UserName, tblWorkingGroup.POP3Password, POP3Port, POP3DebugLevel, POP3EMailPrefix, WorkingDayStart, WorkingDayEnd, " &
                        "tblCaseType.Id AS EMailDefaultCaseType_Id, tblCategory.Id AS EMailDefaultCategory_Id, tblWorkingGroup.CreateCase_ProductArea_Id AS EMailDefaultProductArea_Id, " &
                        "tblPriority.Id AS EMailDefaultPriority_Id, tblStatus.Id AS DefaultStatus_Id, tblStateSecondary.Id AS DefaultStateSecondary_Id, " &
                        "tblStateSecondary2.Id AS DefaultStateSecondary_Id2, " &
                        "tblDepartment.Id AS EMailDefaultDepartment_Id, tblSettings.EmailSubjectPattern, tblSettings.ExternalEmailSubjectPattern, tblSettings.EMailAnswerSeparator, " &
                        "tblSettings.XMLFileFolder, tblSettings.XMLLogLevel, tblSettings.XMLAllFiles, MailServerProtocol, EMailRegistrationMailID, " &
                        "tblCustomer.OverWriteFromMasterDirectory, tblSettings.LDAPLogLevel, tblSettings.OpenCase_StateSecondary_Id, " &
                        "tblSettings.LDAPUserName, tblSettings.LDAPPassword, tblSettings.LDAPBase, tblSettings.LDAPFilter, " &
                        "tblSettings.LDAPSyncType, tblCustomer.Days2WaitBeforeDelete, " &
                        "tblSettings.XMLFileFolder, tblCustomer.NDSPath AS LDAPServerName, tblSettings.LDAPAuthenticationType, tblWorkingGroup.Id AS WorkingGroup_Id, " &
                        "tblSettings.InventoryDays2WaitBeforeDelete, tblSettings.LDAPAllUsers, tblSettings.EMailAnswerDestination, tblSettings.EMailImportType, tblSettings.ModuleOrder, tblSettings.ModuleAccount, tblSettings.PhysicalFilePath, " &
                        "tblSettings.InventoryCreate, tblSettings.AllowedEMailRecipients, tblWorkingGroup.WorkingGroupEMail, tblCustomer.CaseStatisticsEMailList, " &
                        "tblSettings.EMailFolder, tblSettings.EMailFolderArchive, tblSettings.DefaultAdministratorExternal, tblWorkingGroup.EMailCaseType_Id AS WorkingGroupDefaultCaseType_Id, tblCustomer.NewCaseEMailList, " &
                        "tblRegistrationSourceCustomer.Id AS RegistrationSourceCustomer_Id, tblSettings.DefaultEmailLogDestination, TimeZone_offset, LDAPCreateOrganization, tblSettings.M2TNewCaseMailTo, tblSettings.EwsApplicationId, tblSettings.EwsClientSecret, tblSettings.EwsTenantId, tblSettings.UseEws, tblSettings.BlockedEmailRecipients, tblSettings.ErrorMailTo, tblCustomer.Status, " &
                        "tblSettings.UseGraphSendingEmail, tblSettings.GraphClientId, tblSettings.GraphTenantId, tblSettings.GraphClientSecret, tblSettings.GraphClientSecretExpireDate, tblSettings.GraphUserName, tblSettings.EwsClientSecretExpireDate "

            sSQL = sSQL & "FROM tblCustomer " &
                            "INNER JOIN tblSettings ON tblCustomer.Id = tblSettings.Customer_Id " &
                            "LEFT JOIN tblCaseType ON tblCustomer.Id = tblCaseType.Customer_Id AND tblCaseType.isEMailDefault=1 " &
                            "LEFT JOIN tblCategory ON tblCustomer.Id = tblCategory.Customer_Id AND tblCategory.isEMailDefault=1 " &
                            "LEFT JOIN tblProductArea ON tblCustomer.Id = tblProductArea.Customer_Id AND tblProductArea.isEMailDefault=1 " &
                            "LEFT JOIN tblPriority ON tblCustomer.Id = tblPriority.Customer_Id AND tblPriority.isEMailDefault=1 " &
                            "LEFT JOIN tblStatus ON tblCustomer.Id = tblStatus.Customer_Id AND tblStatus.isDefault=1 " &
                            "LEFT JOIN tblStateSecondary ON tblCustomer.Id = tblStateSecondary.Customer_Id AND tblStateSecondary.isDefault=1 " &
                            "LEFT JOIN tblStateSecondary tblStateSecondary2 ON tblStatus.StateSecondary_Id = tblStateSecondary2.Id " &
                            "LEFT JOIN tblDepartment ON tblCustomer.Id = tblDepartment.Customer_Id AND tblDepartment.isEMailDefault=1 " &
                            "LEFT JOIN tblRegistrationSourceCustomer ON tblCustomer.Id = tblRegistrationSourceCustomer.Customer_Id AND tblRegistrationSourceCustomer.SystemCode=3 " &
                            "INNER JOIN tblWorkingGroup ON tblCustomer.Id=tblWorkingGroup.Customer_Id " &
                        "WHERE (tblWorkingGroup.POP3UserName IS NOT NULL AND tblWorkingGroup.POP3UserName != '') " &
                        "ORDER BY tblCustomer.Name "

            dt = getDataTable(gsConnectionString, sSQL)

            Dim c As Customer

            For Each dr In dt.Rows
                c = New Customer(dr)
                colCustomer.Add(c)
            Next

            Return colCustomer
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function GetCustomerDataFromEmailBoxInUse(sPop3UserName As String, syncType As SyncType) As Customer
        Dim cCustomer As New Customer()
        Dim sSQL As String
        If (syncType = SyncType.SyncByWorkingGroup) Then
            ' Use parameterized query to prevent SQL injection
            sSQL = "  select tblSettings.Customer_Id, tblSettings.POP3Server, tblSettings.POP3Port, tblSettings.EMailFolder, tblSettings.EMailFolderArchive, tblSettings.EwsApplicationId, tblSettings.EwsClientSecret, tblSettings.EwsTenantId," &
            " tblWorkingGroup.Pop3UserName from tblSettings " &
            " inner join tblWorkingGroup on tblWorkingGroup.Customer_Id = tblSettings.Customer_Id " &
            " WHERE tblWorkingGroup.Pop3UserName = @Pop3UserName "
        Else
            sSQL = "  select tblSettings.Customer_Id, tblSettings.POP3Server, tblSettings.POP3Port, tblSettings.EMailFolder, tblSettings.EMailFolderArchive, tblSettings.EwsApplicationId, tblSettings.EwsClientSecret, tblSettings.EwsTenantId," &
            " tblSettings.Pop3UserName from tblSettings " &
            " WHERE tblSettings.Pop3UserName = @Pop3UserName "
        End If

        ' Log if needed
        If giLoglevel > 0 Then
            objLogFile.WriteLine(Now() & ", GetCustomerDataFromEmailBoxInUse, SQL query for ID: " & sPop3UserName)
        End If

        Using connection As New SqlConnection(gsConnectionString)
            Using cmd As New SqlCommand(sSQL, connection)
                ' Add parameter
                cmd.Parameters.AddWithValue("@Pop3UserName", sPop3UserName)

                Try
                    connection.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            cCustomer.POP3UserName = reader("POP3UserName").ToString()
                            cCustomer.POP3Port = reader("POP3Port").ToString()
                            cCustomer.POP3Server = reader("POP3Server").ToString()
                            cCustomer.EMailFolder = reader("EMailFolder").ToString()
                            cCustomer.EMailFolderArchive = reader("EMailFolderArchive").ToString()
                            cCustomer.EwsApplicationId = reader("EwsApplicationId").ToString()
                            cCustomer.EwsClientSecret = reader("EwsClientSecret").ToString()
                            cCustomer.EwsTenantId = reader("EwsTenantId").ToString()
                            cCustomer.Id = reader("Customer_Id").ToString()
                        End If
                    End Using
                Catch ex As Exception
                    ' Log the exception details
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", GetCustomerDataFromEmailBoxInUse, Error: " & ex.Message)
                    End If
                    Throw New Exception("Error retrieving working group email data: " & ex.Message, ex)
                End Try
            End Using
        End Using

        Return cCustomer
    End Function
    Public Function GetCaseFieldsSettings(customerId As Integer) As Dictionary(Of String, String)
        Dim sSQL As String
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ret As New Dictionary(Of String, String)

        Try
            sSQL = "select CaseField, Mail2TicketIdentifier from tblCaseFieldSettings where (Customer_Id = " & customerId.ToString() & ") and (Mail2TicketIdentifier != '') "

            dt = getDataTable(gsConnectionString, sSQL)

            For Each dr In dt.Rows
                ret.Add(dr(0).ToString(), dr(1).ToString())
            Next

            Return ret

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function validateAPIKey(ByVal APIKey As String) As Boolean
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblCustomer.Id FROM tblCustomer WHERE CustomerGUID='" & APIKey & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
