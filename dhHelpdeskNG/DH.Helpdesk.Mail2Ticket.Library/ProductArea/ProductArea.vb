Public Class ProductArea
#Region "Declarations"
    Private miId As Integer
    Private miParent_Id As Integer = 0
    Private miPriority_Id As Integer = 0
    Private miSubState_Id As Integer = 0
    Private miWorkingGroup_Id As Integer = 0
    Private msName As String = ""
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("ProductArea")) Then
            msName = dr("ProductArea").ToString
        End If

        If Not IsDBNull(dr("Parent_ProductArea_Id")) Then
            miParent_Id = CInt(dr("Parent_ProductArea_Id"))
        End If

        If Not IsDBNull(dr("Priority_Id")) Then
            miPriority_Id = CInt(dr("Priority_Id"))
        End If

        If Not IsDBNull(dr("WorkingGroup_Id")) Then
            miWorkingGroup_Id = CInt(dr("WorkingGroup_Id"))
        End If

        If Not IsDBNull(dr("StateSecondary_Id")) Then
            miSubState_Id = CInt(dr("StateSecondary_Id"))
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

    Public Property Parent_Id() As Integer
        Get
            Return miParent_Id
        End Get
        Set(ByVal Value As Integer)
            miParent_Id = Value
        End Set
    End Property

    Public Property Priority_Id() As Integer
        Get
            Return miPriority_Id
        End Get
        Set(ByVal Value As Integer)
            miPriority_Id = Value
        End Set
    End Property

    Public Property SubState_Id() As Integer
        Get
            Return miSubState_Id
        End Get
        Set(ByVal Value As Integer)
            miSubState_Id = Value
        End Set
    End Property

    Public Property WorkingGroup_Id() As Integer
        Get
            Return miWorkingGroup_Id
        End Get
        Set(ByVal Value As Integer)
            miWorkingGroup_Id = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return msName
        End Get
        Set(ByVal Value As String)
            msName = Value
        End Set
    End Property

#End Region
End Class
