Public Class Invoice

#Region "Declarations"
    Private miCase_Id As Integer

#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miCase_Id = dr("Id")
    End Sub

#End Region

#Region "Properties"
    Public Property Case_Id() As Integer
        Get
            Return miCase_Id
        End Get
        Set(ByVal Value As Integer)
            miCase_Id = Value
        End Set
    End Property
#End Region

End Class
