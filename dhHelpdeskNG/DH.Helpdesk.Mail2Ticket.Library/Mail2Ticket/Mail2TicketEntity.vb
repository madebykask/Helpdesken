
Public Class Mail2TicketEntity

    Public Sub New(dr As DataRow)        

        Id = dr("Id")
        If Not IsDBNull(dr("Case_Id")) Then
            CaseId = CInt(dr("Case_Id"))
        End If

        If Not IsDBNull(dr("Log_Id")) Then
            LogId = CInt(dr("Log_Id"))
        End If

        If Not IsDBNull(dr("EMailAddress")) Then
            EmailAddress = dr("EMailAddress").ToString()
        End If

        If Not IsDBNull(dr("Type")) Then
            Type = dr("UniqueMessageId").ToString()
        End If
        
        If Not IsDBNull(dr("UniqueMessageId")) Then
            UniqueMessageId = dr("UniqueMessageId").ToString()
        End If

    End Sub
    
    Public Property Id As Integer
    Public Property LogId As Integer?
    Public Property CaseId As Integer?
    Public Property EmailAddress as String
    Public Property Type as String
    Public Property UniqueMessageId As String

End Class
