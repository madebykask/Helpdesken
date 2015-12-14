namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using System;
    using System.Configuration;
    using System.IO;
    
    public class DataLogService
    {      
        public static void SaveLog(string logData, DataLogTypes logType)
        {
            DataLogRepository.SaveLog(logData, logType);
        }
    }
}
