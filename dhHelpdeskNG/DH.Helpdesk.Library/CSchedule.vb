Imports System.Data
Imports Microsoft.Win32

<ComClass(CSchedule.ClassId, CSchedule.InterfaceId, CSchedule.EventsId)> _
Public Class CSchedule

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "fef25cc2-7fe0-49ba-8a64-e79f8ae2c993"
    Public Const InterfaceId As String = "4583b2cc-340c-4ac2-809e-78e1f55afecd"
    Public Const EventsId As String = "83b2aadf-6026-468a-8d51-47c45183cdc4"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub CaseSolution(ByVal sConnectionString As String)

        Dim conDH_Support
        Dim rsDH_Support, rsTemp
        Dim sSQL
        Dim iTime
        Dim sScheduleDay
        Dim sScheduleMonth
        Dim flag

        Dim sReportedBy
        Dim iDepartment_Id
        Dim sPlace
        Dim sPersons_Name
        Dim sPersons_EMail
        Dim sPersons_Phone
        Dim iDay
        Dim iMonth
        Dim sWatchdate

        Dim iOrder
        Dim iWeekday
        Dim iPos

        Dim sEmail
        Dim vCase
        Dim sMailTemplateSubject
        Dim sMailTemplateBody

        ' Skapa Connection objekt
        conDH_Support = CreateObject("ADODB.Connection")

        ' Öppna anslutning
        conDH_Support.Open(DSN)

        ' Skapa recordsetobjekt
        rsDH_Support = CreateObject("ADODB.Recordset")
        rsTemp = CreateObject("ADODB.Recordset")

        iTime = DatePart("h", Now())

        sSQL = "SELECT tblCaseSolution.*, tblCaseSolutionSchedule.ScheduleType, ScheduleDay, ScheduleWatchDate " & _
          "FROM tblCaseSolution " & _
           "INNER JOIN tblCaseSolutionSchedule ON tblCaseSolution.Id = tblCaseSolutionSchedule.CaseSolution_Id " & _
          "WHERE ScheduleTime=" & iTime

        rsDH_Support.Open(sSQL, conDH_Support)

        If Not isEmptyRecordset(rsDH_Support) Then
            Do Until rsDH_Support.EOF
                flag = False

                Select Case rsDH_Support("ScheduleType")
                    Case "1"
                        flag = True
                    Case "2"
                        iDay = DatePart("w", Now(), 2)

                        If isNull(rsDH_Support("ScheduleDay")) Then
                            sScheduleDay = ""
                        Else
                            sScheduleDay = rsDH_Support("ScheduleDay")
                        End If

                        If sScheduleDay <> "" Then
                            If InStr(1, sScheduleDay, "," & iDay & ",", 1) > 0 Then
                                flag = True
                            End If
                        End If
                    Case "3"
                        iDay = DatePart("d", Now())
                        iMonth = DatePart("m", Now())

                        If isNull(rsDH_Support("ScheduleDay")) Then
                            sScheduleDay = ""
                        Else
                            sScheduleDay = rsDH_Support("ScheduleDay")
                        End If

                        iPos = InStr(1, sScheduleDay, ";", 1)

                        If iPos > 0 Then
                            sScheduleMonth = Mid(sScheduleDay, iPos + 1)
                            sScheduleDay = Left(sScheduleDay, iPos - 1)
                        End If

                        iPos = InStr(1, sScheduleDay, ":", 1)

                        If iPos > 0 Then
                            iOrder = Left(sScheduleDay, iPos - 1)
                            iWeekday = Mid(sScheduleDay, iPos + 1)
                        End If

                        ' Kontrollera om rätt månad
                        If InStr(1, sScheduleMonth, "," & iMonth & ",", 1) > 0 Then
                            If iOrder = "" Then
                                If StrComp(iDay, sScheduleDay, 1) = 0 Then
                                    flag = True
                                End If
                            ElseIf iOrder = "5" Then
						If StrComp(iWeekday, WeekDay(Date, 2), 1) = 0 Then
                                    If isLastWeekDay() Then
                                        flag = True
                                    End If
                                End If
                            Else
						If StrComp(iWeekday, WeekDay(Date, 2), 1) = 0 Then

                                    If StrComp(iOrder, getWeekDayOrder, 1) = 0 Then
                                        flag = True
                                    End If
                                End If
                            End If
                        End If
                End Select


                If flag = True Then
                    If CLng(rsDH_Support("ScheduleWatchDate")) > 0 Then
				sWatchdate = DateAdd("d", CInt(rsDH_Support("ScheduleWatchDate")), Date)
                    End If

                    ' Kontrollera om användaruppgifter ska hämtas
                    If Not isNull(rsDH_Support("ReportedBy")) Then
                        sSQL = "SELECT tblComputerUsers.*, tblDepartment.Region_Id FROM tblComputerUsers " & _
                           "LEFT JOIN tblDepartment ON tblComputerUsers.Department_Id=tblDepartment.Id " & _
                          "WHERE " & caseInsensitiveSearch("UserId") & " = '" & LCase(rsDH_Support("ReportedBy")) & "'" & _
                           " AND tblComputerUsers.Customer_Id=" & rsDH_Support("Customer_Id") & _
                          " ORDER BY tblComputerUsers.UserId "

                        rsTemp.Open(sSQL, conDH_Support)

                        If Not IsEmptyRecordset(rsTemp) Then
                            sReportedBy = rsTemp("UserId")

                            If isNull(rsTemp("Department_Id")) Then
                                iDepartment_Id = ""
                            Else
                                iDepartment_Id = rsTemp("Department_Id")
                            End If

                            sPlace = rsTemp("Location")
                            sPersons_Name = rsTemp("FirstName") & " " & rsTemp("SurName")
                            sPersons_EMail = rsTemp("EMail")
                            sPersons_Phone = rsTemp("Phone")
                        End If

                        rsTemp.Close()
                    Else
                        sReportedBy = ""
                        iDepartment_Id = ""
                        sPlace = ""
                        sPersons_Name = ""
                        sPersons_EMail = ""
                        sPersons_Phone = ""
                    End If

                    ' Om avdelning saknas hämta första bästa
                    If iDepartment_Id = "" Then
                        sSQL = "SELECT Id FROM tblDepartment " & _
                         "WHERE Customer_Id=" & rsDH_Support("Customer_Id") & _
                          " AND Status=1 " & _
                         " ORDER BY Department "

                        rsTemp.Open(sSQL, conDH_Support)

                        If Not IsEmptyRecordset(rsTemp) Then
                            iDepartment_Id = rsTemp("Id")
                        End If

                        rsTemp.Close()
                    End If

                    If flag = True Then
                        ' Hämta max ärendenummer
                        sSQL = "SELECT Max(CaseNumber) AS Max_CaseNumber FROM tblCase"

                        rsTemp.Open(sSQL, conDH_Support)

                        iCaseNumber = rsTemp("Max_CaseNumber")
                        iCaseNumber = CDbl(iCaseNumber) + 1

                        ' Stäng recordset
                        rsTemp.Close()

                        ' Skapa ärende
                        sSQL = "INSERT INTO tblCase(CaseNumber, CaseType_Id, Customer_Id, ReportedBy, Department_Id, Place, Persons_Name, Persons_EMail, Persons_Phone, " & _
                               "Caption, Description, Miscellaneous, Priority_Id, Performer_User_Id, WatchDate, RegTime, ChangeTime) " & _
                           "VALUES(" & _
                            iCaseNumber & ", "

                        If isNull(rsDH_Support("CaseType_Id")) Then
                            sSQL = sSQL & "Null, "
                        Else
                            sSQL = sSQL & rsDH_Support("CaseType_Id") & ", "
                        End If

                        sSQL = sSQL & rsDH_Support("Customer_Id") & ", '" & _
                             sReportedBy & "', " & _
                             iDepartment_Id & ", '" & _
                             sPlace & "', '" & _
                             sPersons_Name & "', '" & _
                             sPersons_EMail & "', '" & _
                             sPersons_Phone & "', '" & _
                             rsDH_Support("Caption") & "', '"

                        If isNull(rsDH_Support("Description")) Then
                            sSQL = sSQL & "', '"
                        Else
                            sSQL = sSQL & rsDH_Support("Description") & "', '"
                        End If

                        If isNull(rsDH_Support("Miscellaneous")) Then
                            sSQL = sSQL & "', "
                        Else
                            sSQL = sSQL & rsDH_Support("Miscellaneous") & "', "
                        End If

                        If isNull(rsDH_Support("Priority_Id")) Then
                            sSQL = sSQL & "Null, "
                        Else
                            sSQL = sSQL & rsDH_Support("Priority_Id") & ", "
                        End If

                        If isNull(rsDH_Support("PerformerUser_Id")) Then
                            sSQL = sSQL & "0, "
                        Else
                            sSQL = sSQL & rsDH_Support("PerformerUser_Id") & ", "
                        End If

                        If IsDate(sWatchdate) Then
                            sSQL = sSQL & convertDateTime(sWatchdate) & ", "
                        Else
                            sSQL = sSQL & "Null, "
                        End If

                        sSQL = sSQL & "getdate(), getdate())"

                        conDH_Support.Execute(sSQL)

                        ' Kontrollera om vi ska skicka mail till handläggaren
                        ' Hämta uppgifter om ärendet
                        If Not isNull(rsDH_Support("PerformerUser_Id")) Then
                            sSQL = "SELECT tblCase.Id, tblCase.CaseNumber, tblCase.CaseGUID, tblCase.Caption, tblCase.Description, " & _
                                 "tblCase.Persons_Name, tblCase.Persons_Phone, tblCase.Persons_EMail, tblCase.Persons_CellPhone, tblCase.Customer_Id, tblCase.InventoryNumber, " & _
                                 "tblCustomer.Name AS CustomerName, tblCustomer.HelpDeskEmail, tblCustomer.Language_Id, " & _
                                 "tblCustomer.NewCaseEMailList, tblUsers.Id AS Performer_User_Id, tblUsers.FirstName, tblUsers.SurName, tblUsers.Email, " & _
                                 "tblUsers.AllocateCaseMail, ISNULL(tblWorkingGroup.WorkingGroupEMail, '') AS WorkingGroupEMail, tblPriority.PriorityName, " & _
                                 "isNull(tblPriority.EMailImportance, 1) AS EMailImportance, '' AS LogGUID, " & _
                                 "'' AS Text_External, '' AS Text_Internal, tblStateSecondary.StateSecondary " & _
                              "FROM tblUsers " & _
                               "LEFT JOIN tblWorkingGroup ON tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id " & _
                               "RIGHT OUTER JOIN tblCase " & _
                               "INNER JOIN tblCustomer ON tblCase.Customer_Id = tblCustomer.Id ON tblUsers.Id = tblCase.Performer_User_Id " & _
                               "LEFT OUTER JOIN tblPriority ON tblCase.Priority_Id=tblPriority.Id " & _
                               "LEFT OUTER JOIN tblStateSecondary ON tblCase.StateSecondary_id = tblStateSecondary.Id " & _
                              "WHERE (tblCase.Casenumber = " & iCaseNumber & ") "

                            rsTemp.Open(sSQL, conDH_Support)

                            If Not IsEmptyRecordset(rsTemp) Then
                                sEmail = rsTemp("HelpdeskEMail")

                                If isValidEMailAddress(rsTemp("WorkingGroupEMail")) Then
                                    sEmail = rsTemp("WorkingGroupEMail")
                                End If

                                vCase = createPropertyBagFromRecordset(rsTemp)
                            End If

                            rsTemp.Close()

                            If isValidEMailAddress(getPropBagValue(vCase, "EMail", 1)) = True And getPropBagValue(vCase, "AllocateCaseMail", 1) = "1" Then
                                ' Hämta mailtemplate för nytt mail
                                sSQL = "SELECT * FROM tblMailTemplate " & _
                                  "WHERE MailID=2 " & _
                                   "AND Language_Id=" & getPropBagValue(vCase, "Language_Id", 1) & _
                                   " AND (Customer_Id=" & getPropBagValue(vCase, "Customer_Id", 1) & " OR Customer_Id IS NULL) " & _
                                  "ORDER BY isNull(Customer_Id, 0) DESC"

                                rsTemp.Open(sSQL, conDH_Support)

                                If Not IsEmptyRecordset(rsTemp) Then
                                    sMailTemplateSubject = rsTemp("Subject")
                                    sMailTemplateBody = rsTemp("Body")
                                End If

                                rsTemp.Close()

                                SendEMail(sEmail, _
                                    getPropBagValue(vCase, "EMail", 1), _
                                       vCase, _
                                       sMailTemplateSubject, _
                                       sMailTemplateBody, _
                                       2)
                            End If
                        End If
                    End If
                End If

                rsDH_Support.MoveNext()
            Loop
        End If

        rsDH_Support.Close()

        ' Stäng anslutning mot databasen
        conDH_Support.Close()

        ' Ta bort objekt
        rsTemp = Nothing
        rsDH_Support = Nothing
        conDH_Support = Nothing

Function getWeekDayOrder()
        Dim iDay

        iDay = CInt(Day(Of Date)())

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

    Function getDaysPerMonth(ByVal syear, ByVal smonth)
        If IsDate(syear & "-" & smonth & "-31") Then
            getDaysPerMonth = 31
        ElseIf IsDate(syear & "-" & smonth & "-30") Then
            getDaysPerMonth = 30
        ElseIf IsDate(syear & "-" & smonth & "-29") Then
            getDaysPerMonth = 29
        Else
            getDaysPerMonth = 28
        End If
    End Function

    Function isLastWeekDay()
        Dim iYear
        Dim iMonth
        Dim iDaysPerMonth

	iDaysPerMonth = getDaysPerMonth(Year(Date()), Month(Date()))

        If iDaysPerMonth - Day(Of Date)() < 7 Then
            isLastWeekDay = True
        Else
            isLastWeekDay = False
        End If
    End Function

    Function Include(ByVal vbsFile)
        fso = CreateObject("Scripting.FileSystemObject")
        f = fso.OpenTextFile(vbsFile)
        s = f.ReadAll()
        f.Close()
        ExecuteGlobal(s)
    End Function

    End Sub
End Class


