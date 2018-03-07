using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Logging;
using CsvHelper;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Components;
using Newtonsoft.Json;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Services
{
    internal class ImportInitiatorService : IImportInitiatorService
    {
        private readonly IDbQueryExecutorFactory _execFactory;
        private readonly ILog _logger;

        //private readonly IFtpFileDownloader _downloader;

        public ImportInitiatorService(IDbQueryExecutorFactory execFactory
                                      //,IFtpFileDownloader downloader
                                      )
        {
            _execFactory = execFactory;
            _logger = LogManager.GetLogger<ImportInitiatorService>();
            //_downloader = downloader;
        }

        public ImportInitiator_JobSettings GetJobSettings()
        {
            //var dbQueryExecutor = _execFactory.Create();
            //var settings = dbQueryExecutor.QuerySingleOrDefault<ImportInitiator_JobSettings>("Select * from tblReportScheduler");
            //_logger.DebugFormat("Settings: {0}", settings != null ? JsonConvert.SerializeObject(settings) : "null");
            var settings = new ImportInitiator_JobSettings()
            {
                Id = 0,
                Url = @"C:\Users\sg\Downloads",
                AppendTime = true,
                CronExpression = "0 * 8-17 * * ?",
                ImportFormat = "CSV",
                InputFilename = "ExampleFromCustomer.csv",
                LastRun = "",
                SqlQuery = "",
                StartTime = "2018-03-06",
                TimeZone = ""
            };
            return settings;
        }

        public ITrigger GetTrigger()
        {
            var settings = GetJobSettings();
            if (settings == null)
            {
                return TriggerBuilder.Create()
                    .WithIdentity(Constants.ImportInitiator_JobTriggerName)
                    .ForJob(Constants.ImportInitiator_JobName)
                    //.StartNow()
                    .EndAt(DateTimeOffset.UtcNow)
                    .Build();
            }
            //if (settings == null) throw new ArgumentNullException(nameof(settings));

            var trigger = TriggerBuilder.Create()
                        .WithIdentity(Constants.ImportInitiator_JobTriggerName)
                        .StartNow()
                        .ForJob(Constants.ImportInitiator_JobName);

            var cronExpression = "";
            if (!string.IsNullOrWhiteSpace(settings.CronExpression))
            {
                cronExpression = settings.CronExpression;
            } else if (!string.IsNullOrWhiteSpace(settings.StartTime))
            {
                var r = new Regex(@"(\d{1,2}):(\d{1,2})");
                var match = r.Match(settings.StartTime);
                if (match.Success)
                {
                    cronExpression = $"0 {match.Groups[2].Value} {match.Groups[1].Value} ? * *";
                }
            }
            if(string.IsNullOrWhiteSpace(cronExpression)) throw new Exception("No StartTime or CronExpression defined.");

            TimeZoneInfo timeZoneInfo;
            if (!string.IsNullOrWhiteSpace(settings.TimeZone))
            {
                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(settings.TimeZone);
                }
                catch (InvalidTimeZoneException)
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                }
            } else
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            _logger.DebugFormat("Creating trigger - CronExp:{0}, TimeZone:{1}", cronExpression, timeZoneInfo.Id);
            return trigger.WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression)
                .InTimeZone(timeZoneInfo))
                .Build();
        }

        public DataTable GetJobData(string query)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            var dbQueryExecutor = _execFactory.Create();
            return dbQueryExecutor.ExecuteTable(query);
        }


        //protected List<Tuple<string, string>> GetUrlsToImport(ImportInitiator_JobSettings settings)
        //{
        //    var res = new List<Tuple<string, string>>();
        //    try
        //    {
        //        var ftpList = _downloader.GetFileList(settings.Url, settings.UserName, settings.Password);
        //        var files = ftpList.Select(Path.GetFileName).ToList();
        //        foreach (var file in files)
        //        {
        //            _logger.Debug("Validating file for import: " + file);

        //            if (file.Length > 18)
        //            {
        //                var ftpDateString = file.Substring(9, 8);
        //                DateTime ftpDate;
        //                if (DateTime.TryParseExact(ftpDateString, "yyyyMMdd", CultureInfo.InvariantCulture,
        //                    DateTimeStyles.None, out ftpDate))
        //                {
        //                    if (ftpDate.Date == DateTime.Now.Date)
        //                    {
        //                        res.Add(new Tuple<string, string>(file, $"{settings.Url}/{file}"));
        //                        //var downloaded = _emdDataAccess.CheckIfDownloaded(file);
        //                        //if (!downloaded)
        //                        //{
        //                        //    res.Add(new Tuple<string, string>(file, $"{settings.Url}/{file}"));
        //                        //}
        //                        //else
        //                        //{
        //                        //    _logger.Warn($"File ({file}) has already been downloaded.");
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        _logger.Warn($"File ({file}) is older than today and will not be imported.");
        //                    }
        //                }                       
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error getting file list from ftp and creating import list", ex);
        //    }

        //    return res;
        //}



        public List<string> ReadCsv(ImportInitiator_JobSettings settings)
        {
            var dateTimeExtension = settings.AppendTime ? $"{DateTime.Now.ToString("_d_M_yyyy_H_m_s")}" : "";
            const string defaultFileName = "ExampleFromCustomer";


            var filePath = settings.Url;
            var fileName = $"{settings.InputFilename ?? defaultFileName}{dateTimeExtension}.csv";
          
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(filePath + fileName))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read())
                {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        result.Add(value);
                    }
                }
            }
            return result;

            //const string defaultFileName = "Not set";
            //var dateTimeExtension = settings.AppendTime ? $"{DateTime.Now.ToString("_d_M_yyyy_H_m_s")}" : "";

            //var filePath = settings.Url;
            //var fileName = $"{settings.InputFilename ?? defaultFileName}{dateTimeExtension}.csv";
            //if (!Directory.Exists(settings.Url)) throw new DirectoryNotFoundException($"Directory {settings.Url} not found.");

            //var files = GetUrlsToImport(settings);
            //foreach (var file in files)
            //{
            //    var filePath = string.Empty;

            //    try
            //    {
            //        _downloader.Download(file.Item2, settings.UserName, settings.Password, settings.SaveFolder, out filePath);
            //        if (string.IsNullOrEmpty(filePath))
            //        {
            //            throw new Exception("filPath is empty after download");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception($"Error downloading file: {file}", ex);
            //    }

            //    try
            //    {
            //        //var code = fileName.Substring(0, 2);
            //        //var countryId = _emdDataAccess.GetCountryId(code);

            //        //if (countryId != 0)
            //        //{
            //        //    var excelFormatter = _excelFormatterFactory.Get(filePath, "0");
            //        //    var employeeLoader = _employeeLoaderFactory.Get(excelFormatter);
            //        //    var result = employeeLoader.Load(settings.AmUserId, countryId);

            //        //    _emdDataAccess.SaveImportLog(result, settings.LogMode, file.Item1, countryId);
            //        //    _log.Debug($"File has been imported: {file.Item2}");
            //        //}
            //        //else
            //        //{
            //        //    _log.Warn($"Ignore file ({fileName}) import. Failed to find a country by code: " + code);
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception($"Error parsing file: {file}", ex);
            //    }
            //    _downloader.Upload(settings.SaveFolder, settings.UserName, settings.Password, file.Item1, filePath);
            //     }
            //var reader = new CsvReader(file);

            //CSVReader will now read the whole file into an enumerable
            // IEnumerable<DataRecord> records = reader.GetRecords<DataRecord>();

            //using (var csv = new CsvWriter(textWriter))
            //{
            //    csv.Configuration.Delimiter = ";";
            //    _logger.DebugFormat($"Saving file {fileName} to path {filePath}");
            //    csv.WriteRecords(data);
            //}

        }
        
        public void UpdateLastRun(int id)
        {
            var dbQueryExecutor = _execFactory.Create();
            dbQueryExecutor.ExecQuery("Update [tblReportScheduler] set [LastRun]=@Datetime where [Id] = @Id", new { DateTime = DateTime.UtcNow, Id = id });
        }
}
 

    internal interface IImportInitiatorService
    {
        ImportInitiator_JobSettings GetJobSettings();

        ITrigger GetTrigger();

        DataTable GetJobData(string query);

        List<string> ReadCsv(ImportInitiator_JobSettings settings);

        void UpdateLastRun(int id);
    }
}
