Public Class MailTemplate
#Region "Declarations"
    Private miMailId As Integer
    Private msSubject As String
    Private msBody As String
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miMailId = dr("MailId")
        msSubject = dr("Subject")
        msBody = dr("Body")
    End Sub

#End Region

#Region "Properties"
    Public Property Id() As Integer
        Get
            Return miMailId
        End Get
        Set(ByVal Value As Integer)
            miMailId = Value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return msSubject
        End Get
        Set(ByVal Value As String)
            msSubject = Value
        End Set
    End Property

    Public Property Body() As String
        Get
            Return msBody
        End Get
        Set(ByVal Value As String)
            msBody = Value
        End Set
    End Property
#End Region
End Class
