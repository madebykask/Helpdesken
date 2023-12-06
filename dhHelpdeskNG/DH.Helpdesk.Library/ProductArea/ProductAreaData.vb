Imports DH.Helpdesk.Library.SharedFunctions

Public Class ProductAreaData

    Public Function GetProductArea(ByVal iCustomer_Id As Integer, ByVal Name As String, ByVal ParentId As Integer) As ProductArea
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "select pa.Id, pa.ProductArea, pa.WorkingGroup_Id, pa.Priority_Id, pa.Parent_ProductArea_Id, w.StateSecondary_Id from tblProductArea as pa "
            sSQL = sSQL & "left outer join tblWorkingGroup as w on pa.WorkingGroup_Id = w.Id "
            sSQL = sSQL & "where (pa.ProductArea like '" & Name & "')"
            sSQL = sSQL & " and (pa.Customer_Id = " & iCustomer_Id.ToString() & ")"
            If ParentId = 0 Then
                sSQL = sSQL & " and (pa.Parent_ProductArea_Id is null)"
            Else
                sSQL = sSQL & " and (pa.Parent_ProductArea_Id =" & ParentId.ToString() & ")"
            End If

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return New ProductArea(dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetProductArea(ByVal iId As Integer) As ProductArea
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblProductArea.* " &
                   "FROM tblProductArea " &
                   "WHERE tblProductArea.Id = " & iId

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return New ProductArea(dt.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function


End Class
