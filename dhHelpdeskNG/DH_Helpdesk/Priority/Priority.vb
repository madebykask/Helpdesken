Public Class Priority

#Region "Declarations"
    Private miId As Integer
    Private miSolutionTime As Integer
    Private msEMailList As String
    Private msName As String
#End Region

#Region "Constructors"
    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miSolutionTime = dr("SolutionTime")
        msEMailList = dr("EMailList")
        msName = dr("PriorityName")
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

    Public Property SolutionTime() As Integer
        Get
            Return miSolutionTime
        End Get
        Set(ByVal Value As Integer)
            miSolutionTime = Value
        End Set
    End Property

    Public Property EMailList() As String
        Get
            Return msEMailList
        End Get
        Set(ByVal Value As String)
            msEMailList = Value
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

#End Region
End Class
