Imports DH.Helpdesk.Mail2Ticket.Library.SharedFunctions

Public Class TextTranslationData
    Public Function getTextTranslation(ByVal sText As String, ByVal iLanguage_Id As Integer) As String
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblTextTranslation.TextTranslation " & _
                    "FROM tblText " & _
                        "INNER JOIN tblTextTranslation ON tblText.Id = tblTextTranslation.Text_Id " & _
                    "WHERE tblText.TextString like '" & sText & "' AND Language_Id=" & iLanguage_Id

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)

                Return dr("TextTranslation")
            Else
                Return sText
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
