Imports System.DirectoryServices
Imports System.Data.SqlClient

Imports System.Configuration
Imports System.IO

Imports DH.Helpdesk.Library

Module DH_Helpdesk_LDAP_AD
    Private miConnectionType As SharedFunctions.ConnectionType
    Private miDomain_Id As Integer
    Private miCustomer_Id As Integer

    Public Sub Main()
        ' 1: SyncByCustomer
        ' 2: SyncByCustomerHTTP
        ' 3: syncByDomain
        ' 4: SyncByDomainHTTP

        ' Secure connection string
        Dim secureConnectionString As String = ConfigurationManager.AppSettings("SecureConnectionString")
        If (Not String.IsNullOrEmpty(secureConnectionString) AndAlso secureConnectionString.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) Then
            Dim fileName = Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location)
            ToggleConfigEncryption(fileName)
        End If

        Dim sCommand As String = Command()
        Dim aArguments() As String = sCommand.Split(",")

        Dim mode As Integer = GetConfigIntSafe("Mode", 1) ' Default is 1
        Dim sConnectionstring As String = ConfigurationManager.ConnectionStrings("Helpdesk")?.ConnectionString
        Dim iSyncById As Integer = GetConfigIntSafe("SyncById", 1)
        Dim sLogPath As String = String.Empty


        'Process SqlClientMetaDataCollectionNames Arguements
        If aArguments IsNot Nothing AndAlso aArguments.Length > 0 Then
            'Override arfs with values from command line
            mode = GetCmdArgOrDefault(aArguments, 0, mode)
            sConnectionstring = GetCmdArgOrDefault(aArguments, 1, sConnectionstring)
            iSyncById = GetCmdArgOrDefaultInt(aArguments, 2, iSyncById)
            sLogPath = GetCmdArgOrDefault(aArguments, 3, gsLogPath)
        End If

        'Log input params
        Try
            openLogFile()
            Dim logConnectionString As String = FormatConnectionString(sConnectionstring)

            LogToFile(String.Format(
                "Cmd Line Args:" & vbCrLf & vbTab &
                "- Mode: {0}" & vbCrLf & vbTab &
                "- ConnectionString: {1}" & vbCrLf & vbTab &
                "- CustomerOrDomain: {2}" & vbCrLf & vbTab &
                "- Log path: {3}",
                mode, logConnectionString, iSyncById, sLogPath), 1)
        Finally
            closeLogFile()
        End Try

        If mode = 1 Then
            miConnectionType = SharedFunctions.ConnectionType.DB
            gsLogPath = IIf(String.IsNullOrEmpty(sLogPath), gsLogPath, sLogPath)
            'By customer
            SyncByCustomer(sConnectionstring, iSyncById)
        ElseIf mode = 3 Then
            miConnectionType = SharedFunctions.ConnectionType.DB
            'By domain
            SyncByDomain(sConnectionstring, iSyncById)
        End If
    End Sub

    Private Function GetConfigIntSafe(appKey As String, Optional defaultValue As Integer = 0) As String
        Dim str As String = ConfigurationManager.AppSettings(appKey)
        Dim val As Integer = defaultValue
        If (Int32.TryParse(str, val)) Then
            Return val
        End If
        Return defaultValue
    End Function
    Private Function GetCmdArgOrDefault(args As String(), index As Int32, defaultValue As String) As String
        Dim val As String = defaultValue
        If args.Length > index AndAlso Not String.IsNullOrEmpty(args(index)) Then
            val = args(index)
        End If
        Return val
    End Function

    Private Function GetCmdArgOrDefaultInt(args As String(), index As Int32, defaultValue As Integer) As Integer
        Dim val As Integer = defaultValue
        If args.Length > index AndAlso Not String.IsNullOrEmpty(args(index)) Then
            val = CInt(args(index))
        End If
        Return val
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
            LogError(ex.ToString())
        End Try
    End Sub

    Private Function FormatConnectionString(connectionString As String) As String
        Dim builder As SqlConnectionStringBuilder = New SqlConnectionStringBuilder(connectionString)
        Return String.Format("Data Source={0}; Initial Catalog={1};Network Library={2}", builder.DataSource, builder.InitialCatalog, builder.NetworkLibrary)
    End Function

    Private Sub SyncByCustomer(ByVal sConnectionstring As String, Optional ByVal Customer_Id As Int32 = 0)
        miConnectionType = SharedFunctions.ConnectionType.DB
        gsConnectionString = sConnectionstring
        miCustomer_Id = Customer_Id

        Sync()
    End Sub

    Private Sub SyncByCustomerHTTP(ByVal sURL As String, Optional ByVal Customer_Id As Int32 = 0)
        miConnectionType = SharedFunctions.ConnectionType.HTTP
        gsURL = sURL
        miCustomer_Id = Customer_Id

        Sync()
    End Sub

    Private Sub SyncByDomain(ByVal sConnectionstring As String, Optional ByVal Domain_Id As Int32 = 0)
        miConnectionType = SharedFunctions.ConnectionType.DB
        gsConnectionString = sConnectionstring
        miDomain_Id = Domain_Id

        Sync()
    End Sub

    Private Sub SyncByDomainHTTP(ByVal sURL As String, Optional ByVal Domain_Id As Int32 = 0)
        miConnectionType = SharedFunctions.ConnectionType.HTTP
        gsURL = sURL
        miDomain_Id = Domain_Id

        Sync()
    End Sub

    Private Sub Sync()
        Dim objGlobalSettingsData As New GlobalSettingsData
        Dim objGlobalSettings As GlobalSettings

        Dim objCustomerData As New CustomerData
        Dim objCustomer As Customer = Nothing

        Dim objDepartmentData As New DepartmentData
        Dim colDepartment As Collection

        Dim objOUData As New OUData
        Dim colOU As Collection

        Dim objComputerUserData As New ComputerUserData
        Dim colComputerUserFieldSettings As New Collection

        Try

            objGlobalSettings = objGlobalSettingsData.getGlobalSettings()
            giDBType = objGlobalSettings.DBType
            gsDBVersion = objGlobalSettings.DBVersion

            If miDomain_Id <> 0 Then
                objCustomer = objCustomerData.getCustomerByDomain(miDomain_Id)

                miCustomer_Id = objCustomer.Id
            Else
                objCustomer = objCustomerData.getCustomerById(miCustomer_Id)
            End If

            giLoglevel = objCustomer.LDAPLogLevel

            If giLoglevel > 0 Then
                openLogFile()
            End If

            colComputerUserFieldSettings = objComputerUserData.getComputerUserFieldSettings(miCustomer_Id)

            ' Hämta inställningar för avdelningarna
            colDepartment = objDepartmentData.getDepartments(miCustomer_Id)

            ' Hämta inställningar för avdelningarna
            colOU = objOUData.getOUs(miCustomer_Id)

            If objCustomer.LDAPSyncType = 1 Or objCustomer.LDAPSyncType = 3 Or objCustomer.LDAPSyncType = 5 Then
                ' Starta synkning av detta träd
                syncUsers(colComputerUserFieldSettings,
                          colDepartment,
                          colOU,
                          objCustomer.LDAPServerName,
                          objCustomer.LDAPUserName,
                          objCustomer.LDAPPassword,
                          objCustomer.LDAPBase,
                          objCustomer.LDAPFilter,
                          objCustomer.LDAPAuthenticationType,
                          objCustomer.LDAPAllUsers,
                          objCustomer.OverwriteFromMasterDirectory)



                ' Kontrollera om ej synkade ska tas bort
                If objCustomer.Days2WaitBeforeDelete > 0 Then
                    objComputerUserData.DeleteUsers(miCustomer_Id, miDomain_Id, objCustomer.Days2WaitBeforeDelete)
                End If
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", Error, Sync " & ex.ToString)
            End If

            Throw ex
        Finally
            closeLogFile()
        End Try

    End Sub

    Private Sub syncUsers(ByVal colComputerUserFieldSettings As Collection,
                          ByVal colDepartment As Collection,
                          ByVal colOU As Collection,
                          ByVal sLDAPServerName As String,
                          ByVal sLDAPUserName As String,
                          ByVal sLDAPPassword As String,
                          ByVal sLDAPBase As String,
                          ByVal sLDAPFilter As String,
                          ByVal iLDAPAuthenticationType As Integer,
                          ByVal iLDAPAllUsers As Integer,
                          ByVal iOverwriteFromMasterDirectory As Integer)

        Dim sDN As String
        Dim iuserAccountControl As Long = 0
        Dim sLDAPAttributeDepartment As String = ""
        Dim sLDAPAttributeDepartment2 As String = ""
        Dim iStatus As Integer = 1
        Dim objComputerUserData As New ComputerUserData
        Dim objComputerUser As ComputerUser
        Dim sLDAPFilterDN As String = ""
        Dim iIndexEntry As Integer = 0
        Dim vLDAPBase() As String
        Dim sLDAPServerNameNew As String = sLDAPServerName
        Dim objOU As OU

        Try
            ' Kontrollera om LDAPBAse innehåller ;
            If sLDAPBase <> "" Then
                vLDAPBase = sLDAPBase.Split(";")
            Else
                ReDim vLDAPBase(0)
            End If

            For iCount As Integer = 0 To vLDAPBase.Length - 1

                sLDAPAttributeDepartment = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "Department_Id")

                ' Kontrollera om sLDAPAttributeDepartment innehåller ;, i så fall ska vi kombinera 2 attribut
                If InStr(sLDAPAttributeDepartment, ";", CompareMethod.Text) > 0 Then
                    sLDAPAttributeDepartment2 = Mid(sLDAPAttributeDepartment, InStr(sLDAPAttributeDepartment, ";", CompareMethod.Text) + 1)
                    sLDAPAttributeDepartment = Left(sLDAPAttributeDepartment, InStr(sLDAPAttributeDepartment, ";", CompareMethod.Text) - 1)
                End If

                If Trim(vLDAPBase(iCount)) <> "" Then
                    ' Kontrollera om Server innehåller DC
                    If sLDAPServerName.ToUpper.Contains("DC=") = True Then
                        sLDAPServerNameNew = "LDAP://" & Trim(vLDAPBase(iCount)) & "," & sLDAPServerName
                    Else
                        sLDAPServerNameNew = "LDAP://" & sLDAPServerName & "/" & Trim(vLDAPBase(iCount))
                    End If

                Else
                    sLDAPServerNameNew = "LDAP://" & sLDAPServerName
                End If

                If giLoglevel = 1 Then

                    objLogFile.WriteLine(Now() & ", SyncUsers " & sLDAPServerNameNew & ", " & sLDAPUserName & ", " & sLDAPPassword & ", " & sLDAPFilter & ", " & sLDAPAttributeDepartment)
                End If


                Dim objLDAPClient As DirectoryEntry

                If sLDAPUserName <> "" And sLDAPPassword <> "" Then
                    objLDAPClient = New DirectoryEntry(sLDAPServerNameNew, sLDAPUserName, sLDAPPassword)
                Else
                    objLDAPClient = New DirectoryEntry(sLDAPServerNameNew)
                End If

                If iLDAPAuthenticationType <> 0 Then
                    objLDAPClient.AuthenticationType = iLDAPAuthenticationType
                End If

                If giLoglevel = 1 Then
                    objLogFile.WriteLine(Now() & ", SyncUsers " & vLDAPBase(iCount))
                End If

                Dim searcher As New DirectorySearcher(objLDAPClient)

                ' Kontrollera om det är ett specialfilter per DN
                If sLDAPFilter.Contains(";(dn=") Then
                    sLDAPFilterDN = sLDAPFilter.Substring(sLDAPFilter.IndexOf(";(dn=") + 5)
                    sLDAPFilterDN = sLDAPFilterDN.Substring(0, sLDAPFilterDN.Length - 1)

                    sLDAPFilter = sLDAPFilter.Substring(0, sLDAPFilter.IndexOf(";(dn="))
                End If

                searcher.PageSize = 1000
                searcher.Filter = "(&(objectClass=user)" & sLDAPFilter & ")"

                Dim entries As SearchResultCollection = searcher.FindAll()

                If giLoglevel = 1 Then
                    objLogFile.WriteLine(Now() & ", SyncUsers, Users: " & entries.Count.ToString())
                End If

                For Each searchresult As SearchResult In entries
                    If giLoglevel = 1 Then
                        objLogFile.WriteLine(Now() & ", SyncUsers, searchrequest: " & searchresult.Path)
                    End If

                    Dim entry As DirectoryEntry

                    If sLDAPUserName <> "" And sLDAPPassword <> "" Then
                        entry = New DirectoryEntry(searchresult.Path, sLDAPUserName, sLDAPPassword)
                    Else
                        entry = New DirectoryEntry(searchresult.Path)
                    End If

                    If giLoglevel = 1 Then
                        objLogFile.WriteLine(Now() & ", SyncUsers, entry: " & entry.Path)
                    End If


                    If Not entry Is Nothing Then
                        iIndexEntry = iIndexEntry + 1

                        Try
                            If Not entry.Properties("distinguishedName") Is Nothing Then
                                sDN = entry.Properties("distinguishedName").Value

                                ' Kontrollera om det finns ett specialfilter
                                If sLDAPFilterDN.ToLower = "" Or sDN.ToLower.Contains(sLDAPFilterDN.ToLower) Then

                                    objComputerUser = objComputerUserData.getComputerUserByNDSPath(sDN, miCustomer_Id)

                                    If objComputerUser Is Nothing Then
                                        objComputerUser = New ComputerUser
                                        objComputerUser.NDSPath = sDN
                                    End If

                                    objComputerUser.Customer_Id = miCustomer_Id

                                    If miDomain_Id <> 0 Then
                                        objComputerUser.Domain_Id = miDomain_Id
                                    End If

                                    If giLoglevel = 1 Then
                                        objLogFile.WriteLine(Now() & ", SyncUsers, entry: " & sDN)
                                    End If

                                    iuserAccountControl = entry.Properties("userAccountControl").Value
                                    If iuserAccountControl = 0 Then iuserAccountControl = 512

                                    If giLoglevel = 1 Then
                                        objLogFile.WriteLine(Now() & ", SyncUsers, User: " & sDN & ", " & iuserAccountControl)
                                    End If

                                    If iuserAccountControl > 4194304 Then
                                        iuserAccountControl = iuserAccountControl - 4194304
                                    End If

                                    If iuserAccountControl > 65536 Then
                                        iuserAccountControl = iuserAccountControl - 65536
                                    End If

                                    If iuserAccountControl = 512 Or iuserAccountControl = 544 Or iLDAPAllUsers = 1 Then
                                        If iuserAccountControl = 512 Or iuserAccountControl = 544 Then
                                            iStatus = 1
                                        Else
                                            iStatus = 0
                                        End If

                                        objComputerUser.Status = iStatus

                                        ' Ta bort cn
                                        Dim iPos As Int32 = sDN.IndexOf(",")

                                        sDN = sDN.Substring(iPos + 1)

                                        ' Nollställ department
                                        objComputerUser.Department_Id = 0

                                        If sLDAPAttributeDepartment <> "" Then
                                            If sLDAPAttributeDepartment.ToLower = "memberof" Then
                                                Dim ig As Integer

                                                For ig = 0 To entry.Properties("memberOf").Count() - 1
                                                    objComputerUser.Department_Id = getDepartmentId(entry.Properties("memberOf")(ig).ToString(), colDepartment)

                                                    If objComputerUser.Department_Id <> 0 Then
                                                        Exit For
                                                    End If
                                                Next
                                            Else
                                                If Not entry.Properties(sLDAPAttributeDepartment) Is Nothing Then
                                                    If Not entry.Properties(sLDAPAttributeDepartment).Value Is Nothing Then
                                                        objComputerUser.Department_Id = getDepartmentByProperty(entry.Properties(sLDAPAttributeDepartment).Value, colDepartment)

                                                        If giLoglevel = 1 Then
                                                            objLogFile.WriteLine(Now() & ", SyncUsers, Department: " & entry.Properties(sLDAPAttributeDepartment).Value & ", " & objComputerUser.Department_Id)
                                                        End If
                                                    End If

                                                End If
                                            End If
                                        End If

                                        If objComputerUser.Department_Id = 0 Then
                                            objComputerUser.Department_Id = getDepartmentId(sDN, colDepartment)
                                        End If

                                        If giLoglevel = 1 Then
                                            objLogFile.WriteLine(Now() & ", SyncUsers, Department: " & sDN & ", " & objComputerUser.Department_Id)
                                        End If

                                        Dim sLDAPAttribute As String

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "UserId")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.UserId = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "FirstName")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.FirstName = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "SurName")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.SurName = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "DisplayName")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.DisplayName = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If


                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "Location")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.Location = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "Phone")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.Phone = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "CellPhone")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.CellPhone = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "Email")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.EMail = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "UserCode")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.UserCode = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "PostalAddress")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.PostalAddress = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "Title")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.Title = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "OU_Id")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objOU = getOUByProperty(entry.Properties(sLDAPAttribute).Value, colOU)

                                                    If Not objOU Is Nothing Then
                                                        objComputerUser.OU_Id = objOU.Id
                                                        objComputerUser.Department_Id = objOU.Department_Id
                                                    End If
                                                End If
                                            End If
                                        End If

                                        sLDAPAttribute = getComputerUserFieldSettingsValue(colComputerUserFieldSettings, "OU")
                                        If sLDAPAttribute <> "" Then
                                            If Not entry.Properties(sLDAPAttribute) Is Nothing Then
                                                If Not entry.Properties(sLDAPAttribute).Value Is Nothing Then
                                                    objComputerUser.OU = entry.Properties(sLDAPAttribute).Value
                                                End If

                                            End If
                                        End If

                                        objComputerUserData.save(objComputerUser, iOverwriteFromMasterDirectory)
                                    Else
                                        objComputerUserData.DisableUser(sDN, miCustomer_Id)

                                        If giLoglevel = 1 Then
                                            objLogFile.WriteLine(Now() & ", SyncUsers, DisableUser: " & sDN & ", " & iuserAccountControl)
                                        End If
                                    End If
                                End If
                            End If
                        Catch ex As Exception
                            If giLoglevel = 1 Then
                                objLogFile.WriteLine(Now() & ", SyncUsers, error: " & ex.Message.ToString)
                            End If
                        End Try


                    End If
                Next
                entries = Nothing
                searcher = Nothing
                objLDAPClient = Nothing
                iIndexEntry = 0
            Next
        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, SyncUsers " & ex.ToString)
            End If

            Throw ex
        End Try

    End Sub

    Private Function getComputerUserFieldSettingsValue(ByVal colComputerUserFieldSettings As Collection, ByVal sComputerUserField As String) As String
        Dim objComputerUserFieldSettings As ComputerUserFieldSettings

        For i As Integer = 1 To colComputerUserFieldSettings.Count
            objComputerUserFieldSettings = colComputerUserFieldSettings(i)

            If objComputerUserFieldSettings.ComputerUserField.ToString.ToLower = sComputerUserField.ToLower Then
                Return objComputerUserFieldSettings.LDAPAttribute
            End If
        Next

        Return ""
    End Function

    Private Function getDepartmentId(ByVal sDN As String, ByVal colDepartment As Collection) As Integer
        Dim objDepartment As Department
        Dim iDepartment_Id As Integer = 0

        ' Kontrollera om avdelningen finns 
        For i As Int32 = 1 To colDepartment.Count
            objDepartment = colDepartment(i)

            If objDepartment.NDSPath.Trim <> "" Then
                ' Om strängen innehåller ; så dela upp
                If objDepartment.NDSPath.Contains(";") Then
                    Dim a() As String = objDepartment.NDSPath.Split(";")

                    For j As Integer = 0 To a.Length - 1
                        If a(j).ToLower <> "" Then
                            If InStr(sDN.ToLower, a(j).ToLower, CompareMethod.Text) > 0 Then
                                iDepartment_Id = objDepartment.Id

                                Exit For
                            End If
                        End If
                    Next
                Else
                    If objDepartment.NDSPath <> "" Then
                        If InStr(sDN.ToLower, objDepartment.NDSPath.ToLower, CompareMethod.Text) > 0 Then
                            iDepartment_Id = objDepartment.Id

                            Exit For
                        End If
                    End If
                End If
            End If
        Next

        Return iDepartment_Id

        'If iDepartment_Id <> 0 Then
        '    Return iDepartment_Id
        'Else
        '    ' Prova med nästa nivå
        '    Dim ipos As Int32 = sDN.IndexOf(",")

        '    If ipos > 0 Then
        '        sDN = sDN.Substring(ipos + 1)

        '        Return getDepartmentId(sDN, colDepartment)

        '    Else
        '        Return iDepartment_Id
        '    End If
        'End If
    End Function

    Private Function getDepartmentByProperty(ByVal sDepartment As String, ByVal colDepartment As Collection) As String
        Dim objDepartment As Department

        ' Kontrollera om avdelningen finns 
        For i As Int32 = 1 To colDepartment.Count
            objDepartment = colDepartment(i)

            If objDepartment.SearchKey.ToLower = sDepartment.ToLower Then
                Return objDepartment.Id.ToString
            End If
        Next

        Return "0"
    End Function

    Private Function getOUByProperty(ByVal sOU As String, ByVal colOU As Collection) As OU
        Dim objOU As OU

        ' Kontrollera om avdelningen finns 
        For i As Int32 = 1 To colOU.Count
            objOU = colOU(i)

            If objOU.OU <> "" Then
                If objOU.OU.ToLower = sOU.ToLower Then
                    Return objOU
                End If
            End If
        Next

        Return Nothing
    End Function

    Private Sub openLogFile()
        Dim sDirPath As String
        Dim sFileName As String = String.Empty
        Dim sTemp As String

        If gsLogPath <> "" Then
            sDirPath = gsLogPath & "\"
        Else
            sDirPath = Environment.CurrentDirectory & "\log\"
        End If

        If Not Directory.Exists(sDirPath) Then
            Directory.CreateDirectory(sDirPath)
        End If

        sTemp = DatePart(DateInterval.Year, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Month, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Day, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Hour, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Minute, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp

        sTemp = DatePart(DateInterval.Second, Now())
        sTemp = sTemp.PadLeft(2, "0")
        sFileName = sFileName & sTemp & ".log"

        Dim sFilePath As String = Path.Combine(sDirPath, sFileName)
        objLogFile = New StreamWriter(sFilePath)
        objLogFile.WriteLine("----- Start -----")
    End Sub
    Private Sub closeLogFile()
        If objLogFile IsNot Nothing Then
            objLogFile.WriteLine("----- Slut -----")
            objLogFile.Close()
            objLogFile = Nothing
        End If
    End Sub
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

End Module
