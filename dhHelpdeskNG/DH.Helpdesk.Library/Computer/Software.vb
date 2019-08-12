Public Class Software
#Region "Declarations"
    Private msName As String
    Private msManufacturer As String
    Private msInstall_directory As String
    Private msVersion As String
    Private msRegistrationCode As String
    Private msProductKey As String
#End Region

#Region "Properties"
    Public Property Name() As String
        Get
            Return msName
        End Get
        Set(ByVal Value As String)
            msName = Value
        End Set
    End Property

    Public Property Install_directory() As String
        Get
            Return msInstall_directory
        End Get
        Set(ByVal Value As String)
            msInstall_directory = Value
        End Set
    End Property

    Public Property Manufacturer() As String
        Get
            Return msManufacturer
        End Get
        Set(ByVal Value As String)
            msManufacturer = Value
        End Set
    End Property

    Public Property Version() As String
        Get
            Return msVersion
        End Get
        Set(ByVal Value As String)
            msVersion = Value
        End Set
    End Property

    Public Property RegistrationCode() As String
        Get
            Return msRegistrationCode
        End Get
        Set(ByVal Value As String)
            msRegistrationCode = Value
        End Set
    End Property

    Public Property ProductKey() As String
        Get
            Return msProductKey
        End Get
        Set(ByVal Value As String)
            msProductKey = Value
        End Set
    End Property
#End Region
End Class
