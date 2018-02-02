Public Class OU
#Region "Declarations"
    Private miId As Integer
    Private msOU As String = ""
    Private msOUId As String = ""
    Private msSearchKey As String = ""
    Private miDepartment_Id As Integer
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("OU")) Then
            msOU = dr("OU").ToString
        End If

        If Not IsDBNull(dr("OUId")) Then
            msOUId = dr("OUId").ToString
        End If

        If Not IsDBNull(dr("SearchKey")) Then
            msSearchKey = dr("SearchKey").ToString
        End If

        If Not IsDBNull(dr("Department_Id")) Then
            miDepartment_id = dr("Department_Id").ToString
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

    Public Property OU() As String
        Get
            Return msOU
        End Get
        Set(ByVal Value As String)
            msOU = Value
        End Set
    End Property

    Public Property OUId() As String
        Get
            Return msOUId
        End Get
        Set(ByVal Value As String)
            msOUId = Value
        End Set
    End Property

    Public Property SearchKey() As String
        Get
            Return msSearchKey
        End Get
        Set(ByVal Value As String)
            msSearchKey = Value
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
#End Region
End Class
