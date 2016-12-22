using System;
using System.IO;
using System.Web;
using Microsoft.Win32;

namespace DH.Helpdesk.EForm.Core.FileStore
{
    public static class FileStore
    {
        public static bool HasFile(HttpPostedFileBase fileBase)
        {
            return (fileBase != null && fileBase.ContentLength > 0);
        }

        public static bool ValidFileSize(HttpPostedFileBase fileBase, int maxKiloByte)
        {
            return (fileBase.ContentLength / 1024 > maxKiloByte);
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void RemoveFile(string path)
        {
            if(File.Exists(path))
                File.Delete(path);
        }

        public static void Save(HttpPostedFileBase fileBase, string folderPath)
        {
            if(HasFile(fileBase) && string.IsNullOrEmpty(folderPath)) return;

            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            fileBase.SaveAs(Path.Combine(folderPath, fileBase.FileName));
        }

        public static bool Move(string sourcePath, string destinationPath)
        {
            try
            {
                if(!File.Exists(sourcePath))
                    return false;

                var sourceFileName = Path.GetFileName(sourcePath);
                var directory = Path.GetDirectoryName(destinationPath);

                if(!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if(File.Exists(destinationPath))
                    File.Delete(destinationPath);

                File.Move(sourcePath, destinationPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            // if whe have / in path Path.GetFileNameWithoutExtension() wont work as we need it.
            //return Path.GetFileNameWithoutExtension(path);
            return path.Substring(0, path.Length - System.IO.Path.GetExtension(path).Length);
        }

        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetMimeTypeSupport(string path)
        {
            var defaultMimeType = "application/octet-stream";

            var extension = Path.GetExtension(path);
            if(string.IsNullOrEmpty(extension))
                return defaultMimeType;

            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);
            if(registryKey == null)
                return defaultMimeType;

            var mimeType = registryKey.GetValue("Content Type") as string;
            if(string.IsNullOrEmpty(mimeType))
                return defaultMimeType;

            return mimeType;

            /*
            switch(Path.GetExtension(path))
            {
                case ".ai":
                case ".eps":
                    return "application/postscript";
                case ".psd":
                    return "application/psd";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".tif":
                case ".tiff":
                    return "image/tiff";
                case ".bmp":
                    return "image/bmp";
                case ".doc":
                case ".docx":
                    return "application/msword";
                case ".xls":
                case ".xlsx":
                case ".xlt":
                case ".xltx":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".pps":
                case ".ppt":
                case ".ppsx":
                case ".pptx":
                    return "application/vnd.ms-powerpoint";
                case ".msg":
                    return "application/msoutlook";
                case ".txt":
                    return "text/plain";
                case ".rtx":
                    return "text/richtext";
                case ".pdf":
                    return "application/pdf";
                case ".mp2":
                case ".mp3":
                case ".mp4":
                case ".mpa":
                case ".mpe":
                case ".mpeg":
                case ".mpg":
                case ".mpv2":
                case ".wmv":
                    return "video/mpeg";
                case ".mov":
                case ".qt":
                    return "video/quicktime";
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/rar";
                default:
                    return "image/jpeg";
            }*/
        }
    }
}
