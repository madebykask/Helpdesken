Public Class Department
#Region "Declarations"
    Private miId As Integer
    Private miRegion_Id As Integer = 0
    Private msDepartment As String = ""
    Private msDepartmentId As String = ""
    Private msSearchKey As String = ""
    Private msNDSPath As String = ""
    Private mdtWatchDate As DateTime
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("Department")) Then
            msDepartment = dr("Department").ToString
        End If

        If Not IsDBNull(dr("DepartmentId")) Then
            msDepartmentId = dr("DepartmentId").ToString
        End If

        If Not IsDBNull(dr("SearchKey")) Then
            msSearchKey = dr("SearchKey").ToString
        End If

        If Not IsDBNull(dr("NDSPath")) Then
            msNDSPath = dr("NDSPath").ToString
        End If

        If Not IsDBNull(dr("Region_Id")) Then
            miRegion_Id = CInt(dr("Region_Id"))
        End If

        If Not IsDBNull(dr("WatchDate")) Then
            mdtWatchDate = dr("WatchDate")
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

    Public Property Region_Id() As Integer
        Get
            Return miRegion_Id
        End Get
        Set(ByVal Value As Integer)
            miRegion_Id = Value
        End Set
    End Property

    Public Property Department() As String
        Get
            Return msDepartment
        End Get
        Set(ByVal Value As String)
            msDepartment = Value
        End Set
    End Property

    Public Property DepartmentId() As String
        Get
            Return msDepartmentId
        End Get
        Set(ByVal Value As String)
            msDepartmentId = Value
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

    Public Property NDSPath() As String
        Get
            Return msNDSPath
        End Get
        Set(ByVal Value As String)
            msNDSPath = Value
        End Set
    End Property

    Public Property WatchDate() As DateTime
        Get
            Return mdtWatchDate
        End Get
        Set(ByVal Value As DateTime)
            mdtWatchDate = Value
        End Set
    End Property
#End Region
End Class
