Imports System.Reflection

Public Class CurrentAssemblyInfo

    Public Shared ReadOnly Property Version() As String
        Get
            Return AssemblyVersion()
        End Get
    End Property

    Private Shared Function AssemblyVersion() As String
        Dim assembly As Assembly = Assembly.GetExecutingAssembly()
        Dim FileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location)
        Return FileVersionInfo.ProductVersion
    End Function

End Class