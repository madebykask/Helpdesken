'Imports System.Data

<Serializable()> Public Class Customer

#Region "Declarations"
    Private miId As Integer
    Private msCustomerNumber As String
    Private msName As String
    Private msHelpdeskEMail As String
    Private msWorkingGroupEMail As String
    Private miCaseWorkingGroupSource As Integer
    Private miLanguage_Id As Integer
    Private miMailServerProtocol As Integer
    Private msPOP3Server As String
    Private msPOP3UserName As String
    Private msPOP3Password As String
    Private miPOP3Port As Integer
    Private miPOP3DebugLevel As Integer
    Private msPOP3EMailPrefix As String
    Private miWorkingDayStart As Integer
    Private miWorkingDayEnd As Integer

    Private miEMailDefaultCaseType_Id As Integer
    Private miEMailDefaultCategory_Id As Integer
    Private miEMailDefaultProductArea_Id As Integer
    Private miEMailDefaultPriority_Id As Integer
    Private miEMailDefaultDepartment_Id As Integer
    Private miDefaultStatus_Id As Integer
    Private miDefaultStateSecondary_Id As Integer
    Private miDefaultAdministratorExternalUser_Id As Integer
    Private miDefaultWorkingGroup_Id As Integer
    Private msEMailSubjectPattern As String
    Private msExternalEMailSubjectPattern As String
    Private msEMailAnswerSeparator As String
    Private miEMailAnswerDestination As Integer
    Private miEMailRegistrationMailID As Integer
    Private miEMailImportType As Integer
    Private msEMailFolder As String
    Private msEMailFolderArchive As String

    Private msXMLFileFolder As String
    Private miXMLLogLevel As Integer
    Private miXMLAllFiles As Integer

    Private msLDAPServerName As String = ""
    Private msLDAPUserName As String = ""
    Private msLDAPPassword As String = ""
    Private msLDAPBase As String = ""
    Private msLDAPFilter As String = ""
    Private miLDAPAuthenticationType As Integer = 0
    Private miLDAPAllUsers As Integer = 0

    Private miLDAPLogLevel As Integer
    Private miLDAPSyncType As Integer
    Private miDays2WaitBeforeDelete As Integer

    Private miOverwriteFromMasterDirectory As Integer
    Private miInventoryDays2WaitBeforeDelete As Integer = 0
    Private miOpenCase_StateSecondary_Id As Integer

    Private miModuleOrder As Integer = 0
    Private miModuleAccount As Integer = 0
    Private miInventoryCreate As Integer = 1

    Private msAllowedEMailRecipients As String
    Private msCaseStatisticsEMailList As String
    Private msNewCaseEMailList As String

    Private msPhysicalFilePath As String

    Private miRegistrationSourceCustomer_Id As Integer
    Private miDefaultEmailLogDestination As Integer
    Private miTimeZone_offset As Integer
    Private miLDAPCreateOrganization As Integer

    Private miNewCaseMailTo As Integer

    ' Ews Exchange Web Services
    Private msEwsApplicationId As String
    Private msEwsClientSecret As String
    Private msEwsTenantId As String
    Private mbUseEws As Boolean

    Private strBlockedEmailRecipients As String
    Private strErrorMailTo As String


#End Region

#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal dr As DataRow)
        Try
            miId = dr("Id")

            If Not IsDBNull(dr("CustomerNumber")) Then
                msCustomerNumber = dr("CustomerNumber")
            End If

            If Not IsDBNull(dr("Name")) Then
                msName = dr("Name")
            End If

            msHelpdeskEMail = dr("HelpdeskEMail")
            If Not IsDBNull(dr("WorkingGroupEMail")) Then
                msWorkingGroupEMail = dr("WorkingGroupEMail")
            End If
            miCaseWorkingGroupSource = dr("CaseWorkingGroupSource")
            miLanguage_Id = dr("Language_Id")
            miMailServerProtocol = dr("MailServerProtocol")
            If Not IsDBNull(dr("POP3Server")) Then
                msPOP3Server = dr("POP3Server")
            End If
            If Not IsDBNull(dr("POP3UserName")) Then
                msPOP3UserName = dr("POP3UserName")
            End If
            If Not IsDBNull(dr("POP3Password")) Then
                msPOP3Password = dr("POP3Password")
            End If

            miPOP3Port = dr("POP3Port")
            miPOP3DebugLevel = dr("POP3DebugLevel")

            If IsDBNull(dr("POP3EMailPrefix")) Then
                msPOP3EMailPrefix = ""
            Else
                msPOP3EMailPrefix = dr("POP3EMailPrefix")
            End If

            miWorkingDayStart = dr("WorkingDayStart")
            miWorkingDayEnd = dr("WorkingDayEnd")

            If Not IsDBNull(dr("WorkingGroupDefaultCaseType_Id")) Then
                miEMailDefaultCaseType_Id = dr("WorkingGroupDefaultCaseType_Id")
            Else
                If IsDBNull(dr("EMailDefaultCaseType_Id")) Then
                    miEMailDefaultCaseType_Id = 0
                Else
                    miEMailDefaultCaseType_Id = dr("EMailDefaultCaseType_Id")
                End If
            End If


            If IsDBNull(dr("EMailDefaultCategory_Id")) Then
                miEMailDefaultCategory_Id = 0
            Else
                miEMailDefaultCategory_Id = dr("EMailDefaultCategory_Id")
            End If

            If IsDBNull(dr("EMailDefaultProductArea_Id")) Then
                miEMailDefaultProductArea_Id = 0
            Else
                miEMailDefaultProductArea_Id = dr("EMailDefaultProductArea_Id")
            End If

            If IsDBNull(dr("EMailDefaultPriority_Id")) Then
                miEMailDefaultPriority_Id = 0
            Else
                miEMailDefaultPriority_Id = dr("EMailDefaultPriority_Id")
            End If

            If IsDBNull(dr("EMailDefaultDepartment_Id")) Then
                miEMailDefaultDepartment_Id = 0
            Else
                miEMailDefaultDepartment_Id = dr("EMailDefaultDepartment_Id")
            End If

            If IsDBNull(dr("DefaultStatus_Id")) Then
                miDefaultStatus_Id = 0
            Else
                miDefaultStatus_Id = dr("DefaultStatus_Id")
            End If

            If IsDBNull(dr("DefaultStateSecondary_Id2")) Then
                If IsDBNull(dr("DefaultStateSecondary_Id")) Then
                    miDefaultStateSecondary_Id = 0
                Else
                    miDefaultStateSecondary_Id = dr("DefaultStateSecondary_Id")
                End If
            Else
                miDefaultStateSecondary_Id = dr("DefaultStateSecondary_Id2")
            End If

            If IsDBNull(dr("DefaultAdministratorExternal")) Then
                miDefaultAdministratorExternalUser_Id = 0
            Else
                miDefaultAdministratorExternalUser_Id = dr("DefaultAdministratorExternal")
            End If

            miDefaultWorkingGroup_Id = dr("WorkingGroup_Id")

            If IsDBNull(dr("EMailSubjectPattern")) Then
                msEMailSubjectPattern = ""
            Else
                msEMailSubjectPattern = dr("EMailSubjectPattern")
            End If

            If IsDBNull(dr("ExternalEMailSubjectPattern")) Then
                msExternalEMailSubjectPattern = ""
            Else
                msExternalEMailSubjectPattern = dr("ExternalEMailSubjectPattern")
            End If

            If IsDBNull(dr("EMailAnswerSeparator")) Then
                msEMailAnswerSeparator = ""
            Else
                msEMailAnswerSeparator = dr("EMailAnswerSeparator")
            End If

            If IsDBNull(dr("EMailFolder")) Then
                msEMailFolder = ""
            Else
                msEMailFolder = dr("EMailFolder")
            End If

            If IsDBNull(dr("EMailFolderArchive")) Then
                msEMailFolderArchive = ""
            Else
                msEMailFolderArchive = dr("EMailFolderArchive")
            End If

            miEMailAnswerDestination = dr("EMailAnswerDestination")
            miEMailRegistrationMailID = dr("EMailRegistrationMailID")
            miEMailImportType = dr("EMailImportType")

            If IsDBNull(dr("XMLFileFolder")) Then
                msXMLFileFolder = ""
            Else
                msXMLFileFolder = dr("XMLFileFolder")
            End If

            miXMLLogLevel = dr("XMLLogLevel")
            miXMLAllFiles = dr("XMLAllFiles")

            If Not IsDBNull(dr("LDAPServerName")) Then
                msLDAPServerName = dr("LDAPServerName")
            End If

            If Not IsDBNull(dr("LDAPUserName")) Then
                msLDAPUserName = dr("LDAPUserName")
            End If

            If Not IsDBNull(dr("LDAPPassword")) Then
                msLDAPPassword = dr("LDAPPassword")
            End If

            If Not IsDBNull(dr("LDAPBase")) Then
                msLDAPBase = dr("LDAPBase")
            End If

            If Not IsDBNull(dr("LDAPFilter")) Then
                msLDAPFilter = dr("LDAPFilter")
            End If

            miLDAPLogLevel = dr("LDAPLogLevel")
            miLDAPSyncType = dr("LDAPSyncType")
            miLDAPAuthenticationType = dr("LDAPAuthenticationType")
            miLDAPAllUsers = dr("LDAPAllUsers")

            miOverwriteFromMasterDirectory = dr("OverwriteFromMasterDirectory")
            miDays2WaitBeforeDelete = dr("Days2WaitBeforeDelete")
            If Not IsDBNull(dr("InventoryDays2WaitBeforeDelete")) Then
                miInventoryDays2WaitBeforeDelete = dr("InventoryDays2WaitBeforeDelete")
            Else
                miInventoryDays2WaitBeforeDelete = 0
            End If

            If Not IsDBNull(dr("OpenCase_StateSecondary_Id")) Then
                miOpenCase_StateSecondary_Id = dr("OpenCase_StateSecondary_Id")
            Else
                miOpenCase_StateSecondary_Id = 0
            End If

            miInventoryCreate = dr("InventoryCreate")

            If Not IsDBNull(dr("AllowedEMailRecipients")) Then
                msAllowedEMailRecipients = dr("AllowedEMailRecipients")
            End If

            If Not IsDBNull(dr("CaseStatisticsEMailList")) Then
                msCaseStatisticsEMailList = dr("CaseStatisticsEMailList")
            End If

            If Not IsDBNull(dr("NewCaseEMailList")) Then
                msNewCaseEMailList = dr("NewCaseEMailList")
            End If

            If Not IsDBNull(dr("PhysicalFilePath")) Then
                msPhysicalFilePath = dr("PhysicalFilePath")
            End If

            If Not IsDBNull(dr("RegistrationSourceCustomer_Id")) Then
                miRegistrationSourceCustomer_Id = dr("RegistrationSourceCustomer_Id")
            End If


            If Not IsDBNull(dr("EwsApplicationId")) Then
                msEwsApplicationId = dr("EwsApplicationId")
            End If
            If Not IsDBNull(dr("EwsClientSecret")) Then
                msEwsClientSecret = dr("EwsClientSecret")
            End If
            If Not IsDBNull(dr("EwsTenantId")) Then
                msEwsTenantId = dr("EwsTenantId")
            End If
            mbUseEws = dr("UseEws")

            miModuleOrder = dr("ModuleOrder")
            miModuleAccount = dr("ModuleAccount")

            miDefaultEmailLogDestination = dr("DefaultEmailLogDestination")
            miTimeZone_offset = dr("TimeZone_offset")
            miLDAPCreateOrganization = dr("LDAPCreateOrganization")

            miNewCaseMailTo = dr("M2TNewCaseMailTo")

            If Not IsDBNull(dr("BlockedEmailRecipients")) Then
                strBlockedEmailRecipients = dr("BlockedEmailRecipients")
            End If

            If Not IsDBNull(dr("ErrorMailTo")) Then
                strErrorMailTo = dr("ErrorMailTo")
            End If


        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "Properties"
    Public Property Id() As Integer
        Get
            Return miId
        End Get
        Set(ByVal Value As Integer)
            miId = Value
        End Set
    End Property

    Public Property CustomerNumber() As String
        Get
            Return msCustomerNumber
        End Get
        Set(ByVal Value As String)
            msCustomerNumber = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return msName
        End Get
        Set(ByVal Value As String)
            msName = Value
        End Set
    End Property

    Public Property HelpdeskEMail() As String
        Get
            If SharedFunctions.IsValidEmailAddress(msWorkingGroupEMail) = True Then
                Return msWorkingGroupEMail
            Else
                Return msHelpdeskEMail
            End If

        End Get
        Set(ByVal Value As String)
            msHelpdeskEMail = Value
        End Set
    End Property

    Public Property CaseWorkingGroupSource() As String
        Get
            Return miCaseWorkingGroupSource
        End Get
        Set(ByVal Value As String)
            miCaseWorkingGroupSource = Value
        End Set
    End Property

    Public Property Language_Id() As Integer
        Get
            Return miLanguage_Id
        End Get
        Set(ByVal Value As Integer)
            miLanguage_Id = Value
        End Set
    End Property

    Public Property MailServerProtocol() As Integer
        Get
            Return miMailServerProtocol
        End Get
        Set(ByVal Value As Integer)
            miMailServerProtocol = Value
        End Set
    End Property

    Public Property POP3Server() As String
        Get
            Return msPOP3Server
        End Get
        Set(ByVal Value As String)
            msPOP3Server = Value
        End Set
    End Property

    Public Property POP3UserName() As String
        Get
            Return msPOP3UserName
        End Get
        Set(ByVal Value As String)
            msPOP3UserName = Value
        End Set
    End Property

    Public Property POP3Password() As String
        Get
            Return msPOP3Password
        End Get
        Set(ByVal Value As String)
            msPOP3Password = Value
        End Set
    End Property

    Public Property POP3Port() As Integer
        Get
            Return miPOP3Port
        End Get
        Set(ByVal Value As Integer)
            miPOP3Port = Value
        End Set
    End Property

    Public Property POP3DebugLevel() As Integer
        Get
            Return miPOP3DebugLevel
        End Get
        Set(ByVal Value As Integer)
            miPOP3DebugLevel = Value
        End Set
    End Property

    Public Property POP3EMailPrefix() As String
        Get
            Return msPOP3EMailPrefix
        End Get
        Set(ByVal Value As String)
            msPOP3EMailPrefix = Value
        End Set
    End Property

    Public Property WorkingDayStart() As Integer
        Get
            Return miWorkingDayStart
        End Get
        Set(ByVal Value As Integer)
            miWorkingDayStart = Value
        End Set
    End Property

    Public Property WorkingDayEnd() As Integer
        Get
            Return miWorkingDayEnd
        End Get
        Set(ByVal Value As Integer)
            miWorkingDayEnd = Value
        End Set
    End Property

    Public Property EMailDefaultCaseType_Id() As Integer
        Get
            Return miEMailDefaultCaseType_Id
        End Get
        Set(ByVal Value As Integer)
            miEMailDefaultCaseType_Id = Value
        End Set
    End Property

    Public Property EMailDefaultCategory_Id() As Integer
        Get
            Return miEMailDefaultCategory_Id
        End Get
        Set(ByVal Value As Integer)
            miEMailDefaultCategory_Id = Value
        End Set
    End Property

    Public Property EMailDefaultProductArea_Id() As Integer
        Get
            Return miEMailDefaultProductArea_Id
        End Get
        Set(ByVal Value As Integer)
            miEMailDefaultProductArea_Id = Value
        End Set
    End Property

    Public Property EMailDefaultPriority_Id() As Integer
        Get
            Return miEMailDefaultPriority_Id
        End Get
        Set(ByVal Value As Integer)
            miEMailDefaultPriority_Id = Value
        End Set
    End Property

    Public Property DefaultStatus_Id() As Integer
        Get
            Return miDefaultStatus_Id
        End Get
        Set(ByVal Value As Integer)
            miDefaultStatus_Id = Value
        End Set
    End Property

    Public Property DefaultStateSecondary_Id() As Integer
        Get
            Return miDefaultStateSecondary_Id
        End Get
        Set(ByVal Value As Integer)
            miDefaultStateSecondary_Id = Value
        End Set
    End Property

    Public Property EMailDefaultDepartment_Id() As Integer
        Get
            Return miEMailDefaultDepartment_Id
        End Get
        Set(ByVal Value As Integer)
            miEMailDefaultDepartment_Id = Value
        End Set
    End Property

    Public Property DefaultAdministratorExternalUser_Id() As Integer
        Get
            Return miDefaultAdministratorExternalUser_Id
        End Get
        Set(ByVal Value As Integer)
            miDefaultAdministratorExternalUser_Id = Value
        End Set
    End Property

    Public Property DefaultWorkingGroup_Id() As Integer
        Get
            Return miDefaultWorkingGroup_Id
        End Get
        Set(ByVal Value As Integer)
            miDefaultWorkingGroup_Id = Value
        End Set
    End Property

    Public Property EMailSubjectPattern() As String
        Get
            Return msEMailSubjectPattern
        End Get
        Set(ByVal Value As String)
            msEMailSubjectPattern = Value
        End Set
    End Property

    Public Property ExternalEMailSubjectPattern() As String
        Get
            Return msExternalEMailSubjectPattern
        End Get
        Set(ByVal Value As String)
            msExternalEMailSubjectPattern = Value
        End Set
    End Property

    Public Property EMailAnswerSeparator() As String
        Get
            Return msEMailAnswerSeparator
        End Get
        Set(ByVal Value As String)
            msEMailAnswerSeparator = Value
        End Set
    End Property

    Public Property EMailAnswerDestination() As Integer
        Get
            Return miEMailAnswerDestination
        End Get
        Set(ByVal Value As Integer)
            miEMailAnswerDestination = Value
        End Set
    End Property

    Public Property EMailRegistrationMailID() As Integer
        Get
            Return miEMailRegistrationMailID
        End Get
        Set(ByVal Value As Integer)
            miEMailRegistrationMailID = Value
        End Set
    End Property

    Public Property EMailImportType() As Integer
        Get
            Return miEMailImportType
        End Get
        Set(ByVal Value As Integer)
            miEMailImportType = Value
        End Set
    End Property

    Public Property EMailFolder() As String
        Get
            Return msEMailFolder
        End Get
        Set(ByVal Value As String)
            msEMailFolder = Value
        End Set
    End Property

    Public Property EMailFolderArchive() As String
        Get
            Return msEMailFolderArchive
        End Get
        Set(ByVal Value As String)
            msEMailFolderArchive = Value
        End Set
    End Property

    Public Property XMLFileFolder() As String
        Get
            Return msXMLFileFolder
        End Get
        Set(ByVal Value As String)
            msXMLFileFolder = Value
        End Set
    End Property

    Public Property XMLLogLevel() As Integer
        Get
            Return miXMLLogLevel
        End Get
        Set(ByVal Value As Integer)
            miXMLLogLevel = Value
        End Set
    End Property

    Public Property XMLAllFiles() As Integer
        Get
            Return miXMLAllFiles
        End Get
        Set(ByVal Value As Integer)
            miXMLAllFiles = Value
        End Set
    End Property

    Public Property OverwriteFromMasterDirectory() As Integer
        Get
            Return miOverwriteFromMasterDirectory
        End Get
        Set(ByVal Value As Integer)
            miOverwriteFromMasterDirectory = Value
        End Set
    End Property

    Public Property LDAPServerName As String
        Get
            Return msLDAPServerName
        End Get
        Set(ByVal Value As String)
            msLDAPServerName = Value
        End Set
    End Property

    Public Property LDAPUserName As String
        Get
            Return msLDAPUserName
        End Get
        Set(ByVal Value As String)
            msLDAPUserName = Value
        End Set
    End Property

    Public Property LDAPPassword As String
        Get
            Return msLDAPPassword
        End Get
        Set(ByVal Value As String)
            msLDAPPassword = Value
        End Set
    End Property

    Public Property LDAPBase As String
        Get
            Return msLDAPBase
        End Get
        Set(ByVal Value As String)
            msLDAPBase = Value
        End Set
    End Property

    Public Property LDAPFilter As String
        Get
            Dim dt As DateTime = Now.AddDays(-2)
            Dim t As String = dt.Year.ToString() & dt.Month.ToString.PadLeft(2, "0") & dt.Day.ToString.PadLeft(2, "0")
            Return msLDAPFilter.Replace("YYYYMMDD", t)
        End Get
        Set(ByVal Value As String)
            msLDAPFilter = Value
        End Set
    End Property

    Public Property LDAPLogLevel As Integer
        Get
            Return miLDAPLogLevel
        End Get
        Set(ByVal Value As Integer)
            miLDAPLogLevel = Value
        End Set
    End Property

    Public Property LDAPSyncType As Integer
        Get
            Return miLDAPSyncType
        End Get
        Set(ByVal Value As Integer)
            miLDAPSyncType = Value
        End Set
    End Property

    Public Property LDAPAllUsers As Integer
        Get
            Return miLDAPAllUsers
        End Get
        Set(ByVal Value As Integer)
            miLDAPAllUsers = Value
        End Set
    End Property

    Public Property LDAPAuthenticationType As Integer
        Get
            Return miLDAPAuthenticationType
        End Get
        Set(ByVal Value As Integer)
            miLDAPAuthenticationType = Value
        End Set
    End Property

    Public Property Days2WaitBeforeDelete As Integer
        Get
            Return miDays2WaitBeforeDelete
        End Get
        Set(ByVal Value As Integer)
            miDays2WaitBeforeDelete = Value
        End Set
    End Property

    Public Property InventoryDays2WaitBeforeDelete As Integer
        Get
            Return miInventoryDays2WaitBeforeDelete
        End Get
        Set(ByVal Value As Integer)
            miInventoryDays2WaitBeforeDelete = Value
        End Set
    End Property

    Public Property ModuleOrder As Integer
        Get
            Return miModuleOrder
        End Get
        Set(ByVal Value As Integer)
            miModuleOrder = Value
        End Set
    End Property

    Public Property ModuleAccount As Integer
        Get
            Return miModuleAccount
        End Get
        Set(ByVal Value As Integer)
            miModuleAccount = Value
        End Set
    End Property

    Public Property OpenCase_StateSecondary_Id As Integer
        Get
            Return miOpenCase_StateSecondary_Id
        End Get
        Set(ByVal Value As Integer)
            miOpenCase_StateSecondary_Id = Value
        End Set
    End Property

    Public Property InventoryCreate As Integer
        Get
            Return miInventoryCreate
        End Get
        Set(ByVal Value As Integer)
            miInventoryCreate = Value
        End Set
    End Property

    Public Property AllowedEMailRecipients As String
        Get
            Return msAllowedEMailRecipients
        End Get
        Set(ByVal Value As String)
            msAllowedEMailRecipients = Value
        End Set
    End Property

    Public Property CaseStatisticsEMailList As String
        Get
            Return msCaseStatisticsEMailList
        End Get
        Set(ByVal Value As String)
            msCaseStatisticsEMailList = Value
        End Set
    End Property

    Public Property NewCaseEMailList As String
        Get
            Return msNewCaseEMailList
        End Get
        Set(ByVal Value As String)
            msNewCaseEMailList = Value
        End Set
    End Property

    Public Property PhysicalFilePath As String
        Get
            Return msPhysicalFilePath
        End Get
        Set(ByVal Value As String)
            msPhysicalFilePath = Value
        End Set
    End Property

    Public Property RegistrationSourceCustomer_Id() As Integer
        Get
            Return miRegistrationSourceCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miRegistrationSourceCustomer_Id = Value
        End Set
    End Property

    Public Property DefaultEmailLogDestination() As Integer
        Get
            Return miDefaultEmailLogDestination
        End Get
        Set(ByVal Value As Integer)
            miDefaultEmailLogDestination = Value
        End Set
    End Property

    Public Property TimeZone_offset() As Integer
        Get
            Return miTimeZone_offset
        End Get
        Set(ByVal Value As Integer)
            miTimeZone_offset = Value
        End Set
    End Property

    Public Property LDAPCreateOrganization() As Integer
        Get
            Return miLDAPCreateOrganization
        End Get
        Set(ByVal Value As Integer)
            miLDAPCreateOrganization = Value
        End Set
    End Property

    Public Property NewCaseMailTo() As Integer
        Get
            Return miNewCaseMailTo
        End Get
        Set(ByVal Value As Integer)
            miNewCaseMailTo = Value
        End Set
    End Property


    Public Property EwsApplicationId As String
        Get
            Return msEwsApplicationId
        End Get
        Set(ByVal Value As String)
            msEwsApplicationId = Value
        End Set
    End Property

    Public Property EwsClientSecret As String
        Get
            Return msEwsClientSecret
        End Get
        Set(ByVal Value As String)
            msEwsClientSecret = Value
        End Set
    End Property

    Public Property EwsTenantId As String
        Get
            Return msEwsTenantId
        End Get
        Set(ByVal Value As String)
            msEwsTenantId = Value
        End Set
    End Property

    Public Property UseEws As Boolean
        Get
            Return mbUseEws
        End Get
        Set(ByVal Value As Boolean)
            mbUseEws = Value
        End Set
    End Property

    Public Property BlockedEmailRecipients As String
        Get
            Return strBlockedEmailRecipients
        End Get
        Set(ByVal Value As String)
            strBlockedEmailRecipients = Value
        End Set
    End Property

    Public Property ErrorMailTo As String
        Get
            Return strErrorMailTo
        End Get
        Set(ByVal Value As String)
            strErrorMailTo = Value
        End Set
    End Property

#End Region
End Class
