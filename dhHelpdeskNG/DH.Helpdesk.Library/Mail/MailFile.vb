Public Class MailFile 

    Public Sub New(sFileName As String, sFilePath As String, Optional bIsInternal As Boolean = False)
        FileName = sFileName
        FilePath = sFilePath
        IsInternal = bIsInternal
    End Sub

    Public Property FileName as String
    Public Property FilePath as String
    Public Property IsInternal as String
End Class