Imports System.IO

Public Module MGlobal
    Public gsConnectionString As String
    Public gsURL As String = ""
    Public giDBType As DbType
    Public gsLogPath As String
    Public objLogFile As StreamWriter
    Public objErrorLogFile As StreamWriter
    Public giLoglevel As Integer
    Public gsDBVersion As String
    Public giSendMail As Integer = 1

    Public gsInternalLogIdentifier As String
    Public gsAttachedFileFolder As String
    Public gsProductAreaSeperator As String = ";"

End Module
