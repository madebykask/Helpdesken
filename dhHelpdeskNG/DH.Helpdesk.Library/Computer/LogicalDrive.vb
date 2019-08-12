Public Class LogicalDrive
#Region "Declarations"
    Private msDriveLetter As String
    Private miDriveType As Integer
    Private mdblTotalBytes As Double
    Private mdblFreeBytes As Double
    Private msFileSystemName As String
#End Region

#Region "Properties"
    Public Property DriveLetter() As String
        Get
            Return msDriveLetter
        End Get
        Set(ByVal Value As String)
            msDriveLetter = Value
        End Set
    End Property

    Public Property DriveType() As Integer
        Get
            Return miDriveType
        End Get
        Set(ByVal Value As Integer)
            miDriveType = Value
        End Set
    End Property

    Public Property TotalBytes() As Double
        Get
            Return mdblTotalBytes
        End Get
        Set(ByVal Value As Double)
            mdblTotalBytes = Value
        End Set
    End Property

    Public Property FreeBytes() As Double
        Get
            Return mdblFreeBytes
        End Get
        Set(ByVal Value As Double)
            mdblFreeBytes = Value
        End Set
    End Property

    Public Property FileSystemName() As String
        Get
            Return msFileSystemName
        End Get
        Set(ByVal Value As String)
            msFileSystemName = Value
        End Set
    End Property
#End Region
End Class
