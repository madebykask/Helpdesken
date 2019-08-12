Public Class QuestionnaireCircularPart
#Region "Declarations"
    Private miId As Integer
    Private mGUID As Guid
    Private miQuestionnaireCircular_Id As Integer
    Private miCase_Id As Integer
    Private msCasenumber As String
    Private msCaption As String
    Private msPersons_EMail As String
    Private msDescription As String
#End Region

#Region "Constructors"
    
    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If IsDBNull(dr("GUID")) Then
            mGUID = GUID.NewGuid()
        Else
            mGUID = dr("GUID")
        End If

        miQuestionnaireCircular_Id = dr("QuestionnaireCircular_Id")
        miCase_Id = dr("Case_Id")

        msCasenumber = dr("Casenumber").ToString()
        msCaption = dr("Caption").ToString()
        msPersons_EMail = dr("Persons_EMail").ToString()
        msDescription = dr("Description").ToString()
       
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

    Public Property GUID() As Guid
        Get
            Return mGUID
        End Get
        Set(ByVal Value As Guid)
            mGUID = Value
        End Set
    End Property

    Public Property QuestionnaireCircular_Id() As Integer
        Get
            Return miQuestionnaireCircular_Id
        End Get
        Set(ByVal Value As Integer)
            miQuestionnaireCircular_Id = Value
        End Set
    End Property

    Public Property Case_Id() As Integer
        Get
            Return miCase_Id
        End Get
        Set(ByVal Value As Integer)
            miCase_Id = Value
        End Set
    End Property

    Public Property Casenumber() As String
        Get
            Return msCasenumber
        End Get
        Set(ByVal Value As String)
            msCasenumber = Value
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

    Public Property Persons_EMail() As String
        Get
            Return msPersons_EMail
        End Get
        Set(ByVal Value As String)
            msPersons_EMail = Value
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
#End Region
End Class
