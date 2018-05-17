using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Components
{
    public class FtpFileDownloader : IFtpFileDownloader
    {
        public List<string> GetFileList(string url, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new NullReferenceException("url");

            var res = new List<string>();

            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(userName, password);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                //TODO: handle response errors
                //Console.WriteLine("Download Complete, status code {0}"response.StatusCode);
                //Console.WriteLine("Download Complete, status message {0}", response.StatusDescription);

                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    return res;

                using (var reader = new StreamReader(responseStream))
                {
                    while (!reader.EndOfStream)
                    {
                        res.Add(reader.ReadLine());
                    }
                }
            }

            return res;
        }

        public long Download(string url, string userName, string password, string saveFolder, out string filePath)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new NullReferenceException("url");
            if (string.IsNullOrWhiteSpace(saveFolder))
                throw new NullReferenceException("saveFolder");

            //var fileContent = "";
            long totalBytes = 0;
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential(userName, password);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                //TODO: handle response errors
                //Console.WriteLine("Download Complete, status code {0}"response.StatusCode);
                //Console.WriteLine("Download Complete, status message {0}", response.StatusDescription);

                var responseStream = response.GetResponseStream();
                var buffer = new byte[4096];

                filePath = FormatSaveFilePath(url, saveFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    while (true)
                    {
                        var bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        totalBytes += bytesRead;

                        if (bytesRead == 0)
                            break;

                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }

                //
                //using (var reader = new StreamReader(responseStream))
                //{
                //	fileContent = reader.ReadToEnd();
                //}

            }

            return totalBytes;
        }

        private static string FormatSaveFilePath(string url, string saveFolder)
        {
            var splittedUrl = url.Split('/', '.');//suppose url is in ftp://url/folder/fileName.extension format
            var fileName = splittedUrl[splittedUrl.Length - 2];
            var fileExtension = splittedUrl[splittedUrl.Length - 1];
            var newFileName = string.Format("{1}.{2}", fileName, DateTime.UtcNow.ToString("s"), fileExtension);

            return Path.Combine(saveFolder, ToSafeFileName(newFileName));
        }

        public void Upload(string url, string userName, string password, string fileName, string filePath)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new NullReferenceException("url");
            if (string.IsNullOrWhiteSpace(filePath))
                throw new NullReferenceException("saveFolder");

            var request = (FtpWebRequest)WebRequest.Create(url.TrimEnd(new[] { '/' }) + "/" + fileName);

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(userName, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var fileStream = File.OpenRead(filePath))
            {
                using (var requestStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(requestStream);
                    requestStream.Close();
                }
            }

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                //TODO: handle response errors
                //Console.WriteLine("Download Complete, status code {0}"response.StatusCode);
                //Console.WriteLine("Download Complete, status message {0}", response.StatusDescription);

            }
        }

        public void Delete(string url, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new NullReferenceException("url");
            if (string.IsNullOrWhiteSpace(url))
                throw new NullReferenceException("saveFolder");

            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(userName, password);

            request.Method = WebRequestMethods.Ftp.DeleteFile;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                //TODO: handle response errors
                //Console.WriteLine("Download Complete, status code {0}"response.StatusCode);
                //Console.WriteLine("Download Complete, status message {0}", response.StatusDescription);

            }
        }

        public static string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
    }

    public interface IFtpFileDownloader
    {
        List<string> GetFileList(string url, string userName, string password);
        long Download(string url, string userName, string password, string saveFolder, out string filePath);
        void Upload(string url, string userName, string password, string fileName, string filePath);
        //void Delete(string url, string userName, string password);
    }
}
