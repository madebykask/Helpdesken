Imports DH.Helpdesk.Library.SharedFunctions

Public Class InvoiceData

    Public Function getInvoices(ByVal iCustomer_Id As Integer) As Collection
        Dim colInvoice As New Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim dr As DataRow

        Try
            sSQL = "SELECT tblCase.Id " & _
                    "FROM tblCase " & _
                        "INNER JOIN tblLog ON tblCase.Id = tblLog.Case_Id " & _
                        "INNER JOIN tblDepartment ON tblCase.Department_Id = tblDepartment.Id " & _
                        "LEFT JOIN tblInvoiceRow ON tblCase.Id=tblInvoiceRow.Case_Id " & _
                    "WHERE tblCase.Customer_Id=" & iCustomer_Id & _
                        " AND tblCase.FinishingDate IS NOT NULL " & _
                        " AND tblLog.Charge=1 " & _
                        " AND tblInvoiceRow.Case_Id IS NULL "


            'sSQL = "SELECT tblCase.Id, tblCase.Casenumber, SUM(tblLog.WorkingTime) AS WorkingTime, SUM(tblLog.EquipmentPrice) AS EquipmentPrice, tblDepartment.AccountancyAmount, " & _
            '           "tblInvoiceRow.Time_Account1Debit, tblInvoiceRow.Time_Account2Debit, tblInvoiceRow.Time_Account3Debit, tblInvoiceRow.Time_Account4Debit, tblInvoiceRow.Time_Account5Debit, tblInvoiceRow.Time_Account6Debit, tblInvoiceRow.Time_Account7Debit, tblInvoiceRow.Time_Account8Debit, " & _
            '           "tblInvoiceRow.Time_Account1Kredit, tblInvoiceRow.Time_Account2Kredit, tblInvoiceRow.Time_Account3Kredit, tblInvoiceRow.Time_Account4Kredit, tblInvoiceRow.Time_Account5Kredit, tblInvoiceRow.Time_Account6Kredit, tblInvoiceRow.Time_Account7Kredit, tblInvoiceRow.Time_Account8Kredit, " & _
            '           "tblInvoiceRow.Equipment_Account1Debit, tblInvoiceRow.Equipment_Account2Debit, tblInvoiceRow.Equipment_Account3Debit, tblInvoiceRow.Equipment_Account4Debit, tblInvoiceRow.Equipment_Account5Debit, tblInvoiceRow.Equipment_Account6Debit, tblInvoiceRow.Equipment_Account7Debit, tblInvoiceRow.Equipment_Account8Debit, " & _
            '           "tblInvoiceRow.Equipment_Account1Kredit, tblInvoiceRow.Equipment_Account2Kredit, tblInvoiceRow.Equipment_Account3Kredit, tblInvoiceRow.Equipment_Account4Kredit, tblInvoiceRow.Equipment_Account5Kredit, tblInvoiceRow.Equipment_Account6Kredit, tblInvoiceRow.Equipment_Account7Kredit, tblInvoiceRow.Equipment_Account8Kredit, " & _
            '           "tblCase.Caption, tblInvoiceRow.Time_VerificationText, tblInvoiceRow.Equipment_VerificationText, tblDepartment.Department, tblCase.FinishingDate " & _
            '       "FROM tblLog " & _
            '        "INNER JOIN tblCase ON tblLog.Case_Id = tblCase.Id " & _
            '        "INNER JOIN tblDepartment ON tblCase.Department_Id = tblDepartment.Id " & _
            '        "LEFT OUTER JOIN tblInvoiceHeader " & _
            '        "RIGHT OUTER JOIN tblInvoiceRow ON tblInvoiceHeader.Id = tblInvoiceRow.InvoiceHeader_Id ON tblCase.Id = tblInvoiceRow.Case_Id " & _
            '       "WHERE (tblDepartment.Charge = 1) " & _
            '        "AND (tblLog.Charge = 1) " & _
            '        "AND (tblCase.FinishingDate IS NOT NULL) " & _
            '        "AND (tblInvoiceRow.InvoiceHeader_Id IS NULL) " & _
            '        "AND tblCase.Customer_Id=" & iCustomer_Id & _
            '       " GROUP BY tblCase.Id, tblCase.Casenumber, tblDepartment.AccountancyAmount, " & _
            '           "tblInvoiceRow.Time_Account1Debit, tblInvoiceRow.Time_Account2Debit, tblInvoiceRow.Time_Account3Debit, tblInvoiceRow.Time_Account4Debit, tblInvoiceRow.Time_Account5Debit, tblInvoiceRow.Time_Account6Debit, tblInvoiceRow.Time_Account7Debit, tblInvoiceRow.Time_Account8Debit, " & _
            '           "tblInvoiceRow.Time_Account1Kredit, tblInvoiceRow.Time_Account2Kredit, tblInvoiceRow.Time_Account3Kredit, tblInvoiceRow.Time_Account4Kredit, tblInvoiceRow.Time_Account5Kredit, tblInvoiceRow.Time_Account6Kredit, tblInvoiceRow.Time_Account7Kredit, tblInvoiceRow.Time_Account8Kredit, " & _
            '           "tblInvoiceRow.Equipment_Account1Debit, tblInvoiceRow.Equipment_Account2Debit, tblInvoiceRow.Equipment_Account3Debit, tblInvoiceRow.Equipment_Account4Debit, tblInvoiceRow.Equipment_Account5Debit, tblInvoiceRow.Equipment_Account6Debit, tblInvoiceRow.Equipment_Account7Debit, tblInvoiceRow.Equipment_Account8Debit, " & _
            '           "tblInvoiceRow.Equipment_Account1Kredit, tblInvoiceRow.Equipment_Account2Kredit, tblInvoiceRow.Equipment_Account3Kredit, tblInvoiceRow.Equipment_Account4Kredit, tblInvoiceRow.Equipment_Account5Kredit, tblInvoiceRow.Equipment_Account6Kredit, tblInvoiceRow.Equipment_Account7Kredit, tblInvoiceRow.Equipment_Account8Kredit, " & _
            '                          "tblCase.Caption, tblInvoiceRow.Time_VerificationText, tblInvoiceRow.Time_VerificationText, " & _
            '                          "tblInvoiceRow.Equipment_VerificationText, tblDepartment.Department, tblCase.FinishingDate "


            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            Dim i As Invoice

            For Each dr In dt.Rows
                i = New Invoice(dr)
                colInvoice.Add(i)
            Next

            Return colInvoice

        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
