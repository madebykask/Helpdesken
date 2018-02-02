Public Class Account
#Region "Declarations"
    Private miId As Integer
    Private miAccountActivity_Id As Integer
    Private mAccountActivity As AccountActivity
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miAccountActivity_Id = dr("AccountActivity_Id")
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

    Public Property AccountActivity_Id() As Integer
        Get
            Return miAccountActivity_Id
        End Get
        Set(ByVal Value As Integer)
            miAccountActivity_Id = Value
        End Set
    End Property

    Public Property AccountActivity() As AccountActivity
        Get
            Return mAccountActivity
        End Get
        Set(ByVal Value As AccountActivity)
            mAccountActivity = Value
        End Set
    End Property

#End Region
End Class
