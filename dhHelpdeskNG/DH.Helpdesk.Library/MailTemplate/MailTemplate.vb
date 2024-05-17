Imports DH.Helpdesk.Common.Enums

Public Class MailTemplate
#Region "Declarations"
    Private miMailId As Integer
    Private msSubject As String
    Private msBody As String
    Private msSendMethod As EmailSendMethod
    Private msIncludeLogExternal As Boolean
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miMailId = dr("MailId")
        msSubject = dr("Subject")
        msBody = dr("Body")
        msSendMethod = dr("SendMethod")
        msIncludeLogExternal = dr("IncludeLogText_External")
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

    Public Property SendMethod() As String
        Get
            Return msSendMethod
        End Get
        Set(ByVal Value As String)
            msSendMethod = Value
        End Set
    End Property
    Public Property IncludeLogExternal() As String
        Get
            Return msIncludeLogExternal
        End Get
        Set(ByVal Value As String)
            msIncludeLogExternal = Value
        End Set
    End Property
#End Region
End Class
