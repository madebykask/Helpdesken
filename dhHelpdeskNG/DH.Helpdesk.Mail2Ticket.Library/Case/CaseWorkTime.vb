Public Class CaseWorkTime
    Public Sub New(ByVal externalTime As Integer, ByVal leadTime As Integer, ByVal now As DateTime)
        miExternalTime = externalTime
        miLeadTime = leadTime
        miNow = now
    End Sub

    Private miExternalTime As Integer
    Public Property ExternalTime() As Integer
        Get
            Return miExternalTime
        End Get
        Set(ByVal value As Integer)
            miExternalTime = value
        End Set
    End Property

    Private miLeadTime As Integer
    Public Property LeadTime() As Integer
        Get
            Return miLeadTime
        End Get
        Set(ByVal value As Integer)
            miLeadTime = value
        End Set
    End Property

    Private miNow As DateTime
    Public Property Now() As DateTime
        Get
            Return miNow
        End Get
        Set(ByVal value As DateTime)
            miNow = value
        End Set
    End Property

End Class
