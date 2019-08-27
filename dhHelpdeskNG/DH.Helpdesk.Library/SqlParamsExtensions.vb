Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

Module SqlParamsExtensions

    <Extension()> 
    Public Function ToNullable(sqlParam As SqlParameter) as SqlParameter
        sqlParam.IsNullable = True
        Return sqlParam
    End Function
    
    <Extension()> 
    Public Function SetDbNullValue(sqlParam As SqlParameter) as SqlParameter
        IF (sqlParam.IsNullable)
            sqlParam.Value = DBNull.Value
        Else 
            Throw New ArgumentException("Parameter provided is not nullable", "sqlParam")
        End If
        Return sqlParam
    End Function

End Module