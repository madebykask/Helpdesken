
<Serializable()> Public Class Computer

#Region "Declarations"

    Private miComputerRole As SharedFunctions.ComputerRole
    Private miId As Integer
    Private miCustomer_Id As Integer
    Private miOS_Id As Integer
    Private msOS_Version As String
    Private msOS_Servicepack As String
    Private msComputerName As String
    Private msManufacturer As String
    Private miComputerModel_Id As Integer
    Private msComputerModel As String
    Private msSerialNumber As String
    Private msBIOSVersion As String
    Private msBIOSDate As String
    Private msChassisType As String
    Private miProcessor_Id As Integer
    Private msProcessorInfo As String
    Private miRAM_Id As Integer
    Private miNIC_Id As Integer
    Private msIPAddress As String
    Private msMACAddress As String
    Private miRAS As Integer
    Private msVideoCard As String
    Private msSoundCard As String
    Private msCarePackNumber As String
    Private msRegistrationCode As String
    Private msProductKey As String
    Private msLocation As String
    Private miDepartment_Id As Integer
    Private miUser_Id As Integer
    Private miDomain_Id As Integer
    Private miComputerType_Id As Integer

    Private mdtScanDate As Date

    Private mcolSoftware As New List(Of Software)
    Private mcolLogicalDrives As New List(Of LogicalDrive)
    Private mcolInventory As New List(Of Inventory)
#End Region

#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal Customer_Id As Integer)
        miCustomer_Id = Customer_Id
    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miCustomer_Id = dr("Customer_Id")
        msComputerName = dr("ComputerName")
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

    Public Property Department_Id() As Integer
        Get
            Return miDepartment_Id
        End Get
        Set(ByVal Value As Integer)
            miDepartment_Id = Value
        End Set
    End Property

    Public Property ComputerRole() As Integer
        Get
            Return miComputerRole
        End Get
        Set(ByVal Value As Integer)
            miComputerRole = Value
        End Set
    End Property

    Public Property OS_Id() As Integer
        Get
            Return miOS_Id
        End Get
        Set(ByVal Value As Integer)
            miOS_Id = Value
        End Set
    End Property

    Public Property OS_Version() As String
        Get
            Return msOS_Version
        End Get
        Set(ByVal Value As String)
            msOS_Version = Value
        End Set
    End Property

    Public Property OS_Servicepack() As String
        Get
            Return msOS_Servicepack
        End Get
        Set(ByVal Value As String)
            msOS_Servicepack = Value
        End Set
    End Property

    Public Property ComputerName() As String
        Get
            Return msComputerName
        End Get
        Set(ByVal Value As String)
            msComputerName = Value
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

    Public Property ComputerModel_Id() As Integer
        Get
            Return miComputerModel_Id
        End Get
        Set(ByVal Value As Integer)
            miComputerModel_Id = Value
        End Set
    End Property

    Public Property ComputerModel() As String
        Get
            Return msComputerModel
        End Get
        Set(ByVal Value As String)
            msComputerModel = Value
        End Set
    End Property

    Public Property SerialNumber() As String
        Get
            Return msSerialNumber
        End Get
        Set(ByVal Value As String)
            msSerialNumber = Value
        End Set
    End Property

    Public Property BIOSVersion() As String
        Get
            Return msBIOSVersion
        End Get
        Set(ByVal Value As String)
            msBIOSVersion = Value
        End Set
    End Property

    Public Property BIOSDate() As String
        Get
            Return msBIOSDate
        End Get
        Set(ByVal Value As String)
            msBIOSDate = Value
        End Set
    End Property

    Public Property ChassisType() As String
        Get
            Return msChassisType
        End Get
        Set(ByVal Value As String)
            msChassisType = Value
        End Set
    End Property

    Public Property Processor_Id() As Integer
        Get
            Return miProcessor_Id
        End Get
        Set(ByVal Value As Integer)
            miProcessor_Id = Value
        End Set
    End Property

    Public Property ProcessorInfo() As String
        Get
            Return msProcessorInfo
        End Get
        Set(ByVal Value As String)
            msProcessorInfo = Value
        End Set
    End Property

    Public Property RAM_Id() As Integer
        Get
            Return miRAM_Id
        End Get
        Set(ByVal Value As Integer)
            miRAM_Id = Value
        End Set
    End Property

    Public Property NIC_Id() As Integer
        Get
            Return miNIC_Id
        End Get
        Set(ByVal Value As Integer)
            miNIC_Id = Value
        End Set
    End Property

    Public Property IPAddress() As String
        Get
            Return msIPAddress
        End Get
        Set(ByVal Value As String)
            msIPAddress = Value
        End Set
    End Property

    Public Property MACAddress() As String
        Get
            Return msMACAddress
        End Get
        Set(ByVal Value As String)
            msMACAddress = Value
        End Set
    End Property

    Public Property RAS() As Integer
        Get
            Return miRAS
        End Get
        Set(ByVal Value As Integer)
            miRAS = Value
        End Set
    End Property

    Public Property VideoCard() As String
        Get
            Return msVideoCard
        End Get
        Set(ByVal Value As String)
            msVideoCard = Value
        End Set
    End Property

    Public Property SoundCard() As String
        Get
            Return msSoundCard
        End Get
        Set(ByVal Value As String)
            msSoundCard = Value
        End Set
    End Property

    Public Property CarePackNumber() As String
        Get
            Return msCarePackNumber
        End Get
        Set(ByVal Value As String)
            msCarePackNumber = Value
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

    Public Property Location() As String
        Get
            Return msLocation
        End Get
        Set(ByVal Value As String)
            msLocation = Value
        End Set
    End Property

    Public Property User_Id() As Integer
        Get
            Return miUser_Id
        End Get
        Set(ByVal Value As Integer)
            miUser_Id = Value
        End Set
    End Property

    Public Property Domain_Id() As Integer
        Get
            Return miDomain_Id
        End Get
        Set(ByVal Value As Integer)
            miDomain_Id = Value
        End Set
    End Property

    Public Property ScanDate() As Date
        Get
            Return mdtScanDate
        End Get
        Set(ByVal Value As Date)
            mdtScanDate = Value
        End Set
    End Property

    Public Property ComputerType_Id() As Integer
        Get
            Return miComputerType_Id
        End Get
        Set(ByVal Value As Integer)
            miComputerType_Id = Value
        End Set
    End Property

    Public Property Software() As List(Of Software)
        Get
            Return mcolSoftware
        End Get
        Set(ByVal Value As List(Of Software))
            mcolSoftware = Value
        End Set
    End Property

    Public Property LogicalDrives() As List(Of LogicalDrive)
        Get
            Return mcolLogicalDrives
        End Get
        Set(ByVal Value As List(Of LogicalDrive))
            mcolLogicalDrives = Value
        End Set
    End Property

    Public Property Accessories() As List(Of Inventory)
        Get
            Return mcolInventory
        End Get
        Set(ByVal Value As List(Of Inventory))
            mcolInventory = Value
        End Set
    End Property
#End Region
End Class
