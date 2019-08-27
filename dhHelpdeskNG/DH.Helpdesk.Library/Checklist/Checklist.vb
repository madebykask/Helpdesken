Public Class Checklist
#Region "Declarations"
    Private miId As Integer
    Private msChecklistName As String
    Private msRecipients As String
    Private msChecklistMailBody As String
    Private miCustomer_Id As Integer
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

    Public Property Customer_Id() As Integer
        Get
            Return miCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miCustomer_Id = Value
        End Set
    End Property

    Public Property Recipients() As String
        Get
            Return msRecipients
        End Get
        Set(ByVal Value As String)
            msRecipients = Value
        End Set
    End Property

    Public Property ChecklistName() As String
        Get
            Return msChecklistName
        End Get
        Set(ByVal Value As String)
            msChecklistName = Value
        End Set
    End Property

    Public Property ChecklistMailBody() As String
        Get
            Return msChecklistMailBody
        End Get
        Set(ByVal Value As String)
            msChecklistMailBody = Value
        End Set
    End Property
#End Region

End Class
