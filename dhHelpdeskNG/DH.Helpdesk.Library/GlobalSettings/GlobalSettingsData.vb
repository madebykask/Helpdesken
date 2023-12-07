Imports DH.Helpdesk.Library.SharedFunctions

Public Class GlobalSettingsData
    Public Function getGlobalSettings() As GlobalSettings
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT * FROM tblGlobalSettings "

            dt = getDataTable(gsConnectionString, sSQL)

            Dim gs As GlobalSettings

            gs = New GlobalSettings(dt.Rows(0))

            Return gs
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function isTodayHoliday() As Boolean
        Dim sSQL As String
        Dim dt As DataTable

        Try
            If Today.DayOfWeek = DayOfWeek.Saturday Or Today.DayOfWeek = DayOfWeek.Sunday Then
                Return True
            Else
                sSQL = "SELECT * FROM tblHoliday WHERE " & Call4DateFormat("Holiday", giDBType) & " = " & convertDateTime(Now.Date, giDBType)

                dt = getDataTable(gsConnectionString, sSQL)

                If dt.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If

            
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
