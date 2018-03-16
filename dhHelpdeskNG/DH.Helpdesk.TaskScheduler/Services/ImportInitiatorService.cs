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
using DH.Helpdesk.TaskScheduler.Enums;
using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.TaskScheduler.Helper;

namespace DH.Helpdesk.TaskScheduler.Services
{
    internal class ImportInitiatorService : IImportInitiatorService
    {

        const string departmentId = "department_id";
        const string LanguageId = "languageid";
        const string DivisionId = "division_id";
        const string DomainId = "domain_id";
        const string ComputerUserGroupId = "computerusergroup_id";
        const string ManagerComputerUserId = "managercomputeruser_id";
        const string OUId = "ou_id";

        private string[] relatedFields = new string[] {
                            departmentId, OUId,
                            LanguageId, DomainId, DivisionId,
                            ComputerUserGroupId,
                            ManagerComputerUserId };

        private readonly IDbQueryExecutorFactory _execFactory;
        private readonly ILog _logger;
        private readonly INotifierFieldSettingRepository _notifielrFieldSettingfactory;
        private readonly INotifierRepository _notifielrRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IDomainRepository _domainRepository;


        //private readonly IFtpFileDownloader _downloader;

        public ImportInitiatorService(IDbQueryExecutorFactory execFactory,
                                       INotifierFieldSettingRepository notifielrFieldSettingfactory,
                                       INotifierRepository notifierRepository,
                                       IDepartmentRepository deparmentRepository,
                                       ILanguageRepository languageRepository,
                                       IDivisionRepository divisionRepository,
                                       IDomainRepository domainRepository

                                      //,IFtpFileDownloader downloader
                                      )
        {
            _execFactory = execFactory;
            _logger = LogManager.GetLogger<ImportInitiatorService>();
            _notifielrFieldSettingfactory = notifielrFieldSettingfactory;
            _notifielrRepository = notifierRepository;
            _departmentRepository = deparmentRepository;
            _languageRepository = languageRepository;
            _divisionRepository = divisionRepository;
            _domainRepository = domainRepository;
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
        public void ImportInitiator(CsvInputData inputData, IList<CompuerUsersFieldSetting> fieldSettings)
        {                        
            var multipleFields = fieldSettings.Where(fs => fs.LDAPAttribute.Contains(',')).ToList();
            var ldapFields = fieldSettings.Where(fs => !string.IsNullOrEmpty(fs.LDAPAttribute)).ToList();
            var fieldLenght = new FieldLenght();

            foreach (var row in inputData.InputColumns)
            {
                var updateQuery = "";
                var existingId = 0;
                var customerId = int.Parse(ConfigurationManager.AppSettings["Customers"]);
                existingId = CheckIfExisting(row.Item1, customerId);

                var fieldNames = "";
                var fieldValues = "";

                var delimiter = "";
                var isNew = existingId <= 0;

                foreach (var fs in ldapFields)
                {
                    var _dbFieldName = fs.ComputerUserField.ToLower();
                    var _csvFieldValue = "";
                    var maxLen = 0;
                    var maxLengthAttr = fieldLenght.GetAttributeFrom<MaxLengthAttribute>(_dbFieldName);
                    if (maxLengthAttr != null)
                       maxLen = maxLengthAttr.Length;

                    if (fs.LDAPAttribute.Contains(","))
                    {
                        var _fsFields = fs.LDAPAttribute.Split(',').ToList();
                        foreach(var _fsField in _fsFields)
                        {
                            var _sectionValue = row.Item2[_fsField];
                            _csvFieldValue += string.IsNullOrEmpty(_sectionValue)? "" : $" {_sectionValue}";
                            //NOTE: ForignKeys can not be combined                            
                        }
                    }
                    else
                    {
                        _csvFieldValue = row.Item2[fs.LDAPAttribute];
                        if (relatedFields.Contains(_dbFieldName))
                            _csvFieldValue = GetRelatedValue(_dbFieldName, _csvFieldValue, customerId);                        
                    }

                    if (maxLen > 0 && _csvFieldValue.Length > maxLen)
                        _csvFieldValue = _csvFieldValue.Substring(0, maxLen - 1);

                    // Create Script
                    if (isNew)
                    {
                        //Insert
                        delimiter = fieldNames == "" ? "" : ",";
                        fieldNames += $"{delimiter}{_dbFieldName}";
                        fieldValues += (_csvFieldValue == "null") ?
                                        $"{delimiter}{_csvFieldValue}" :
                                        $"{delimiter}'{_csvFieldValue}'";                        
                    }
                    else
                    {
                        //Update
                        delimiter = string.IsNullOrEmpty(updateQuery) ? "" : ",";
                        var _leftSide = $"{delimiter}{_dbFieldName} = ";
                        var rightSide = (_csvFieldValue == "null") ? 
                                        $"{_csvFieldValue}" : 
                                        $"'{_csvFieldValue}'";
                        updateQuery += (_leftSide + rightSide);                        
                    }
                } //for each LDAP Attr

                var queryToRun = (isNew) ?

                        $"Insert into tblComputerUsers " +
                                      $"({fieldNames}, Customer_Id) " +
                                      $"Values ({fieldValues}, {customerId})"
                        :
                        $"Update tblComputerUsers SET {updateQuery} " +
                                      $",ChangeTime = '{DateTime.UtcNow}' " +
                                      $"where id = {existingId}";
                                
                var dbQueryExecutor = _execFactory.Create();
                var ret = dbQueryExecutor.ExecQuery(queryToRun);
            }
        }

        private string GetRelatedValue(string fieldName, string forignKey, int customerId)
        {            
            switch (fieldName)
            {
                case departmentId:
                    var depId = _departmentRepository.GetDepartmentId(forignKey, customerId);
                    return depId == 0 ? "null" : depId.ToString();
                    
                case LanguageId:                   
                    var langId = _languageRepository.GetLanguageIdByText(forignKey);
                    return langId == 0 ? "null" : langId.ToString();                    

                case DivisionId:
                    var divId = _divisionRepository.GetDivisionIdByName(forignKey, customerId);
                    return divId == 0 ? "null" : divId.ToString();                  

                case DomainId:
                    var domainId = _domainRepository.GetDomainId(forignKey, customerId);
                    return domainId == 0 ? "null" : domainId.ToString();

                case ComputerUserGroupId:
                    //To do Write function to get
                    break;

                case ManagerComputerUserId:
                    //To do Write function to get
                    break;

                case OUId:
                    //To do Write function to get
                    break;
            }

            return string.Empty;
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
