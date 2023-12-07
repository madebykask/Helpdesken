Imports System.Data.SqlClient
Imports DH.Helpdesk.Library.SharedFunctions

Public Class ContractData
    Public Function getContractsForNoticeOfRemoval() As Collection
        Dim colContract As New Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim dtTemp As Date

        Try
            sSQL = "SELECT tblContract.*, tblContractCategory.Customer_Id, " & _
                       "tblContractCategory.CreateCase_UserId, tblContractCategory.CreateCase_CaseType_Id, tblContractCategory.CreateCase_StateSecondary_Id1, tblContractCategory.CreateCase_StateSecondary_Id2, tblContractCategory.Form_Id," & _
                       "tblCustomer.Language_Id " & _
                   "FROM tblContract " & _
                       "INNER JOIN tblContractCategory ON tblContract.ContractCategory_Id=tblContractCategory.Id " & _
                       "INNER JOIN tblCustomer ON tblContractCategory.Customer_Id=tblCustomer.Id " & _
                   "WHERE tblContract.Finished=0 AND NoticeDate IS NOT NULL"

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)
            Dim c As Contract

            For Each dr In dt.Rows
                ' Kontrollera om kontraktet ska sägas upp
                dtTemp = dr("NoticeDate")

                dtTemp = DateAdd(DateInterval.Month, -1 * dr("NoticeTime"), dtTemp)

                If dtTemp = Date.Today Then
                    c = New Contract(dr)
                    colContract.Add(c)
                End If
            Next

            Return colContract
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getContractsForFollowUp() As Collection
        Dim colContract As New Collection
        Dim sSQL As String
        Dim dr As DataRow
        Dim dtTemp As Date
        Dim dtContractEndDate As Date

        Try
            sSQL = "SELECT tblContract.*, tblContractCategory.Customer_Id, " & _
                       "tblContractCategory.CreateCase_UserId, tblContractCategory.CreateCase_CaseType_Id, tblContractCategory.CreateCase_StateSecondary_Id1, tblContractCategory.CreateCase_StateSecondary_Id2, tblContractCategory.Form_Id, " & _
                       "tblCustomer.Language_Id " & _
                    "FROM tblContract " & _
                        "INNER JOIN tblContractCategory ON tblContract.ContractCategory_Id=tblContractCategory.Id " & _
                        "INNER JOIN tblCustomer ON tblContractCategory.Customer_Id=tblCustomer.Id " & _
                    "WHERE tblContract.Finished=0 AND tblContract.FollowUpInterVal <> 0"

            Dim dt As DataTable = getDataTable(gsConnectionString, sSQL)
            Dim c As Contract

            For Each dr In dt.Rows
                ' Kontrollera om kontraktet ska följas upp
                dtTemp = dr("ContractStartDate")

                If IsDBNull(dr("ContractEndDate")) Then
                    dtContractEndDate = DateAdd(DateInterval.Month, 1, Today)
                Else
                    dtContractEndDate = dr("ContractEndDate")
                End If

                Do While dtTemp <= dtContractEndDate
                    dtTemp = DateAdd(DateInterval.Month, dr("FollowUpInterval"), dtTemp)

                    If DateAdd(DateInterval.Month, -1, dtTemp) = Date.Today Then
                        c = New Contract(dr)
                        colContract.Add(c)

                        Exit Do
                    End If
                Loop
            Next

            Return colContract
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub createLog(ByVal Contract_Id As Integer, ByVal LogType As Integer, ByVal EMail As String, ByVal Case_Id As Integer)
        Dim sSQL As String = ""

        Try
            ' Skapa loggpost
            sSQL = "INSERT INTO tblContractLog(Contract_Id, LogType, EMail, Case_Id) " & _
                        "VALUES(" & Contract_Id & ", " & LogType & ", '" & EMail & "', " & Case_Id & ")"

            executeSQL(gsConnectionString, sSQL)
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR createLog " & ex.Message.ToString)
            End If

            Throw ex
        End Try
    End Sub
End Class
