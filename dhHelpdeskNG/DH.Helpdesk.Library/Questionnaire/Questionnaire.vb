Public Class Questionnaire
#Region "Declarations"
    Private miId As Integer
    Private miCustomer_Id As Integer
    Private msQuestionnaireName As String
    Private msDepartments As String
    Private msCaseTypes As String
    Private msProductAreas As String
    Private msWorkingGroups As String
    Private miSelection As Integer
    Private miFilter As Integer
#End Region

#Region "Constructors"
    Sub New()
    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miCustomer_Id = dr("Customer_Id")
        msQuestionnaireName = dr("QuestionnaireName").ToString

        If Not IsDBNull(dr("Departments")) Then
            msDepartments = dr("Departments").ToString
        End If

        If Not IsDBNull(dr("CaseTypes")) Then
            msCaseTypes = dr("CaseTypes").ToString
        End If

        If Not IsDBNull(dr("ProductAreas")) Then
            msProductAreas = dr("ProductAreas").ToString
        End If

        If Not IsDBNull(dr("WorkingGroups")) Then
            msWorkingGroups = dr("WorkingGroups").ToString
        End If

        miSelection = dr("Selection")
        miFilter = dr("Filter")
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
    Public Property QuestionnaireName() As String
        Get
            Return msQuestionnaireName
        End Get
        Set(ByVal Value As String)
            msQuestionnaireName = Value
        End Set
    End Property

    Public Property Departments() As String
        Get
            Return msDepartments
        End Get
        Set(ByVal Value As String)
            msDepartments = Value
        End Set
    End Property

    Public Property CaseTypes() As String
        Get
            Return msCaseTypes
        End Get
        Set(ByVal Value As String)
            msCaseTypes = Value
        End Set
    End Property

    Public Property ProductAreas() As String
        Get
            Return msProductAreas
        End Get
        Set(ByVal Value As String)
            msProductAreas = Value
        End Set
    End Property

    Public Property WorkingGroups() As String
        Get
            Return msWorkingGroups
        End Get
        Set(ByVal Value As String)
            msWorkingGroups = Value
        End Set
    End Property

    Public Property Selection() As Integer
        Get
            Return miSelection
        End Get
        Set(ByVal Value As Integer)
            miSelection = Value
        End Set
    End Property

    Public Property Filter() As Integer
        Get
            Return miFilter
        End Get
        Set(ByVal Value As Integer)
            miFilter = Value
        End Set
    End Property
#End Region
End Class
