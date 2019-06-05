Public Class Contract

#Region "Declarations"
    Private miId As Integer
    Private miCustomer_Id As Integer
    Private mContractGUID As String
    Private msContractNumber As String
    Private miDepartment_Id As Integer
    Private miResponsibleUser_Id As Integer
    Private miFollowUpResponsibleUser_Id As Integer
    Private mdtConstractStartDate As Date
    Private mdtContractEndDate As Date
    Private mdtNoticeDate As Date
    Private miNoticeTime As Integer
    Private miFollowUpInterval As Integer
    Private miLanguage_Id As Integer
    Private msCreateCase_UserId As String
    Private miCaseType_Id As Integer
    Private miStateSecondary_Id1 As Integer
    Private miStateSecondary_Id2 As Integer
    Private miForm_Id As Integer
#End Region

#Region "Constructors"

    Sub New()
        ' Skapa nytt GUID
        mContractGUID = System.Guid.NewGuid.ToString()
    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miCustomer_Id = dr("Customer_Id")
        mContractGUID = dr("ContractGUID").ToString
        msContractNumber = dr("ContractNumber").ToString

        If Not IsDBNull(dr("Department_Id")) Then
            miDepartment_Id = dr("Department_Id")
        End If

        If Not IsDBNull(dr("ResponsibleUser_Id")) Then
            miResponsibleUser_Id = dr("ResponsibleUser_Id")
        End If

        If Not IsDBNull(dr("FollowUpResponsibleUser_Id")) Then
            miFollowUpResponsibleUser_Id = dr("FollowUpResponsibleUser_Id")
        End If

        If Not IsDBNull(dr("ContractStartDate")) Then
            mdtConstractStartDate = dr("ContractStartDate")
        End If

        If Not IsDBNull(dr("ContractEndDate")) Then
            mdtContractEndDate = dr("ContractEndDate")
        End If

        If Not IsDBNull(dr("NoticeDate")) Then
            mdtNoticeDate = dr("NoticeDate")
        End If

        miNoticeTime = dr("NoticeTime")
        miFollowUpInterval = dr("FollowUpInterval")
        miLanguage_Id = dr("Language_Id")
        If Not IsDBNull(dr("CreateCase_UserId")) Then
            msCreateCase_userId = dr("CreateCase_UserId")
        End If
        miCaseType_Id = dr("CreateCase_CaseType_Id")
        If Not IsDBNull(dr("CreateCase_StateSecondary_Id1")) Then
            miStateSecondary_Id1 = dr("CreateCase_StateSecondary_Id1")
        End If
        If Not IsDBNull(dr("CreateCase_StateSecondary_Id2")) Then
            miStateSecondary_Id2 = dr("CreateCase_StateSecondary_Id2")
        End If
        If Not IsDBNull(dr("Form_Id")) Then
            miForm_Id = dr("Form_Id")
        End If
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

    Public Property Customer_Id() As Integer
        Get
            Return miCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miCustomer_Id = Value
        End Set
    End Property

    Public Property ContractGUID() As String
        Get
            Return mContractGUID
        End Get
        Set(ByVal Value As String)
            mContractGUID = Value
        End Set
    End Property

    Public Property ContractNumber() As String
        Get
            Return msContractNumber
        End Get
        Set(ByVal Value As String)
            msContractNumber = Value
        End Set
    End Property

    Public Property Department_Id() As String
        Get
            Return miDepartment_Id
        End Get
        Set(ByVal Value As String)
            miDepartment_Id = Value
        End Set
    End Property

    Public Property ResponsibleUser_Id() As Integer
        Get
            Return miResponsibleUser_Id
        End Get
        Set(ByVal Value As Integer)
            miResponsibleUser_Id = Value
        End Set
    End Property

    Public Property FollowUpResponsibleUser_Id() As Integer
        Get
            Return miFollowUpResponsibleUser_Id
        End Get
        Set(ByVal Value As Integer)
            miFollowUpResponsibleUser_Id = Value
        End Set
    End Property

    Public Property ContractStartDate() As Date
        Get
            Return mdtConstractStartDate
        End Get
        Set(ByVal Value As Date)
            mdtConstractStartDate = Value
        End Set
    End Property

    Public Property ContractEndDate() As Date
        Get
            Return mdtContractEndDate
        End Get
        Set(ByVal Value As Date)
            mdtContractEndDate = Value
        End Set
    End Property

    Public Property NoticeDate() As Date
        Get
            Return mdtNoticeDate
        End Get
        Set(ByVal Value As Date)
            mdtNoticeDate = Value
        End Set
    End Property

    Public Property NoticeTime() As Integer
        Get
            Return miNoticeTime
        End Get
        Set(ByVal Value As Integer)
            miNoticeTime = Value
        End Set
    End Property

    Public Property FollowUpInterval() As Integer
        Get
            Return miFollowUpInterval
        End Get
        Set(ByVal Value As Integer)
            miFollowUpInterval = Value
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

    Public Property CreateCase_UserId() As String
        Get
            Return msCreateCase_UserId
        End Get
        Set(ByVal Value As String)
            msCreateCase_UserId = Value
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

    Public Property FollowUp_StateSecondary_Id() As Integer
        Get
            Return miStateSecondary_Id1
        End Get
        Set(ByVal Value As Integer)
            miStateSecondary_Id1 = Value
        End Set
    End Property

    Public Property InNoticeOfRemoval_StateSecondary_Id() As Integer
        Get
            Return miStateSecondary_Id2
        End Get
        Set(ByVal Value As Integer)
            miStateSecondary_Id2 = Value
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
#End Region

End Class
