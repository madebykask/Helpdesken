Public Class InventoryType

#Region "Declarations"
    Private miId As Integer
    Private msInventoryType As String
    Private msXMLElement As String
#End Region

#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        msInventoryType = dr("InventoryType")
        If Not IsDBNull(dr("XMLElement")) Then
            msXMLElement = dr("XMLElement")
        End If
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

    Public Property InventoryType() As String
        Get
            Return msInventoryType
        End Get
        Set(ByVal Value As String)
            msInventoryType = Value
        End Set
    End Property

    Public Property XMLElement() As String
        Get
            Return msXMLElement
        End Get
        Set(ByVal Value As String)
            msXMLElement = Value
        End Set
    End Property
#End Region
End Class
