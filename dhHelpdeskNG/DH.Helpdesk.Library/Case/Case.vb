Imports System.Data

<Serializable()> Public Class CCase

#Region "Declarations"

    Private miId As Integer
    Private msCaseGUID As String
    Private msCasenumber As Long
    Private miCustomer_Id As Integer
    Private miCaseType_Id As Integer
    Private msCaseTypeName As String
    Private miProductArea_Id As Integer
    Private msProductAreaName As String
    Private miCategory_Id As Integer
    Private msCategoryName As String
    Private miPriority_Id As Integer
    Private miRegion_Id As Integer
    Private miDepartment_Id As Integer
    Private msDepartment As String
    Private miOU_Id As Integer
    Private msCustomerName As String
    Private msReportedBy As String
    Private msPersons_Name As String
    Private msPersons_EMail As String
    Private msPersons_Phone As String
    Private msPersons_CellPhone As String
    Private msPlace As String
    Private msUserCode As String
    Private msCostCentre As String
    Private msInvoiceNumber As String
    Private msCaption As String
    Private msDescription As String
    Private msMiscellaneous As String
    Private msPriorityName As String
    Private msPriorityDescription As String
    Private miWorkingGroup_Id As Integer
    Private miPerformer_User_Id As Integer
    Private msPerformerFirstName As String
    Private msPerformerSurName As String
    Private msPerformerEMail As String
    Private miRegLanguage_Id As Integer
    Private msCaseWorkingGroup As String
    Private msPerformerWorkingGroup As String
    Private msPerformerWorkingGroup_Id As Integer
    Private msPerformerWorkingGroupAllocateCaseMail As Integer
    Private msPerformerWorkingGroupEMail As String
    Private mdtRegTime As DateTime
    Private mdtChangeTime As DateTime
    Private msInventoryNumber As String
    Private mdtFinishingDate As DateTime
    Private miAutomaticApproveTime As Integer
    Private miExternalUpdateMail As Integer
    Private miStatus_Id As Integer
    Private miStateSecondary_Id As Integer
    Private msStateSecondary As String
    Private miResetOnExternalUpdate As Integer = 0
    Private mdtWatchDate As DateTime
    Private miRegistrationSource As Integer = 3
    Private miForm_Id As Integer
    Private msWorkingGroupEMail As String
    Private msAllocateCaseMail As Integer = 0
    Private miHolidayHeader_Id As Integer = 1
    Private miRegistrationSourceCustomer_Id As Integer
    Private mcolLog As New List(Of Log)
    Private msRegUserName As String
    Private msChangedName As String
    Private msChangedSurName As String
    Private msAvailable As String
    Private msReferenceNumber As String
    Private miIncludeInCaseStatistics As Integer
    Private miExternalTime As Integer
    Private miLeadTime As Integer

#End Region

#Region "Constructors"

    Sub New()
        ' Skapa nytt GUID
        msCaseGUID = System.Guid.NewGuid.ToString()

    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        msCaseGUID = dr("CaseGUID").ToString
        msCasenumber = dr("CaseNumber")
        miCustomer_Id = dr("Customer_Id")
        miCaseType_Id = dr("CaseType_Id")
        msCaseTypeName = dr("CaseType")

        If IsDBNull(dr("ProductArea_Id")) Then
            miProductArea_Id = 0
            msProductAreaName = ""
        Else
            miProductArea_Id = dr("ProductArea_Id")
            msProductAreaName = dr("ProductArea")
        End If

        If IsDBNull(dr("Category_Id")) Then
            miCategory_Id = 0
            msCategoryName = ""
        Else
            miCategory_Id = dr("Category_Id")
            msCategoryName = dr("Category")
        End If

        If IsDBNull(dr("Priority_Id")) Then
            miPriority_Id = 0
        Else
            miPriority_Id = dr("Priority_Id")
        End If

        If IsDBNull(dr("Region_Id")) Then
            miRegion_Id = 0
        Else
            miRegion_Id = dr("Region_Id")
        End If

        If IsDBNull(dr("Department_Id")) Then
            miDepartment_Id = 0
        Else
            miDepartment_Id = dr("Department_Id")
        End If

        If IsDBNull(dr("OU_Id")) Then
            miOU_Id = 0
        Else
            miOU_Id = dr("OU_Id")
        End If

        If Not IsDBNull(dr("Department")) Then
            msDepartment = dr("Department")
        End If

        msCustomerName = dr("CustomerName")

        If IsDBNull(dr("Performer_User_Id")) Then
            miPerformer_User_Id = 0
        Else
            miPerformer_User_Id = dr("Performer_User_Id")
        End If

        miRegLanguage_Id = dr("RegLanguage_Id")

        If IsDBNull(dr("Caption")) Then
            msCaption = ""
        Else
            msCaption = dr("Caption")
        End If

        If IsDBNull(dr("Description")) Then
            msDescription = ""
        Else
            msDescription = dr("Description")
        End If

        If IsDBNull(dr("Miscellaneous")) Then
            msMiscellaneous = ""
        Else
            msMiscellaneous = dr("Miscellaneous")
        End If

        If IsDBNull(dr("PerformerFirstName")) Then
            msPerformerFirstName = ""
        Else
            msPerformerFirstName = dr("PerformerFirstName")
        End If

        If IsDBNull(dr("PerformerSurName")) Then
            msPerformerSurName = ""
        Else
            msPerformerSurName = dr("PerformerSurName")
        End If

        If IsDBNull(dr("PerformerEMail")) Then
            msPerformerEMail = ""
        Else
            msPerformerEMail = dr("PerformerEMail")
        End If

        If IsDBNull(dr("ReportedBy")) Then
            msReportedBy = ""
        Else
            msReportedBy = dr("ReportedBy")
        End If

        If IsDBNull(dr("Persons_Name")) Then
            msPersons_Name = ""
        Else
            msPersons_Name = dr("Persons_Name")
        End If

        If IsDBNull(dr("Persons_Phone")) Then
            msPersons_Phone = ""
        Else
            msPersons_Phone = dr("Persons_Phone")
        End If

        If IsDBNull(dr("Persons_CellPhone")) Then
            msPersons_CellPhone = ""
        Else
            msPersons_CellPhone = dr("Persons_CellPhone")
        End If

        If IsDBNull(dr("Persons_EMail")) Then
            msPersons_EMail = ""
        Else
            msPersons_EMail = dr("Persons_EMail")
        End If

        If IsDBNull(dr("Place")) Then
            msPlace = ""
        Else
            msPlace = dr("Place")
        End If

        If IsDBNull(dr("UserCode")) Then
            msUserCode = ""
        Else
            msUserCode = dr("UserCode")
        End If

        If IsDBNull(dr("CostCentre")) Then
            msCostCentre = ""
        Else
            msCostCentre = dr("CostCentre")
        End If

        If Not IsDBNull(dr("PriorityName")) Then
            msPriorityName = dr("PriorityName")
        Else
            msPriorityName = ""
        End If

        If Not IsDBNull(dr("PriorityDescription")) Then
            msPriorityDescription = dr("PriorityDescription")
        Else
            msPriorityDescription = ""
        End If

        If Not IsDBNull(dr("PerformerWorkingGroup")) Then
            msPerformerWorkingGroup = dr("PerformerWorkingGroup")
        Else
            msPerformerWorkingGroup = ""
        End If

        If Not IsDBNull(dr("PerformerWorkingGroup_Id")) Then
            msPerformerWorkingGroup_Id = dr("PerformerWorkingGroup_Id")
        End If

        If Not IsDBNull(dr("PerformerWorkingGroupAllocateCaseMail")) Then
            msPerformerWorkingGroupAllocateCaseMail = dr("PerformerWorkingGroupAllocateCaseMail")
        End If

        If Not IsDBNull(dr("PerformerWorkingGroupEMail")) Then
            msPerformerWorkingGroupEMail = dr("PerformerWorkingGroupEMail")
        Else
            msPerformerWorkingGroupEMail = ""
        End If

        If Not IsDBNull(dr("WorkingGroup_Id")) Then
            miWorkingGroup_Id = dr("WorkingGroup_Id")
        End If

        If Not IsDBNull(dr("CaseWorkingGroup")) Then
            msCaseWorkingGroup = dr("CaseWorkingGroup")
        Else
            msCaseWorkingGroup = ""
        End If
        mdtRegTime = dr("RegTime")
        mdtChangeTime = dr("ChangeTime")

        If Not IsDBNull(dr("InventoryNumber")) Then
            msInventoryNumber = dr("InventoryNumber")
        Else
            msInventoryNumber = ""
        End If

        If Not IsDBNull(dr("InvoiceNumber")) Then
            msInvoiceNumber = dr("InvoiceNumber")
        Else
            msInvoiceNumber = ""
        End If

        miAutomaticApproveTime = dr("AutomaticApproveTime")
        If Not IsDBNull(dr("FinishingDate")) Then
            mdtFinishingDate = dr("FinishingDate")
        End If

        miExternalUpdateMail = dr("ExternalUpdateMail")

        If IsDBNull(dr("StateSecondary_Id")) Then
            miStateSecondary_Id = 0
        Else
            miStateSecondary_Id = dr("StateSecondary_Id")
        End If

        If Not IsDBNull(dr("StateSecondary")) Then
            msStateSecondary = dr("StateSecondary")
        End If

        If Not IsDBNull(dr("ResetOnExternalUpdate")) Then
            miResetOnExternalUpdate = dr("ResetOnExternalUpdate")
        End If

        If Not IsDBNull(dr("WatchDate")) Then
            mdtWatchdate = dr("WatchDate")
        End If

        If Not IsDBNull(dr("WorkingGroupEMail")) Then
            msWorkingGroupEMail = dr("WorkingGroupEMail")
        End If

        If Not IsDBNull(dr("AllocateCaseMail")) Then
            msAllocateCaseMail = dr("AllocateCaseMail")
        End If

        miHolidayHeader_Id = dr("HolidayHeader_Id")

        miRegistrationSource = dr("RegistrationSource")

        If Not IsDBNull(dr("RegUserName")) Then
            msRegUserName = dr("RegUserName")
        Else
            If Not IsDBNull(dr("RegUserFirstName")) Then
                msRegUserName = dr("RegUserFirstName")
            End If

            If Not IsDBNull(dr("RegUserSurName")) Then
                msRegUserName = msRegUserName & " " & dr("RegUserSurName")
            End If
        End If

        If Not IsDBNull(dr("ChangedName")) Then
            msChangedName = dr("ChangedName")
        Else
            msChangedName = ""
        End If

        If Not IsDBNull(dr("ChangedSurName")) Then
            msChangedSurName = dr("ChangedSurName")
        Else
            msChangedSurName = ""
        End If

        If Not IsDBNull(dr("Available")) Then
            msAvailable = dr("Available")
        End If

        If Not IsDBNull(dr("ReferenceNumber")) Then
            msReferenceNumber = dr("ReferenceNumber")
        End If

        miIncludeInCaseStatistics = dr("IncludeInCaseStatistics")

        miExternalTime = dr("ExternalTime")
        miLeadTime = dr("LeadTime")

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

    Public Property CaseGUID() As String
        Get
            Return msCaseGUID
        End Get
        Set(ByVal Value As String)
            msCaseGUID = Value
        End Set
    End Property

    Public Property Casenumber() As Long
        Get
            Return msCasenumber
        End Get
        Set(ByVal Value As Long)
            msCasenumber = Value
        End Set
    End Property

    Public Property Customer_Id() As Integer
        Get
            Return miCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miCustomer_Id = Value
        End Set
    End Property

    Public Property CaseType_Id() As Integer
        Get
            Return miCaseType_Id
        End Get
        Set(ByVal Value As Integer)
            miCaseType_Id = Value
        End Set
    End Property

    Public Property CaseTypeName() As String
        Get
            Return msCaseTypeName
        End Get
        Set(ByVal Value As String)
            msCaseTypeName = Value
        End Set
    End Property

    Public Property ProductArea_Id() As Integer
        Get
            Return miProductArea_Id
        End Get
        Set(ByVal Value As Integer)
            miProductArea_Id = Value
        End Set
    End Property

    Public Property ProductAreaName() As String
        Get
            Return msProductAreaName
        End Get
        Set(ByVal Value As String)
            msProductAreaName = Value
        End Set
    End Property

    Public Property Category_Id() As Integer
        Get
            Return miCategory_Id
        End Get
        Set(ByVal Value As Integer)
            miCategory_Id = Value
        End Set
    End Property

    Public Property CategoryName() As String
        Get
            Return msCategoryName
        End Get
        Set(ByVal Value As String)
            msCategoryName = Value
        End Set
    End Property

    Public Property Priority_Id() As Integer
        Get
            Return miPriority_Id
        End Get
        Set(ByVal Value As Integer)
            miPriority_Id = Value
        End Set
    End Property

    Public Property Region_Id() As Integer
        Get
            Return miRegion_Id
        End Get
        Set(ByVal Value As Integer)
            miRegion_Id = Value
        End Set
    End Property

    Public Property Department_Id() As Integer
        Get
            Return miDepartment_Id
        End Get
        Set(ByVal Value As Integer)
            miDepartment_Id = Value
        End Set
    End Property

    Public ReadOnly Property Department() As String
        Get
            Return msDepartment
        End Get
    End Property

    Public Property OU_Id() As Integer
        Get
            Return miOU_Id
        End Get
        Set(ByVal Value As Integer)
            miOU_Id = Value
        End Set
    End Property

    Public Property CustomerName() As String
        Get
            Return msCustomerName
        End Get
        Set(ByVal Value As String)
            msCustomerName = Value
        End Set
    End Property

    Public Property ReportedBy() As String
        Get
            Return msReportedBy
        End Get
        Set(ByVal Value As String)
            msReportedBy = Value
        End Set
    End Property

    Public Property Persons_Name() As String
        Get
            Return msPersons_Name
        End Get
        Set(ByVal Value As String)
            msPersons_Name = Value
        End Set
    End Property

    Public Property Persons_EMail() As String
        Get
            Return msPersons_EMail
        End Get
        Set(ByVal Value As String)
            msPersons_EMail = Value
        End Set
    End Property

    Public Property Persons_Phone() As String
        Get
            Return msPersons_Phone
        End Get
        Set(ByVal Value As String)
            msPersons_Phone = Value
        End Set
    End Property

    Public Property Persons_CellPhone() As String
        Get
            Return msPersons_CellPhone
        End Get
        Set(ByVal Value As String)
            msPersons_CellPhone = Value
        End Set
    End Property

    Public Property Place() As String
        Get
            Return msPlace
        End Get
        Set(ByVal Value As String)
            msPlace = Value
        End Set
    End Property

    Public Property UserCode() As String
        Get
            Return msUserCode
        End Get
        Set(ByVal Value As String)
            msUserCode = Value
        End Set
    End Property

    Public Property CostCentre() As String
        Get
            Return msCostCentre
        End Get
        Set(ByVal Value As String)
            msCostCentre = Value
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return msCaption
        End Get
        Set(ByVal Value As String)
            msCaption = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return msDescription
        End Get
        Set(ByVal Value As String)
            msDescription = Value
        End Set
    End Property

    Public Property Miscellaneous() As String
        Get
            Return msMiscellaneous
        End Get
        Set(ByVal Value As String)
            msMiscellaneous = Value
        End Set
    End Property

    Public Property PriorityName() As String
        Get
            Return msPriorityName
        End Get
        Set(ByVal Value As String)
            msPriorityName = Value
        End Set
    End Property

    Public Property PriorityDescription() As String
        Get
            Return msPriorityDescription
        End Get
        Set
            msPriorityDescription = Value
        End Set
    End Property

    Public Property WorkingGroup_Id() As Integer
        Get
            Return miWorkingGroup_Id
        End Get
        Set(ByVal Value As Integer)
            miWorkingGroup_Id = Value
        End Set
    End Property

    Public Property Performer_User_Id() As Integer
        Get
            Return miPerformer_User_Id
        End Get
        Set(ByVal Value As Integer)
            miPerformer_User_Id = Value
        End Set
    End Property

    Public Property PerformerFirstName() As String
        Get
            Return msPerformerFirstName
        End Get
        Set(ByVal Value As String)
            msPerformerFirstName = Value
        End Set
    End Property

    Public Property PerformerSurName() As String
        Get
            Return msPerformerSurName
        End Get
        Set(ByVal Value As String)
            msPerformerSurName = Value
        End Set
    End Property

    Public Property PerformerEMail() As String
        Get
            Return msPerformerEMail
        End Get
        Set(ByVal Value As String)
            msPerformerEMail = Value
        End Set
    End Property

    Public Property CaseWorkingGroup() As String
        Get
            Return msCaseWorkingGroup
        End Get
        Set(ByVal Value As String)
            msCaseWorkingGroup = Value
        End Set
    End Property

    Public Property PerformerWorkingGroup_Id() As String
        Get
            Return msPerformerWorkingGroup_Id
        End Get
        Set
            msPerformerWorkingGroup_Id = Value
        End Set
    End Property

    Public Property PerformerWorkingGroupAllocateCaseMail() As String
        Get
            Return msPerformerWorkingGroupAllocateCaseMail
        End Get
        Set
            msPerformerWorkingGroupAllocateCaseMail = Value
        End Set
    End Property

    Public Property PerformerWorkingGroup() As String
        Get
            Return msPerformerWorkingGroup
        End Get
        Set(ByVal Value As String)
            msPerformerWorkingGroup = Value
        End Set
    End Property

    Public Property PerformerWorkingGroupEMail() As String
        Get
            Return msPerformerWorkingGroupEMail
        End Get
        Set(ByVal Value As String)
            msPerformerWorkingGroupEMail = Value
        End Set
    End Property

    Public Property InventoryNumber() As String
        Get
            Return msInventoryNumber
        End Get
        Set(ByVal Value As String)
            msInventoryNumber = Value
        End Set
    End Property

    Public Property InvoiceNumber() As String
        Get
            Return msInvoiceNumber
        End Get
        Set(ByVal Value As String)
            msInvoiceNumber = Value
        End Set
    End Property

    Public Property WorkingGroupEMail() As String
        Get
            Return msWorkingGroupEMail
        End Get
        Set(ByVal Value As String)
            msWorkingGroupEMail = Value
        End Set
    End Property

    Public Property WorkingGroupAllocateCaseMail() As Integer
        Get
            Return msAllocateCaseMail
        End Get
        Set
            msAllocateCaseMail = Value
        End Set
    End Property

    Public Property FinishingDate() As DateTime
        Get
            Return mdtFinishingDate
        End Get
        Set(ByVal Value As DateTime)
            mdtFinishingDate = Value
        End Set
    End Property

    Public Property RegTime() As DateTime
        Get
            Return mdtRegTime
        End Get
        Set(ByVal Value As DateTime)
            mdtRegTime = Value
        End Set
    End Property

    Public Property ChangeTime() As DateTime
        Get
            Return mdtChangeTime
        End Get
        Set(ByVal Value As DateTime)
            mdtChangeTime = Value
        End Set
    End Property

    Public Property RegLanguage_Id() As Integer
        Get
            Return miRegLanguage_Id
        End Get
        Set(ByVal Value As Integer)
            miRegLanguage_Id = Value
        End Set
    End Property

    Public Property AutomaticApproveTime() As Integer
        Get
            Return miAutomaticApproveTime
        End Get
        Set(ByVal Value As Integer)
            miAutomaticApproveTime = Value
        End Set
    End Property

    Public Property ExternalUpdateMail() As Integer
        Get
            Return miExternalUpdateMail
        End Get
        Set(ByVal Value As Integer)
            miExternalUpdateMail = Value
        End Set
    End Property

    Public Property Status_Id() As Integer
        Get
            Return miStatus_Id
        End Get
        Set(ByVal Value As Integer)
            miStatus_Id = Value
        End Set
    End Property

    Public Property StateSecondary_Id() As Integer
        Get
            Return miStateSecondary_Id
        End Get
        Set(ByVal Value As Integer)
            miStateSecondary_Id = Value
        End Set
    End Property

    Public ReadOnly Property StateSecondary() As String
        Get
            Return msStateSecondary
        End Get
    End Property

    Public ReadOnly Property ResetOnExternalUpdate() As Integer
        Get
            Return miResetOnExternalUpdate
        End Get
    End Property

    Public Property WatchDate() As DateTime
        Get
            Return mdtWatchDate
        End Get
        Set(ByVal Value As DateTime)
            mdtWatchDate = Value
        End Set
    End Property

    Public Property RegistrationSource() As Integer
        Get
            Return miRegistrationSource
        End Get
        Set(ByVal Value As Integer)
            miRegistrationSource = Value
        End Set
    End Property

    Public Property Form_Id() As Integer
        Get
            Return miForm_Id
        End Get
        Set(ByVal Value As Integer)
            miForm_Id = Value
        End Set
    End Property

    Public Property HolidayHeader_Id() As Integer
        Get
            Return miHolidayHeader_Id
        End Get
        Set(ByVal Value As Integer)
            miHolidayHeader_Id = Value
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

    Public Property Log() As List(Of Log)
        Get
            Return mcolLog
        End Get
        Set(ByVal Value As List(Of Log))
            mcolLog = Value
        End Set
    End Property

    Public Property RegUserName() As String
        Get
            Return msRegUserName
        End Get
        Set(ByVal Value As String)
            msRegUserName = Value
        End Set
    End Property

    Public Property ChangedName() As String
        Get
            Return msChangedName
        End Get
        Set
            msChangedName = Value
        End Set
    End Property

    Public Property ChangedSurName() As String
        Get
            Return msChangedSurName
        End Get
        Set
            msChangedSurName = Value
        End Set
    End Property

    Public Property Available() As String
        Get
            Return msAvailable
        End Get
        Set(ByVal Value As String)
            msAvailable = Value
        End Set
    End Property

    Public Property ReferenceNumber() As String
        Get
            Return msReferenceNumber
        End Get
        Set(ByVal Value As String)
            msReferenceNumber = Value
        End Set
    End Property

    Public Property IncludeInCaseStatistics() As Integer
        Get
            Return miIncludeInCaseStatistics
        End Get
        Set(ByVal Value As Integer)
            miIncludeInCaseStatistics = Value
        End Set
    End Property

    Public Property ExternalTime() As Integer
        Get
            Return miExternalTime
        End Get
        Set(ByVal Value As Integer)
            miExternalTime = Value
        End Set
    End Property


    Public Property LeadTime() As Integer
        Get
            Return miLeadTime
        End Get
        Set(ByVal Value As Integer)
            miLeadTime = Value
        End Set
    End Property
#End Region

End Class