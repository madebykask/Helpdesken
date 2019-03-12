Imports System.Data.SqlClient
Imports System.Linq
Imports DH.Helpdesk.BusinessData.Models.WorktimeCalculator
Imports DH.Helpdesk.Dal.Infrastructure
Imports DH.Helpdesk.Dal.Repositories
'Imports System.Data.Odbc
Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions
Imports DH.Helpdesk.Services.Infrastructure
Imports DH.Helpdesk.Services.Services
Imports DH.Helpdesk.Services.Utils

Public Class CaseData
    Public Function getTodayPlanDate() As Collection
        Try
            Return getCases(iPlanDate:=1)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getTodayWatchDate() As Collection
        Try
            Return getCases(iWatchdate:=1)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseForAutomaticApprove() As Collection
        Try

            Return getCases(iApproval:=1)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCasePerPerformer(ByVal iPerformerUser_Id As Integer) As Collection
        Try
            Return getCases(iPerformerUser_Id:=iPerformerUser_Id)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub saveFileInfo(ByVal iCase_Id As Integer, ByVal sFileName As String)
        Dim sSQL As String = ""

        Try
            ' Spara filinfo i databasen
            sSQL = "INSERT INTO tblCaseFile(Case_Id, FileName) VALUES(" & iCase_Id & ", " & getDBStringPrefix() & "'" & Replace(sFileName, "'", "''") & "')"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", saveFileInfo: " & sSQL)
            End If

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR saveFileInfo " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Sub

    Public Sub saveCaseIsAbout(ByVal iCase_Id As Integer, ByVal u As ComputerUser)
        Dim sSQL As String = ""

        Try

            sSQL = " INSERT INTO tblCaseIsAbout ([Case_Id],[ReportedBy],[Person_Name],[Person_Email],[Person_Phone],[Person_CellPhone],[Region_Id],[Department_Id],[OU_Id],[CostCentre],[Place],[UserCode]"
            sSQL = sSQL & ") Values (" & CStr(iCase_Id) & ", "

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(u.UserId, "'", ""), 40) & "', "
            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(u.FirstName & u.SurName, "'", ""), 50) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(u.EMail, "'", "''"), 100) & "', " &
                            getDBStringPrefix() & "'" & Replace(Left(u.Phone, 40), "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Left(u.CellPhone, 30) & "', "
            If u.Region_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & u.Region_Id & ", "
            End If
            If u.Department_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & u.Department_Id & ", "
            End If
            If u.OU_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & u.OU_Id & ", "
            End If
            sSQL = sSQL & getDBStringPrefix() & "'" & Left(u.CostCentre, 50) & "', " &
                            getDBStringPrefix() & "'" & Left(u.Location, 100) & "', " &
                            getDBStringPrefix() & "'" & Left(u.UserCode, 20) & "'"
            sSQL = sSQL & ")"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", saveCaseIsAbout: " & sSQL)
            End If

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR saveCaseIsAbout " & ex.Message.ToString & ", " & sSQL)
            End If
            Throw ex
        End Try
    End Sub

    Public Sub approveCase(ByVal objCase As CCase)
        Dim sSQL As String

        Try

            Dim workTime As CaseWorkTime = calculateWorkTimeOnChange(objCase)
            sSQL = $"UPDATE tblCase SET ApprovedDate='{workTime.Now}', ChangeTime='{workTime.Now}', ExternalTime={workTime.ExternalTime}, LeadTime={workTime.LeadTime} WHERE Id={objCase.Id}"

            executeSQL(gsConnectionString, sSQL)
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Public Function calculateWorkTimeOnChange(ByVal ccase As CCase) As CaseWorkTime
        Dim databaseFactory = New DatabaseFactory(gsConnectionString)
        Dim userService = New UserService(Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              New UserRepository(databaseFactory),
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing,
                                              Nothing)

        Dim customerService As CustomerService = New CustomerService(Nothing,
                                                      Nothing,
                                                      New CustomerRepository(databaseFactory),
                                                      Nothing,
                                                      Nothing,
                                                      Nothing,
                                                      Nothing,
                                                      Nothing,
                                                      Nothing,
                                                      Nothing,
                                                      Nothing)

        Dim settingsService As SettingService = New SettingService(New SettingRepository(databaseFactory), Nothing, Nothing)

        Dim departmentService = New DepartmentService(New DepartmentRepository(databaseFactory), Nothing, Nothing, Nothing)
        Dim holidayService = New HolidayService(New HolidayRepository(databaseFactory), Nothing, Nothing, departmentService)

        Dim customer As DH.Helpdesk.Domain.Customer = customerService.GetCustomer(ccase.Customer_Id)

        Dim setting As DH.Helpdesk.Domain.Setting = settingsService.GetCustomerSetting(customer.Id)

        Dim timeZone As TimeZoneInfo = TimeZoneInfo.GetSystemTimeZones().First(Function(o) o.BaseUtcOffset.TotalMinutes = setting.TimeZone_offset)

        Dim workTimeCalcFactory As WorkTimeCalculatorFactory = New WorkTimeCalculatorFactory(
                holidayService,
                customer.WorkingDayStart,
                customer.WorkingDayEnd,
                timeZone)


        Dim departmentIds As Integer()

        If ccase.Department_Id > 0 Then
            departmentIds = New Integer() {ccase.Department_Id}
        End If

        Dim utcNow = DateTime.UtcNow
        Dim workTimeCalc = workTimeCalcFactory.Build(ccase.RegTime, utcNow, departmentIds)

        Dim isExternalTime As Boolean = False

        If (ccase.IncludeInCaseStatistics = 0 Or ccase.FinishingDate <> DateTime.MinValue) Then
            isExternalTime = True
        End If

        Dim workTime = calculateCaseWorkTime(workTimeCalc, ccase.RegTime, ccase.ChangeTime, ccase.ExternalTime, ccase.Department_Id, isExternalTime, utcNow, setting.TimeZone_offset)

        Return workTime
    End Function

    Public Function calculateCaseWorkTime(ByVal workTimeCalculator As WorkTimeCalculator, ByVal regTime As DateTime, ByVal from As DateTime, ByVal currentExternalTime As Integer, ByVal departmentId As Integer, ByVal isExternalTime As Boolean, ByVal utcNow As DateTime, ByVal customerTimeoffset As Integer) As CaseWorkTime

        Dim possibleWorktime = workTimeCalculator.CalculateWorkTime(
                regTime,
                utcNow,
                departmentId,
                customerTimeoffset)

        Dim externalTime = currentExternalTime
        If (isExternalTime) Then
            Dim externalTimeToAdd = workTimeCalculator.CalculateWorkTime(
                from,
                utcNow,
                departmentId,
                customerTimeoffset)
            externalTime = externalTime + externalTimeToAdd
        End If

        Dim leadTime = possibleWorktime - externalTime

        Dim workTime = New CaseWorkTime(externalTime, leadTime, utcNow)
        Return workTime

    End Function

    Public Sub activateCase(ByVal objCase As CCase, ByVal iOpenCase_StateSecondary_Id As Integer, ByVal iWorkingDayStart As Integer, ByVal iWorkingDayEnd As Integer, iTimeZone_offset As Integer)
        Dim sSQL As String

        Try

            Dim workTime = calculateWorkTimeOnChange(objCase)

            'Dim iExternalTime As Integer = LeadTimeMinutes(getLocalTime(objCase.ChangeTime, iTimeZone_offset), getLocalTime(DateTime.UtcNow, iTimeZone_offset), iWorkingDayStart, iWorkingDayEnd, objCase.HolidayHeader_Id)

            sSQL = $"UPDATE tblCase SET " &
                            "Finishingdate=Null, " &
                            "ApprovedDate=Null, " &
                            "ApprovedBy_User_Id=Null, " &
                            "Status=1, " &
                            $"ChangeTime='{workTime.Now}', " &
                            $"ExternalTime={workTime.ExternalTime}, " &
                            $"LeadTime={workTime.LeadTime}"

            If iOpenCase_StateSecondary_Id <> 0 Then
                sSQL = sSQL & ", StateSecondary_Id=" & iOpenCase_StateSecondary_Id
            End If

            sSQL = sSQL & " WHERE Id=" & objCase.Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Public Sub markCaseUnread(ByVal objCase As CCase)
        Dim sSQL As String

        Try
            Dim workTime = calculateWorkTimeOnChange(objCase)
            sSQL = $"UPDATE tblCase SET Status=1, ChangeTime='{workTime.Now}', ExternalTime={workTime.ExternalTime}, LeadTime={workTime.LeadTime} WHERE Id={objCase.Id}"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub closeCase(ByVal objCase As CCase)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblCase SET FinishingDate=getutcdate() WHERE Id=" & objCase.Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub clearFileViewLog()
        Dim sSQL As String

        Try
            sSQL = "DELETE FROM tblFileViewLog WHERE " & Call4DateFormat("CreatedDate", giDBType) & " < " & convertDateTime(DateAdd(DateInterval.Year, -1, Now.Date), giDBType)

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub resetStateSecondary(ByVal objCase As CCase, ByVal iWorkingDayStart As Integer, ByVal iWorkingDayEnd As Integer, iTimeZone_offset As Integer)
        Dim sSQL As String

        Try

            'Dim iExternalTime As Integer = 0

            'If objCase.IncludeInCaseStatistics = 0 And objCase.FinishingDate = Date.MinValue Then
            'iExternalTime = LeadTimeMinutes(getLocalTime(objCase.ChangeTime, iTimeZone_offset), getLocalTime(DateTime.UtcNow, iTimeZone_offset), iWorkingDayStart, iWorkingDayEnd, objCase.HolidayHeader_Id)
            'End If

            Dim workTime = calculateWorkTimeOnChange(objCase)

            sSQL = $"UPDATE tblCase SET StateSecondary_Id=Null, ChangeTime='{workTime.Now}', ExternalTime={workTime.ExternalTime}, LeadTime={workTime.LeadTime} WHERE tblCase.Id={objCase.Id}"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function createCase(ByVal objCase As CCase) As CCase
        Dim sSQL As String = ""
        'Dim sDescription As String

        Try

            ' Skapa nytt ärende
            sSQL = "INSERT INTO tblCase(CaseGUID, CaseType_Id, Customer_Id, ProductArea_Id,Category_Id, Region_Id, ReportedBy, Department_Id, OU_Id, Persons_Name, " &
                            "Persons_EMail, Persons_Phone, Persons_CellPhone, Place, UserCode, CostCentre, InventoryNumber, InvoiceNumber, Caption, Description, Miscellaneous, Available, ReferenceNumber, Priority_Id, WorkingGroup_Id, Performer_User_Id, Status_Id, StateSecondary_Id, " &
                            "WatchDate, RegistrationSource, RegLanguage_Id, RegistrationSourceCustomer_Id, RegUserName, RegTime, ChangeTime) " &
                        "VALUES(" &
                            getDBStringPrefix() & "'" & objCase.CaseGUID & "', " &
                            objCase.CaseType_Id & ", " &
                            objCase.Customer_Id & ", "

            If objCase.ProductArea_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.ProductArea_Id & ", "
            End If

            If objCase.Category_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Category_Id & ", "
            End If

            If objCase.Region_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Region_Id & ", "
            End If
            sSQL = sSQL & getDBStringPrefix() & "'" & objCase.ReportedBy & "', "

            If objCase.Department_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Department_Id & ", "
            End If

            If objCase.OU_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.OU_Id & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.Persons_Name, "'", ""), 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Persons_EMail, "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Replace(Left(objCase.Persons_Phone, 40), "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Left(objCase.Persons_CellPhone, 30) & "', " &
                            getDBStringPrefix() & "'" & Left(objCase.Place, 50) & "', " &
                            getDBStringPrefix() & "'" & Left(objCase.UserCode, 20) & "', " &
                            getDBStringPrefix() & "'" & Left(objCase.CostCentre, 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InventoryNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InvoiceNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Caption, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Description, "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Miscellaneous, "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Available, "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.ReferenceNumber, "'", "''") & "', "

            If objCase.Priority_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Priority_Id & ", "
            End If

            If objCase.WorkingGroup_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.WorkingGroup_Id & ", "
            End If

            If objCase.Performer_User_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Performer_User_Id & ", "
            End If

            If objCase.Status_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Status_Id & ", "
            End If

            If objCase.StateSecondary_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.StateSecondary_Id & ", "
            End If

            If objCase.WatchDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.WatchDate, giDBType) & ", "
            End If

            sSQL = sSQL & objCase.RegistrationSource & ", " & objCase.RegLanguage_Id & ", "

            If objCase.RegistrationSourceCustomer_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.RegistrationSourceCustomer_Id & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.RegUserName, "'", "''"), 200) & "', "

            sSQL = sSQL & "getutcdate(), getutcdate())"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)

            '    sDescription = objCase.Description

            '    If Len(sDescription) > 2000 Then
            '        sDescription = Mid(sDescription, 2001)

            '        Do Until Len(sDescription) = 0
            '            sSQL = "UPDATE tblCase SET Description = Description || '" & Replace(Left(sDescription, 2000), "'", "''") & "' WHERE CaseGUID='" & objCase.CaseGUID & "'"

            '            executeSQLOracle(gsConnectionString, sSQL)

            '            sDescription = Mid(sDescription, 2001)
            '        Loop
            '    End If
            'End If

            Dim newCase As CCase = getCaseById(sCaseGUID:=objCase.CaseGUID)

            If objCase.Form_Id <> 0 Then
                sSQL = "INSERT INTO tblFormFieldValue(Case_Id, FormField_Id, FormFieldValue) " &
                        "SELECT " & newCase.Id & ", Id, ' ' FROM tblFormField WHERE Form_Id=" & objCase.Form_Id

                'If giDBType = 0 Then
                executeSQL(gsConnectionString, sSQL)
                'Else
                '    executeSQLOracle(gsConnectionString, sSQL)
                'End If
            End If

            Return newCase
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createCase " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex

        End Try
    End Function


    Public Function getCase(ByVal caseId As Integer) As CCase
        Try
            Return getCaseById(iCaseId:=caseId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseByGUID(ByVal sCaseGUID As String) As CCase
        Try
            Return getCaseById(sCaseGUID:=sCaseGUID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseByMessageID(ByVal sMessageId As String) As CCase
        Try
            Return getCaseById(sMessageId:=sMessageId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseByOrderMessageID(ByVal sMessageId As String) As CCase
        Try
            Return getCaseById(sOrderMessageId:=sMessageId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseByCaseNumber(ByVal iCaseNumber As Integer) As CCase
        Try
            Return getCaseById(iCaseNumber:=iCaseNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseReminder() As Collection
        Try
            Return getCases(iReminder:=1)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getCaseById(Optional iCaseId As Integer = 0, Optional ByVal sCaseGUID As String = "", Optional ByVal sMessageId As String = "", Optional ByVal iCaseNumber As Integer = 0, Optional ByVal sOrderMessageId As String = "") As CCase
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblCase.Id, tblCase.CaseGUID, tblCase.CaseNumber, tblCase.Customer_Id, tblCase.CaseType_Id, tblCaseType.CaseType, tblCase.ProductArea_Id, tblCase.Category_Id, tblCategory.Category, tblProductArea.ProductArea, " &
                        "tblCase.Priority_Id, tblCase.Region_Id, tblCase.Department_Id, tblCase.OU_Id, tblCustomer.Name AS CustomerName, tblCase.Performer_User_Id, tblCase.RegLanguage_Id, " &
                       "tblCase.ReportedBy, tblCase.Persons_Name, tblCase.InvoiceNumber, tblCase.Caption, tblCase.Description, tblCase.Miscellaneous, tblUsers.FirstName AS PerformerFirstName, tblUsers.SurName AS PerformerSurName, tblUsers.EMail AS PerformerEMail, " &
                       "u2.FirstName AS RegUserFirstName, u2.SurName AS RegUserSurName, tblCase.WorkingGroup_Id," &
                       "tblCase.Persons_EMail, tblCase.Persons_Phone, tblCase.Place, tblCase.UserCode, tblCase.CostCentre, tblPriority.PriorityName, " &
                       "tblWorkingGroup.WorkingGroup AS PerformerWorkingGroup, tblWorkingGroup.Id AS PerformerWorkingGroup_Id, tblWorkingGroup.AllocateCaseMail AS PerformerWorkingGroupAllocateCaseMail, " &
                       "tblWorkingGroup_1.WorkingGroup AS CaseWorkingGroup, ISNULL(tblWorkingGroup_1.WorkingGroupEMail, '') AS WorkingGroupEMail, tblWorkingGroup_1.AllocateCaseMail AS AllocateCaseMail, " &
                       "tblCase.RegTime, tblCase.ChangeTime, tblCase.InventoryNumber, tblCase.Persons_CellPhone, tblCaseType.AutomaticApproveTime, " &
                       "tblCase.FinishingDate, Isnull(tblUsers.ExternalUpdateMail, 0) AS ExternalUpdateMail, ISNULL(tblWorkingGroup.WorkingGroupEMail, '') AS PerformerWorkingGroupEMail, " &
                       "tblCase.StateSecondary_Id, tblStateSecondary.StateSecondary, tblStateSecondary.ResetOnExternalUpdate, tblDepartment.Department, tblCase.WatchDate, tblCase.RegistrationSource, " &
                       "IsNull(tblDepartment.HolidayHeader_Id, 1) AS HolidayHeader_Id, tblCase.RegUserName, tblCase.Available, tblCase.ReferenceNumber, isnull(tblStateSecondary.IncludeInCaseStatistics, 1) AS IncludeInCaseStatistics, tblCase.ExternalTime, tblCase.LeadTime " &
                   "FROM tblCase " &
                       "INNER JOIN tblCustomer ON tblCase.Customer_Id = tblCustomer.Id " &
                       "LEFT OUTER JOIN tblUsers ON tblCase.Performer_user_Id=tblUsers.Id " &
                       "LEFT JOIN tblUsers u2 ON tblCase.User_Id = u2.Id " &
                       "LEFT OUTER JOIN tblWorkingGroup ON tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id " &
                       "LEFT OUTER JOIN tblWorkingGroup tblWorkingGroup_1 ON tblCase.WorkingGroup_Id = tblWorkingGroup_1.Id " &
                       "LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id " &
                       "INNER JOIN tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id " &
                       "LEFT JOIN tblStateSecondary ON tblCase.StateSecondary_Id=tblStateSecondary.Id " &
                       "LEFT JOIN tblCategory ON tblCase.Category_Id=tblCategory.Id " &
                       "LEFT JOIN tblProductArea ON tblCase.ProductArea_Id=tblProductArea.Id " &
                       "LEFT JOIN tblDepartment ON tblCase.Department_Id=tblDepartment.Id "

            If iCaseId > 0 Then
                sSQL = sSQL & "WHERE tblCase.Id=" & iCaseId.ToString()
            ElseIf sCaseGUID <> "" Then
                sSQL = sSQL & " WHERE tblCase.CaseGUID='" & sCaseGUID & "'"
            ElseIf sMessageId <> "" Then
                sSQL = sSQL & "WHERE tblCase.Id IN (SELECT Case_Id FROM tblCaseHistory WHERE Id IN (SELECT CaseHistory_Id FROM tblEMailLog WHERE MessageId='" & sMessageId & "'))"
            ElseIf iCaseNumber <> 0 Then
                sSQL = sSQL & "WHERE tblCase.CaseNumber=" & iCaseNumber
            ElseIf sOrderMessageId <> "" Then
                sSQL = sSQL & "WHERE tblCase.Id IN (SELECT Id FROM tblCase WHERE CaseNumber IN (SELECT CaseNumber FROM tblOrder WHERE Id IN (SELECT Order_Id FROM tblOrderEmailLog WHERE MessageId='" & sOrderMessageId & "')))"
            End If

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If dt.Rows.Count > 0 Then
                Dim c As CCase

                c = New CCase(dt.Rows(0))

                Return c
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getCaseById " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Function

    Public Function getMailIDByMessageID(ByVal sMessageId As String) As Integer
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblEMailLog.MailId " &
                   "FROM tblEMailLog " &
                   "WHERE MessageId='" & sMessageId & "'"

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("MailId")
            Else
                Return 0
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getMailIDByMessageID " & ex.Message.ToString & ", " & sSQL)
            End If

            Return 0
        End Try
    End Function

    Private Function getCases(Optional ByVal iPerformerUser_Id As Integer = 0, Optional ByVal iPlanDate As Integer = 0, Optional ByVal iApproval As Integer = 0, Optional ByVal iWatchdate As Integer = 0, Optional ByVal iReminder As Integer = 0) As Collection
        Dim colCase As New Collection
        Dim sSQL As String
        Dim dr As DataRow

        Try
            sSQL = "SELECT tblCase.Id, tblCase.CaseGUID, tblCase.CaseNumber, tblCase.Customer_Id, tblCase.CaseType_Id, tblCaseType.CaseType, tblCase.ProductArea_Id, tblCase.Category_Id, tblCategory.Category, tblProductArea.ProductArea, " &
                        "tblCase.Priority_Id, tblCase.Region_Id, tblCase.Department_Id, tblCase.OU_Id, tblCustomer.Name AS CustomerName, tblCase.Performer_User_Id, tblCase.RegLanguage_Id, " &
                        "tblCase.ReportedBy, tblCase.Persons_Name, tblCase.InvoiceNumber, tblCase.Caption, tblCase.Description, tblCase.Miscellaneous, tblUsers.FirstName AS PerformerFirstName, tblUsers.SurName AS PerformerSurName, tblUsers.EMail AS PerformerEMail, " &
                        "u2.FirstName AS RegUserFirstName, u2.SurName AS RegUserSurName, tblCase.WorkingGroup_Id," &
                        "tblCase.Persons_EMail, tblCase.Persons_Phone, tblCase.Place, tblCase.UserCode, tblCase.CostCentre, tblPriority.PriorityName, " &
                        "tblCase.RegTime, tblCase.ChangeTime, tblCase.InventoryNumber, tblCase.Persons_CellPhone, tblCaseType.AutomaticApproveTime, " &
                        "tblWorkingGroup.WorkingGroup AS PerformerWorkingGroup, tblWorkingGroup.Id AS PerformerWorkingGroup_Id, tblWorkingGroup.AllocateCaseMail AS PerformerWorkingGroupAllocateCaseMail, " &
                        "tblWorkingGroup_1.WorkingGroup AS CaseWorkingGroup, ISNULL(tblWorkingGroup_1.WorkingGroupEMail, '') AS WorkingGroupEMail, tblWorkingGroup_1.AllocateCaseMail AS AllocateCaseMail, " &
                        "tblCase.FinishingDate, Isnull(tblUsers.ExternalUpdateMail, 0) AS ExternalUpdateMail, ISNULL(tblWorkingGroup.WorkingGroupEMail, '') AS PerformerWorkingGroupEMail, " &
                        "tblCase.StateSecondary_Id, tblStateSecondary.StateSecondary, tblStateSecondary.ResetOnExternalUpdate, tblDepartment.Department, tblCase.RegistrationSource, tblCase.WatchDate, tblCase.Available, tblCase.ReferenceNumber, " &
                        "IsNull(tblDepartment.HolidayHeader_Id, 1) AS HolidayHeader_Id, tblCase.RegUserName, isnull(tblStateSecondary.IncludeInCaseStatistics, 1) AS IncludeInCaseStatistics, tblCase.ExternalTime, tblCase.LeadTime " &
                    "FROM tblCase " &
                        "INNER JOIN tblCustomer ON tblCase.Customer_Id = tblCustomer.Id " &
                        "INNER JOIN tblUsers ON tblCase.Performer_user_Id=tblUsers.Id " &
                        "LEFT JOIN tblUsers u2 ON tblCase.User_Id = u2.Id " &
                        "LEFT OUTER JOIN tblWorkingGroup ON tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id " &
                        "LEFT OUTER JOIN tblWorkingGroup tblWorkingGroup_1 ON tblCase.WorkingGroup_Id = tblWorkingGroup_1.Id " &
                        "LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id " &
                        "INNER JOIN tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id " &
                        "LEFT JOIN tblStateSecondary ON tblCase.StateSecondary_Id=tblStateSecondary.Id " &
                        "LEFT JOIN tblCategory ON tblCase.Category_Id=tblCategory.Id " &
                        "LEFT JOIN tblProductArea ON tblCase.ProductArea_Id=tblProductArea.Id " &
                        "LEFT JOIN tblDepartment ON tblCase.Department_Id=tblDepartment.Id "

            If iPerformerUser_Id <> 0 Then
                sSQL = sSQL & "WHERE tblCase.FinishingDate IS NULL AND tblCase.Performer_user_Id=" & iPerformerUser_Id
            ElseIf iPlanDate <> 0 Then
                sSQL = sSQL & "WHERE " & Call4DateFormat("PlanDate", giDBType) & " = " & convertDateTime(Now.Date, giDBType) &
                        " AND tblUsers.PlanDateMail = 1 " &
                        " AND tblCase.FinishingDate IS NULL"
            ElseIf iWatchdate <> 0 Then
                sSQL = sSQL & "WHERE " & Call4DateFormat("WatchDate", giDBType) & " = " & convertDateTime(Now.Date, giDBType) & " AND tblCase.FinishingDate IS NULL AND tblUsers.WatchDateMail = 1 "
            ElseIf iApproval = 1 Then
                sSQL = sSQL & "WHERE tblCaseType.AutomaticApproveTime <> 0 AND tblCase.ApprovedDate IS NULL AND tblCase.FinishingDate IS NOT NULL "
            ElseIf iReminder = 1 Then
                sSQL = sSQL & "WHERE tblStateSecondary.ReminderDays <> 0 AND tblCase.FinishingDate IS NULL "

                sSQL = sSQL & "AND tblCase.Id IN (SELECT tblcasehistory.Case_Id FROM tblCasehistory INNER JOIN tblStateSecondary ON tblCaseHistory.StateSecondary_Id=tblstatesecondary.Id WHERE tblStateSecondary.Reminderdays <> 0 AND Datediff(d, tblcasehistory.CreatedDate, getdate()) = tblstatesecondary.Reminderdays)"
            End If

            sSQL = sSQL & " ORDER BY tblCustomer.Name, tblCase.CaseNumber"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCases, " & sSQL)
            End If

            Dim dt As DataTable

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            Dim c As CCase

            For Each dr In dt.Rows
                c = New CCase(dr)
                colCase.Add(c)
            Next

            Return colCase
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function saveCaseHistory(ByVal iCase_Id As Integer, ByVal sCreatedByUser As String) As Integer
        Dim sSQL As String
        Dim sCaseHistoryGUID As String

        Try
            ' Skapa nytt GUID
            sCaseHistoryGUID = System.Guid.NewGuid.ToString()

            ' Lägg in den nya posten i historiken
            sSQL = "INSERT INTO tblCaseHistory(CaseHistoryGUID, Case_Id ,ReportedBy ,Persons_Name,Persons_EMail,Persons_Phone,Persons_CellPhone," &
                                                "Customer_Id,Region_Id,Department_Id,OU_Id,Place,UserCode,CostCentre,InventoryNumber,InventoryType,InventoryLocation, " &
                                                "Casenumber,User_Id,IPAddress,CaseType_Id,ProductArea_Id,ProductAreaSetDate,Category_Id,Supplier_Id,InvoiceNumber, " &
                                                "ReferenceNumber,Caption,Description,Miscellaneous,ContactBeforeAction,SMS,Available,Cost,OtherCost,Currency," &
                                                "Performer_User_Id,CaseResponsibleUser_Id,Priority_Id,Status_Id,StateSecondary_Id,ExternalTime,Project_Id, " &
                                                "PlanDate,ApprovedDate,ApprovedBy_User_Id,WatchDate,LockCaseToWorkingGroup_Id,WorkingGroup_Id,FinishingDate, " &
                                                "FollowUpDate,RegistrationSource,RelatedCaseNumber,Problem_Id," &
                                                "Deleted,Status,RegLanguage_Id,RegUserId,RegUserDomain,RegistrationSourceCustomer_Id, CreatedDate,CreatedByUser, LeadTime ) " &
                    " SELECT '" & sCaseHistoryGUID & "', " & iCase_Id & " ,ReportedBy ,Persons_Name,Persons_EMail,Persons_Phone,Persons_CellPhone," &
                                                "Customer_Id,Region_Id,Department_Id,OU_Id,Place,UserCode,CostCentre,InventoryNumber,InventoryType,InventoryLocation, " &
                                                "Casenumber,User_Id,IPAddress,CaseType_Id,ProductArea_Id,ProductAreaSetDate,Category_Id,Supplier_Id,InvoiceNumber, " &
                                                "ReferenceNumber,Caption,Description,Miscellaneous,ContactBeforeAction,SMS,Available,Cost,OtherCost,Currency," &
                                                "Performer_User_Id,CaseResponsibleUser_Id,Priority_Id,Status_Id,StateSecondary_Id,ExternalTime,Project_Id, " &
                                                "PlanDate,ApprovedDate,ApprovedBy_User_Id,WatchDate,LockCaseToWorkingGroup_Id,WorkingGroup_Id,FinishingDate, " &
                                                "FollowUpDate,RegistrationSource,RelatedCaseNumber,Problem_Id," &
                                                "Deleted,Status,RegLanguage_Id,RegUserId,RegUserDomain, RegistrationSourceCustomer_Id, getutcdate(),'" & Replace(sCreatedByUser, "'", "''") & "', tblCase.LeadTime  FROM tblCase WHERE Id=" & iCase_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

            Return getCaseHistoryIdByGUID(sCaseHistoryGUID)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Function getCaseHistoryIdByGUID(ByVal sCaseHistoryGUID As String) As Integer
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblCaseHistory.Id " &
                   "FROM tblCaseHistory " &
                "WHERE tblCaseHistory.CaseHistoryGUID='" & sCaseHistoryGUID & "'"


            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("Id")
            Else
                Return 0
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getCaseHistoryIdByGUID " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Function

    Public Function getCaseSolutionSchedule() As Collection
        Dim colCase As New Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim iTime As Integer = Now.Hour
        Dim flag As Boolean = False
        Dim c As CCase
        Dim iDay As Integer = 0
        Dim iMonth As Integer = 0
        Dim sScheduleday As String = ""
        Dim sScheduleMonth As String = ""
        Dim iOrder As String = ""
        Dim iWeekday As Integer = 0
        Dim objComputerUserData As New ComputerUserData
        Dim logAdded As Boolean = False

        Try
            sSQL = "SELECT tblCaseSolution.*, tblCaseSolutionSchedule.ScheduleType, tblCaseSolutionSchedule.ScheduleDay, tblCaseSolutionSchedule.ScheduleWatchDate, " &
                        "tblCustomer.Language_Id, tblDepartment.Region_Id " &
                      "FROM tblCaseSolution " &
                       "INNER JOIN tblCaseSolutionSchedule ON tblCaseSolution.Id = tblCaseSolutionSchedule.CaseSolution_Id " &
                       "INNER JOIN tblCustomer ON tblCaseSolution.Customer_Id=tblCustomer.Id " &
                        "LEFT JOIN tblDepartment ON tblCaseSolution.Department_Id=tblDepartment.Id " &
                      "WHERE ScheduleTime=" & iTime

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCaseSolutionSchedule " & sSQL)
            End If

            Dim dt As DataTable

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCaseSolutionSchedule rows " & dt.Rows.Count.ToString)
            End If

            For Each dr In dt.Rows
                flag = False
                sScheduleMonth = ""
                iOrder = 0
                iWeekday = 0

                Select Case dr("ScheduleType")
                    Case "1"
                        flag = True
                    Case "2"
                        iDay = DatePart(DateInterval.Weekday, Today, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

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
                        iDay = DatePart(DateInterval.Day, Today.Date, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

                        iMonth = DatePart(DateInterval.Month, Today.Date, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

                        If IsDBNull(dr("ScheduleDay")) Then
                            sScheduleday = ""
                        Else
                            sScheduleday = dr("ScheduleDay")
                        End If

                        Dim iPos As Integer = InStr(1, sScheduleday, ";", 1)

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
                                If iWeekday = DatePart(DateInterval.Weekday, Today, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) Then
                                    If isLastWeekDay() Then
                                        flag = True
                                    End If
                                End If
                            Else
                                If iWeekday = DatePart(DateInterval.Weekday, Today, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) Then

                                    If iOrder = getWeekDayOrder() Then
                                        flag = True
                                    End If
                                End If
                            End If
                        End If

                End Select

                If giLoglevel > 0 Then
                    objLogFile.WriteLine(Now() & ", getCaseSolutionSchedule, ScheduleType:" & dr("ScheduleType").ToString & ", Scheduleday:" & sScheduleday & ", ScheduleMonth:" & sScheduleMonth & ", Order:" & iOrder.ToString & ", Day:" & iDay.ToString)
                End If

                If flag = True Then
                    c = New CCase

                    c.RegistrationSource = 4
                    c.RegLanguage_Id = dr("Language_Id")

                    If dr("ScheduleWatchDate") > 0 Then
                        c.WatchDate = DateAdd(DateInterval.Day, dr("ScheduleWatchDate"), Today)
                    End If

                    If Not IsDBNull(dr("Department_Id")) Then
                        c.Department_Id = dr("Department_Id")
                    End If

                    ' Kontrollera om användaruppgifter ska hämtas
                    If Not IsDBNull(dr("ReportedBy")) Then
                        Dim objComputerUser As ComputerUser = objComputerUserData.getComputerUserByUserId(dr("ReportedBy"), dr("Customer_Id"))

                        If Not objComputerUser Is Nothing Then
                            c.ReportedBy = objComputerUser.UserId
                            c.Persons_Name = objComputerUser.FirstName & " " & objComputerUser.SurName
                            c.Persons_EMail = objComputerUser.EMail
                            c.Persons_Phone = objComputerUser.Phone

                            If c.Department_Id = 0 Then
                                If objComputerUser.Department_Id <> 0 Then
                                    c.Department_Id = objComputerUser.Department_Id
                                End If
                            End If
                        End If

                    End If



                    If Not IsDBNull(dr("Region_Id")) Then
                        c.Region_Id = dr("Region_Id")
                    End If

                    If Not IsDBNull(dr("CaseType_Id")) Then
                        c.CaseType_Id = dr("CaseType_Id")
                    End If

                    If Not IsDBNull(dr("Category_Id")) Then
                        c.Category_Id = dr("Category_Id")
                    End If

                    c.Customer_Id = dr("Customer_Id")
                    c.Caption = dr("Caption")

                    If Not IsDBNull(dr("Description")) Then
                        c.Description = dr("Description")
                    End If

                    If Not IsDBNull(dr("Miscellaneous")) Then
                        c.Miscellaneous = dr("Miscellaneous")
                    End If

                    If Not IsDBNull(dr("Priority_Id")) Then
                        c.Priority_Id = dr("Priority_Id")
                    End If

                    If Not IsDBNull(dr("PerformerUser_Id")) Then
                        c.Performer_User_Id = dr("PerformerUser_Id")
                    End If

                    logAdded = False
                    Dim l As New Log

                    If Not IsDBNull(dr("Text_Internal")) Then
                        If dr("Text_Internal") <> "" Then
                            l.Text_Internal = dr("Text_Internal")

                            logAdded = True

                        End If

                    End If

                    If Not IsDBNull(dr("Text_External")) Then
                        If dr("Text_External") <> "" Then
                            l.Text_External = dr("Text_External")

                            logAdded = True

                        End If

                    End If

                    If logAdded = True Then
                        c.Log.Add(l)
                    End If

                    colCase.Add(c)
                End If

            Next

            Return colCase
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getCaseSolutionSchedule " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Function

    Public Sub CaseCleanUp()
        Dim sSQL As String
        Dim dr As DataRow
        Dim drCase As DataRow
        Dim sFilePath As String = gsAttachedFileFolder

        Try
            sSQL = "SELECT tblCaseCleanUp.*, tblSettings.PhysicalFilePath " &
                    "FROM tblCaseCleanUp " &
                        "INNER JOIN tblSettings ON tblCaseCleanUp.Customer_Id=tblSettings.Customer_Id " &
                    "WHERE (" & Call4DateFormat("tblCaseCleanUp.ScheduleDate", giDBType) & " = " & convertDateTime(Today, giDBType) & ")"


            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", CaseCleanUp " & sSQL)
            End If

            Dim dt As DataTable

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If


            For Each dr In dt.Rows
                If IsDBNull(dr("PhysicalFilePath")) Then
                    sFilePath = gsAttachedFileFolder
                Else
                    sFilePath = dr("PhysicalFilePath")
                End If

                ' Hämta ärenden
                sSQL = "SELECT tblCase.Id, tblCase.CaseNumber  " &
                        "FROM tblCase " &
                        "WHERE tblCase.Customer_Id=" & dr("Customer_Id")

                sSQL = sSQL & " AND FinishingDate IS NOT NULL "

                If IsDate(dr("RegTimeFrom")) Then
                    sSQL = sSQL & " AND (" & Call4DateFormat("tblCase.RegTime", giDBType) & " >= " & convertDateTime(dr("RegTimeFrom"), giDBType) & ")"
                End If

                If IsDate(dr("RegTimeUntil")) Then
                    sSQL = sSQL & " AND (" & Call4DateFormat("tblCase.RegTime", giDBType) & " <= " & convertDateTime(dr("RegTimeUntil"), giDBType) & ") "
                End If

                If dr("ProductArea_Id") <> "" Then
                    sSQL = sSQL & " AND (tblCase.ProductArea_Id IN (" & dr("ProductArea_Id") & ")) "
                End If

                If dr("FinishingCause_Id") <> "" Then
                    sSQL = sSQL & " AND tblCase.Id IN (SELECT Case_Id FROM tblLog WHERE FinishingType IN (" & dr("FinishingCause_Id") & "))"
                End If

                If dr("Exclude_Status_Id") <> "" Then
                    sSQL = sSQL & " AND tblCase.Id NOT IN (SELECT Case_Id FROM tblCaseHistory WHERE Status_Id IN (" & dr("Exclude_Status_Id") & "))"
                End If


                Dim dtCase As DataTable

                'If giDBType = 0 Then
                dtCase = getDataTable(gsConnectionString, sSQL)
                'Else
                '    dtCase = getDataTableOracle(gsConnectionString, sSQL)
                'End If

                For Each drCase In dtCase.Rows

                    clearCase(drCase("Id"), dr("Id"), drCase("CaseNumber"), sFilePath)

                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", CaseCleanUp, Case: " & drCase("CaseNumber"))
                    End If
                Next

                closeCaseCleanUp(dr("Id"))
            Next
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR CaseCleanUp " & ex.Message.ToString)
            End If

            Throw ex
        End Try

    End Sub

    Private Sub clearCase(ByVal iCase_Id As Integer, ByVal iCaseCleanUp_Id As Integer, ByVal sCaseNumber As String, ByVal sFilePath As String)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblLog SET " &
                        "Text_Internal='-', " &
                        "RegUser='-', " &
                        "User_Id=Null " &
                    "WHERE Case_Id=" & iCase_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

            sSQL = "DELETE FROM tblCaseFile WHERE Case_Id=" & iCase_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

            If System.IO.Directory.Exists(sFilePath & "\" & sCaseNumber) Then
                Try
                    System.IO.Directory.Delete(sFilePath & "\" & sCaseNumber, True)
                Catch ex As Exception

                End Try
            End If

            sSQL = "UPDATE tblEmailLog SET " &
                        "EMailAddress='-' " &
                    " WHERE tblEmailLog.CaseHistory_Id IN (SELECT Id FROM tblCaseHistory WHERE Case_Id=" & iCase_Id & ")"

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

            sSQL = "UPDATE tblCaseHistory SET " &
                        "ReportedBy='-', " &
                        "Persons_name='-', " &
                        "Persons_phone='-', " &
                        "Persons_EMail='-', " &
                        "UserCode='-', " &
                        "Caption='-', " &
                        "CreatedByUser='-', " &
                        "User_Id=Null " &
                    " WHERE tblCaseHistory.Case_Id=" & iCase_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If

            sSQL = "UPDATE tblCase SET " &
                        "ReportedBy='-', " &
                        "Persons_name='-', " &
                        "Persons_phone='-', " &
                        "UserCode='-', " &
                        "Caption='-', " &
                        "User_Id=Null, " &
                        "CaseCleanUp_Id=" & iCaseCleanUp_Id &
                    " WHERE tblCase.Id=" & iCase_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub closeCaseCleanUp(ByVal iCaseCleanUp_Id As Integer)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblCaseCleanUp SET " &
                        "RunDate=getutcdate() " &
                    " WHERE tblCaseCleanUp.Id=" & iCaseCleanUp_Id

            'If giDBType = 0 Then
            executeSQL(gsConnectionString, sSQL)
            'Else
            '    executeSQLOracle(gsConnectionString, sSQL)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function LeadTimeMinutes(ByVal Startdate As DateTime, ByVal Enddate As DateTime, ByVal WorkingDayStart As Integer, ByVal WorkingDayEnd As Integer, ByVal HolidayHeader_Id As Integer) As Integer
        Dim iLeadTime As Integer = 0
        Dim iLeadTimeMinutes As Integer = 0

        Dim StartdateTemp As DateTime
        Dim EndDateTemp As DateTime

        Dim hd As New HolidayData
        Dim sHoliday As String = ""
        Dim colHolidays As Collection
        Dim objHoliday As Holiday

        colHolidays = hd.getHolidays

        For i As Integer = 1 To colHolidays.Count
            objHoliday = colHolidays(i)
            If objHoliday.HolidayHeader_Id = HolidayHeader_Id Then
                If sHoliday <> "" Then
                    sHoliday = sHoliday & "|"
                End If

                sHoliday = sHoliday & objHoliday.Holiday
            End If
        Next

        Dim days As Integer = ((Enddate.Date - Startdate.Date)).Days

        ' Räkna ut antalet hela dagar
        If days > 1 Then

            StartdateTemp = Startdate

            ' Räkna ut antalet hela dagar
            For i As Integer = 1 To days - 1

                StartdateTemp = StartdateTemp.AddDays(1)

                If isHoliday(sHoliday, StartdateTemp) = "" Then

                    If (StartdateTemp.DayOfWeek > 0 And StartdateTemp.DayOfWeek < 6) Then
                        iLeadTime = iLeadTime + 1
                    End If

                End If

            Next

            iLeadTime = (iLeadTime * (WorkingDayEnd - WorkingDayStart)) * 60
        End If

        ' Räkna antalet minuter dag 1
        If isHoliday(sHoliday, Startdate) = "" Then
            If (Startdate.TimeOfDay.Hours >= WorkingDayStart) Then
                StartdateTemp = Startdate

            Else
                StartdateTemp = Startdate.Date.AddHours(WorkingDayStart)
            End If

            If (Startdate.Date <> Enddate.Date) Then
                If (WorkingDayEnd = 24) Then
                    EndDateTemp = Startdate.Date.AddDays(1)
                Else
                    EndDateTemp = Startdate.Date.AddHours(WorkingDayEnd)
                End If

            Else
                If Enddate < Enddate.Date.AddHours(WorkingDayEnd) Then
                    EndDateTemp = Enddate

                Else
                    EndDateTemp = Enddate.Date.AddHours(WorkingDayEnd)
                End If
            End If

            If (StartdateTemp.DayOfWeek > 0 And StartdateTemp.DayOfWeek < 6) Then

                iLeadTimeMinutes = ((EndDateTemp - StartdateTemp)).Minutes + (((EndDateTemp - StartdateTemp)).Hours * 60)
            End If

        End If

        If iLeadTimeMinutes < 0 Then
            iLeadTimeMinutes = 0
        End If

        iLeadTime = iLeadTime + iLeadTimeMinutes

        ' Räkna antalet minuter avslutsdagen
        If (Startdate.Date <> Enddate.Date) Then

            iLeadTimeMinutes = 0

            If isHoliday(sHoliday, Enddate) = "" Then

                StartdateTemp = Enddate.Date.AddHours(WorkingDayStart)

                If (Enddate < Enddate.Date.AddHours(WorkingDayEnd)) Then
                    EndDateTemp = Enddate
                Else
                    EndDateTemp = Enddate.Date.AddHours(WorkingDayEnd)
                End If

                If (StartdateTemp.DayOfWeek > 0 And StartdateTemp.DayOfWeek < 6) Then
                    iLeadTimeMinutes = ((EndDateTemp - StartdateTemp)).Minutes + (((EndDateTemp - StartdateTemp)).Hours * 60)
                End If

            End If

            If iLeadTimeMinutes < 0 Then
                iLeadTimeMinutes = 0
            End If

            iLeadTime = iLeadTime + iLeadTimeMinutes
        End If

        Return iLeadTime

    End Function


    Private Function GetDate(dtDate As Date) As String
        GetDate = dtDate.Year.ToString & "-" & dtDate.Month.ToString.PadLeft(2, "0") & "-" & dtDate.Day.ToString.PadLeft(2, "0")
    End Function


    Private Function isHoliday(sHolidays As String, sDate As Date)
        Dim iPos As Integer
        Dim sTemp As String

        isHoliday = ""

        If sHolidays <> "" Then
            iPos = InStr(1, sHolidays, sDate, 1)
            If iPos > 0 Then
                ' Kontrollera om del av dag
                sTemp = Mid(sHolidays, iPos + 11, 5)

                If sTemp = "00-00" Then
                    isHoliday = sDate
                Else
                    isHoliday = sTemp
                End If
            End If
        End If
    End Function

    Private Function getLeadTimeHours(TimeFrom As Integer, TimeUntil As Integer, WorkingDayStart As Integer, WorkingDayEnd As Integer) As Integer
        Dim iTemp As Integer = 0

        For i As Integer = TimeFrom + 1 To TimeUntil
            If i > WorkingDayStart And i <= WorkingDayEnd Then
                iTemp = iTemp + 1
            End If
        Next

        getLeadTimeHours = iTemp
    End Function

    Private Function getLocalTime(UTCDate As DateTime, iTimeZone_offset As Integer) As DateTime
        If iTimeZone_offset <> 0 Then
            getLocalTime = DateAdd(DateInterval.Minute, iTimeZone_offset, UTCDate)
        Else
            getLocalTime = UTCDate
        End If

    End Function
End Class
