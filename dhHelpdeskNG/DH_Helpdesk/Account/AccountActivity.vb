Public Class AccountActivity
#Region "Declarations"
    Private miId As Integer
    Private msCloseCase_M2T_Sender As String = ""
    Private miCloseCase_FinishingCause_Id As Integer
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("CloseCase_M2T_Sender")) Then
            msCloseCase_M2T_Sender = dr("CloseCase_M2T_Sender").ToString
        End If

        If Not IsDBNull(dr("CloseCase_FinishingCause_Id")) Then
            miCloseCase_FinishingCause_Id = dr("CloseCase_FinishingCause_Id")
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

    Public Property CloseCase_M2T_Sender() As String
        Get
            Return msCloseCase_M2T_Sender
        End Get
        Set(ByVal Value As String)
            msCloseCase_M2T_Sender = Value
        End Set
    End Property

    Public Property CloseCase_FinishingCause_Id() As Integer
        Get
            Return miCloseCase_FinishingCause_Id
        End Get
        Set(ByVal Value As Integer)
            miCloseCase_FinishingCause_Id = Value
        End Set
    End Property
#End Region
End Class
