Imports DH_Helpdesk.SharedFunctions

Public Class MailTemplateData
    Public Function getMailTemplateById(ByVal MailId As Integer, ByVal Customer_Id As Integer, ByVal Language_Id As Integer, Optional ByVal dbVersion As String = "") As MailTemplate
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblMailTemplate.MailId, tblMailTemplate_tblLanguage.Subject, tblMailTemplate_tblLanguage.Body " & _
                    "FROM tblMailTemplate " & _
                        "INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id=tblMailTemplate_tblLanguage.MailTemplate_Id " & _
                    "WHERE MailId = " & MailId & _
                        " AND tblMailTemplate.Customer_Id = " & Customer_Id & _
                        " AND tblMailTemplate_tblLanguage.Language_Id = " & Language_Id & _
                        " AND tblMailTemplate_tblLanguage.Subject <> '' "
            
            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            Dim mt As MailTemplate = Nothing

            If dt.Rows.Count > 0 Then
                mt = New MailTemplate(dt.Rows(0))
            End If

            Return mt
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR getMailTemplateById " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Function
End Class
