namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Common.Enums;
    using System;
    using System.Configuration;
    using System.IO;
    
    public static class DataLogRepository
    {
        private static string _LOG_DIRECTORY_NAME = "LogData";
        private static string _LOG_FILE_NAME_OPEN_CASE = "Helpdesk5_OpenCase.txt";
        private static string _LOG_FILE_NAME_SAVE_CASE = "Helpdesk5_SaveCase.txt";
        private static string _LOG_FILE_NAME_GENERAL = "Helpdesk5_General.txt";

        public static void SaveLog(string logData, DataLogTypes logType)
        {
            var fileName = string.Empty;
            switch (logType)
            {
                case DataLogTypes.GENERAL:
                    fileName = _LOG_FILE_NAME_GENERAL;
                    break;

                case DataLogTypes.OPEN_CASE:
                    fileName = _LOG_FILE_NAME_OPEN_CASE;
                    break;
                
                case DataLogTypes.SAVE_CASE:
                    fileName = _LOG_FILE_NAME_SAVE_CASE;
                    break;
                
                default:
                    fileName = string.Empty;
                    break;
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                var basePath = ConfigurationManager.AppSettings[AppSettingsKey.FilesDirectory];
                var directory = Path.Combine(basePath, _LOG_DIRECTORY_NAME);
                try
                {

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                
                    var filePath = Path.Combine(directory, fileName);

                    TextWriter tw = new StreamWriter(filePath, true);
                    tw.WriteLine(logData);                
                    tw.Close();
                }
                catch
                {

                }
            }
        }
    }
}
