Imports System.Xml.Serialization

Public Class GlobalSettings

#Region "Declarations"
    Private miDBType As Integer
    Private msServerName As String
    Private miServerPort As Integer
    Private msSMTPServer As String
    Private msAttachedFileFolder As String
    Private msEmailBodyEncoding As String
    Private msDBVersion As String
    Private msExternalSite As String
#End Region

#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal dr As DataRow)
        miDBType = dr("DBType")
        If IsDBNull(dr("ServerName")) Then
            msServerName = ""
        Else
            msServerName = dr("ServerName")
        End If

        miServerPort = dr("ServerPort")

        If IsDBNull(dr("SMTPServer")) Then
            msSMTPServer = ""
        Else
            msSMTPServer = dr("SMTPServer")
        End If

        If IsDBNull(dr("AttachedFileFolder")) Then
            msAttachedFileFolder = ""
        Else
            msAttachedFileFolder = dr("AttachedFileFolder")
        End If

        If IsDBNull(dr("EMailBodyEncoding")) Then
            msEmailBodyEncoding = ""
        Else
            msEmailBodyEncoding = dr("EMailBodyEncoding")
        End If

        msDBVersion = dr("DBVersion").ToString

        If IsDBNull(dr("ExternalSite")) Then
            msExternalSite = ""
        Else
            msExternalSite = dr("ExternalSite")
        End If
    End Sub

#End Region

#Region "Properties"
    Public Property DBType() As Integer
        Get
            Return miDBType
        End Get
        Set(ByVal Value As Integer)
            miDBType = Value
        End Set
    End Property

    Public Property ServerName() As String
        Get
            Return msServerName
        End Get
        Set(ByVal Value As String)
            msServerName = Value
        End Set
    End Property

    Public Property ServerPort() As Integer
        Get
            Return miServerPort
        End Get
        Set(ByVal Value As Integer)
            miServerPort = Value
        End Set
    End Property

    Public Property SMTPServer() As String
        Get
            Return msSMTPServer
        End Get
        Set(ByVal Value As String)
            msSMTPServer = Value
        End Set
    End Property

    Public Property AttachedFileFolder() As String
        Get
            Return msAttachedFileFolder
        End Get
        Set(ByVal Value As String)
            msAttachedFileFolder = Value
        End Set
    End Property

    Public Property EMailBodyEncoding() As String
        Get
            Return msEmailBodyEncoding
        End Get
        Set(ByVal Value As String)
            msEmailBodyEncoding = Value
        End Set
    End Property

    Public ReadOnly Property DBVersion() As String
        Get
            Return msDBVersion
        End Get
    End Property

    Public Property ExternalSite() As String
        Get
            Return msExternalSite
        End Get
        Set(ByVal Value As String)
            msExternalSite = Value
        End Set
    End Property
#End Region
End Class
