Public Class Region
#Region "Declarations"
    Private miId As Integer
    Private msName As String = ""
    Private msSearchKey As String = ""
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("Region")) Then
            msName = dr("Region").ToString
        End If
        If Not IsDBNull(dr("SearchKey")) Then
            msSearchKey = dr("SearchKey").ToString
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

    Public Property Name() As String
        Get
            Return msName
        End Get
        Set(ByVal Value As String)
            msName = Value
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

#End Region
End Class