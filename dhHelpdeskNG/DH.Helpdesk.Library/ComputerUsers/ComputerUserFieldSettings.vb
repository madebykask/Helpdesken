Public Class ComputerUserFieldSettings
#Region "Declarations"
    Private msComputerUserField As String = ""
    Private msLDAPAttribute As String = ""
#End Region

#Region "Constructors"
    
    Sub New(ByVal dr As DataRow)
        If Not IsDBNull(dr("ComputerUserField")) Then
            msComputerUserField = dr("ComputerUserField")
        End If

        If Not IsDBNull(dr("LDAPAttribute")) Then
            msLDAPAttribute = dr("LDAPAttribute")
        End If
    End Sub

#End Region

#Region "Properties"
    Public Property ComputerUserField() As String
        Get
            Return msComputerUserField
        End Get
        Set(ByVal Value As String)
            msComputerUserField = Value
        End Set
    End Property

    Public Property LDAPAttribute() As String
        Get
            Return msLDAPAttribute
        End Get
        Set(ByVal Value As String)
            msLDAPAttribute = Value
        End Set
    End Property
#End Region

End Class
