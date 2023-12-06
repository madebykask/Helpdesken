Imports DH.Helpdesk.Library.SharedFunctions

Public Class QuestionnaireData
    Public Function getQuestionnaires() As Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim iTime As Integer = Now.Hour
        Dim colQuestionnaire As New Collection
        Dim q As Questionnaire

        Try
            sSQL = "SELECT tblQuestionnaire.* " & _
                    "FROM tblQuestionnaire " & _
                      "WHERE ScheduleTime <> 0 And ScheduleTime=" & iTime

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

            For Each dr In dt.Rows
                q = New Questionnaire(dr)

                colQuestionnaire.Add(q)
            Next

            Return colQuestionnaire
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getQuestionnaires " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Function

    Public Function createQuestionnaireCircular(ByVal q As Questionnaire, ByVal c As Customer) As QuestionnaireCircular
        Dim sSQL As String = ""
        Dim GUID As Guid = GUID.NewGuid()
        Dim qc As QuestionnaireCircular

        Try
            ' Skapa loggpost
            sSQL = "INSERT INTO tblQuestionnaireCircular (GUID, Questionnaire_Id,CircularName,Departments,CaseTypes,ProductAreas,WorkingGroups,Selection,Filter,Status) " & _
                    "VALUES('" & _
                        GUID.ToString() & "', " & _
                        q.Id & ", '" & _
                        q.QuestionnaireName & "', '" & _
                        q.Departments & "', '" & _
                        q.CaseTypes & "', '" & _
                        q.ProductAreas & "', '" & _
                        q.WorkingGroups & "', " & _
                        q.Selection & ", " & _
                        q.Filter & ", 1)"

            executeSQL(gsConnectionString, sSQL)

            qc = getQuestionnaireCircularById(GUID.ToString())

            qc.QuestionnaireCircularPart = getQuestionnaireCircularPart(qc, c)

            Return qc
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createQuestionnaireCircular " & ex.Message.ToString)
            End If

            Throw ex

            Return Nothing
        End Try
    End Function

    Private Function getQuestionnaireCircularById(ByVal sGUID As String) As QuestionnaireCircular
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblQuestionnaireCircular.* " & _
                   "FROM tblQuestionnaireCircular " & _
                    "WHERE GUID='" & sGUID & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim qc As QuestionnaireCircular

                qc = New QuestionnaireCircular(dt.Rows(0))


                Return qc
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getQuestionnaireCircularById " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Function

    Private Function getQuestionnaireCircularPartById(ByVal sGUID As String) As QuestionnaireCircularPart
        Dim sSQL As String = ""
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblQuestionnaireCircularPart.*, tblCase.Id AS Case_Id, CaseNumber, Caption, Description, Persons_EMail " & _
                   "FROM tblQuestionnaireCircularPart " & _
                        "INNER JOIN tblCase ON tblQuestionnaireCircularPart.Case_Id=tblCase.Id " & _
                    "WHERE GUID='" & sGUID & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim qcp As QuestionnaireCircularPart

                qcp = New QuestionnaireCircularPart(dt.Rows(0))

                Return qcp
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getQuestionnaireCircularPartById " & ex.Message.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Function

    Public Function getQuestionnaireCircularPart(ByVal qc As QuestionnaireCircular, ByVal cu As Customer) As Collection
        Dim colQuestionnaireCircularPart As New Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim iCount As Integer = 0

        Try
            ' Selectera ut ärenden
            sSQL = "SELECT Null AS GUID, 0 AS Id, " & qc.Id & " AS QuestionnaireCircular_Id, tblCase.Id AS Case_Id, CaseNumber, Caption, Description, Persons_EMail " & _
                    "FROM tblCase "

            If cu.CaseWorkingGroupSource = 0 Then
                sSQL = sSQL & "INNER JOIN tblUsers ON tblCase.Performer_User_Id = tblUsers.Id "
            End If

            sSQL = sSQL & "WHERE tblCase.Customer_Id=" & cu.Id & _
                            " AND tblCase.Deleted=0 " & _
                            " AND FinishingDate IS NOT NULL " & _
                            " AND Len(Persons_Email) > 3 "

            If qc.Departments <> "" Then
                sSQL = sSQL & " AND Department_Id IN (" & qc.Departments & ")"
            End If

            If qc.CaseTypes <> "" Then
                sSQL = sSQL & " AND CaseType_Id IN (" & qc.CaseTypes & ")"
            End If

            If qc.ProductAreas <> "" Then
                sSQL = sSQL & " AND ProductArea_Id IN (" & qc.ProductAreas & ")"
            End If

            If qc.WorkingGroups <> "" Then
                If cu.CaseWorkingGroupSource = 0 Then
                    sSQL = sSQL & " AND tblUsers.Default_WorkingGroup_Id IN (" & qc.WorkingGroups & ")"
                Else
                    sSQL = sSQL & " AND WorkingGroup_Id IN (" & qc.WorkingGroups & ")"
                End If

            End If

            sSQL = sSQL & " AND Convert(nvarchar(18), FinishingDate, 121) >= '" & Now.AddDays(-1).ToString("yyyy-MM-dd hh:MM:ss") & "'"

            sSQL = sSQL & " AND tblCase.Id NOT IN (SELECT Case_Id FROM tblQuestionnaireCircularPart) "

            If qc.Filter >= 4 Then
                sSQL = sSQL & " AND tblCase.Persons_Email NOT IN (SELECT Email FROM tblUsers) "
            End If

            sSQL = sSQL & "ORDER BY CaseNumber"

            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", getQuestionnaireCircularPart " & sSQL)
            End If

            Dim dt As DataTable

            dt = getDataTable(gsConnectionString, sSQL)

            Dim qcp As QuestionnaireCircularPart

            For Each dr In dt.Rows
                If iCount Mod (100 / qc.Selection) = 0 Then
                    If qc.Filter = 0 Or isInEmailList(colQuestionnaireCircularPart, dr("Persons_EMail").ToString()) = False Then

                        qcp = New QuestionnaireCircularPart(dr)

                        qcp = createQuestionnaireCircularPart(qcp)

                        colQuestionnaireCircularPart.Add(qcp)

                    End If
                End If
                iCount = iCount + 1
            Next

            Return colQuestionnaireCircularPart
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function createQuestionnaireCircularPart(ByVal qcpnew As QuestionnaireCircularPart) As QuestionnaireCircularPart
        Dim sSQL As String = ""
        Dim qcp As QuestionnaireCircularPart

        Try
            ' Skapa loggpost
            sSQL = "INSERT INTO tblQuestionnaireCircularPart (GUID, QuestionnaireCircular_Id, Case_Id) " & _
                    "VALUES('" & _
                        qcpnew.GUID.ToString() & "', " & _
                        qcpnew.QuestionnaireCircular_Id & ", " & _
                        qcpnew.Case_Id & ")"

            executeSQL(gsConnectionString, sSQL)

            qcp = getQuestionnaireCircularPartById(qcpnew.GUID.ToString())

            Return qcp
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createQuestionnaireCircular " & ex.Message.ToString)
            End If

            Throw ex

            Return Nothing
        End Try
    End Function

    Public Sub updateQuestionnaireCircularPartStatus(ByVal iId As Integer)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblQuestionnaireCircularPart SET SendDate=getutcDate() WHERE tblQuestionnaireCircularPart.Id=" & iId

            executeSQL(gsConnectionString, sSQL)
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Public Sub updateQuestionnaireCircularStatus(ByVal iId As Integer)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblQuestionnaireCircular SET Status=2 WHERE Id=" & iId

            executeSQL(gsConnectionString, sSQL)
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

    Private Function isInEmailList(qcpl As Collection, EMail As String)
        Dim res As Boolean = False

        For i As Integer = 1 To qcpl.Count
            Dim qcp As QuestionnaireCircularPart = qcpl(i)

            If StrComp(qcp.Persons_EMail, EMail, CompareMethod.Text) = 0 Then
                res = True

                Exit For
            End If
        Next

        Return res
    End Function
End Class
