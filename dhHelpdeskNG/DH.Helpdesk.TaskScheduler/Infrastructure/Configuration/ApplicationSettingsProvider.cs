using System;
using System.Configuration;

namespace DH.Helpdesk.TaskScheduler.Infrastructure.Configuration
{
    internal interface IGDPRJobSettings
    {
        bool IsGDPRTasksEnabled { get; }
        int GDPRBatchSize { get; }
    }

    internal interface IApplicationSettings  : IGDPRJobSettings
    {
        string EnvName { get; }
        string Customers { get; }

        bool IsDailyReportEnabled { get; }
        bool IsInitiatorImportEnabled { get; }
    }

    internal class ApplicationSettingsProvider : IApplicationSettings
    {
        protected readonly System.Configuration.Configuration Config;

        #region ctor()

        public ApplicationSettingsProvider()
        {
            Config = ConfigurationManager.OpenExeConfiguration(GetType().Assembly.Location);
        }

        #endregion

        #region Public Properties

        public string EnvName => Config.AppSettings.Settings["EnvName"]?.Value;

        public string Customers => Config.AppSettings.Settings["Customers"]?.Value;

        public bool IsDailyReportEnabled
        {
            get
            {
                var val = Config.AppSettings.Settings["DailyReport"].ToString();
                var dailyReport = GetInt32(val);
                return dailyReport > 0;
            }
        }

        public bool IsInitiatorImportEnabled
        {
            get
            {
                var val = Config.AppSettings.Settings["InitiatorImport"]?.Value;
                var initiatoImport = GetInt32(val);
                return initiatoImport > 0;
            }
        }

        public bool IsGDPRTasksEnabled
        {
            get
            {
                var val = Config.AppSettings.Settings["GDPRTasks"]?.Value;
                var gdprVal = GetInt32(val);
                return gdprVal > 0;
            }
        }
        
        public int GDPRBatchSize
        {
            get
            {
                var val = Config.AppSettings.Settings["GDPRBatchSize"]?.Value;
                var batchSize = GetInt32(val);
                return batchSize > 0 ? batchSize : 5000;    
            }
        }

        #endregion

        #region Helper Methods

        private int GetInt32(string val)
        {
            var res = 0;
            if (Int32.TryParse(val, out res))
                return res;

            return 0;
        }

        #endregion
    }
}