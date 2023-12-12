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

            dt = getDataTable(gsConnectionString, sSQL)

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
