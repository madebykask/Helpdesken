Public Class User
#Region "Declarations"
    Private miId As Integer
    Private msEMail As String
    Private miDefaultCustomer_Id As Integer
    Private miAllocateCaseMail As Integer
    Private miCaseInfoMail As Integer
    Private miStatus As Integer
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("EMail")) Then
            msEMail = dr("EMail")
        End If

        miStatus = dr("Status")
        miDefaultCustomer_Id = dr("DefaultCustomer_Id")
        miAllocateCaseMail = dr("AllocateCaseMail")
        miCaseInfoMail = dr("CaseInfoMail")
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

    Public Property EMail() As String
        Get
            Return msEMail
        End Get
        Set(ByVal Value As String)
            msEMail = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return miStatus
        End Get
        Set(ByVal Value As Integer)
            miStatus = Value
        End Set
    End Property

    Public Property DefaultCustomer_Id() As Integer
        Get
            Return miDefaultCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miDefaultCustomer_Id = Value
        End Set
    End Property

    Public Property AllocateCaseMail() As Integer
        Get
            Return miAllocateCaseMail
        End Get
        Set(ByVal Value As Integer)
            miAllocateCaseMail = Value
        End Set
    End Property

    Public Property CaseInfoMail() As Integer
        Get
            Return miCaseInfoMail
        End Get
        Set(ByVal Value As Integer)
            miCaseInfoMail = Value
        End Set
    End Property
#End Region
End Class
