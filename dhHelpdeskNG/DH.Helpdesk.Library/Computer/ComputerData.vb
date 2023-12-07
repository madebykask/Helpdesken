Imports System.Data.SqlClient
Imports DH.Helpdesk.Library.SharedFunctions

Public Class ComputerData
    Public Function getComputerByName(ByVal sComputerName As String, ByVal iCustomer_Id As Integer, ByVal iComputerRole As SharedFunctions.ComputerRole) As Computer
        Dim sSQL As String
        Dim dt As DataTable


        Try
            If iComputerRole = ComputerRole.PC Then
                sSQL = "SELECT c.Id, c.ComputerName, c.Customer_Id " & _
                       "FROM tblComputer c " & _
                       "WHERE Customer_Id = " & iCustomer_Id & _
                       " AND UPPER(ComputerName) = '" & UCase(sComputerName) & "'"
            Else
                sSQL = "SELECT c.Id, c.ServerName AS ComputerName, c.Customer_Id  " & _
                                       "FROM tblServer c " & _
                                       "WHERE Customer_Id = " & iCustomer_Id & _
                                       " AND UPPER(ServerName) = '" & UCase(sComputerName) & "'"
            End If

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", ComputerData.getComputerByName, " & sSQL)
            End If

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim c As Computer

                c = New Computer(dt.Rows(0))

                Return c
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function existsObject(ByVal iObjectType As SharedFunctions.ObjectType, ByVal sName As String) As Integer
        'If giDBType = 0 Then

        Dim con As SqlConnection = New SqlConnection(gsConnectionString)
            Dim cmd As New SqlCommand

            con.Open()

            Try
                cmd.Connection = con

                Select Case iObjectType
                    Case SharedFunctions.ObjectType.OS
                        cmd.CommandText = "dhexistsOs"
                    Case SharedFunctions.ObjectType.Processor
                        cmd.CommandText = "dhExistsProcessor"
                    Case SharedFunctions.ObjectType.RAM
                        cmd.CommandText = "dhExistsRam"
                    Case SharedFunctions.ObjectType.ComputerModel
                        cmd.CommandText = "dhExistsComputerModel"
                    Case SharedFunctions.ObjectType.NIC
                        cmd.CommandText = "dhExistsNetworkAdapter"
                End Select

                cmd.CommandType = CommandType.StoredProcedure

                Select Case iObjectType
                    Case SharedFunctions.ObjectType.ComputerModel
                        cmd.Parameters.Add(New SqlParameter("@Namn", SqlDbType.NVarChar, 100)).Value = sName
                    Case Else
                        cmd.Parameters.Add(New SqlParameter("@Namn", SqlDbType.NVarChar, 50)).Value = sName
                End Select

                cmd.Parameters.Add("@RETURN", SqlDbType.Int)
                cmd.Parameters("@RETURN").Direction = ParameterDirection.ReturnValue

                cmd.ExecuteScalar()

                Return cmd.Parameters("@RETURN").Value
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        'Else
        '    Return existsObjectODBC(iObjectType, sName)
        'End If
    End Function


    'Private Function existsObjectODBC(ByVal iObjectType As SharedFunctions.ObjectType, ByVal sName As String) As Integer
    '    Dim con As OdbcConnection = New OdbcConnection(gsConnectionString)
    '    Dim cmd As New OdbcCommand

    '    con.Open()

    '    Try
    '        cmd.Connection = con

    '        Select Case iObjectType
    '            Case SharedFunctions.ObjectType.OS
    '                cmd.CommandText = "{? = CALL dhexistsOs(?)}"
    '            Case SharedFunctions.ObjectType.Processor
    '                cmd.CommandText = "{? = CALL dhExistsProcessor(?)}"
    '            Case SharedFunctions.ObjectType.RAM
    '                cmd.CommandText = "{? = CALL dhExistsRam(?)}"
    '            Case SharedFunctions.ObjectType.ComputerModel
    '                cmd.CommandText = "{? = CALL dhExistsComputerModel(?)}"
    '            Case SharedFunctions.ObjectType.NIC
    '                cmd.CommandText = "{? = CALL dhExistsNetworkAdapter(?)}"
    '        End Select

    '        cmd.CommandType = CommandType.StoredProcedure

    '        cmd.Parameters.Add("@RETURN_VALUE", OdbcType.Numeric, 4)
    '        cmd.Parameters("@RETURN_VALUE").Direction = ParameterDirection.ReturnValue

    '        Select Case iObjectType
    '            Case SharedFunctions.ObjectType.ComputerModel
    '                cmd.Parameters.Add(New OdbcParameter("@Namn", OdbcType.VarChar, 100)).Value = sName
    '            Case Else
    '                cmd.Parameters.Add(New OdbcParameter("@Namn", OdbcType.VarChar, 50)).Value = sName
    '        End Select



    '        cmd.ExecuteNonQuery()


    '        Return CType(cmd.Parameters("@RETURN_VALUE").Value, Integer)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    'End Function

    Function getChassisTypeName(ByVal iChassisType As Long) As String
        Select Case iChassisType
            Case 1
                getChassisTypeName = "Other"
            Case 2
                getChassisTypeName = "Unknown"
            Case 3
                getChassisTypeName = "Desktop"
            Case 4
                getChassisTypeName = "Low Profile Desktop"
            Case 5
                getChassisTypeName = "Pizza Box"
            Case 6
                getChassisTypeName = "Mini Tower"
            Case 7
                getChassisTypeName = "Tower"
            Case 8
                getChassisTypeName = "Portable"
            Case 9
                getChassisTypeName = "Laptop"
            Case 10
                getChassisTypeName = "Notebook"
            Case 11
                getChassisTypeName = "Hand Held"
            Case 12
                getChassisTypeName = "Docking Station"
            Case 13
                getChassisTypeName = "All in One"
            Case 14
                getChassisTypeName = "Sub Notebook"
            Case 15
                getChassisTypeName = "Space-Saving"
            Case 16
                getChassisTypeName = "Lunch Box"
            Case 17
                getChassisTypeName = "Main System Chassis"
            Case 18
                getChassisTypeName = "Expansion Chassis"
            Case 19
                getChassisTypeName = "Sub Chassis"
            Case 20
                getChassisTypeName = "Bus Expansion Chassis"
            Case 21
                getChassisTypeName = "Peripheral Chassis"
            Case 22
                getChassisTypeName = "Storage Chassis"
            Case 23
                getChassisTypeName = "Rack Mount Chassis"
            Case 24
                getChassisTypeName = "Sealed-Case PC"
            Case Else
                getChassisTypeName = ""

        End Select
    End Function

    Public Function getFileSystem(ByVal iFileSystem As Integer) As String
        Select Case iFileSystem
            Case 0
                Return "Unknown"
            Case 1
                Return "FAT"
            Case 2
                Return "FAT 32"
            Case 3
                Return "NTFS"
            Case 4
                Return "HPFS"
            Case 5
                Return "NetWare"
            Case 6
                Return "Xenix"
            Case 7
                Return "Linux"
            Case 8
                Return "UNIX"
            Case 9
                Return "AIX"
            Case 10
                Return "Mac"
            Case 11
                Return "Hidden"
            Case Else
                Return ""
        End Select

    End Function

    Public Function getRAM(ByVal dblRAM As Double) As String
        If dblRAM > 12000000000 Then
            Return "12 GB"
        ElseIf dblRAM > 11000000000 Then
            Return "11 GB"
        ElseIf dblRAM > 10000000000 Then
            Return "10 GB"
        ElseIf dblRAM > 9000000000 Then
            Return "9 GB"
        ElseIf dblRAM > 8000000000 Then
            Return "8 GB"
        ElseIf dblRAM > 7000000000 Then
            Return "7 GB"
        ElseIf dblRAM > 6000000000 Then
            Return "6 GB"
        ElseIf dblRAM > 5000000000 Then
            Return "5 GB"
        ElseIf dblRAM > 4000000000 Then
            Return "4 GB"
        ElseIf dblRAM > 3900000000 Then
            Return "3,9 GB"
        ElseIf dblRAM > 3800000000 Then
            Return "3,8 GB"
        ElseIf dblRAM > 3700000000 Then
            Return "3,7 GB"
        ElseIf dblRAM > 3600000000 Then
            Return "3,6 GB"
        ElseIf dblRAM > 3500000000 Then
            Return "3,5 GB"
        ElseIf dblRAM > 3400000000 Then
            Return "3,4 GB"
        ElseIf dblRAM > 3300000000 Then
            Return "3,3 GB"
        ElseIf dblRAM > 3200000000 Then
            Return "3,2 GB"
        ElseIf dblRAM > 3100000000 Then
            Return "3,1 GB"
        ElseIf dblRAM > 3000000000 Then
            Return "3 GB"
        ElseIf dblRAM > 2900000000 Then
            Return "2,9 GB"
        ElseIf dblRAM > 2800000000 Then
            Return "2,8 GB"
        ElseIf dblRAM > 2700000000 Then
            Return "2,7 GB"
        ElseIf dblRAM > 2600000000 Then
            Return "2,6 GB"
        ElseIf dblRAM > 2500000000 Then
            Return "2,5 GB"
        ElseIf dblRAM > 2400000000 Then
            Return "2,4 GB"
        ElseIf dblRAM > 2300000000 Then
            Return "2,3 GB"
        ElseIf dblRAM > 2200000000 Then
            Return "2,2 GB"
        ElseIf dblRAM > 2100000000 Then
            Return "2,1 GB"
        ElseIf dblRAM > 2000000000 Then
            Return "2 GB"
        ElseIf dblRAM > 1900000000 Then
            Return "1,9 GB"
        ElseIf dblRAM > 1800000000 Then
            Return "1,8 GB"
        ElseIf dblRAM > 1700000000 Then
            Return "1,7 GB"
        ElseIf dblRAM > 1600000000 Then
            Return "1,6 GB"
        ElseIf dblRAM > 1500000000 Then
            Return "1,5 GB"
        ElseIf dblRAM > 1400000000 Then
            Return "1,4 GB"
        ElseIf dblRAM > 1300000000 Then
            Return "1,3 GB"
        ElseIf dblRAM > 1200000000 Then
            Return "1,2 GB"
        ElseIf dblRAM > 1100000000 Then
            Return "1,1 GB"
        ElseIf dblRAM > 1000000000 Then
            Return "1 GB"
        ElseIf dblRAM > 700000000 Then
            Return "768 MB"
        ElseIf dblRAM > 500000000 Then
            Return "512 MB"
        ElseIf dblRAM > 250000000 Then
            Return "256 MB"
        ElseIf dblRAM > 120000000 Then
            Return "128 MB"
        Else
            Return "64 MB"
        End If

    End Function

    Public Function getComputerType(ByVal sPath As String) As Integer
        If InStr(1, sPath, "/Anpassad/", CompareMethod.Text) <> 0 Then
            Return 1
        ElseIf InStr(1, sPath, "/Bas/", CompareMethod.Text) <> 0 Then
            Return 2
        ElseIf InStr(1, sPath, "/Mellan/", CompareMethod.Text) <> 0 Then
            Return 3
        Else
            getComputerType = 0
        End If
    End Function

    Public Function getXMLElement(ByVal iCustomer_Id As Integer, ByVal sXMLElement As String) As String
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT XMLElement " & _
                    "FROM tblComputerFieldSettings " & _
                    "WHERE LOWER(ComputerField) = '" & LCase(sXMLElement) & "'"

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)

                If Not IsDBNull(dr("XMLElement")) Then
                    Return dr("XMLElement")
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function create(ByVal ComputerName As String, ByVal iComputerRole As SharedFunctions.ComputerRole, ByVal iCustomer_id As Integer) As Computer
        Dim sSQL As String = ""

        Try
            sSQL = "INSERT INTO "

            If iComputerRole = ComputerRole.PC Then
                sSQL = sSQL & "tblComputer(ComputerName, "
            Else
                sSQL = sSQL & "tblServer(ServerName, "
            End If

            sSQL = sSQL & "Customer_Id"


            sSQL = sSQL & ") "

            sSQL = sSQL & "VALUES('" & ComputerName & "', " & iCustomer_id

            sSQL = sSQL & ") "

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", ComputerData.create, " & sSQL)
            End If

            executeSQL(gsConnectionString, sSQL)

            Return getComputerByName(ComputerName, iCustomer_id, iComputerRole)
        Catch ex As Exception
            Throw ex

        End Try
    End Function

    Public Function save(ByVal objComputer As Computer) As Integer
        Dim sSQL As String = ""

        Try
            If objComputer.ComputerRole = ComputerRole.PC Then
                sSQL = "UPDATE tblComputer SET "
            Else
                sSQL = "UPDATE tblServer SET "
            End If

            sSQL = sSQL & "ScanDate=" & SharedFunctions.convertDateTime(objComputer.ScanDate, giDBType) 

            If objComputer.OS_Id <> 0 Then
                If objComputer.ComputerRole = ComputerRole.PC Then
                    sSQL = sSQL & ", OS_Id=" & objComputer.OS_Id
                Else
                    sSQL = sSQL & ", OperatingSystem_Id=" & objComputer.OS_Id
                End If
            End If

            sSQL = sSQL & ", SP='" & objComputer.OS_Servicepack & "' " & _
                          ", Version='" & objComputer.OS_Version & "' "

            If objComputer.RAM_Id <> 0 Then
                sSQL = sSQL & ", RAM_Id=" & objComputer.RAM_Id
            Else
                sSQL = sSQL & ", RAM_Id=Null"
            End If

            sSQL = sSQL & ", SerialNumber='" & objComputer.SerialNumber & "' "

            If objComputer.ComputerRole = ComputerRole.PC Then
                sSQL = sSQL & ", BIOSVersion='" & objComputer.BIOSVersion & "' "

                If IsDate(objComputer.BIOSDate) Then
                    sSQL = sSQL & ", BiosDate=" & SharedFunctions.convertDateTime(objComputer.BIOSDate, giDBType)
                End If

                sSQL = sSQL & ", Soundcard='" & Replace(objComputer.SoundCard, "'", "''") & "' " & _
                              ", VideoCard='" & objComputer.VideoCard & "' "

            End If

            sSQL = sSQL & ", ChassisType='" & objComputer.ChassisType & "'"

            If objComputer.ComputerRole = ComputerRole.PC Then
                If objComputer.User_Id <> 0 Then
                    sSQL = sSQL & ", User_Id=" & objComputer.User_Id
                Else
                    sSQL = sSQL & ", User_Id=Null "
                End If
            End If

            If objComputer.Processor_Id <> 0 Then
                sSQL = sSQL & ", Processor_Id=" & objComputer.Processor_Id
            End If

            If objComputer.ComputerRole = ComputerRole.PC Then
                sSQL = sSQL & ", ProcessorInfo='" & objComputer.ProcessorInfo & "' "
            End If

            sSQL = sSQL & ", IPAddress='" & objComputer.IPAddress & "' " & _
                          ", MACAddress='" & objComputer.MACAddress & "' "

            If objComputer.ComputerRole = ComputerRole.PC Then
                sSQL = sSQL & ", RAS=" & objComputer.RAS
            End If

            If objComputer.ComputerModel_Id <> 0 And objComputer.ComputerRole = ComputerRole.PC Then
                sSQL = sSQL & ", ComputerModel_Id=" & objComputer.ComputerModel_Id
            ElseIf objComputer.ComputerModel <> "" And objComputer.ComputerRole = ComputerRole.SERVER Then
                sSQL = sSQL & ", ServerModel='" & objComputer.ComputerModel & "'"
            End If

            If objComputer.NIC_Id <> 0 Then
                sSQL = sSQL & ", NIC_Id=" & objComputer.NIC_Id
            End If

            sSQL = sSQL & ", Manufacturer='" & objComputer.Manufacturer & "' "

            If objComputer.RegistrationCode <> "" Then
                sSQL = sSQL & ", RegistrationCode='" & objComputer.RegistrationCode & "' "
            End If

            If objComputer.ProductKey <> "" Then
                sSQL = sSQL & ", ProductKey='" & objComputer.ProductKey & "' "
            End If

            If objComputer.ComputerRole = ComputerRole.PC Then
                If objComputer.CarePackNumber <> "" Then
                    sSQL = sSQL & ", CarePackNumber='" & objComputer.CarePackNumber & "' "
                End If

                If objComputer.Location <> "" Then
                    sSQL = sSQL & ", Location='" & objComputer.Location & "' "
                End If

                If objComputer.Domain_Id <> 0 Then
                    sSQL = sSQL & ", Domain_Id=" & objComputer.Domain_Id
                End If

                If objComputer.Department_Id <> 0 Then
                    sSQL = sSQL & ", Department_Id=" & objComputer.Department_Id & " "
                End If

                If objComputer.ComputerType_Id <> 0 Then
                    sSQL = sSQL & ", ComputerType_Id=" & objComputer.ComputerType_Id
                End If
            End If

            sSQL = sSQL & ", SyncChangedDate=getutcDate() "

            sSQL = sSQL & " WHERE Id=" & objComputer.Id

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            ' Läs in program
            If objComputer.ComputerRole = ComputerRole.PC Then
                ' Clear all software
                sSQL = "DELETE FROM tblSoftware WHERE Computer_Id=" & objComputer.Id
            Else
                sSQL = "DELETE FROM tblServerSoftware WHERE Server_Id=" & objComputer.Id
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            For Each objSoftware As Software In objComputer.Software
                If objComputer.ComputerRole = ComputerRole.PC Then
                    sSQL = "INSERT INTO tblSoftware (Computer_Id, "
                Else
                    sSQL = "INSERT INTO tblServerSoftware (Server_Id, "
                End If

                sSQL = sSQL & "Name, Manufacturer, Install_directory, Version, Registration_code, Product_key)"

                sSQL = sSQL & "VALUES(" & _
                                objComputer.Id & ", '" & _
                                Left(Replace(objSoftware.Name, "'", ""), 100) & "', '" & _
                                Replace(objSoftware.Manufacturer, "'", "") & "', '" & _
                                objSoftware.Install_directory & "', '" & _
                                objSoftware.Version & "', '" & _
                                objSoftware.RegistrationCode & "', '" & _
                                objSoftware.ProductKey & "')"

                If giLoglevel = 2 Then
                    objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
                End If

                If gsURL = "" Then
                    executeSQL(gsConnectionString, sSQL)
                Else
                    executeSQLHTTP(sSQL)
                End If
            Next

            ' Läs in program
            If objComputer.ComputerRole = ComputerRole.PC Then
                ' Clear all software
                sSQL = "DELETE FROM tblLogicalDrive WHERE Computer_Id=" & objComputer.Id
            Else
                sSQL = "DELETE FROM tblServerLogicalDrive WHERE Server_Id=" & objComputer.Id
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            For Each objLogicalDrive As LogicalDrive In objComputer.LogicalDrives
                If objComputer.ComputerRole = ComputerRole.PC Then
                    sSQL = "INSERT INTO tblLogicalDrive(Computer_Id, "
                Else
                    sSQL = "INSERT INTO tblServerLogicalDrive(Server_Id, "
                End If

                sSQL = sSQL & "DriveLetter, DriveType, TotalBytes, FreeBytes, FileSystemName) " & _
                                "VALUES(" & objComputer.Id & ", '" & _
                                            objLogicalDrive.DriveLetter & "', " & _
                                            objLogicalDrive.DriveType & ", " & _
                                            Replace(objLogicalDrive.TotalBytes, ",", ".") & ", " & _
                                            Replace(objLogicalDrive.FreeBytes, ",", ".") & ", '" & _
                                            objLogicalDrive.FileSystemName & "')"


                If giLoglevel = 2 Then
                    objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
                End If

                If gsURL = "" Then
                    executeSQL(gsConnectionString, sSQL)
                Else
                    executeSQLHTTP(sSQL)
                End If
            Next

            If objComputer.ComputerRole = ComputerRole.PC Then
                ' Hämta inventarietyper
                Dim objComputerData As New ComputerData
                Dim colInventoryType As Collection = objComputerData.getInventoryTypes(objComputer.Customer_Id)

                For Each objInventoryType As InventoryType In colInventoryType
                    ' Ta bort kopplingar
                    sSQL = "DELETE FROM tblComputer_tblInventory WHERE Computer_Id=" & objComputer.Id & " AND Inventory_Id IN (SELECT tblInventory.Id FROM tblInventory INNER JOIN tblComputer_tblInventory ON tblInventory.Id = tblComputer_tblInventory.Inventory_Id WHERE Computer_Id=" & objComputer.Id & " AND tblInventory.InventoryType_Id=" & objInventoryType.Id & ")"

                    If gsURL = "" Then
                        executeSQL(gsConnectionString, sSQL)
                    Else
                        executeSQLHTTP(sSQL)
                    End If
                Next

                For Each objInventory As Inventory In objComputer.Accessories
                    objInventory.Id = inventoryExists(objComputer.Customer_Id, objInventory)
                    If objInventory.Id <> 0 Then
                        sSQL = "UPDATE tblInventory SET " & _
                                    "InventoryName='" & objInventory.InventoryName & "', " & _
                                    "InventoryModel='" & objInventory.InventoryModel & "', " & _
                                    "Manufacturer='" & objInventory.Manufacturer & "', " & _
                                    "SerialNumber='" & objInventory.SerialNumber & "', " & _
                                    "SyncChangedDate=getutcDate() " & _
                                "WHERE Id=" & objInventory.Id

                        If giLoglevel = 2 Then
                            objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
                        End If

                        If gsURL = "" Then
                            executeSQL(gsConnectionString, sSQL)
                        Else
                            executeSQLHTTP(sSQL)
                        End If
                    Else
                        sSQL = "INSERT INTO tblInventory(InventoryType_Id, InventoryName, InventoryModel, Manufacturer, SerialNumber, SyncChangedDate) VALUES(" & _
                                    objInventory.InventoryType_Id & ", '" & _
                                    objInventory.InventoryName & "', '" & _
                                    objInventory.InventoryModel & "', '" & _
                                    objInventory.Manufacturer & "', '" & _
                                    objInventory.SerialNumber & "', getutcDate())"

                        If giLoglevel = 2 Then
                            objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
                        End If

                        If gsURL = "" Then
                            executeSQL(gsConnectionString, sSQL)
                        Else
                            executeSQLHTTP(sSQL)
                        End If

                        objInventory.Id = inventoryExists(objComputer.Customer_Id, objInventory)
                    End If

                    ' Skapa koppling till inventarie
                    sSQL = "INSERT INTO tblComputer_tblInventory(Computer_Id, Inventory_Id) VALUES(" & objComputer.Id & ", " & objInventory.Id & ")"

                    If giLoglevel = 2 Then
                        objLogFile.WriteLine(Now() & ", ComputerData.save, " & sSQL)
                    End If

                    If gsURL = "" Then
                        executeSQL(gsConnectionString, sSQL)
                    Else
                        executeSQLHTTP(sSQL)
                    End If
                Next
            End If
        Catch ex As Exception
            If giLoglevel > 0 Then
                objLogFile.WriteLine(Now() & ", ERROR " & sSQL)
                objLogFile.WriteLine(Now() & ", ERROR " & ex.Message.ToString)
            End If

            Throw ex

        End Try
    End Function

    Public Function getInventoryTypes(ByVal iCustomer_Id As Integer) As Collection
        Dim colInventoryTypes As New Collection
        Dim sSQL As String
        Dim dt As DataTable
        Dim dr As DataRow


        Try
            sSQL = "SELECT Id, InventoryType, XMLElement FROM tblInventoryType " & _
                    "WHERE Customer_Id=" & iCustomer_Id & _
                        " AND XMLElement IS NOT NULL "

            dt = getDataTable(gsConnectionString, sSQL)

            Dim it As InventoryType

            For Each dr In dt.Rows
                it = New InventoryType(dr)

                colInventoryTypes.Add(it)
            Next

            Return colInventoryTypes

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function checkMacAddress(ByVal objComputer As Computer) As Integer
        Dim sSQL As String
        Dim dt As DataTable
        If objComputer.MACAddress <> "" Then
            sSQL = "SELECT tblComputer.MACAddress " &
                    "FROM tblComputer WHERE LOWER(tblComputer.MACAddress)= '" & LCase(objComputer.MACAddress) & "' "

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Return 1
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function

    Private Function inventoryExists(ByVal iCustomer_Id As Integer, ByVal objInventory As Inventory) As Integer
        Dim sSQL As String
        Dim dt As DataTable

        Try
            sSQL = "SELECT tblInventory.Id " & _
                    "FROM tblInventory " & _
                        "INNER JOIN tblInventoryType ON tblInventory.InventoryType_id=tblInventoryType.Id " & _
                    "WHERE InventoryType_Id=" & objInventory.InventoryType_Id & _
                        " AND Customer_Id=" & iCustomer_Id

            If objInventory.SerialNumber <> "" Then
                sSQL = sSQL & " AND LOWER(SerialNumber) = '" & LCase(objInventory.SerialNumber) & "' "
            Else
                sSQL = sSQL & " AND LOWER(InventoryModel) = '" & LCase(objInventory.InventoryModel) & "' "
            End If

            dt = getDataTable(gsConnectionString, sSQL)

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)

                Return dr("Id")
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub DeleteComputers(ByVal iCustomer_Id As Integer, ByVal iDomain_Id As Integer, ByVal iInventoryDays2WaitBeforeDelete As Integer)
        Dim sSQL As String = ""
        Dim sSQLWHERE As String

        Try
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, InventoryDays2WaitBeforeDelete " & iInventoryDays2WaitBeforeDelete)
            End If

            ' Nollställ koppling datorer
            sSQLWHERE = "SELECT Id FROM tblComputer WHERE SyncChangedDate IS NOT NULL AND Customer_Id=" & iCustomer_Id

            If iDomain_Id <> 0 Then
                sSQLWHERE = sSQLWHERE & " AND Domain_Id=" & iDomain_Id
            End If

            sSQLWHERE = sSQLWHERE & " AND (ScanDate < " & SharedFunctions.convertDateTime(DateAdd(DateInterval.Day, -1 * iInventoryDays2WaitBeforeDelete, Now()), giDBType) & ") "

            sSQL = "DELETE FROM tblSoftware WHERE Computer_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "DELETE FROM tblLogicalDrive WHERE Computer_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "DELETE FROM tblComputerLog WHERE Computer_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "DELETE FROM tblComputer_tblInventory WHERE Computer_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "DELETE FROM tblComputer_History WHERE Computer_Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            sSQL = "DELETE FROM tblComputer WHERE Id IN (" & sSQLWHERE & ")"

            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", DeleteComputers, " & sSQL)
            End If

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If
        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, DeleteComputers " & ex.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Sub


    Public Sub UpdateApplication(ByVal iCustomer_Id As Integer)
        Dim sSQL As String = ""
        
        Try
            ' Ta bort gamla program som inte används
            sSQL = "DELETE FROM tblApplication WHERE Id NOT IN (SELECT Application_Id FROM tblProduct_tblApplication)"

            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

            ' Lägg in alla program som inventeras i licensmodulen
            If giDBType = 0 Then
                sSQL = "INSERT INTO tblApplication(Customer_Id, Application) " & _
                            "SELECT DISTINCT " & iCustomer_Id & ", Name FROM tblSoftware WHERE Name Not IN (SELECT Application FROM tblApplication WHERE Customer_Id=" & iCustomer_Id & ")"
            Else
                sSQL = "INSERT INTO tblApplication(Customer_Id, Application) " & _
                            "SELECT DISTINCT " & iCustomer_Id & ", Name FROM tblSoftware WHERE LOWER(Name) Not IN (SELECT LOWER(Application) FROM tblApplication WHERE Customer_Id=" & iCustomer_Id & ")"
            End If


            If gsURL = "" Then
                executeSQL(gsConnectionString, sSQL)
            Else
                executeSQLHTTP(sSQL)
            End If

        Catch ex As Exception
            If giLoglevel = 1 Then
                objLogFile.WriteLine(Now() & ", Error, UpdateApplication " & ex.ToString & ", " & sSQL)
            End If

            Throw ex
        End Try
    End Sub
End Class
