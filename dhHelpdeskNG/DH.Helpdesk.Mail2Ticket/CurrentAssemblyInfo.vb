Imports System.Reflection

Public Class CurrentAssemblyInfo

    Private Shared Readonly _currentAssembly As Assembly = Assembly.GetExecutingAssembly()
    
    Public Shared ReadOnly Property Version() As String
        Get
            Return _currentAssembly.GetName().Version.ToString()
        End Get
    End Property
                                                                
End Class