<Serializable()> Public Class ComputerUser

#Region "Declarations"
    Private miId As Integer
    Private miCustomer_Id As Integer = 0
    Private msUserId As String = ""
    Private msFirstName As String = ""
    Private msSurName As String = ""
    Private msDisplayName As String = ""
    Private msLogonName As String = ""
    Private msEMail As String = ""
    Private msPhone As String = ""
    Private msCellphone As String = ""
    Private msTitle As String
    Private msUserCode As String = ""
    Private msPostalAddress As String = ""
    Private msPostalCode As String = ""
    Private msCity As String = ""
    Private msLocation As String = ""
    Private msCostCentre As String = ""
    Private miDepartment_Id As Integer = 0
    Private miOU_Id As Integer = 0
    Private miRegion_Id As Integer = 0
    Private miDomain_Id As Integer = 0
    Private msOU As String = ""
    Private msNDSPath As String = ""
    Private miStatus As Integer = 0
    Private miManagerComputerUser_Id As Integer = 0

    'dhal används om vi ska skapa up organisation
    Private msDepartmentName As String = ""
    Private msRegionName As String = ""
    Private msOuName As String = ""
    'dhal används om vi ska skapa up organisation

#End Region

#Region "Constructors"
    Sub New()
       
    End Sub

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        miCustomer_Id = dr("Customer_Id")

        If Not IsDBNull(dr("UserId")) Then
            msUserId = dr("UserId")
        End If

        If Not IsDBNull(dr("FirstName")) Then
            msFirstName = dr("FirstName")
        End If

        If Not IsDBNull(dr("SurName")) Then
            msSurName = dr("SurName")
        End If

        If Not IsDBNull(dr("DisplayName")) Then
            msDisplayName = dr("DisplayName")
        End If

        If Not IsDBNull(dr("LogonName")) Then
            msLogonName = dr("LogonName")
        End If

        If Not IsDBNull(dr("Location")) Then
            msLocation = dr("Location")
        End If

        If Not IsDBNull(dr("EMail")) Then
            msEMail = dr("EMail")
        End If

        If Not IsDBNull(dr("Phone")) Then
            msPhone = dr("Phone")
        End If

        If Not IsDBNull(dr("CellPhone")) Then
            msCellphone = dr("CellPhone")
        End If

        If Not IsDBNull(dr("UserCode")) Then
            msUserCode = dr("UserCode")
        End If

        If Not IsDBNull(dr("PostalAddress")) Then
            msPostalAddress = dr("PostalAddress")
        End If

        If Not IsDBNull(dr("PostalCode")) Then
            msPostalCode = dr("PostalCode")
        End If

        If Not IsDBNull(dr("City")) Then
            msCity = dr("City")
        End If

        If Not IsDBNull(dr("Title")) Then
            msTitle = dr("Title")
        End If

        If Not IsDBNull(dr("OU")) Then
            msOU = dr("OU")
        End If

        If Not IsDBNull(dr("CostCentre")) Then
            msCostCentre = dr("CostCentre")
        End If

        If IsDBNull(dr("Department_Id")) Then
            miDepartment_Id = 0
        Else
            miDepartment_Id = dr("Department_Id")
        End If

        If IsDBNull(dr("OU_Id")) Then
            miOU_Id = 0
        Else
            miOU_Id = dr("OU_Id")
        End If

        If IsDBNull(dr("Region_Id")) Then
            miRegion_Id = 0
        Else
            miRegion_Id = dr("Region_Id")
        End If

        If Not IsDBNull(dr("Domain_Id")) Then
            miDomain_Id = dr("Domain_Id")
        End If

        If Not IsDBNull(dr("NDSPath")) Then
            msNDSPath = dr("NDSPath")
        End If

        miStatus = dr("Status")
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

    Public Property Customer_Id() As Integer
        Get
            Return miCustomer_Id
        End Get
        Set(ByVal Value As Integer)
            miCustomer_Id = Value
        End Set
    End Property

    Public Property UserId() As String
        Get
            Return msUserId
        End Get
        Set(ByVal Value As String)
            msUserId = Value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return msFirstName
        End Get
        Set(ByVal Value As String)
            msFirstName = Value
        End Set
    End Property

    Public Property SurName() As String
        Get
            Return msSurName
        End Get
        Set(ByVal Value As String)
            msSurName = Value
        End Set
    End Property

    Public Property LogonName() As String
        Get
            Return msLogonName
        End Get
        Set(ByVal Value As String)
            msLogonName = Value
        End Set
    End Property

    Public Property DisplayName() As String
        Get
            Return msDisplayName
        End Get
        Set(ByVal Value As String)
            msDisplayName = Value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return msLocation
        End Get
        Set(ByVal Value As String)
            msLocation = Value
        End Set
    End Property

    Public Property EMail() As String
        Get
            Return msEMail
        End Get
        Set(ByVal Value As String)
            msEMail = Value
        End Set
    End Property

    Public Property Phone() As String
        Get
            Return msPhone
        End Get
        Set(ByVal Value As String)
            msPhone = Value
        End Set
    End Property

    Public Property CellPhone() As String
        Get
            Return msCellphone
        End Get
        Set(ByVal Value As String)
            msCellphone = Value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return msTitle
        End Get
        Set(ByVal Value As String)
            msTitle = Value
        End Set
    End Property

    Public Property UserCode() As String
        Get
            If msUserCode.Length > 20 Then
                Return msUserCode.Substring(0, 20)
            Else
                Return msUserCode
            End If
        End Get
        Set(ByVal Value As String)
            msUserCode = Value
        End Set
    End Property

    Public Property CostCentre() As String
        Get
            Return msCostCentre
        End Get
        Set(ByVal Value As String)
            msCostCentre = Value
        End Set
    End Property

    Public Property PostalAddress() As String
        Get
            If msPostalAddress.Length > 50 Then
                Return msPostalAddress.Substring(0, 50)
            Else
                Return msPostalAddress
            End If
        End Get
        Set(ByVal Value As String)
            msPostalAddress = Value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            If msPostalCode.Length > 50 Then
                Return msPostalCode.Substring(0, 50)
            Else
                Return msPostalCode
            End If

        End Get
        Set(ByVal Value As String)
            msPostalCode = Value
        End Set
    End Property

    Public Property City() As String
        Get
            If Not msCity Is Nothing Then
                If msCity.Length > 50 Then
                    Return msCity.Substring(0, 50)
                Else
                    Return msCity
                End If
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            msCity = Value
        End Set
    End Property

    Public Property OU() As String
        Get
            Return msOU
        End Get
        Set(ByVal Value As String)
            msOU = Value
        End Set
    End Property

    Public Property Department_Id() As Integer
        Get
            Return miDepartment_Id
        End Get
        Set(ByVal Value As Integer)
            miDepartment_Id = Value
        End Set
    End Property

    Public Property OU_Id() As Integer
        Get
            Return miOU_Id
        End Get
        Set(ByVal Value As Integer)
            miOU_Id = Value
        End Set
    End Property

    Public Property Region_Id() As Integer
        Get
            Return miRegion_Id
        End Get
        Set(ByVal Value As Integer)
            miRegion_Id = Value
        End Set
    End Property

    Public Property Domain_Id() As Integer
        Get
            Return miDomain_Id
        End Get
        Set(ByVal Value As Integer)
            miDomain_Id = Value
        End Set
    End Property

    Public Property NDSPath() As String
        Get
            Return msNDSPath
        End Get
        Set(ByVal Value As String)
            msNDSPath = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return miStatus
        End Get
        Set(ByVal Value As Integer)
            miStatus = Value
        End Set
    End Property

    Public Property ManagerComputerUser_Id() As Integer
        Get
            Return miManagerComputerUser_Id
        End Get
        Set(ByVal Value As Integer)
            miManagerComputerUser_Id = Value
        End Set
    End Property

    Public Property RegionName() As String
        Get
            Return msRegionName
        End Get
        Set(ByVal Value As String)
            msRegionName = Value
        End Set
    End Property

    Public Property DepartmentName() As String
        Get
            Return msDepartmentName
        End Get
        Set(ByVal Value As String)
            msDepartmentName = Value
        End Set
    End Property

    Public Property OuName() As String
        Get
            Return msOuName
        End Get
        Set(ByVal Value As String)
            msOuName = Value
        End Set
    End Property

#End Region

End Class
