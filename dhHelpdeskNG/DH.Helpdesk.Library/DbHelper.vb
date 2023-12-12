Imports System.Data.SqlClient

Public Class DbHelper

    Public Shared Function createNullableDbParameter(Of TValue)(name As String,
                                                        val As TValue,
                                                        Optional paramType As DbType? = Nothing,
                                                        Optional size As Integer? = Nothing) As SqlParameter
        Return createDbParameter(name, val, True, paramType, size)
    End Function

    Public Shared Function createDbParameter(Of TValue)(name As String,
                                                         val As TValue,
                                                         Optional isNullable As Boolean = False,
                                                         Optional paramType As DbType? = Nothing,
                                                         Optional size As Integer? = Nothing) As SqlParameter

        Dim sqlParam As SqlParameter = New SqlParameter(name, val)

        'IsNullable 
        If isNullable Then
            sqlParam.IsNullable = True

            'Set DbNull if value is Nothing
            If val Is Nothing Then sqlParam.SetDbNullValue()
        End If

        'DbType
        If paramType IsNot Nothing AndAlso paramType.HasValue Then
            sqlParam.DbType = paramType.Value
        End If

        'Size
        If size IsNot Nothing AndAlso size.HasValue Then
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

                If (params IsNot Nothing AndAlso params.Length > 0) Then
                    cmd.Parameters.AddRange(params)
                End If

                Dim sda As SqlDataAdapter = New SqlDataAdapter(cmd)

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
        Using con As SqlConnection = New SqlConnection(sConnectionString)
            Using cmd As SqlCommand = New SqlCommand(sSql, con)
                cmd.CommandType = commandType
                cmd.CommandTimeout = 240

                If (params IsNot Nothing AndAlso params.Length > 0) Then
                    cmd.Parameters.AddRange(params)
                End If

                Try
                    con.Open()

                    'exec query
                    rowsAffected = cmd.ExecuteNonQuery()

                Catch ex As Exception
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & Err.ToString())
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & sSql)
                    End If
                    'Rethrow
                    Throw
                End Try
            End Using
        End Using
        Return rowsAffected
    End Function

    Public Shared Function executeScalarQuery(Of TReturn)(sConnectionString As String, sSql As String, commandType As CommandType, ParamArray params As SqlParameter()) As TReturn
        Dim res As TReturn = CType(Nothing, TReturn)
        Using con As New SqlConnection(sConnectionString)
            Using cmd As New SqlCommand(sSql, con)
                cmd.CommandType = commandType
                cmd.CommandTimeout = 240
                If (params IsNot Nothing AndAlso params.Length > 0) Then
                    cmd.Parameters.AddRange(params)
                End If

                Try
                    con.Open()

                    'exec query and do type cast
                    res = CType(cmd.ExecuteScalar(), TReturn)

                Catch ex As Exception
                    If giLoglevel > 0 Then
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & Err.ToString())
                        objLogFile.WriteLine(Now() & ", ERROR executeNonQuery " & sSql)
                    End If
                    'Rethrow
                    Throw
                End Try
            End Using
        End Using
        Return res
    End Function
End Class
