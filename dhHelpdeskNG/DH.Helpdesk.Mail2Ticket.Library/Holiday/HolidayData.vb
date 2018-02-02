Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions

Public Class HolidayData
    Public Function getHolidays() As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim colHolidays As New Collection

        Try
            sSQL = "SELECT tblHoliday.Id, tblHoliday.Holiday, tblHoliday.TimeFrom, tblHoliday.TimeUntil, tblHoliday.HolidayHeader_Id FROM tblHoliday ORDER BY Holiday"

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            Dim h As Holiday

            For Each dr As DataRow In dt.Rows
                h = New Holiday(dr)
                colHolidays.Add(h)
            Next

            Return colHolidays

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
