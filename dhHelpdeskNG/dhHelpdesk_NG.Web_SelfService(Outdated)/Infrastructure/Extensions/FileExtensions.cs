namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System;
    using System.Web;

    public static class FileExtensions
    {
        public static bool HasFile(this HttpPostedFileBase fileUploadedName)
        {
            return (fileUploadedName != null && fileUploadedName.ContentLength > 0) ? true : false;
        }

        public static Boolean DeleteFile(this string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                if (System.IO.File.Exists(s))
                {
                    System.IO.File.Delete(s);
                }
            }

            return true;
        }
    }    
}