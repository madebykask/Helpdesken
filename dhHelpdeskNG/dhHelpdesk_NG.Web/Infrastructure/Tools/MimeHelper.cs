namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.IO;

    using Microsoft.Win32;

    public static class MimeHelper
    {
        private const string DefaultMimeType = "application/octet-stream";

        public static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                return DefaultMimeType;
            }

            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);
            if (registryKey == null)
            {
                return DefaultMimeType;
            }

            var mimeType = registryKey.GetValue("Content Type") as string;
            if (string.IsNullOrEmpty(mimeType))
            {
                return DefaultMimeType;
            }

            return mimeType;
        }
    }
}