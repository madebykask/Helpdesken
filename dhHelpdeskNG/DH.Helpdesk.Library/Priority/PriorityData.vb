Imports DH.Helpdesk.Library.SharedFunctions

Public Class PriorityData

    Public Function getPriorityById(ByVal iId As Integer) As Priority
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblPriority.* " & _
                   "FROM tblPriority " & _
                   "WHERE tblPriority.Id = " & iId

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            If dt.Rows.Count > 0 Then
                Dim p As Priority

                p = New Priority(dt.Rows(0))

                Return p
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetPriorityByCustomerId(ByVal iId As Integer) As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim colPrio As New Collection

        Try
            sSQL = "SELECT * FROM tblPriority WHERE (Customer_Id = " & iId & ") and ([Status] = 1) ORDER BY SolutionTime"

            'If giDBType = 0 Then
            dt = getDataTable(gsConnectionString, sSQL)
            'Else
            '    dt = getDataTableOracle(gsConnectionString, sSQL)
            'End If

            For Each dr As DataRow In dt.Rows
                Dim p As Priority = New Priority(dr)
                colPrio.Add(p)
            Next

            Return colPrio

        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class