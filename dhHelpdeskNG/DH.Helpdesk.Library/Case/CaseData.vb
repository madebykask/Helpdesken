Imports System.Data.SqlClient
Imports System.Linq
Imports DH.Helpdesk.BusinessData.Models.WorktimeCalculator
Imports DH.Helpdesk.Dal.Infrastructure
Imports DH.Helpdesk.Dal.Repositories
Imports DH.Helpdesk.Library.SharedFunctions
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

    Public Function getCasesByCustomer(ByVal iCustomer_Id As Integer) As Collection
        Try
            Return getCases(iCustomer_Id:=iCustomer_Id)
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

            executeSQL(gsConnectionString, sSQL)

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

            If (String.IsNullOrEmpty(u.UserId)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(u.UserId, "'", ""), 40) & "', "
            End If
            If (String.IsNullOrEmpty(u.FirstName) And String.IsNullOrEmpty(u.SurName)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(If(u.FirstName, "") & If(u.SurName, ""), "'", ""), 50) & "', "
            End If
            If (String.IsNullOrEmpty(u.EMail)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(u.EMail, "'", "''"), 100) & "', "
            End If
            If (String.IsNullOrEmpty(u.Phone)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Replace(Left(u.Phone, 40), "'", "''") & "', "
            End If
            If (String.IsNullOrEmpty(u.CellPhone)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(u.CellPhone, 30) & "', "
            End If
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
            If (String.IsNullOrEmpty(u.CostCentre)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(u.CostCentre, 50) & "', "
            End If
            If (String.IsNullOrEmpty(u.Location)) Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(u.Location, 100) & "', "
            End If
            If (String.IsNullOrEmpty(u.UserCode)) Then
                sSQL = sSQL & "Null "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & Left(u.UserCode, 50) & "' "
            End If
            sSQL = sSQL & ")"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", saveCaseIsAbout: " & sSQL)
            End If

            executeSQL(gsConnectionString, sSQL)

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
            sSQL = $"UPDATE tblCase SET ApprovedDate='{getDateAsSqlString(workTime.Now)}', ChangeTime='{getDateAsSqlString(workTime.Now)}', ExternalTime={workTime.ExternalTime}, LeadTime={workTime.LeadTime} WHERE Id={objCase.Id}"

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
                            $"ChangeTime='{getDateAsSqlString(workTime.Now)}', " &
                            $"ExternalTime={workTime.ExternalTime}, " &
                            $"LeadTime={workTime.LeadTime}"

            If iOpenCase_StateSecondary_Id <> 0 Then
                sSQL = sSQL & ", StateSecondary_Id=" & iOpenCase_StateSecondary_Id
            End If

            sSQL = sSQL & " WHERE Id=" & objCase.Id

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Public Sub markCaseUnread(ByVal objCase As CCase)
        Dim sSQL As String

        Try
            Dim workTime As CaseWorkTime = calculateWorkTimeOnChange(objCase)

            sSQL = $"UPDATE tblCase 
                     SET Status=1, 
                         ChangeTime = @changeTime, 
                         ExternalTime = @externalTime, 
                         LeadTime = @leadTime
                     WHERE Id = @caseId"

            Dim parameters As New List(Of SqlParameter) From {
                DbHelper.createDbParameter("@caseId", objCase.Id),
                DbHelper.createDbParameter("@changeTime", workTime.Now),
                DbHelper.createDbParameter("@externalTime", workTime.ExternalTime),
                DbHelper.createDbParameter("@leadTime", workTime.LeadTime)
            }

            DbHelper.executeNonQuery(gsConnectionString, sSQL, CommandType.Text, parameters.ToArray())

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub closeCase(ByVal objCase As CCase)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblCase SET FinishingDate=getutcdate() WHERE Id=" & objCase.Id

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub updateCaseDescription(ByVal objBody As String, ByVal id As Integer)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblCase SET Description='" & objBody & "' where Id=" & id

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub clearFileViewLog()
        Dim sSQL As String

        Try
            sSQL = "DELETE FROM tblFileViewLog WHERE " & Call4DateFormat("CreatedDate", giDBType) & " < " & convertDateTime(DateAdd(DateInterval.Year, -1, Now.Date), giDBType)

            executeSQL(gsConnectionString, sSQL)

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

            sSQL = $"UPDATE tblCase SET StateSecondary_Id=Null, ChangeTime='{getDateAsSqlString(workTime.Now)}', ExternalTime={workTime.ExternalTime}, LeadTime={workTime.LeadTime} WHERE tblCase.Id={objCase.Id}"

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function getDateAsSqlString(ByVal d As DateTime) As String
        Return $"{d:yyyy-MM-dd HH:mm:ss}"
    End Function

    Public Function createCase(ByVal objCase As CCase) As CCase
        Dim sSQL As String = ""
        'Dim sDescription As String

        Try

            ' Skapa nytt ärende
            sSQL = "INSERT INTO tblCase(CaseGUID, ExternalCaseNumber, CaseType_Id, Customer_Id, ProductArea_Id,Category_Id, Region_Id, ReportedBy, Department_Id, OU_Id, " &
                            "Project_Id, System_Id, Urgency_Id, Impact_Id, Supplier_Id, SMS, Cost, OtherCost, Problem_Id, Change_Id, CausingPartId, Verified, VerifiedDescription, SolutionRate," &
                            "InventoryType, InventoryLocation, Currency, ContactBeforeAction, FinishingDescription, " &
                            "Persons_Name, Persons_EMail, Persons_Phone, Persons_CellPhone, Place, UserCode, CostCentre, InventoryNumber, InvoiceNumber, " &
                            "Caption, Description, Miscellaneous, Available, ReferenceNumber, Priority_Id, WorkingGroup_Id, Performer_User_Id, Status_Id, StateSecondary_Id, " &
                            "WatchDate, PlanDate, AgreedDate, FinishingDate, RegistrationSource, RegLanguage_Id, RegistrationSourceCustomer_Id, RegUserName, CaseSolution_Id, RegTime, ChangeTime) " &
                        "VALUES(" &
                            getDBStringPrefix() & "'" & objCase.CaseGUID & "', " &
                            getDBStringPrefix() & "'" & objCase.ExternalCasenumber & "', " &
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
            'Check this
            If Not IsNullOrEmpty(objCase.ReportedBy) Then
                sSQL = sSQL & getDBStringPrefix() & "'" & objCase.ReportedBy.Replace("'", "''") & "', "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & objCase.ReportedBy & "', "
            End If


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

            If objCase.Project_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Project_Id & ", "
            End If

            If objCase.System_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.System_Id & ", "
            End If

            If objCase.Urgency_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Urgency_Id & ", "
            End If

            If objCase.Impact_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Impact_Id & ", "
            End If

            If objCase.Supplier_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Supplier_Id & ", "
            End If

            If objCase.Sms = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Sms & ", "
            End If

            If objCase.Cost = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Cost & ", "
            End If

            If objCase.OtherCost = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.OtherCost & ", "
            End If

            If objCase.Problem_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Problem_Id & ", "
            End If

            If objCase.Change_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Change_Id & ", "
            End If

            If objCase.CausingPartId = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.CausingPartId & ", "
            End If

            If objCase.Verified = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Verified & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.VerifiedDescription, "'", ""), 200) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.SolutionRate, "'", ""), 10) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.InventoryType, "'", ""), 50) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.InventoryLocation, "'", ""), 100) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Currency, "'", ""), 10) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.ContactBeforeAction, "'", ""), 100) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.FinishingDescription, "'", ""), 200) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Persons_Name, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Persons_EMail, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(Left(objCase.Persons_Phone, 40), "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Persons_CellPhone, "'", "''"), 30) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Place, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.UserCode, "'", "''"), 20) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.CostCentre, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InventoryNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InvoiceNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Caption, "'", "") & "', " &
                            getDBStringPrefix() & "'" & ReplaceSingleApostrophe(objCase.Description) & "', " &
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

            If objCase.PlanDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.PlanDate, giDBType) & ", "
            End If

            If objCase.AgreedDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.AgreedDate, giDBType) & ", "
            End If

            If objCase.FinishingDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.FinishingDate, giDBType) & ", "
            End If

            sSQL = sSQL & objCase.RegistrationSource & ", " & objCase.RegLanguage_Id & ", "

            If objCase.RegistrationSourceCustomer_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.RegistrationSourceCustomer_Id & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.RegUserName, "'", "''"), 200) & "', "

            If objCase.CaseSolution_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.CaseSolution_Id & ", "
            End If

            sSQL = sSQL & "getutcdate(), getutcdate())"

            executeSQL(gsConnectionString, sSQL)

            Dim newCase As CCase = getCaseById(sCaseGUID:=objCase.CaseGUID)

            If objCase.Form_Id <> 0 Then
                sSQL = "INSERT INTO tblFormFieldValue(Case_Id, FormField_Id, FormFieldValue) " &
                        "SELECT " & newCase.Id & ", Id, ' ' FROM tblFormField WHERE Form_Id=" & objCase.Form_Id

                executeSQL(gsConnectionString, sSQL)

            End If

            Return newCase
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createCase " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex

        End Try
    End Function
    Public Function createCaseFromCaseSolutionScedule(ByVal objCase As CCase) As CCase
        Dim sSQL As String = ""
        'Dim sDescription As String

        Try
            ' Skapa nytt ärende
            sSQL = "INSERT INTO tblCase(CaseGUID, ExternalCaseNumber, CaseType_Id, Customer_Id, ProductArea_Id,Category_Id, Region_Id, ReportedBy, Department_Id, OU_Id, " &
                            "Project_Id, System_Id, Urgency_Id, Impact_Id, Supplier_Id, SMS, Cost, OtherCost, Problem_Id, Change_Id, CausingPartId, Verified, VerifiedDescription, SolutionRate," &
                            "InventoryType, InventoryLocation, Currency, ContactBeforeAction, FinishingDescription, " &
                            "Persons_Name, Persons_EMail, Persons_Phone, Persons_CellPhone, Place, UserCode, CostCentre, InventoryNumber, InvoiceNumber, " &
                            "Caption, Description, Miscellaneous, Available, ReferenceNumber, Priority_Id, WorkingGroup_Id, Performer_User_Id, Status_Id, StateSecondary_Id, " &
                            "WatchDate, PlanDate, AgreedDate, FinishingDate, RegistrationSource, RegLanguage_Id, RegistrationSourceCustomer_Id, RegUserName, CaseSolution_Id, RegTime, ChangeTime) " &
                        "VALUES(" &
                            getDBStringPrefix() & "'" & objCase.CaseGUID & "', " &
                            getDBStringPrefix() & "'" & objCase.ExternalCasenumber & "', " &
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
            'Check this
            If Not IsNullOrEmpty(objCase.ReportedBy) Then
                sSQL = sSQL & getDBStringPrefix() & "'" & objCase.ReportedBy.Replace("'", "''") & "', "
            Else
                sSQL = sSQL & getDBStringPrefix() & "'" & objCase.ReportedBy & "', "
            End If


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

            If objCase.Project_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Project_Id & ", "
            End If

            If objCase.System_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.System_Id & ", "
            End If

            If objCase.Urgency_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Urgency_Id & ", "
            End If

            If objCase.Impact_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Impact_Id & ", "
            End If

            If objCase.Supplier_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Supplier_Id & ", "
            End If

            If objCase.Sms = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Sms & ", "
            End If

            If objCase.Cost = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Cost & ", "
            End If

            If objCase.OtherCost = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.OtherCost & ", "
            End If

            If objCase.Problem_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Problem_Id & ", "
            End If

            If objCase.Change_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.Change_Id & ", "
            End If

            If objCase.CausingPartId = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.CausingPartId & ", "
            End If

            If objCase.Verified = 0 Then
                sSQL = sSQL & "0, "
            Else
                sSQL = sSQL & objCase.Verified & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.VerifiedDescription, "'", ""), 200) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.SolutionRate, "'", ""), 10) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.InventoryType, "'", ""), 50) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.InventoryLocation, "'", ""), 100) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Currency, "'", ""), 10) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.ContactBeforeAction, "'", ""), 100) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.FinishingDescription, "'", ""), 200) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Persons_Name, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Persons_EMail, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(Left(objCase.Persons_Phone, 40), "'", "''") & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Persons_CellPhone, "'", "''"), 30) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.Place, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.UserCode, "'", "''"), 20) & "', " &
                            getDBStringPrefix() & "'" & Left(Replace(objCase.CostCentre, "'", "''"), 50) & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InventoryNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.InvoiceNumber, "'", "") & "', " &
                            getDBStringPrefix() & "'" & Replace(objCase.Caption, "'", "") & "', " &
                            getDBStringPrefix() & "'" & ReplaceSingleApostrophe(objCase.Description) & "', " &
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

            If objCase.PlanDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.PlanDate, giDBType) & ", "
            End If

            If objCase.AgreedDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.AgreedDate, giDBType) & ", "
            End If

            If objCase.FinishingDate = Date.MinValue Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & convertDateTime(objCase.FinishingDate, giDBType) & ", "
            End If

            sSQL = sSQL & objCase.RegistrationSource & ", " & objCase.RegLanguage_Id & ", "

            If objCase.RegistrationSourceCustomer_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.RegistrationSourceCustomer_Id & ", "
            End If

            sSQL = sSQL & getDBStringPrefix() & "'" & Left(Replace(objCase.RegUserName, "'", "''"), 200) & "', "

            If objCase.CaseSolution_Id = 0 Then
                sSQL = sSQL & "Null, "
            Else
                sSQL = sSQL & objCase.CaseSolution_Id & ", "
            End If

            sSQL = sSQL & "getutcdate(), getutcdate())"

            executeSQL(gsConnectionString, sSQL)

            Dim newCase As CCase = getCaseById(sCaseGUID:=objCase.CaseGUID)

            If objCase.Form_Id <> 0 Then
                sSQL = "INSERT INTO tblFormFieldValue(Case_Id, FormField_Id, FormFieldValue) " &
                        "SELECT " & newCase.Id & ", Id, ' ' FROM tblFormField WHERE Form_Id=" & objCase.Form_Id

                executeSQL(gsConnectionString, sSQL)

            End If

            Return newCase
        Catch ex As Exception
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

    Public Function GetCaseByExternalCaseNumber(ByVal sExternalCaseNumber As String) As CCase
        Try
            Return getCaseById(sExternalCaseNumber:=sExternalCaseNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseReminder() As Collection
        Try

            Dim tempcol As Collection = getCases(iReminder:=1)
            Dim col As New Collection

            For Each c As CCase In tempcol
                ' Check when StateSecondary First Set
                Dim dt As DateTime? = getStateSecondarySetDate(c.StateSecondary_Id, c.Id)

                Dim numberOfDays As Integer = (DateTime.Now - dt.Value).TotalDays

                If numberOfDays > 0 Then
                    Dim commonDifference As Integer = c.ReminderDays

                    ' Check if value1 is included in the series
                    Dim isInSeries As Boolean = (numberOfDays Mod commonDifference) = 0

                    If isInSeries Then
                        col.Add(c)
                    End If

                End If

            Next

            Return col

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getCaseAutoClose() As Collection
        Try
            Dim tempcol As Collection = getCases(iAutoClose:=1)
            Dim col As New Collection

            For Each c As CCase In tempcol
                ' Check when StateSecondary First Set
                Dim dt As DateTime? = getStateSecondarySetDate(c.StateSecondary_Id, c.Id)
                'How many days passed since the state secondary was set
                Dim numberOfDays As Integer = (DateTime.Now - dt.Value).TotalDays
                'if the number of days is greater than 0 
                If numberOfDays > 0 Then
                    'if the number of days is equal to or greater than auto close days
                    If numberOfDays >= c.AutoCloseDays Then
                        col.Add(c)
                    End If
                End If
            Next

            Return col
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getCaseById(Optional iCaseId As Integer = 0,
                             Optional ByVal sCaseGUID As String = "",
                             Optional ByVal sMessageId As String = "",
                             Optional ByVal iCaseNumber As Integer = 0,
                             Optional ByVal sOrderMessageId As String = "",
                             Optional ByVal sExternalCaseNumber As String = "") As CCase

        Dim sSQL As String = "SELECT tblCase.Id, tblCase.CaseGUID, tblCase.CaseNumber, tblCase.ExternalCaseNumber, tblCase.Customer_Id, tblCase.CaseType_Id, tblCaseType.CaseType, tblCase.ProductArea_Id, tblCase.Category_Id, tblCategory.Category, tblProductArea.ProductArea, " &
                       "tblCase.Priority_Id, tblCase.Region_Id, tblCase.Department_Id, tblCase.OU_Id, tblCustomer.Name AS CustomerName, tblCase.Performer_User_Id, tblCase.RegLanguage_Id, " &
                       "tblCase.Project_Id, tblCase.System_Id, tblCase.Urgency_Id, tblCase.Impact_Id, tblCase.Supplier_Id, tblCase.SMS, tblCase.VerifiedDescription, tblCase.SolutionRate, " &
                       "tblCase.InventoryType, tblCase.InventoryLocation, tblCase.Cost, tblCase.OtherCost, tblCase.Currency, tblCase.ContactBeforeAction, tblCase.Change_Id, tblCase.Problem_Id, " &
                       "tblCase.ReportedBy, tblCase.Persons_Name, tblCase.InvoiceNumber, tblCase.Caption, tblCase.Description, tblCase.Miscellaneous, tblUsers.FirstName AS PerformerFirstName, tblUsers.SurName AS PerformerSurName, tblUsers.EMail AS PerformerEMail, " &
                       "tblUsers.Phone As PerformerPhone, tblUsers.Cellphone As PerformerCellPhone, " &
                       "u2.FirstName AS RegUserFirstName, u2.SurName AS RegUserSurName, tblCase.WorkingGroup_Id, tblCase.PlanDate, tblCase.CausingPartId, tblCase.AgreedDate, tblCase.Verified, " &
                       "tblCase.Persons_EMail, tblCase.Persons_Phone, tblCase.Place, tblCase.UserCode, tblCase.CostCentre, tblPriority.PriorityName, tblPriority.PriorityDescription, " &
                       "tblWorkingGroup.WorkingGroup AS PerformerWorkingGroup, tblWorkingGroup.Id AS PerformerWorkingGroup_Id, tblWorkingGroup.AllocateCaseMail AS PerformerWorkingGroupAllocateCaseMail, " &
                       "tblWorkingGroup_1.WorkingGroup AS CaseWorkingGroup, ISNULL(tblWorkingGroup_1.WorkingGroupEMail, '') AS WorkingGroupEMail, tblWorkingGroup_1.AllocateCaseMail AS AllocateCaseMail, " &
                       "tblCase.RegTime, tblCase.ChangeTime, u3.FirstName AS ChangedName, u3.SurName AS ChangedSurName, tblCase.InventoryNumber, tblCase.Persons_CellPhone, tblCaseType.AutomaticApproveTime, " &
                       "tblCase.CaseSolution_Id,tblCase.FinishingDate, tblCase.FinishingDescription, Isnull(tblUsers.ExternalUpdateMail, 0) AS ExternalUpdateMail, ISNULL(tblWorkingGroup.WorkingGroupEMail, '') AS PerformerWorkingGroupEMail, " &
                       "tblCase.StateSecondary_Id, tblStateSecondary.StateSecondary, tblStateSecondary.ResetOnExternalUpdate, tblDepartment.Department, tblCase.WatchDate, tblCase.RegistrationSource, tblCase.RegistrationSourceCustomer_Id, " &
                       "IsNull(tblDepartment.HolidayHeader_Id, 1) AS HolidayHeader_Id, tblCase.RegUserName, tblCase.Available, tblCase.ReferenceNumber, isnull(tblStateSecondary.IncludeInCaseStatistics, 1) AS IncludeInCaseStatistics, " &
                       "tblStateSecondary.FinishingCause_Id AS StateSecondary_FinishingCause_Id, tblStateSecondary.ReminderDays AS StateSecondary_ReminderDays, tblStateSecondary.AutoCloseDays AS StateSecondary_AutoCloseDays, tblCase.ExternalTime, tblCase.LeadTime, tblCase.Status, tblCase.MovedFromCustomer_Id " &
                   "FROM tblCase " &
                       "INNER JOIN tblCustomer ON tblCase.Customer_Id = tblCustomer.Id " &
                       "LEFT OUTER JOIN tblUsers ON tblCase.Performer_user_Id=tblUsers.Id " &
                       "LEFT JOIN tblUsers u2 ON tblCase.User_Id = u2.Id " &
                       "LEFT OUTER JOIN tblWorkingGroup ON tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id " &
                       "LEFT OUTER JOIN tblWorkingGroup tblWorkingGroup_1 ON tblCase.WorkingGroup_Id = tblWorkingGroup_1.Id " &
                       "LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id " &
                       "LEFT JOIN tblUsers u3 ON tblCase.ChangeByUser_Id = u3.Id " &
                       "INNER JOIN tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id " &
                       "LEFT JOIN tblStateSecondary ON tblCase.StateSecondary_Id=tblStateSecondary.Id " &
                       "LEFT JOIN tblCategory ON tblCase.Category_Id=tblCategory.Id " &
                       "LEFT JOIN tblProductArea ON tblCase.ProductArea_Id=tblProductArea.Id " &
                       "LEFT JOIN tblDepartment ON tblCase.Department_Id=tblDepartment.Id "

        Dim whereClause As String = ""
        Dim parameters As New List(Of SqlParameter)

        If iCaseId > 0 Then
            parameters.Add(New SqlParameter("@caseId", iCaseId))
            whereClause = "WHERE tblCase.Id = @caseId"

        ElseIf Not String.IsNullOrWhiteSpace(sCaseGUID) Then
            whereClause = "WHERE tblCase.CaseGUID = @caseGUID"
            parameters.Add(New SqlParameter("@caseGUID", sCaseGUID))

        ElseIf Not String.IsNullOrWhiteSpace(sMessageId) Then
            parameters.Add(New SqlParameter("@messageId", sMessageId))
            whereClause = "WHERE tblCase.Id IN (SELECT Case_Id FROM tblCaseHistory WHERE Id IN (SELECT CaseHistory_Id FROM tblEMailLog WHERE MessageId = @messageId))"

        ElseIf iCaseNumber > 0 Then
            parameters.Add(New SqlParameter("@caseNumber", iCaseNumber))
            whereClause = "WHERE tblCase.CaseNumber = @caseNumber"

        ElseIf Not String.IsNullOrWhiteSpace(sOrderMessageId) Then
            parameters.Add(New SqlParameter("@orderMessageId", sOrderMessageId))
            whereClause = "WHERE tblCase.Id IN (SELECT Id FROM tblCase WHERE CaseNumber IN (SELECT CaseNumber FROM tblOrder WHERE Id IN (SELECT Order_Id FROM tblOrderEmailLog WHERE MessageId = @orderMessageId)))"

        ElseIf Not String.IsNullOrWhiteSpace(sExternalCaseNumber) Then
            parameters.Add(New SqlParameter("@externalCaseNumber", sExternalCaseNumber))
            whereClause = "WHERE tblCase.ExternalCaseNumber = @externalCaseNumber"

        End If

        sSQL &= " " & whereClause

        Try
            Dim dt As New DataTable()

            Using conn As New SqlConnection(gsConnectionString)
                Using cmd As New SqlCommand(sSQL, conn)
                    cmd.Parameters.AddRange(parameters.ToArray())
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)   ' Läser in alla rader direkt
                    End Using
                End Using
            End Using

            If dt.Rows.Count > 0 Then
                Return New CCase(dt.Rows(0))
            Else
                Return Nothing
            End If


        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getCaseById " & ex.Message & ", SQL: " & sSQL)
            End If
            Throw
        End Try
    End Function


    Public Function getMailIDByMessageID(ByVal sMessageId As String) As Integer
        Dim sSQL As String = "SELECT MailId FROM tblEMailLog WHERE MessageId = @messageId"

        Try
            Using conn As New SqlConnection(gsConnectionString)
                Using cmd As New SqlCommand(sSQL, conn)
                    cmd.Parameters.AddWithValue("@messageId", sMessageId)
                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return Convert.ToInt32(reader("MailId"))
                        Else
                            Return 0
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getMailIDByMessageID " & ex.Message.ToString() & ", SQL: " & sSQL)
            End If
            Return 0
        End Try
    End Function


    Public Function checkIfCaseIsMerged(ByVal CaseId As Long) As Integer
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT TOP 1 tblMergedCases.MergedParent_Id " &
                   "FROM tblMergedCases " &
                   "WHERE MergedChild_Id=" & CaseId & ""


            dt = getDataTable(gsConnectionString, sSQL)


            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("MergedParent_Id")
            Else
                Return 0
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR checkIfCaseIsMerged " & ex.Message.ToString & ", " & sSQL)
            End If

            Return 0
        End Try
    End Function

    Public Function updateChangeTime(ByVal CaseId As Long) As Integer
        Dim sSQL As String = ""

        Try

            sSQL = "UPDATE tblCase SET ChangeTime = '" & Now.ToUniversalTime & "', Status=0 WHERE Id=" & CaseId & ""

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", Error updateChangeTime " & ex.Message.ToString & ", " & sSQL)
            End If

            Return 0
        End Try
    End Function

    Private Function getCases(Optional ByVal iPerformerUser_Id As Integer = 0, Optional ByVal iPlanDate As Integer = 0, Optional ByVal iApproval As Integer = 0, Optional ByVal iWatchdate As Integer = 0, Optional ByVal iReminder As Integer = 0, Optional ByVal iAutoClose As Integer = 0, Optional ByVal iCustomer_Id As Integer = 0) As Collection
        Dim colCase As New Collection
        Dim sSQL As String
        Dim dr As DataRow

        Try
            sSQL = "Select tblCase.Id, tblCase.CaseGUID, tblCase.CaseNumber, tblCase.ExternalCaseNumber, tblCase.Customer_Id, tblCase.CaseType_Id, tblCaseType.CaseType, tblCase.ProductArea_Id, " &
                        "tblCase.Category_Id, tblCategory.Category, tblProductArea.ProductArea, tblCase.Status, " &
                        "tblCase.Priority_Id, tblCase.Region_Id, tblCase.Department_Id, tblCase.OU_Id, tblCustomer.Name As CustomerName, tblCase.Performer_User_Id, tblCase.RegLanguage_Id, " &
                        "tblCase.ReportedBy, tblCase.Persons_Name, tblCase.InvoiceNumber, tblCase.Caption, tblCase.Description, tblCase.Miscellaneous, " &
                        "tblUsers.FirstName As PerformerFirstName, tblUsers.SurName As PerformerSurName, tblUsers.EMail As PerformerEMail, tblUsers.Phone As PerformerPhone, tblUsers.Cellphone As PerformerCellPhone, " &
                        "u2.FirstName As RegUserFirstName, u2.SurName As RegUserSurName, tblCase.WorkingGroup_Id," &
                        "tblCase.Persons_EMail, tblCase.Persons_Phone, tblCase.Place, tblCase.UserCode, tblCase.CostCentre, tblPriority.PriorityName,  tblPriority.PriorityDescription, " &
                        "tblCase.Project_Id, tblCase.System_Id, tblCase.Urgency_Id, tblCase.Impact_Id, tblCase.Supplier_Id, tblCase.SMS, tblCase.VerifiedDescription, " &
                        "tblCase.SolutionRate, tblCase.InventoryType, tblCase.InventoryLocation, tblCase.Cost, tblCase.OtherCost, tblCase.Currency, tblCase.ContactBeforeAction, " &
                        "tblCase.Change_Id,  tblCase.Problem_Id, tblCase.FinishingDescription, tblCase.PlanDate, tblCase.CausingPartId, tblCase.AgreedDate, tblCase.Verified, " &
                        "tblCase.RegistrationSourceCustomer_Id," &
                        "tblCase.RegTime, tblCase.ChangeTime, u3.FirstName As ChangedName, u3.SurName As ChangedSurName, tblCase.InventoryNumber, tblCase.Persons_CellPhone, tblCaseType.AutomaticApproveTime, " &
                        "tblWorkingGroup.WorkingGroup As PerformerWorkingGroup, tblWorkingGroup.Id As PerformerWorkingGroup_Id, tblWorkingGroup.AllocateCaseMail As PerformerWorkingGroupAllocateCaseMail, " &
                        "tblWorkingGroup_1.WorkingGroup As CaseWorkingGroup, ISNULL(tblWorkingGroup_1.WorkingGroupEMail, '') AS WorkingGroupEMail, tblWorkingGroup_1.AllocateCaseMail AS AllocateCaseMail, " &
                        "tblCase.CaseSolution_Id, tblCase.FinishingDate, Isnull(tblUsers.ExternalUpdateMail, 0) AS ExternalUpdateMail, ISNULL(tblWorkingGroup.WorkingGroupEMail, '') AS PerformerWorkingGroupEMail, " &
                        "tblCase.StateSecondary_Id, tblStateSecondary.StateSecondary, tblStateSecondary.ResetOnExternalUpdate, tblDepartment.Department, tblCase.RegistrationSource, tblCase.WatchDate, tblCase.Available, tblCase.ReferenceNumber, " &
                        "IsNull(tblDepartment.HolidayHeader_Id, 1) AS HolidayHeader_Id, tblCase.RegUserName, isnull(tblStateSecondary.IncludeInCaseStatistics, 1) AS IncludeInCaseStatistics, " &
                        "tblStateSecondary.FinishingCause_Id AS StateSecondary_FinishingCause_Id, tblStateSecondary.ReminderDays AS StateSecondary_ReminderDays, tblStateSecondary.AutoCloseDays AS StateSecondary_AutoCloseDays, tblCase.ExternalTime, tblCase.LeadTime, tblCase.MovedFromCustomer_Id " &
                    "FROM tblCase " &
                        "INNER JOIN tblCustomer ON tblCase.Customer_Id = tblCustomer.Id " &
                        "LEFT JOIN tblUsers ON tblCase.Performer_user_Id=tblUsers.Id " &
                        "LEFT JOIN tblUsers u2 ON tblCase.User_Id = u2.Id " &
                        "LEFT OUTER JOIN tblWorkingGroup ON tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id " &
                        "LEFT OUTER JOIN tblWorkingGroup tblWorkingGroup_1 ON tblCase.WorkingGroup_Id = tblWorkingGroup_1.Id " &
                        "LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id " &
                        "LEFT JOIN tblUsers u3 ON tblCase.ChangeByUser_Id = u3.Id " &
                        "INNER JOIN tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id " &
                        "LEFT JOIN tblStateSecondary ON tblCase.StateSecondary_Id=tblStateSecondary.Id " &
                        "LEFT JOIN tblCategory ON tblCase.Category_Id=tblCategory.Id " &
                        "LEFT JOIN tblProductArea ON tblCase.ProductArea_Id=tblProductArea.Id " &
                        "LEFT JOIN tblDepartment ON tblCase.Department_Id=tblDepartment.Id "

            If iCustomer_Id <> 0 Then
                sSQL = sSQL & "WHERE tblCase.FinishingDate Is NULL And tblCase.Deleted=0 and tblCase.Customer_Id=" & iCustomer_Id
            ElseIf iPerformerUser_Id <> 0 Then
                sSQL = sSQL & "WHERE tblCase.FinishingDate Is NULL And tblCase.Performer_user_Id=" & iPerformerUser_Id
            ElseIf iPlanDate <> 0 Then
                sSQL = sSQL & "WHERE " & Call4DateFormat("PlanDate", giDBType) & " = " & convertDateTime(Now.Date, giDBType) &
                        " And tblUsers.PlanDateMail = 1 " &
                        " And tblCase.FinishingDate Is NULL"
            ElseIf iWatchdate <> 0 Then
                sSQL = sSQL & "WHERE " & Call4DateFormat("WatchDate", giDBType) & " = " & convertDateTime(Now.Date, giDBType) & " And tblCase.FinishingDate Is NULL And tblUsers.WatchDateMail = 1 "
            ElseIf iApproval = 1 Then
                sSQL = sSQL & "WHERE tblCaseType.AutomaticApproveTime <> 0 And tblCase.ApprovedDate Is NULL And tblCase.FinishingDate Is Not NULL "
            ElseIf iReminder = 1 Then
                sSQL = sSQL & "WHERE tblStateSecondary.ReminderDays <> 0 And tblCase.FinishingDate Is NULL "

                'sSQL = sSQL & "And tblCase.Id IN (SELECT tblcasehistory.Case_Id FROM tblCasehistory INNER JOIN tblStateSecondary ON tblCaseHistory.StateSecondary_Id=tblstatesecondary.Id WHERE tblStateSecondary.Reminderdays <> 0 And Datediff(d, tblcasehistory.CreatedDate, getdate()) = tblstatesecondary.Reminderdays)"
            ElseIf iAutoClose = 1 Then
                sSQL = sSQL & "WHERE tblStateSecondary.AutocloseDays <> 0 And tblCase.FinishingDate Is NULL "

                'sSQL = sSQL & "And tblCase.Id IN (SELECT tblcasehistory.Case_Id FROM tblCasehistory INNER JOIN tblStateSecondary ON tblCaseHistory.StateSecondary_Id=tblstatesecondary.Id WHERE tblStateSecondary.AutocloseDays <> 0 And Datediff(d, tblcasehistory.CreatedDate, getdate()) = tblstatesecondary.AutocloseDays)"

            End If

            sSQL = sSQL & " ORDER BY tblCustomer.Name, tblCase.CaseNumber"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCases, " & sSQL)
            End If

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

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

            sSQL = "INSERT INTO tblCaseHistory(CaseHistoryGUID, Case_Id, ReportedBy, Persons_Name, Persons_EMail, Persons_Phone, Persons_CellPhone, Customer_Id, Region_Id, Department_Id, OU_Id, Place, UserCode, InventoryNumber, InventoryType," &
                          "InventoryLocation, Casenumber, ExternalCaseNumber, User_Id, IPAddress, CaseType_Id, ProductArea_Id, ProductAreaSetDate, System_Id, Urgency_Id, Impact_Id, Category_Id, Supplier_Id, InvoiceNumber, ReferenceNumber, Caption, Description," &
                          "Miscellaneous, ContactBeforeAction, SMS, AgreedDate, Available, Cost, OtherCost, Currency, Performer_User_Id, CaseResponsibleUser_Id, Priority_Id, Status_Id, StateSecondary_Id, ExternalTime, Project_Id," &
                          "ProjectSchedule_Id, Verified, VerifiedDescription, SolutionRate, PlanDate, ApprovedDate, ApprovedBy_User_Id, WatchDate, LockCaseToWorkingGroup_Id, WorkingGroup_Id, FinishingDate, FinishingDescription, FollowUpDate," &
                          "RegistrationSource, RelatedCaseNumber, Problem_Id, Change_Id, Deleted, Status, RegLanguage_Id, RegUserId, RegUserDomain, ProductAreaQuestionVersion_Id, LeadTime, CreatedDate, CreatedByUser, CausingPartId, " &
                          "DefaultOwnerWG_Id, RegistrationSourceCustomer_Id, CostCentre, IsAbout_Persons_Name, IsAbout_ReportedBy, IsAbout_Persons_Phone, IsAbout_UserCode," &
                          "IsAbout_Department_Id, CreatedByApp, LatestSLACountDate, IsAbout_Persons_EMail, IsAbout_Persons_CellPhone, IsAbout_Region_Id, IsAbout_OU_Id, " &
                          "IsAbout_CostCentre, IsAbout_Place) " &
                       "Select top 1  '" & sCaseHistoryGUID & "', " & iCase_Id & " , " &
                                "c.ReportedBy, c.Persons_Name, c.Persons_EMail, c.Persons_Phone, c.Persons_CellPhone, Customer_Id, c.Region_Id, c.Department_Id, c.OU_Id, c.Place, c.UserCode, InventoryNumber, InventoryType," &
                              "InventoryLocation, Casenumber, ExternalCaseNumber, User_Id, IPAddress, CaseType_Id, ProductArea_Id, ProductAreaSetDate, System_Id, Urgency_Id, Impact_Id, Category_Id, Supplier_Id, InvoiceNumber, ReferenceNumber,  Caption, Description," &
                              "Miscellaneous, ContactBeforeAction, SMS, AgreedDate, Available, c.Cost, OtherCost, Currency, Performer_User_Id, CaseResponsibleUser_Id, Priority_Id, Status_Id, StateSecondary_Id, ExternalTime, Project_Id, " &
                              "ProjectSchedule_Id, Verified, VerifiedDescription, SolutionRate, PlanDate, ApprovedDate, ApprovedBy_User_Id, WatchDate, LockCaseToWorkingGroup_Id, WorkingGroup_Id, FinishingDate, FinishingDescription, FollowUpDate, " &
                              "RegistrationSource, RelatedCaseNumber, Problem_Id, Change_Id, Deleted, Status, RegLanguage_Id, RegUserId, RegUserDomain, ProductAreaQuestionVersion_Id, LeadTime, getutcdate(),'" & Replace(sCreatedByUser, "'", "''") & "', CausingPartId, " &
                              "DefaultOwnerWG_Id, RegistrationSourceCustomer_Id, c.CostCentre, ca.Person_Name, ca.ReportedBy, ca.Person_Phone, ca.UserCode, " &
                              "ca.Department_Id, '', LatestSLACountDate, ca.Person_Email, ca.Person_CellPhone,ca.Region_Id, ca.OU_Id, " &
                              "ca.CostCentre, ca.Place " &
                       "From tblCase c " &
                       "LEFT JOIN tblCaseIsAbout ca ON c.Id = ca.Case_Id" &
                       " where c.Id = " & iCase_Id & " "

            executeSQL(gsConnectionString, sSQL)

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


            dt = getDataTable(gsConnectionString, sSQL)

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

    Public Function getCaseExtraFollowers(iCase_Id As Integer) As List(Of String)
        Dim sSQL As String
        Dim dr As DataRow
        Dim Followers As New List(Of String)()

        Try
            sSQL = "Select tblCaseExtraFollowers.Follower " &
                    "FROM tblCaseExtraFollowers " &
                    "WHERE tblCaseExtraFollowers.Case_Id=" & iCase_Id

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCaseExtraFollowers, " & sSQL)
            End If

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)

            For Each dr In dt.Rows
                Followers.Add(dr("Follower"))
            Next

            Return Followers
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function getCaseSolutionSchedule(ByVal dateAndTime As DateTime) As Collection
        Dim colCase As New Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim iTime As Integer = dateAndTime.Hour
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
                        "tblCustomer.Language_Id, tblDepartment.Region_Id, tblCaseSolution_ExtendedCaseForms.ExtendedCaseForms_Id " &
                      "FROM tblCaseSolution " &
                       "INNER JOIN tblCaseSolutionSchedule ON tblCaseSolution.Id = tblCaseSolutionSchedule.CaseSolution_Id " &
                       "INNER JOIN tblCustomer ON tblCaseSolution.Customer_Id=tblCustomer.Id " &
                        "LEFT JOIN tblDepartment ON tblCaseSolution.Department_Id=tblDepartment.Id " &
                        "LEFT JOIN tblCaseSolution_ExtendedCaseForms ON tblCaseSolution.Id=tblCaseSolution_ExtendedCaseForms.CaseSolution_Id " &
                      "WHERE ScheduleTime=" & iTime & " AND tblCaseSolution.Status = 1 AND tblCaseSolutionSchedule.RepeatType = 'Yearly'"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCaseSolutionSchedule " & sSQL)
            End If

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

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
                        iDay = DatePart(DateInterval.Weekday, dateAndTime, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

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

                        iDay = DatePart(DateInterval.Day, dateAndTime.Date, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

                        iMonth = DatePart(DateInterval.Month, dateAndTime.Date, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

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
                                If iWeekday = DatePart(DateInterval.Weekday, dateAndTime, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) Then
                                    If isLastWeekDayForDate(dateAndTime) Then
                                        flag = True
                                    End If
                                End If
                            Else
                                If iWeekday = DatePart(DateInterval.Weekday, dateAndTime, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays) Then

                                    If iOrder = getWeekDayOrderForDate(dateAndTime) Then
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
                    c.CaseSolution_Id = dr("Id")
                    c.RegistrationSource = 4
                    c.RegLanguage_Id = dr("Language_Id")

                    If dr("ScheduleWatchDate") > 0 Then
                        c.WatchDate = DateAdd(DateInterval.Day, dr("ScheduleWatchDate"), dateAndTime)
                    End If

                    If Not IsDBNull(dr("Department_Id")) Then
                        c.Department_Id = dr("Department_Id")
                    End If

                    ' Kontrollera om användaruppgifter ska hämtas
                    ' Commented: helpdesk don't do this
                    'If Not IsDBNull(dr("ReportedBy")) AndAlso Not String.IsNullOrEmpty(DirectCast(dr("ReportedBy"), String)) Then
                    '    Dim objComputerUser As ComputerUser = objComputerUserData.getComputerUserByUserId(dr("ReportedBy"), dr("Customer_Id"))

                    '    If Not objComputerUser Is Nothing Then
                    '        c.ReportedBy = objComputerUser.UserId
                    '        c.Persons_Name = objComputerUser.FirstName & " " & objComputerUser.SurName
                    '        c.Persons_EMail = objComputerUser.EMail
                    '        c.Persons_Phone = objComputerUser.Phone

                    '        If c.Department_Id = 0 Then
                    '            If objComputerUser.Department_Id <> 0 Then
                    '                c.Department_Id = objComputerUser.Department_Id
                    '            End If
                    '        End If
                    '    End If

                    'End If

                    If Not IsDBNull(dr("ReportedBy")) Then
                        c.ReportedBy = dr("ReportedBy")
                    End If

                    'Dim setCurrentUsersWorkingGroup As Integer = 0
                    'If Not IsDBNull(dr("SetCurrentUsersWorkingGroup")) Then
                    '    setCurrentUsersWorkingGroup = dr("SetCurrentUsersWorkingGroup")
                    'End If

                    'If setCurrentUsersWorkingGroup = 1 Then
                    '    '        var userDefaultWGId = _userService.GetUserDefaultWorkingGroupId(SessionFacade.CurrentUser.Id, Customer.Id);
                    '    '    m.case_.WorkingGroup_Id = userDefaultWGId;
                    'Else
                    If Not IsDBNull(dr("CaseWorkingGroup_Id")) Then
                        c.WorkingGroup_Id = dr("CaseWorkingGroup_Id")
                    End If
                    'End If

                    If Not IsDBNull(dr("Project_Id")) Then
                        c.Project_Id = dr("Project_Id")
                    End If

                    'If Not IsDBNull(dr("NoMailToNotifier")) Then
                    '    c.NoMailToNotifier = dr("NoMailToNotifier")
                    'End If

                    If Not IsDBNull(dr("PersonsName")) Then
                        c.Persons_Name = dr("PersonsName")
                    End If

                    If Not IsDBNull(dr("PersonsPhone")) Then
                        c.Persons_Phone = dr("PersonsPhone")
                    End If

                    If Not IsDBNull(dr("PersonsCellPhone")) Then
                        c.Persons_CellPhone = dr("PersonsCellPhone")
                    End If

                    If Not IsDBNull(dr("OU_Id")) Then
                        c.OU_Id = dr("OU_Id")
                    End If

                    If Not IsDBNull(dr("Place")) Then
                        c.Place = dr("Place")
                    End If

                    If Not IsDBNull(dr("UserCode")) Then
                        c.UserCode = dr("UserCode")
                    End If

                    If Not IsDBNull(dr("System_Id")) Then
                        c.System_Id = dr("System_Id")
                    End If

                    If Not IsDBNull(dr("Urgency_Id")) Then
                        c.Urgency_Id = dr("Urgency_Id")
                    End If

                    If Not IsDBNull(dr("Impact_Id")) Then
                        c.Impact_Id = dr("Impact_Id")
                    End If

                    If Not IsDBNull(dr("InvoiceNumber")) Then
                        c.InvoiceNumber = dr("InvoiceNumber")
                    End If

                    If Not IsDBNull(dr("ReferenceNumber")) Then
                        c.ReferenceNumber = dr("ReferenceNumber")
                        'Else
                        '    c.ReferenceNumber = DBNull.Value
                    End If

                    If Not IsDBNull(dr("Status_Id")) Then
                        c.Status_Id = dr("Status_Id")
                    End If

                    If Not IsDBNull(dr("StateSecondary_Id")) Then
                        c.StateSecondary_Id = dr("StateSecondary_Id")
                    End If

                    If Not IsDBNull(dr("Verified")) Then
                        c.Verified = dr("Verified")
                    End If

                    If Not IsDBNull(dr("VerifiedDescription")) Then
                        c.VerifiedDescription = dr("VerifiedDescription")
                    End If

                    If Not IsDBNull(dr("SolutionRate")) Then
                        c.SolutionRate = dr("SolutionRate")
                    End If

                    If Not IsDBNull(dr("InventoryNumber")) Then
                        c.InventoryNumber = dr("InventoryNumber")
                    End If

                    If Not IsDBNull(dr("InventoryType")) Then
                        c.InventoryType = dr("InventoryType")
                    End If

                    If Not IsDBNull(dr("InventoryLocation")) Then
                        c.InventoryLocation = dr("InventoryLocation")
                    End If

                    If Not IsDBNull(dr("Supplier_Id")) Then
                        c.Supplier_Id = dr("Supplier_Id")
                    End If

                    If Not IsDBNull(dr("SMS")) Then
                        c.Sms = dr("SMS")
                    End If

                    If Not IsDBNull(dr("Available")) Then
                        c.Available = dr("Available")
                    End If

                    If Not IsDBNull(dr("Cost")) Then
                        c.Cost = dr("Cost")
                    End If

                    If Not IsDBNull(dr("OtherCost")) Then
                        c.OtherCost = dr("OtherCost")
                    End If

                    If Not IsDBNull(dr("Currency")) Then
                        c.Currency = dr("Currency")
                    End If

                    If Not IsDBNull(dr("ContactBeforeAction")) Then
                        c.ContactBeforeAction = dr("ContactBeforeAction")
                    End If

                    If Not IsDBNull(dr("Problem_Id")) Then
                        c.Problem_Id = dr("Problem_Id")
                    End If

                    If Not IsDBNull(dr("Change_Id")) Then
                        c.Change_Id = dr("Change_Id")
                    End If

                    If Not IsDBNull(dr("FinishingDate")) Then
                        c.FinishingDate = dr("FinishingDate")
                    End If

                    If Not IsDBNull(dr("FinishingDescription")) Then
                        c.FinishingDescription = dr("FinishingDescription")
                    End If

                    If Not IsDBNull(dr("PlanDate")) Then
                        c.PlanDate = dr("PlanDate")
                    End If

                    If Not IsDBNull(dr("AgreedDate")) Then
                        c.AgreedDate = dr("AgreedDate")
                    End If

                    If Not IsDBNull(dr("CausingPartId")) Then
                        c.CausingPartId = dr("CausingPartId")
                    End If

                    If Not IsDBNull(dr("RegistrationSource")) Then
                        c.RegistrationSourceCustomer_Id = dr("RegistrationSource")
                    End If

                    If Not IsDBNull(dr("PersonsEmail")) Then
                        c.Persons_EMail = dr("PersonsEmail")
                    End If

                    If Not IsDBNull(dr("CostCentre")) Then
                        c.CostCentre = dr("CostCentre")
                    End If

                    If Not IsDBNull(dr("Region_Id")) Then
                        c.Region_Id = dr("Region_Id")
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

                    If Not IsDBNull(dr("IsAbout_ReportedBy")) Then
                        c.IsAbout_ReportedBy = dr("IsAbout_ReportedBy")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsName")) Then
                        c.IsAbout_PersonsName = dr("IsAbout_PersonsName")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsEmail")) Then
                        c.IsAbout_PersonsEMail = dr("IsAbout_PersonsEmail")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsPhone")) Then
                        c.IsAbout_PersonsPhone = dr("IsAbout_PersonsPhone")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsCellPhone")) Then
                        c.IsAbout_PersonsCellPhone = dr("IsAbout_PersonsCellPhone")
                    End If

                    If Not IsDBNull(dr("IsAbout_Region_Id")) Then
                        c.IsAbout_Region_Id = dr("IsAbout_Region_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_Department_Id")) Then
                        c.IsAbout_Department_Id = dr("IsAbout_Department_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_OU_Id")) Then
                        c.IsAbout_OU_Id = dr("IsAbout_OU_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_Place")) Then
                        c.IsAbout_Place = dr("IsAbout_Place")
                    End If

                    If Not IsDBNull(dr("IsAbout_CostCentre")) Then
                        c.IsAbout_CostCentre = dr("IsAbout_CostCentre")
                    End If

                    If Not IsDBNull(dr("IsAbout_UserCode")) Then
                        c.IsAbout_UserCode = dr("IsAbout_UserCode")
                    End If

                    If Not IsDBNull(dr("ExtendedCaseForms_Id")) Then
                        c.ExtendedCaseFormId = dr("ExtendedCaseForms_Id")
                    End If

                    If Not IsDBNull(dr("CaseType_Id")) Then
                        c.CaseType_Id = dr("CaseType_Id")
                        If (Not c.CaseType_Id = 0) Then
                            Dim caseTypeData As New CaseTypeData()
                            Dim caseType As CaseType = caseTypeData.getCaseTypeById(c.CaseType_Id)
                            If (c.WorkingGroup_Id = 0 And Not caseType.WorkingGroup_Id = 0) Then
                                c.WorkingGroup_Id = caseType.WorkingGroup_Id
                            End If
                            If (c.Performer_User_Id = 0 And Not caseType.User_Id = 0) Then
                                c.Performer_User_Id = caseType.User_Id
                            End If
                        End If
                    End If

                    If Not IsDBNull(dr("ProductArea_Id")) Then
                        c.ProductArea_Id = dr("ProductArea_Id")
                        If (Not c.ProductArea_Id = 0) Then
                            Dim productAreaData As New ProductAreaData()
                            Dim productArea As ProductArea = productAreaData.GetProductArea(c.ProductArea_Id)
                            If (c.WorkingGroup_Id = 0 And Not productArea.WorkingGroup_Id = 0) Then
                                c.WorkingGroup_Id = productArea.WorkingGroup_Id
                            End If
                            If (c.Priority_Id = 0 And Not productArea.Priority_Id = 0) Then
                                c.Priority_Id = productArea.Priority_Id
                            End If
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
                        "tblCustomer.Language_Id, tblDepartment.Region_Id, tblCaseSolution_ExtendedCaseForms.ExtendedCaseForms_Id " &
                      "FROM tblCaseSolution " &
                       "INNER JOIN tblCaseSolutionSchedule ON tblCaseSolution.Id = tblCaseSolutionSchedule.CaseSolution_Id " &
                       "INNER JOIN tblCustomer ON tblCaseSolution.Customer_Id=tblCustomer.Id " &
                        "LEFT JOIN tblDepartment ON tblCaseSolution.Department_Id=tblDepartment.Id " &
                        "LEFT JOIN tblCaseSolution_ExtendedCaseForms ON tblCaseSolution.Id=tblCaseSolution_ExtendedCaseForms.CaseSolution_Id " &
                      "WHERE ScheduleTime=" & iTime & " AND tblCaseSolution.Status = 1 AND tblCaseSolutionSchedule.RepeatType = 'Yearly'"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getCaseSolutionSchedule " & sSQL)
            End If

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

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
                    c.CaseSolution_Id = dr("Id")
                    c.RegistrationSource = 4
                    c.RegLanguage_Id = dr("Language_Id")

                    If dr("ScheduleWatchDate") > 0 Then
                        c.WatchDate = DateAdd(DateInterval.Day, dr("ScheduleWatchDate"), Today)
                    End If

                    If Not IsDBNull(dr("Department_Id")) Then
                        c.Department_Id = dr("Department_Id")
                    End If

                    ' Kontrollera om användaruppgifter ska hämtas
                    ' Commented: helpdesk don't do this
                    'If Not IsDBNull(dr("ReportedBy")) AndAlso Not String.IsNullOrEmpty(DirectCast(dr("ReportedBy"), String)) Then
                    '    Dim objComputerUser As ComputerUser = objComputerUserData.getComputerUserByUserId(dr("ReportedBy"), dr("Customer_Id"))

                    '    If Not objComputerUser Is Nothing Then
                    '        c.ReportedBy = objComputerUser.UserId
                    '        c.Persons_Name = objComputerUser.FirstName & " " & objComputerUser.SurName
                    '        c.Persons_EMail = objComputerUser.EMail
                    '        c.Persons_Phone = objComputerUser.Phone

                    '        If c.Department_Id = 0 Then
                    '            If objComputerUser.Department_Id <> 0 Then
                    '                c.Department_Id = objComputerUser.Department_Id
                    '            End If
                    '        End If
                    '    End If

                    'End If

                    If Not IsDBNull(dr("ReportedBy")) Then
                        c.ReportedBy = dr("ReportedBy")
                    End If

                    'Dim setCurrentUsersWorkingGroup As Integer = 0
                    'If Not IsDBNull(dr("SetCurrentUsersWorkingGroup")) Then
                    '    setCurrentUsersWorkingGroup = dr("SetCurrentUsersWorkingGroup")
                    'End If

                    'If setCurrentUsersWorkingGroup = 1 Then
                    '    '        var userDefaultWGId = _userService.GetUserDefaultWorkingGroupId(SessionFacade.CurrentUser.Id, Customer.Id);
                    '    '    m.case_.WorkingGroup_Id = userDefaultWGId;
                    'Else
                    If Not IsDBNull(dr("CaseWorkingGroup_Id")) Then
                        c.WorkingGroup_Id = dr("CaseWorkingGroup_Id")
                    End If
                    'End If

                    If Not IsDBNull(dr("Project_Id")) Then
                        c.Project_Id = dr("Project_Id")
                    End If

                    'If Not IsDBNull(dr("NoMailToNotifier")) Then
                    '    c.NoMailToNotifier = dr("NoMailToNotifier")
                    'End If

                    If Not IsDBNull(dr("PersonsName")) Then
                        c.Persons_Name = dr("PersonsName")
                    End If

                    If Not IsDBNull(dr("PersonsPhone")) Then
                        c.Persons_Phone = dr("PersonsPhone")
                    End If

                    If Not IsDBNull(dr("PersonsCellPhone")) Then
                        c.Persons_CellPhone = dr("PersonsCellPhone")
                    End If

                    If Not IsDBNull(dr("OU_Id")) Then
                        c.OU_Id = dr("OU_Id")
                    End If

                    If Not IsDBNull(dr("Place")) Then
                        c.Place = dr("Place")
                    End If

                    If Not IsDBNull(dr("UserCode")) Then
                        c.UserCode = dr("UserCode")
                    End If

                    If Not IsDBNull(dr("System_Id")) Then
                        c.System_Id = dr("System_Id")
                    End If

                    If Not IsDBNull(dr("Urgency_Id")) Then
                        c.Urgency_Id = dr("Urgency_Id")
                    End If

                    If Not IsDBNull(dr("Impact_Id")) Then
                        c.Impact_Id = dr("Impact_Id")
                    End If

                    If Not IsDBNull(dr("InvoiceNumber")) Then
                        c.InvoiceNumber = dr("InvoiceNumber")
                    End If

                    If Not IsDBNull(dr("ReferenceNumber")) Then
                        c.ReferenceNumber = dr("ReferenceNumber")
                        'Else
                        '    c.ReferenceNumber = DBNull.Value
                    End If

                    If Not IsDBNull(dr("Status_Id")) Then
                        c.Status_Id = dr("Status_Id")
                    End If

                    If Not IsDBNull(dr("StateSecondary_Id")) Then
                        c.StateSecondary_Id = dr("StateSecondary_Id")
                    End If

                    If Not IsDBNull(dr("Verified")) Then
                        c.Verified = dr("Verified")
                    End If

                    If Not IsDBNull(dr("VerifiedDescription")) Then
                        c.VerifiedDescription = dr("VerifiedDescription")
                    End If

                    If Not IsDBNull(dr("SolutionRate")) Then
                        c.SolutionRate = dr("SolutionRate")
                    End If

                    If Not IsDBNull(dr("InventoryNumber")) Then
                        c.InventoryNumber = dr("InventoryNumber")
                    End If

                    If Not IsDBNull(dr("InventoryType")) Then
                        c.InventoryType = dr("InventoryType")
                    End If

                    If Not IsDBNull(dr("InventoryLocation")) Then
                        c.InventoryLocation = dr("InventoryLocation")
                    End If

                    If Not IsDBNull(dr("Supplier_Id")) Then
                        c.Supplier_Id = dr("Supplier_Id")
                    End If

                    If Not IsDBNull(dr("SMS")) Then
                        c.Sms = dr("SMS")
                    End If

                    If Not IsDBNull(dr("Available")) Then
                        c.Available = dr("Available")
                    End If

                    If Not IsDBNull(dr("Cost")) Then
                        c.Cost = dr("Cost")
                    End If

                    If Not IsDBNull(dr("OtherCost")) Then
                        c.OtherCost = dr("OtherCost")
                    End If

                    If Not IsDBNull(dr("Currency")) Then
                        c.Currency = dr("Currency")
                    End If

                    If Not IsDBNull(dr("ContactBeforeAction")) Then
                        c.ContactBeforeAction = dr("ContactBeforeAction")
                    End If

                    If Not IsDBNull(dr("Problem_Id")) Then
                        c.Problem_Id = dr("Problem_Id")
                    End If

                    If Not IsDBNull(dr("Change_Id")) Then
                        c.Change_Id = dr("Change_Id")
                    End If

                    If Not IsDBNull(dr("FinishingDate")) Then
                        c.FinishingDate = dr("FinishingDate")
                    End If

                    If Not IsDBNull(dr("FinishingDescription")) Then
                        c.FinishingDescription = dr("FinishingDescription")
                    End If

                    If Not IsDBNull(dr("PlanDate")) Then
                        c.PlanDate = dr("PlanDate")
                    End If

                    If Not IsDBNull(dr("AgreedDate")) Then
                        c.AgreedDate = dr("AgreedDate")
                    End If

                    If Not IsDBNull(dr("CausingPartId")) Then
                        c.CausingPartId = dr("CausingPartId")
                    End If

                    If Not IsDBNull(dr("RegistrationSource")) Then
                        c.RegistrationSourceCustomer_Id = dr("RegistrationSource")
                    End If

                    If Not IsDBNull(dr("PersonsEmail")) Then
                        c.Persons_EMail = dr("PersonsEmail")
                    End If

                    If Not IsDBNull(dr("CostCentre")) Then
                        c.CostCentre = dr("CostCentre")
                    End If

                    If Not IsDBNull(dr("Region_Id")) Then
                        c.Region_Id = dr("Region_Id")
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

                    If Not IsDBNull(dr("IsAbout_ReportedBy")) Then
                        c.IsAbout_ReportedBy = dr("IsAbout_ReportedBy")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsName")) Then
                        c.IsAbout_PersonsName = dr("IsAbout_PersonsName")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsEmail")) Then
                        c.IsAbout_PersonsEmail = dr("IsAbout_PersonsEmail")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsPhone")) Then
                        c.IsAbout_PersonsPhone = dr("IsAbout_PersonsPhone")
                    End If

                    If Not IsDBNull(dr("IsAbout_PersonsCellPhone")) Then
                        c.IsAbout_PersonsCellPhone = dr("IsAbout_PersonsCellPhone")
                    End If

                    If Not IsDBNull(dr("IsAbout_Region_Id")) Then
                        c.IsAbout_Region_Id = dr("IsAbout_Region_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_Department_Id")) Then
                        c.IsAbout_Department_Id = dr("IsAbout_Department_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_OU_Id")) Then
                        c.IsAbout_OU_Id = dr("IsAbout_OU_Id")
                    End If

                    If Not IsDBNull(dr("IsAbout_Place")) Then
                        c.IsAbout_Place = dr("IsAbout_Place")
                    End If

                    If Not IsDBNull(dr("IsAbout_CostCentre")) Then
                        c.IsAbout_CostCentre = dr("IsAbout_CostCentre")
                    End If

                    If Not IsDBNull(dr("IsAbout_UserCode")) Then
                        c.IsAbout_UserCode = dr("IsAbout_UserCode")
                    End If

                    If Not IsDBNull(dr("ExtendedCaseForms_Id")) Then
                        c.ExtendedCaseFormId = dr("ExtendedCaseForms_Id")
                    End If

                    If Not IsDBNull(dr("CaseType_Id")) Then
                        c.CaseType_Id = dr("CaseType_Id")
                        If (Not c.CaseType_Id = 0) Then
                            Dim caseTypeData As New CaseTypeData()
                            Dim caseType As CaseType = caseTypeData.getCaseTypeById(c.CaseType_Id)
                            If (c.WorkingGroup_Id = 0 And Not caseType.WorkingGroup_Id = 0) Then
                                c.WorkingGroup_Id = caseType.WorkingGroup_Id
                            End If
                            If (c.Performer_User_Id = 0 And Not caseType.User_Id = 0) Then
                                c.Performer_User_Id = caseType.User_Id
                            End If
                        End If
                    End If

                    If Not IsDBNull(dr("ProductArea_Id")) Then
                        c.ProductArea_Id = dr("ProductArea_Id")
                        If (Not c.ProductArea_Id = 0) Then
                            Dim productAreaData As New ProductAreaData()
                            Dim productArea As ProductArea = productAreaData.GetProductArea(c.ProductArea_Id)
                            If (c.WorkingGroup_Id = 0 And Not productArea.WorkingGroup_Id = 0) Then
                                c.WorkingGroup_Id = productArea.WorkingGroup_Id
                            End If
                            If (c.Priority_Id = 0 And Not productArea.Priority_Id = 0) Then
                                c.Priority_Id = productArea.Priority_Id
                            End If
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

            dt = getDataTable(gsConnectionString, sSQL)

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

                dtCase = getDataTable(gsConnectionString, sSQL)

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

            executeSQL(gsConnectionString, sSQL)

            sSQL = "DELETE FROM tblCaseFile WHERE Case_Id=" & iCase_Id

            executeSQL(gsConnectionString, sSQL)

            If System.IO.Directory.Exists(sFilePath & "\" & sCaseNumber) Then
                Try
                    System.IO.Directory.Delete(sFilePath & "\" & sCaseNumber, True)
                Catch ex As Exception

                End Try
            End If

            sSQL = "UPDATE tblEmailLog SET " &
                        "EMailAddress='-' " &
                    " WHERE tblEmailLog.CaseHistory_Id IN (SELECT Id FROM tblCaseHistory WHERE Case_Id=" & iCase_Id & ")"

            executeSQL(gsConnectionString, sSQL)

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

            executeSQL(gsConnectionString, sSQL)

            sSQL = "UPDATE tblCase SET " &
                        "ReportedBy='-', " &
                        "Persons_name='-', " &
                        "Persons_phone='-', " &
                        "UserCode='-', " &
                        "Caption='-', " &
                        "User_Id=Null, " &
                        "CaseCleanUp_Id=" & iCaseCleanUp_Id &
                    " WHERE tblCase.Id=" & iCase_Id

            executeSQL(gsConnectionString, sSQL)
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

            executeSQL(gsConnectionString, sSQL)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function CheckCaseField(iCustomerId As Integer, sFieldName As String) As Boolean
        Dim sCmd As String = "SELECT Show FROM dbo.tblCaseFieldSettings WHERE CaseField = @fieldName AND Customer_Id = @customerId"
        Dim cmdParams As New List(Of SqlParameter) From {
                DbHelper.createDbParameter("customerId", iCustomerId),
                DbHelper.createDbParameter("fieldName", sFieldName)
        }

        Dim res = DbHelper.executeScalarQuery(Of Integer)(gsConnectionString, sCmd, CommandType.Text, cmdParams.ToArray())
        Return res = 1
    End Function

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

    Public Sub CreateExtendedCaseConnection(caseId As Integer, extendedCaseFormId As Integer, extendedCaseDataId As Integer)
        Dim sSQL = ""

        Try

            sSQL = "INSERT INTO tblCase_ExtendedCaseData(Case_Id, ExtendedCaseData_Id, ExtendedCaseForm_Id) " &
                   "VALUES(@caseId, @extendedCaseDataId ,@extendedCaseFormId)"

            Dim sqlParameters As New List(Of SqlParameter) From {
                    DbHelper.createDbParameter("@caseId", caseId),
                    DbHelper.createDbParameter("@extendedCaseFormId", extendedCaseFormId),
                    DbHelper.createDbParameter("@extendedCaseDataId", extendedCaseDataId)
                    }

            DbHelper.executeNonQuery(gsConnectionString, sSQL, CommandType.Text, sqlParameters.ToArray())
        Catch ex As Exception

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR CreateExtendedCaseConnection " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Sub

    Private Function getStateSecondarySetDate(iStateSecondary_Id As Integer, iCase_Id As Integer) As DateTime
        Dim setDate As DateTime?

        Dim sSQL As String
        Dim dr As DataRow

        Try
            sSQL = "select StateSecondary_Id, CreatedDate " &
                    "from tblCaseHistory where Case_Id=" & iCase_Id &
                    " order by Id desc"

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    If setDate Is Nothing Then
                        setDate = dr("CreatedDate")
                    End If

                    If Convert.IsDBNull(dr("StateSecondary_Id")) Then
                        Exit For
                    End If

                    If dr("StateSecondary_Id") = iStateSecondary_Id Then
                        setDate = dr("CreatedDate")
                    Else
                        Exit For
                    End If
                Next
            End If

            Return setDate
        Catch ex As Exception
            Throw ex
        End Try
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
