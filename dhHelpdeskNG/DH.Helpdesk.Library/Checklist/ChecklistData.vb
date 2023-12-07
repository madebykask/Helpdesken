Imports DH.Helpdesk.Library.SharedFunctions

Public Class ChecklistData
    Public Function getChecklistSchedule() As IList(Of Checklist)
        Dim checklists As New List(Of Checklist)
        Dim sSQL As String
        Dim dr As DataRow
        Dim iTime As Integer
        Dim flag As Boolean = False
        Dim iDay As Integer
        Dim iMonth As Integer
        Dim sScheduleday As String = ""
        Dim sScheduleMonth As String = ""
        Dim iPos As Integer
        Dim iOrder As Integer
        Dim iWeekday As Integer

        iTime = DatePart("h", Now())

        Try
            sSQL = "SELECT tblChecklists.Id, tblChecklists.ChecklistName, tblChecklistSchedule.Recipients, tblChecklistSchedule.ScheduleDay, " & _
                        "tblChecklistSchedule.ScheduleTime, tblChecklistSchedule.ScheduleType, tblChecklists.Customer_Id " & _
                    "FROM tblChecklists " & _
                        "INNER JOIN tblChecklistSchedule ON tblChecklists.Id = tblChecklistSchedule.Checklists_Id " & _
                    "WHERE ScheduleTime=" & iTime

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)
            Dim c As Checklist

            For Each dr In dt.Rows
                flag = False
                Select Case dr("ScheduleType")

                    Case "1"
                        flag = True
                    Case "2"
                        iDay = DatePart("w", Now(), FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

                        If IsDBNull(dr("ScheduleDay")) Then
                            sScheduleday = ""
                        Else
                            sScheduleday = dr("ScheduleDay")
                        End If

                        If sScheduleday <> "" Then
                            If InStr(1, sScheduleday, "," & iDay & ",", 1) > 0 Then
                                flag = True
                            End If
                        End If
                    Case "3"
                        iDay = DatePart("d", Now())
                        iMonth = DatePart("m", Now())

                        If IsDBNull(dr("ScheduleDay")) Then
                            sScheduleday = ""
                        Else
                            sScheduleday = dr("ScheduleDay")
                        End If

                        iPos = InStr(1, sScheduleday, ";", 1)

                        If iPos > 0 Then
                            sScheduleMonth = Mid(sScheduleday, iPos + 1)
                            sScheduleday = Left(sScheduleday, iPos - 1)
                        End If

                        iPos = InStr(1, sScheduleday, ":", 1)

                        If iPos > 0 Then
                            iOrder = Left(sScheduleday, iPos - 1)
                            iWeekday = Mid(sScheduleday, iPos + 1)
                        End If

                        ' Kontrollera om rätt månad
                        If InStr(1, sScheduleMonth, "," & iMonth & ",", 1) > 0 Then
                            If iOrder = 0 Then
                                If StrComp(iDay, sScheduleday, 1) = 0 Then
                                    flag = True
                                End If
                            ElseIf iOrder = 5 Then
                                If StrComp(iWeekday, Date.Today.DayOfWeek(), 1) = 0 Then
                                    If isLastWeekDay() Then
                                        flag = True
                                    End If
                                End If
                            Else
                                If StrComp(iWeekday, Date.Today.DayOfWeek, 1) = 0 Then

                                    If StrComp(iOrder, getWeekDayOrder, 1) = 0 Then
                                        flag = True
                                    End If
                                End If
                            End If
                        End If
                End Select

                If flag = True Then
                    c = New Checklist

                    c.Id = dr("Id")
                    c.Customer_Id = dr("Customer_Id")
                    c.ChecklistName = dr("ChecklistName")

                    If Not IsDBNull(dr("Recipients")) Then
                        c.Recipients = dr("Recipients")

                        c.ChecklistMailBody = getChecklistMailBody(dr("Id"))
                    End If

                    checklists.Add(c)
                End If
            Next

            Return checklists
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function isLastWeekDay() As Boolean
        Dim iDaysPerMonth

        iDaysPerMonth = Date.DaysInMonth(Date.Today.Year, Date.Today.Month)

        If iDaysPerMonth - DatePart(DateInterval.Day, Date.Today, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) < 7 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function getWeekDayOrder() As Integer
        Dim iDay

        iDay = Date.Today.DayOfWeek

        If iDay >= 1 And iDay <= 7 Then
            getWeekDayOrder = 1
        ElseIf iDay >= 8 And iDay <= 14 Then
            getWeekDayOrder = 2
        ElseIf iDay >= 15 And iDay <= 21 Then
            getWeekDayOrder = 3
        ElseIf iDay >= 22 And iDay <= 28 Then
            getWeekDayOrder = 4
        Else
            getWeekDayOrder = 5
        End If
    End Function

    Private Function getChecklistMailBody(ByVal iChecklists_Id As Integer) As String
        Dim sSQL As String
        Dim dr As DataRow
        Dim sMailBody As String
        Dim sChecklistService As String = ""

        Try
            sSQL = "SELECT tblChecklistAction.Id, tblCheckListService.Id AS ChecklistService_Id, tblCheckListService.ChecklistService, tblChecklistAction.CheckListAction " & _
                "FROM tblCheckListService " & _
                    "LEFT JOIN tblCheckListAction ON tblCheckListAction.CheckListService_Id=tblCheckListService.Id " & _
                "WHERE tblCheckListService.Checklists_Id=" & iChecklists_Id.ToString & _
                " ORDER BY tblCheckListService.ChecklistService, tblChecklistAction.ChecklistAction"

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)

            sMailBody = "<table style=""font-family:arial;font-size:12px;width:700px;border:#F0F0F0 1px solid;"" cellpadding=3 cellspacing=0 border=1><tr style=""background-color:#E1E1E1""><td style=""WIDTH:350px"">Åtgärder</td><td style=""WIDTH:350px"">Notering</td></tr>"


            For Each dr In dt.Rows
                If sChecklistService <> dr("ChecklistService") Then
                    sMailBody = sMailBody & "<tr><td><b>" & dr("ChecklistService") & "</b></td><td>&nbsp;</td></tr>"
                
                End If

                sMailBody = sMailBody & "<tr><td>&nbsp;&nbsp;&nbsp;" & dr("CheckListAction") & "</td><td>&nbsp;</td></tr>"


                sChecklistService = dr("ChecklistService")
            Next

            sMailBody = sMailBody & "</table>"

            Return sMailBody
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub createChecklistDate(ByVal iChecklists_Id As Integer, ByVal iCustomer_Id As Integer)
        Dim sSQL As String = ""

        Try
            ' Skapa loggpost
            sSQL = "INSERT INTO tblChecklist(Customer_Id, CheckLists_Id, Checklistdate) " & _
                        "VALUES(" & iCustomer_Id & ", " & iChecklists_Id & ", " & convertDateTime(Date.Today, giDBType) & ")"

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createChecklistDate " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Sub
End Class
