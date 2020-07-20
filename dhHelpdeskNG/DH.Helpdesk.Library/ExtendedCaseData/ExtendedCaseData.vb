Public Class ExtendedCaseData
    Private miId As Integer
    Private msExtendedCaseGuid As String
    Private mdtCreatedOn As DateTime

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")

        If Not IsDBNull(dr("ExtendedCaseGuid")) Then
            msExtendedCaseGuid = dr("ExtendedCaseGuid").ToString()
        End If

        If Not IsDBNull(dr("CreatedOn")) Then
            mdtCreatedOn = dr("CreatedOn")
        End If

    End Sub

    Public Property Id() As Integer
        Get
            Return miId
        End Get
        Set(ByVal value As Integer)
            miId = value
        End Set
    End Property

    Public Property ExtendedCaseGuid() As String
        Get
            Return msExtendedCaseGuid
        End Get
        Set(ByVal value As String)
            msExtendedCaseGuid = value
        End Set
    End Property

    Public Property CreatedOn() As DateTime
        Get
            Return mdtCreatedOn
        End Get
        Set(ByVal value As DateTime)
            mdtCreatedOn = value
        End Set
    End Property

End Class
