using System.Reflection;
using System.Diagnostics;

[assembly: AssemblyCompany("DH Solutions")]
[assembly: AssemblyCopyright("© 2022 DH Solutions")]
[assembly: AssemblyVersion("5.3.55.0")]
[assembly: AssemblyFileVersion("5.3.55.0")]
[assembly: AssemblyInformationalVersion("5.3.55.Debug")]

namespace DH.Helpdesk
{
    public static class Version
    {
        public static string FULL_VERSION = AssemblyVersion();

        public static string AssemblyVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }
    }
}
