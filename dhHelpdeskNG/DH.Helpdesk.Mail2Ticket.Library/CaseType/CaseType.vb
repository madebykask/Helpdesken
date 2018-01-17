Public Class CaseType
#Region "Declarations"
    Private miId As Integer
    Private msCaseType As String = ""
    Private miUser_Id As Integer
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("CaseType")) Then
            msCaseType = dr("CaseType").ToString
        End If

        If Not IsDBNull(dr("User_Id")) Then
            miUser_Id = dr("User_Id").ToString
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

    Public Property CaseType() As String
        Get
            Return msCaseType
        End Get
        Set(ByVal Value As String)
            msCaseType = Value
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
#End Region
End Class
