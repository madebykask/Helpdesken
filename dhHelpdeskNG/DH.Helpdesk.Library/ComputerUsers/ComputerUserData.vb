Imports DH.Helpdesk.Library.SharedFunctions
Imports System.Net
Imports System.IO

Public Class ComputerUserData
    Public Function getComputerUserByEMail(ByVal sEMail As String, ByVal iCustomer_Id As Integer) As ComputerUser
        Try
            Return getComputerUser(iCustomer_Id, , sEMail)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getComputerUserByUserId(ByVal sUserId As String, ByVal iCustomer_Id As Integer) As ComputerUser
        Try
            Return getComputerUser(iCustomer_Id, sUserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getComputerUserByNDSPath(ByVal sNDSPath As String, ByVal iCustomer_Id As Integer) As ComputerUser
        Try
            Return getComputerUser(iCustomer_Id, , , sNDSPath)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getComputerUser(ByVal iCustomer_Id As Integer, Optional ByVal sUserId As String = "", Optional ByVal sEMail As String = "", Optional ByVal sNDSPath As String = "") As ComputerUser
        Dim sSQL As String
        Dim dt As DataTable

        If gsURL = "" Then
            Try
                sSQL = "SELECT tblComputerUsers.*, tblDepartment.Region_Id " & _
                        "FROM tblComputerUsers " & _
                            "LEFT JOIN tblDepartment ON tblComputerUsers.Department_Id=tblDepartment.Id " & _
                        "WHERE tblComputerUsers.Customer_Id=" & iCustomer_Id

                If sUserId <> "" Then
                    sSQL = sSQL & " AND LOWER(tblComputerUsers.UserId) = '" & LCase(sUserId) & "'"
                ElseIf sEMail <> "" Then
                    sSQL = sSQL & " AND LOWER(tblComputerUsers.EMail) = '" & Replace(LCase(sEMail), "'", "''") & "'"
                ElseIf sNDSPath <> "" Then
                    sSQL = sSQL & " AND LOWER(tblComputerUsers.NDSPath) = '" & Replace(LCase(sNDSPath), "'", "''") & "'"
                End If

                sSQL = sSQL & " ORDER BY tblComputerUsers.Status DESC "

                dt = getDataTable(gsConnectionString, sSQL)

                If dt.Rows.Count > 0 Then
                    Dim c As ComputerUser

                    c = New ComputerUser(dt.Rows(0))

                    Return c
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                If giLoglevel > 0 Then
                    objLogFile.WriteLine(Now() & ", ERROR getComputerUser " & ex.Message.ToString)
                End If

                Throw ex
            End Try
        Else
            Return getComputerUserHTTP(iCustomer_Id, sUserId, sEMail)
        End If
    End Function

    Private Function getComputerUserHTTP(ByVal iCustomer_Id As Integer, Optional ByVal sUserId As String = "", Optional ByVal sEMail As String = "") As ComputerUser
        Dim sURL As String
        Dim sXML As String
        Dim xmlNode As System.Xml.XmlNode


        Try
            Dim cu As ComputerUser = New ComputerUser()

            sURL = gsURL & "/getInfo.asp?TableName=tblComputerUsers&Customer_Id=" & iCustomer_Id

            If sUserId <> "" Then
                sURL = sURL & "&Fieldname=tblComputerUsers.UserId&Id=" & LCase(sUserId)
            ElseIf sEMail <> "" Then
                sURL = sURL & "&FieldName=tblComputerUsers.EMail&Id=" & LCase(sEMail)
            End If

            Dim request As WebRequest = WebRequest.Create(sURL)
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            Dim objStream As New StreamReader(response.GetResponseStream())

            sXML = objStream.ReadToEnd()

            response.Close()

            Dim xmlDocument As New System.Xml.XmlDocument
            xmlDocument.LoadXml(sXML)

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Id")

            If Not xmlNode Is Nothing Then
                cu.Id = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/UserId")

            If Not xmlNode Is Nothing Then
                cu.UserId = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/FirstName")

            If Not xmlNode Is Nothing Then
                cu.FirstName = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/SurName")

            If Not xmlNode Is Nothing Then
                cu.SurName = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/EMail")

            If Not xmlNode Is Nothing Then
                cu.EMail = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Phone")

            If Not xmlNode Is Nothing Then
                cu.Phone = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Department_Id")

            If Not xmlNode Is Nothing Then
                cu.Department_Id = Trim(xmlNode.InnerText)
            End If

            xmlNode = xmlDocument.SelectSingleNode("//Tables/Table/Record/Region_Id")

            If Not xmlNode Is Nothing Then
                cu.Region_Id = Trim(xmlNode.InnerText)
            End If

            Return cu
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getComputerUserFieldSettings(ByVal iCustomer_Id As Integer) As Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim colComputerUserFieldSettings As New Collection

        Try
            
            ' Hämta inställningar för vilka fält som ska synkas
            sSQL = "SELECT tblComputerUserFieldSettings.ComputerUserField, tblComputerUserFieldSettings.LDAPAttribute  " & _
                                "FROM tblComputerUserFieldSettings " & _
                                "WHERE Customer_Id=" & iCustomer_Id

            If giDBType = 0 Then
                sSQL = sSQL & " AND LDAPAttribute <> ''"
            Else
                sSQL = sSQL & " AND LDAPAttribute IS NOT NULL "
            End If

            sSQL = sSQL & " ORDER BY ComputerUserField"

            dt = getDataTable(gsConnectionString, sSQL)

            Dim cufs As ComputerUserFieldSettings

            For Each dr As DataRow In dt.Rows
                cufs = New ComputerUserFieldSettings(dr)
                colComputerUserFieldSettings.Add(cufs)
            Next

            Return colComputerUserFieldSettings

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub DeleteUsers(ByVal iCustomer_Id As Integer, ByVal iDomain_Id As Integer, ByVal iDays2WaitBeforeDelete As Integer)
        Dim sSQL As String
        Dim sSQLWHERE As String

        Try
            ' Nollställ koppling datorer
            sSQLWHERE = "SELECT Id FROM tblComputerUsers WHERE SyncChangedDate IS NOT NULL AND Customer_Id=" & iCustomer_Id

            If iDomain_Id <> 0 Then
                sSQLWHERE = sSQLWHERE & " AND Domain_Id=" & iDomain_Id
            End If

            sSQLWHERE = sSQLWHERE & " AND (" & SharedFunctions.Call4DateFormat("SyncChangedDate", giDBType) & " < " & SharedFunctions.convertDateTime(DateAdd(DateInterval.Day, -1 * iDays2WaitBeforeDelete, Now()), giDBType) & ") "

            sSQL = "UPDATE tblComputer SET User_Id=Null " & _
                    "WHERE User_Id IN (" & sSQLWHERE & ")"


            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteUsers " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "UPDATE tblComputerUsers SET ManagerComputerUser_Id=Null " & _
                    "WHERE ManagerComputerUser_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteUsers " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            ' ta bort historik
            sSQL = "DELETE FROM tblComputerUserLog WHERE ComputerUser_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteUsers " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            ' ta bort historik
            sSQL = "DELETE FROM tblComputerUser_tblCUGroup WHERE ComputerUser_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteUsers " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If


            ' ta bort användaren
            sSQL = "DELETE FROM tblComputerUsers WHERE Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteUsers " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If
        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, DeleteUsers " & ex.ToString)
            End If

            Throw ex
        End Try
    End Sub

    Public Sub DisableUser(ByVal sDN As String, ByVal iCustomer_id As Integer)
        Dim sSQL As String

        Try
            sSQL = "UPDATE tblComputerusers SET Status=0 WHERE NDSPath='" & sDN & "' AND Customer_Id=" & iCustomer_id

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If
        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, DisableUser " & ex.ToString)
            End If

            Throw ex
        End Try
    End Sub

    Public Function save(ByVal objComputerUser As ComputerUser, ByVal iOverwriteFromMasterDirectory As Integer) As String
        Dim sSQL As String
        Dim Ret As String = ""

        Try
            If objComputerUser.Id <> 0 Then
                sSQL = "UPDATE tblComputerusers SET " & _
                                                "Customer_Id=" & objComputerUser.Customer_Id

                If objComputerUser.Department_Id <> 0 Then
                    sSQL = sSQL & ", Department_Id=" & objComputerUser.Department_Id
                Else
                    sSQL = sSQL & ", Department_Id=Null"
                End If

                If objComputerUser.OU_Id <> 0 Then
                    sSQL = sSQL & ", OU_Id=" & objComputerUser.OU_Id
                Else
                    sSQL = sSQL & ", OU_Id=Null"
                End If

                If objComputerUser.Domain_Id <> 0 Then
                    sSQL = sSQL & ", Domain_Id=" & objComputerUser.Domain_Id
                Else
                    sSQL = sSQL & ", Domain_Id=Null"
                End If

                If objComputerUser.UserId <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", UserId = " & getDBStringPrefix() & "'" & objComputerUser.UserId.Replace("'", "") & "' "
                End If

                If objComputerUser.LogonName <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", LogonName = " & getDBStringPrefix() & "'" & objComputerUser.LogonName.Replace("'", "") & "' "
                End If

                If objComputerUser.FirstName <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", Firstname = " & getDBStringPrefix() & "'" & objComputerUser.FirstName.Replace("'", "") & "' "
                End If

                If objComputerUser.SurName <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", SurName = " & getDBStringPrefix() & "'" & objComputerUser.SurName.Replace("'", "") & "' "
                End If

                If objComputerUser.Location <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", Location = " & getDBStringPrefix() & "'" & objComputerUser.Location.Replace("'", "") & "' "
                End If

                If objComputerUser.Phone <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", Phone = " & getDBStringPrefix() & "'" & objComputerUser.Phone.Replace("'", "") & "' "
                End If

                If objComputerUser.CellPhone <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", CellPhone = " & getDBStringPrefix() & "'" & objComputerUser.CellPhone.Replace("'", "") & "' "
                End If

                If objComputerUser.PostalAddress <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", PostalAddress = " & getDBStringPrefix() & "'" & objComputerUser.PostalAddress.Replace("'", "") & "' "
                End If

                If objComputerUser.PostalCode <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", PostalCode = " & getDBStringPrefix() & "'" & objComputerUser.PostalCode.Replace("'", "") & "' "
                End If

                If objComputerUser.City <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", City = " & getDBStringPrefix() & "'" & objComputerUser.City.Replace("'", "") & "' "
                End If

                If objComputerUser.Title <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", Title = " & getDBStringPrefix() & "'" & objComputerUser.Title.Replace("'", "") & "' "
                End If

                If objComputerUser.OU <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", OU = " & getDBStringPrefix() & "'" & objComputerUser.OU.Replace("'", "") & "' "
                End If

                If objComputerUser.EMail <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", EMail = " & getDBStringPrefix() & "'" & objComputerUser.EMail.Replace("'", "") & "' "
                End If

                If objComputerUser.UserCode <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", UserCode = " & getDBStringPrefix() & "'" & objComputerUser.UserCode.Replace("'", "") & "' "
                End If

                If objComputerUser.CostCentre <> "" Or iOverwriteFromMasterDirectory = 1 Then
                    sSQL = sSQL & ", CostCentre = " & getDBStringPrefix() & "'" & objComputerUser.CostCentre.Replace("'", "") & "' "
                End If

                If objComputerUser.ManagerComputerUser_Id <> 0 Then
                    sSQL = sSQL & ", ManagerComputerUser_Id=" & objComputerUser.ManagerComputerUser_Id
                Else
                    sSQL = sSQL & ", ManagerComputerUser_Id=Null"
                End If

                sSQL = sSQL & ", NDSPath='" & objComputerUser.NDSPath.Replace("'", "") & "' "
                sSQL = sSQL & ", Status=" & objComputerUser.Status & ", SyncChangedDate=getutcDate()"


                sSQL = sSQL & " WHERE Id=" & objComputerUser.Id
            Else
                sSQL = " INSERT INTO tblComputerUsers(Customer_Id, NDSPath, Department_Id, OU_Id, Domain_Id, UserId, LogonName, FirstName, SurName, Location, Phone, Cellphone, Email, UserCode, PostalAddress, PostalCode, City "

                sSQL = sSQL & ", Status, SyncChangedDate) VALUES(" & objComputerUser.Customer_Id & ", '" & objComputerUser.NDSPath.Replace("'", "") & "' "

                If objComputerUser.Department_Id <> 0 Then
                    sSQL = sSQL & ", " & objComputerUser.Department_Id
                Else
                    sSQL = sSQL & ", Null"
                End If

                If objComputerUser.OU_Id <> 0 Then
                    sSQL = sSQL & ", " & objComputerUser.OU_Id
                Else
                    sSQL = sSQL & ", Null"
                End If

                If objComputerUser.Domain_Id <> 0 Then
                    sSQL = sSQL & ", " & objComputerUser.Domain_Id
                Else
                    sSQL = sSQL & ", Null"
                End If

                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.UserId.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.LogonName.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.FirstName.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.SurName.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.Location.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.Phone.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.CellPhone.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.EMail.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.UserCode.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.PostalAddress.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.PostalCode.Replace("'", "") & "' "
                sSQL = sSQL & ", " & getDBStringPrefix() & "'" & objComputerUser.City.Replace("'", "") & "' "

                sSQL = sSQL & ", " & objComputerUser.Status & ", getutcdate()) "

            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If
        Catch ex As Exception
            Ret = ex.ToString
        End Try

        Return Ret
    End Function
End Class
