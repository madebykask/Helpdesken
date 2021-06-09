namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Web;
    using System.Linq;

    public static class FileExtensions
    {
        public static bool HasFile(this HttpPostedFileBase postedFile)
        {
            return postedFile != null && postedFile.ContentLength > 0;
        }

        public static byte[] GetFileContent(this HttpPostedFileBase postedFile)
        {
            byte[] fileContent = null;
            if (postedFile.HasFile())
            {
                fileContent = new byte[postedFile.InputStream.Length];
                postedFile.InputStream.Read(fileContent, 0, fileContent.Length);
            }
            return fileContent;
        }

        public static bool DeleteFile(this string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                if (System.IO.File.Exists(s))
                    System.IO.File.Delete(s);
            }
            return true;
        }
        public static bool IsImage(this string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                return new[] { "png", "jpg", "jpeg", "gif", "tiff", "eps" }.Any(x => s.ToLower().EndsWith(x));
            }
            return false;
        }
    }    
}