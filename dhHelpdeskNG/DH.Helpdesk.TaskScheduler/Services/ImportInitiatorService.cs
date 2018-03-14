using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Logging;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.TaskScheduler.Dto;
using Quartz;
using DH.Helpdesk.Dal.Repositories.Notifiers;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
using System.Configuration;
using DH.Helpdesk.Dal.Repositories;

namespace DH.Helpdesk.TaskScheduler.Services
{
    internal class ImportInitiatorService : IImportInitiatorService
    {
        private readonly IDbQueryExecutorFactory _execFactory;
        private readonly ILog _logger;
        private readonly INotifierFieldSettingRepository _notifielrFieldSettingfactory;
        private readonly INotifierRepository _notifielrRepository;
        private readonly IDepartmentRepository _departmentRepository;


        //private readonly IFtpFileDownloader _downloader;

        public ImportInitiatorService(IDbQueryExecutorFactory execFactory,
                                       INotifierFieldSettingRepository notifielrFieldSettingfactory,
                                       INotifierRepository notifierRepository,
                                       IDepartmentRepository deparmentRepository
                                      //,IFtpFileDownloader downloader
                                      )
        {
            _execFactory = execFactory;
            _logger = LogManager.GetLogger<ImportInitiatorService>();
            _notifielrFieldSettingfactory = notifielrFieldSettingfactory;
            _notifielrRepository = notifierRepository;
            _departmentRepository = deparmentRepository;
            //_downloader = downloader;
        }

        public IList<CompuerUsersFieldSetting> GetInitiatorSettings(int customerId)
        {
            var dbQueryExecutor = _execFactory.Create();           
            var ret = dbQueryExecutor.QueryList<CompuerUsersFieldSetting>
               ("Select * from tblComputerUserFieldSettings where Customer_id = @Customer_Id", new { Customer_Id = customerId} );            
            return ret;
        }

        public ImportInitiator_JobSettings GetJobSettings()
        {
            //var dbQueryExecutor = _execFactory.Create();
            //var settings = dbQueryExecutor.QuerySingleOrDefault<ImportInitiator_JobSettings>("Select * from tblReportScheduler");
            //_logger.DebugFormat("Settings: {0}", settings != null ? JsonConvert.SerializeObject(settings) : "null");
            var settings = new ImportInitiator_JobSettings()
            {
                Id = 0,
                //Url = @"C:\Users\sg\Downloads\",
                Url = @"C:\",
                AppendTime = true,
                CronExpression = "0 * 8-22 * * ?",
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
            }
            else if (!string.IsNullOrWhiteSpace(settings.StartTime))
            {
                var r = new Regex(@"(\d{1,2}):(\d{1,2})");
                var match = r.Match(settings.StartTime);
                if (match.Success)
                {
                    cronExpression = $"0 {match.Groups[2].Value} {match.Groups[1].Value} ? * *";
                }
            }
            if (string.IsNullOrWhiteSpace(cronExpression)) throw new Exception("No StartTime or CronExpression defined.");

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
            }
            else
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            _logger.DebugFormat("Creating trigger - CronExp:{0}, TimeZone:{1}", cronExpression, timeZoneInfo.Id);
            return trigger.WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression)
                .InTimeZone(timeZoneInfo))
                .Build();
        }

        public CsvInputData ReadCsv(ImportInitiator_JobSettings settings)
        {
            var ret = new CsvInputData();

            var filePath = settings.Url;
            var fileName = $"{settings.InputFilename}";
            const char delimiter = ';';

            using (var reader = new StreamReader(filePath + fileName, Encoding.UTF7, true))
            {
                // First line contains column names.
                var columnNames = reader.ReadLine().Split(delimiter);
                ret.InputHeaders = columnNames.ToList();

                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var curRecord = line.Split(delimiter).ToList();
                    if (curRecord.Any())
                    {
                        var identity = curRecord[0];
                        var rowFields = new Dictionary<string, string>();
                        for (int i = 0; i < curRecord.Count; i++)
                        {
                            rowFields.Add(columnNames[i], curRecord[i]);
                        }
                        ret.InputColumns.Add(new Tuple<string, Dictionary<string, string>>(identity, rowFields));
                    }

                    line = reader.ReadLine();
                }

                return ret;
            }
        }

        public int CheckIfExisting(string UserId , int customerId)
        {                             
            return _notifielrRepository.GetExistingNotifierIdByUserId(UserId,customerId);                         
        }
        public void ImportInitiator(CsvInputData inputData , IList<CompuerUsersFieldSetting> fieldSettings)
        {
            //Template
            //Insert into tblComputerUsers (Customer_Id,LogonName,...) Values (1,)
            //Update tblComputerUsers set {update query} where id = existingId

            var insertQuery = "";
            var updateQuery = "";

            const string departmentId = "department_id";
            foreach (var row in inputData.InputColumns)
            {
                insertQuery = "";
                updateQuery = "";

                var existingId = 0;
                //TODO 1
                var customerId = int.Parse(ConfigurationManager.AppSettings["Customers"]);
                existingId = CheckIfExisting(row.Item1, customerId);

                var fieldNames = "(";
                var values = "(";
                foreach (var field in row.Item2)
                {
                    // Key: LDAPAttr - Value: ComputerUserField
                    var dbFieldName = fieldSettings.FirstOrDefault(fs => fs.LDAPAttribute.Equals(field.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (dbFieldName != null && !string.IsNullOrEmpty(dbFieldName.ComputerUserField))
                    {
                        var _value = field.Value;
                        if (dbFieldName.ComputerUserField.ToLower() == departmentId)
                        {
                           int depId = _departmentRepository.GetDepartmentId(_value, customerId);
                            _value = depId == 0 ? "null" : depId.ToString();                                                     

                        }
                        var delimiter = "";

                        if (existingId != 0)
                        {
                            //Update                
                            delimiter = string.IsNullOrEmpty(updateQuery) ? "" : ",";
                            updateQuery += $"{delimiter}{dbFieldName.ComputerUserField} = '{_value}'";
                        }
                        else
                        {
                            //Insert
                            delimiter = fieldNames == "(" ? "" : ",";                                                      
                            fieldNames += $"{delimiter}{dbFieldName.ComputerUserField}";
                            if (_value == "null")
                            {
                                values += $"{delimiter}{_value}";
                            }
                            else
                            {
                                values += $"{delimiter}'{_value}'";
                            }                                                        
                        }                        
                    }
                }

                if (existingId != 0)
                {                    
                    updateQuery = $"Update tblComputerUsers SET {updateQuery} where id= {existingId} and Customer_Id = {customerId}";
                    var dbQueryExecutor = _execFactory.Create();
                    var ret = dbQueryExecutor.ExecQuery(updateQuery);
                }
                else
                {
                    insertQuery += $"{fieldNames}, Customer_Id) Values {values}, {customerId})"; 
                    insertQuery = $"Insert into tblComputerUsers {insertQuery}";
                    var dbQueryExecutor = _execFactory.Create();
                    var ret = dbQueryExecutor.ExecQuery(insertQuery);
                }
                
            }
        }
    }


    internal interface IImportInitiatorService
    {
        ImportInitiator_JobSettings GetJobSettings();

        ITrigger GetTrigger();

        CsvInputData ReadCsv(ImportInitiator_JobSettings settings);

        IList<CompuerUsersFieldSetting> GetInitiatorSettings(int customerId);

        void ImportInitiator(CsvInputData inputColumns , IList<CompuerUsersFieldSetting> fieldSettings);

        int CheckIfExisting(string UserId , int customerId);

    }
}
