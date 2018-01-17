<Serializable()> Public Class Log
#Region "Declarations"

    Private miId As Integer
    Private msLogGUID As String
    Private msText_Internal As String
    Private msText_External As String

#End Region

#Region "Constructors"

    Sub New()
        ' Skapa nytt GUID
        msLogGUID = System.Guid.NewGuid.ToString()
    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        msLogGUID = dr("CaseGUID").ToString
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

    Public Property LogGUID() As String
        Get
            Return msLogGUID
        End Get
        Set(ByVal Value As String)
            msLogGUID = Value
        End Set
    End Property

    Public Property Text_Internal() As String
        Get
            Return msText_Internal
        End Get
        Set(ByVal Value As String)
            msText_Internal = Value
        End Set
    End Property

    Public Property Text_External() As String
        Get
            Return msText_External
        End Get
        Set(ByVal Value As String)
            msText_External = Value
        End Set
    End Property
#End Region

End Class
