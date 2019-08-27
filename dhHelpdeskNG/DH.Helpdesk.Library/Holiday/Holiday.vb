Public Class Holiday
#Region "Declarations"
    Private miId As Integer
    Private dtHoliday As Date
    Private miHolidayHeader_Id As Integer
#End Region

#Region "Constructors"

    Sub New(ByVal dr As DataRow)
        miId = dr("Id")
        dtHoliday = dr("Holiday")
        miHolidayHeader_Id = dr("HolidayHeader_Id")
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

    Public Property Holiday() As Date
        Get
            Return dtHoliday
        End Get
        Set(ByVal Value As Date)
            dtHoliday = Value
        End Set
    End Property

    Public Property HolidayHeader_Id() As Integer
        Get
            Return miHolidayHeader_Id
        End Get
        Set(ByVal Value As Integer)
            miHolidayHeader_Id = Value
        End Set
    End Property
#End Region
End Class
