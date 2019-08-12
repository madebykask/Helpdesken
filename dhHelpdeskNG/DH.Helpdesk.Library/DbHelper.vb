Imports System.Data.SqlClient

Public Class DbHelper

    #Region "Oracle Db Legacy Methods"

    'Public Shared Function getDataTableOracle(ByVal sConnectionstring As String, ByVal sSQL As String) As DataTable
    '    If sConnectionstring.Contains("ODBC") Then
    '        Return getDataTableOracleODBC(sConnectionstring, sSQL)
    '    Else
    '        Dim con As OleDbConnection = New OleDbConnection(sConnectionstring)
    '        Dim cmd As OleDbCommand = con.CreateCommand()
    '        Dim ds As New DataSet
    '        Dim sda As New OleDbDataAdapter()

    '        Try
    '            con.Open()

    '            cmd.CommandType = CommandType.Text
    '            cmd.CommandText = sSQL

    '            ' DataReader
    '            sda.SelectCommand = cmd
    '            sda.Fill(ds)

    '            Return ds.Tables(0)

    '        Catch ex As Exception
    '            If giLoglevel > 0 Then
    '                objLogFile.WriteLine(Now() & ", ERROR getDataTableOracle " & sSQL)
    '            End If

    '            Throw ex
    '        Finally
    '            con.Close()
    '        End Try
    '    End If
    'End Function

    'Private Shared Function getDataTableOracleODBC(ByVal sConnectionstring As String, ByVal sSQL As String) As DataTable
    '    Dim con As OdbcConnection = New OdbcConnection(sConnectionstring)
    '    Dim cmd As OdbcCommand = con.CreateCommand()
    '    Dim ds As New DataSet
    '    Dim sda As New OdbcDataAdapter()

    '    Try
    '        con.Open()

    '        cmd.CommandType = CommandType.Text
    '        cmd.CommandText = sSQL

    '        ' DataReader
    '        sda.SelectCommand = cmd
    '        sda.Fill(ds)

    '        Return ds.Tables(0)

    '    Catch ex As Exception
    '        If giLoglevel > 0 Then
    '            objLogFile.WriteLine(Now() & ", ERROR getDataTableOracleODBC " & sSQL & ", " & ex.Message)
    '        End If

    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    'End Function

    'Public Shared Sub executeSQLOracle(ByVal sConnectionstring As String, ByVal sSQL As String)
    '    If sConnectionstring.Contains("ODBC") Then
    '        executeSQLOracleODBC(sConnectionstring, sSQL)
    '    Else
    '        Dim con As OleDbConnection = New OleDbConnection(sConnectionstring)
    '        Dim cmd As OleDbCommand = con.CreateCommand()

    '        Try
    '            con.Open()

    '            cmd.CommandType = CommandType.Text
    '            cmd.CommandText = sSQL

    '            cmd.ExecuteNonQuery()
    '        Catch ex As Exception
    '            If giLoglevel > 0 Then
    '                objLogFile.WriteLine(Now() & ", ERROR executeSQLOracle " & sSQL)
    '            End If

    '            Throw ex
    '        Finally
    '            con.Close()
    '        End Try
    '    End If
    'End Sub

    'Private Shared Sub executeSQLOracleODBC(ByVal sConnectionstring As String, ByVal sSQL As String)
    '    Dim con As OdbcConnection = New OdbcConnection(sConnectionstring)
    '    Dim cmd As OdbcCommand = con.CreateCommand()

    '    Try
    '        con.Open()

    '        cmd.CommandType = CommandType.Text
    '        cmd.CommandText = sSQL

    '        cmd.ExecuteNonQuery()

    '    Catch ex As Exception
    '        If giLoglevel > 0 Then
    '            objLogFile.WriteLine(Now() & ", ERROR executeSQLOracleODBC " & sSQL)
    '        End If

    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    'End Sub

    #End Region

    Public Shared Function createNullableDbParameter(Of TValue)(name As String, _
                                                        val as TValue, _
                                                        Optional paramType As DbType? = Nothing, _ 
                                                        Optional size As Integer? = Nothing) As SqlParameter
        Return createDbParameter(name, val, true, paramType, size)
    End Function

    Public Shared Function createDbParameter(Of TValue)(name As String, _
                                                         val as TValue, _
                                                         Optional isNullable As Boolean = false,
                                                         Optional paramType As DbType? = Nothing, _ 
                                                         Optional size As Integer? = Nothing) As SqlParameter

        Dim sqlParam as SqlParameter = new SqlParameter(name, val)
        
        'IsNullable 
        If isNullable Then 
            sqlParam.IsNullable = True
            
            'Set DbNull if value is Nothing
            If val Is Nothing Then sqlParam.SetDbNullValue()
        End If

        'DbType
        IF paramType IsNot Nothing AndAlso paramType.HasValue Then
            sqlParam.DbType = paramType.Value
        End If

        'Size
        IF size IsNot Nothing AndAlso size.HasValue Then
            sqlParam.Size = size.Value
        End If

        Return sqlParam
    End Function
    
    Public Shared Function getDataTable(sConnectionstring As String, sSql As String, ParamArray params As SqlParameter()) As DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = New SqlConnection(sConnectionstring)
            Using cmd As SqlCommand = New SqlCommand(sSql, con)
                cmd.CommandType = CommandType.Text
                
                'dhal 161219, chs sync ims ger timeoutfel
                cmd.CommandTimeout = 90

                If (params IsNot Nothing AndAlso params.Length > 0)
                    cmd.Parameters.AddRange(params)
                End If
                
                Dim sda as SqlDataAdapter = New SqlDataAdapter(cmd)

                Try
                    con.Open()

                    'exec query
                    sda.Fill(ds)    

                    Return ds.Tables(0)
                Catch ex As Exception
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", ERROR getDataTable " & sSql)
                    End If
                    'Rethrow
                    Throw
                End Try
            End Using
        End Using
    End Function
    
    Public Shared Function executeNonQuery(sConnectionString As String, sSql As String, commandType As CommandType, ParamArray params As SqlParameter()) As Int32
        Dim rowsAffected = 0
        Using con As SqlConnection = New SqlConnection(sConnectionstring)
            Using cmd As SqlCommand = New SqlCommand(sSql, con)
                cmd.CommandType = commandType
                cmd.CommandTimeout = 240

                If (params IsNot Nothing AndAlso params.Length > 0)
                    cmd.Parameters.AddRange(params)
                End If

                Try
                    con.Open()

                    'exec query
                    rowsAffected = cmd.ExecuteNonQuery()

                Catch ex As Exception
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & Err.ToString())
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & sSQL)
                    End If
                    'Rethrow
                    Throw
                End Try
            End Using
        End Using
        Return rowsAffected
    End Function

    Public Shared Function executeScalarQuery(Of TReturn )(sConnectionString As String, sSql As String, commandType As CommandType, ParamArray params As SqlParameter()) As TReturn
        Dim res As TReturn = CType(Nothing, TReturn)
        Using con As New SqlConnection(sConnectionString) 
            Using cmd As New SqlCommand(sSql, con) 
                cmd.CommandType = commandType 
                cmd.CommandTimeout = 240
                If (params IsNot Nothing AndAlso params.Length > 0)
                    cmd.Parameters.AddRange(params)
                End If
 
                Try
                    con.Open()

                    'exec query and do type cast
                    res = CType(cmd.ExecuteScalar(), TReturn)

                Catch ex As Exception
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & Err.ToString())
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & sSQL)
                    End If
                    'Rethrow
                    Throw  
                End Try
            End Using 
        End Using 
        Return res
    End Function 
End Class
