Public Class Inventory

#Region "Declarations"
    Private miId As Integer
    Private miInventoryType_Id As Integer
    Private msInventoryName As String
    Private msInventoryModel As String
    Private msSerialNumber As String
    Private msManufacturer As String
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

    Public Property InventoryType_Id() As Integer
        Get
            Return miInventoryType_Id
        End Get
        Set(ByVal Value As Integer)
            miInventoryType_Id = Value
        End Set
    End Property

    Public Property InventoryName() As String
        Get
            Return msInventoryName
        End Get
        Set(ByVal Value As String)
            msInventoryName = Value
        End Set
    End Property

    Public Property InventoryModel() As String
        Get
            Return msInventoryModel
        End Get
        Set(ByVal Value As String)
            msInventoryModel = Value
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

    Public Property Manufacturer() As String
        Get
            Return msManufacturer
        End Get
        Set(ByVal Value As String)
            msManufacturer = Value
        End Set
    End Property
#End Region
End Class
