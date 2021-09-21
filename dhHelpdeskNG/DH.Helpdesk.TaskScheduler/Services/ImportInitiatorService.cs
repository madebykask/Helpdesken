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
using System.Threading;
using DH.Helpdesk.TaskScheduler.Helpers;
using DH.Helpdesk.Common.Extensions.String;

namespace DH.Helpdesk.TaskScheduler.Services
{
    internal class ImportInitiatorService : IImportInitiatorService
    {

        const string departmentId = "department_id";
        const string region = "region_id";
        const string extendedinfo = "extendedinfo";
        const string LanguageId = "language_id";
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
        private string[] BlockFields = new string[] {
                           region, extendedinfo};

        private readonly IDbQueryExecutorFactory _execFactory;
        private readonly ILog _logger;
        private readonly INotifierFieldSettingRepository _notifielrFieldSettingfactory;
        private readonly INotifierRepository _notifielrRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly Helpdesk.Services.Services.IOUService _ouService;


        //private readonly IFtpFileDownloader _downloader;

        public ImportInitiatorService(IDbQueryExecutorFactory execFactory,
                                       INotifierFieldSettingRepository notifielrFieldSettingfactory,
                                       INotifierRepository notifierRepository,
                                       IDepartmentRepository deparmentRepository,
                                       ILanguageRepository languageRepository,
                                       IDivisionRepository divisionRepository,
                                       IDomainRepository domainRepository,
                                       ICustomerRepository customerRepository,
                                       ISettingRepository settingRepository,
                                       IRegionRepository regionRepository,
                                       Helpdesk.Services.Services.IOUService ouService

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
            _customerRepository = customerRepository;
            _settingRepository = settingRepository;
            _regionRepository = regionRepository;
            _ouService = ouService;
            //_downloader = downloader;
        }

        public IList<CompuerUsersFieldSetting> GetInitiatorSettings(int customerId)
        {
            var dbQueryExecutor = _execFactory.Create();
            var ret = dbQueryExecutor.QueryList<CompuerUsersFieldSetting>
               ("Select * from tblComputerUserFieldSettings where Customer_id = @Customer_Id", new { Customer_Id = customerId });
            return ret;
        }

        public IList<ImportInitiator_JobSettings> GetJobSettings(ref DataLogModel logs)
        {
            var CSVcustomers = _settingRepository.GetMany(c => c.IntegrationType == 0).ToList();
            var customerIds = CSVcustomers.Select(c => c.Customer_Id).ToArray();
            var customerSettings = _settingRepository.GetMany(c => customerIds.Contains(c.Customer_Id)).ToList();

            var ret = new List<ImportInitiator_JobSettings>();
            if (CSVcustomers != null)
            {
                foreach (var customer in CSVcustomers)
                {
                    try
                    {
                        var _cs = customerSettings.FirstOrDefault(s => s.Customer_Id == customer.Customer_Id);
                        var _r = _regionRepository.GetDefaultRegion(customer.Customer_Id);
                        var this_Customer = _customerRepository.GetById(customer.Customer_Id);

                        if (_cs != null)
                        {
                            var jobSetting = new ImportInitiator_JobSettings()
                            {
                                CustomerId = customer.Customer_Id,
                                Url = _cs.LDAPBase,
                                InputFilename = _cs.LDAPFilter,
                                OverwriteFromMasterDirectory = this_Customer.OverwriteFromMasterDirectory,
                                Days2WaitBeforeDelete = this_Customer.Days2WaitBeforeDelete,
                                UserName = _cs.LDAPUserName,
                                Password = _cs.LDAPPassword,
                                Logging = _cs.LDAPLogLevel,
                                CreateOrganisation = _cs.LDAPCreateOrganization,
                                DefaultRegion = _r,
                                StartTime = "2018-04-06",
                                CronExpression = "0 * * ? * * *",
                                TimeZone = "",
                                ImportFormat = "CSV"
                            };
                            ret.Add(jobSetting);
                        }
                    }
                    catch (Exception ex)
                    {
                        logs.Add(customer.Id, $"Error: {ex.Message}");
                    }

                }
            }
            return ret;
        }

        public IList<ITrigger> GetTriggers(ref DataLogModel logs)
        {
            var triggers = new List<ITrigger>();
            var settings = GetJobSettings(ref logs);

            if (settings != null || settings.Any())
            {
                var i = 0;
                foreach (var setting in settings)
                {
                    //if (setting == null) throw new ArgumentNullException(nameof(settings));
                    var trigger = TriggerBuilder.Create()
                                .WithIdentity($"{Constants.ImportInitiator_JobTriggerName}_{i}")
                                .StartNow()
                                .ForJob($"{Constants.ImportInitiator_JobName}_{i}");

                    var cronExpression = "";
                    if (!string.IsNullOrWhiteSpace(setting.CronExpression))
                    {
                        cronExpression = setting.CronExpression;
                    }
                    else if (!string.IsNullOrWhiteSpace(setting.StartTime))
                    {
                        var r = new Regex(@"(\d{1,2}):(\d{1,2})");
                        var match = r.Match(setting.StartTime);
                        if (match.Success)
                        {
                            cronExpression = $"0 {match.Groups[2].Value} {match.Groups[1].Value} ? * *";
                        }
                    }
                    if (string.IsNullOrWhiteSpace(cronExpression)) throw new Exception("No StartTime or CronExpression defined.");

                    TimeZoneInfo timeZoneInfo;
                    if (!string.IsNullOrWhiteSpace(setting.TimeZone))
                    {
                        try
                        {
                            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(setting.TimeZone);
                        }
                        catch (InvalidTimeZoneException)
                        {
                            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                        }
                    }
                    else
                        timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                    _logger.DebugFormat("Creating trigger - CronExp:{0}, TimeZone:{1}", cronExpression, timeZoneInfo.Id);
                    triggers.Add(trigger.WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression)
                        .InTimeZone(timeZoneInfo))
                        .Build());

                    i++;
                }
            }

            if (triggers == null) throw new ArgumentNullException(nameof(triggers));

            return triggers;
        }

        public CsvInputData ReadCsv(ImportInitiator_JobSettings setting, ref DataLogModel logs)
        {
            var ret = new CsvInputData();

            var filePath = setting.Url;
            var fileName = $"{setting.InputFilename}";
            const char delimiter = ';';
            var sourcePath = Path.Combine(filePath, fileName);
            var newPath = Path.Combine(filePath, "archive");


            if (File.Exists(sourcePath))
            {
                try
                {
                    using (var reader = new StreamReader(sourcePath, Encoding.UTF7, true))
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
                        reader.Close();
                        var destinationPath = Path.Combine(newPath + @"\" + fileName);

                        if (Directory.Exists(newPath))
                        {
                            try
                            {
                                File.Copy(sourcePath, destinationPath, true);
                                File.Delete(sourcePath);

                            }
                            catch (Exception ex)
                            {
                                logs.Add(setting.CustomerId, $"Error: {ex.Message}");
                            }

                        }
                        else
                        {
                            Directory.CreateDirectory(newPath);
                            try
                            {
                                File.Copy(sourcePath, destinationPath, true);
                                File.Delete(sourcePath);
                            }
                            catch (Exception ex)
                            {
                                logs.Add(setting.CustomerId, $"Error: {ex.Message}");
                            }

                        }

                        return ret;
                    }

                }
                catch (Exception ex)
                {
                    //TODO: Log error
                    if (setting.Logging == 1)
                        logs.Add(setting.CustomerId, $"Error: {ex.Message} Failed to reading file { fileName} from path { filePath}");
                }
            }
            return ret;
        }

        public int CheckIfExisting(string UserId, int customerId)
        {
            return _notifielrRepository.GetExistingNotifierIdByUserId(UserId, customerId);
        }
        public void ImportInitiator(ImportInitiator_JobSettings setting, CsvInputData inputData, IList<CompuerUsersFieldSetting> fieldSettings, ref DataLogModel logs)
        {
            var isOverwrite = Convert.ToBoolean(setting.OverwriteFromMasterDirectory);
            //var deletingDays = setting.Days2WaitBeforeDelete;

            //if (deletingDays > 0)
            //{
            //    //Delete
            //    DeleteInitiators(deletingDays, setting.CustomerId, ref logs);

            //}
            var multipleFields = fieldSettings.Where(fs => fs.LDAPAttribute.Contains(',')).ToList();
            var ldapFields = fieldSettings.Where(fs => !string.IsNullOrEmpty(fs.LDAPAttribute)).ToList();
            //ldapFields.RemoveAll(l => BlockFields.Contains(l.ComputerUserField.ToLower()));

            var fieldLenght = new FieldLenght();
            var regionsToAdd = new List<string>();


            var insertLog = "";
            var updateLog = "";
            var inserted = "";
            var iCount = 0;
            var updated = "";
            var uCount = 0;

            var idIdentifier = ldapFields.Where(o => o.ComputerUserField.ToLower() == "userid" && !string.IsNullOrEmpty(o.LDAPAttribute))
                .Select(o => o.LDAPAttribute)
                .SingleOrDefault();

            var regionIdentifier = ldapFields.Where(o => o.ComputerUserField.ToLower() == region && !string.IsNullOrEmpty(o.LDAPAttribute))
                .Select(o => o.LDAPAttribute)
                .SingleOrDefault();

            if (regionIdentifier != null)
            {
                var regionNames = inputData.InputColumns.Where(o => o.Item2.ContainsKey(regionIdentifier))
                    .Select(o => o.Item2[regionIdentifier])
                    .Where(o => !string.IsNullOrEmpty(o))
                    .Distinct()
                    .ToList();
                if (regionNames.Any())
                {
                    CreateRegions(regionNames, setting.CustomerId, setting.CreateOrganisation);
                }
            }

            var regions = _regionRepository.GetMany(o => o.Customer_Id == setting.CustomerId && o.IsActive == 1)
                .ToList()
                .GroupBy(o => o.Name.ToLower())
                .Select(o => o.OrderByDescending(p => p.Id).First()) // Ensure unique name, if more than 1, take highest ID
                .ToDictionary(o => o.Name.ToLower(), o => o.Id);

            foreach (var row in inputData.InputColumns)
            {
                var updateQuery = "";
                var existingId = 0;
                var id = idIdentifier != null ?
                    row.Item2[idIdentifier] :
                    row.Item1;

                existingId = CheckIfExisting(id, setting.CustomerId);

                var fieldNames = "";
                var fieldValues = "";

                var delimiter = "";
                var isNew = existingId <= 0;

                int? regionId;
                if (regionIdentifier != null && row.Item2.ContainsKey(regionIdentifier) && !string.IsNullOrEmpty(row.Item2[regionIdentifier]))
                {
                    var regionName = row.Item2[regionIdentifier].ToLower();
                    if (regions.ContainsKey(regionName))
                    {
                        regionId = regions[regionName];
                    }
                    else
                    {
                        regionId = setting.DefaultRegion;
                    }
                }
                else
                {
                    regionId = setting.DefaultRegion;
                }


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

                        foreach (var _fsField in _fsFields)
                        {
                            var _sectionValue = row.Item2[_fsField];

                            var c = " ";
                            if (string.IsNullOrEmpty(_csvFieldValue))
                                c = "";
                            _csvFieldValue += string.IsNullOrEmpty(_sectionValue) ? "" : $"{c}{_sectionValue},";
                            //NOTE: ForignKeys can not be combined                                                                                                             
                        }
                        if (_csvFieldValue.EndsWith(","))
                            _csvFieldValue = _csvFieldValue.Substring(0, _csvFieldValue.Length - 1);
                    }
                    else
                    {
                        if (row.Item2.ContainsKey(fs.LDAPAttribute))
                            _csvFieldValue = row.Item2[fs.LDAPAttribute];
                        if (relatedFields.Contains(_dbFieldName))
                            _csvFieldValue = GetRelatedValue(_dbFieldName, _csvFieldValue, setting.CustomerId, regionId, setting.CreateOrganisation);
                        if (BlockFields.Contains(_dbFieldName))
                        {
                            // regionsToAdd.Add(_csvFieldValue);
                            _dbFieldName = string.Empty;
                        }
                    }

                    if (maxLen > 0 && _csvFieldValue.Length > maxLen)
                        _csvFieldValue = _csvFieldValue.Substring(0, maxLen - 1);
                    if (!string.IsNullOrEmpty(_dbFieldName))
                    {
                        // Create Script
                        if (isNew)
                        {
                            //Insert
                            delimiter = fieldNames == "" ? "" : ",";
                            fieldNames += $"{delimiter}{_dbFieldName}";
                            fieldValues += (_csvFieldValue == "null") ?
                                            $"{delimiter}{_csvFieldValue.SafeEncloseQuote()}" :
                                            $"{delimiter}'{_csvFieldValue.SafeEncloseQuote()}'";
                        }
                        else
                        {
                            //Update
                            delimiter = string.IsNullOrEmpty(updateQuery) ? "" : ",";
                            var _leftSide = $"{delimiter}{_dbFieldName} = ";
                            var rightSide = (_csvFieldValue == "null") ?
                                            $"{_csvFieldValue.SafeEncloseQuote()}" :
                                            $"'{_csvFieldValue.SafeEncloseQuote()}'";
                            updateQuery += (_leftSide + rightSide);
                        }
                    }
                } //for each LDAP Attr

                var queryToRun = (isNew) ?

                        $"Insert into tblComputerUsers " +
                                      $"({fieldNames}, Customer_Id, SyncChangedDate) " +
                                      $"Values ({fieldValues}, {setting.CustomerId}, '{DateTime.UtcNow}')"
                        :
                        ((isOverwrite) ?
                        $"Update tblComputerUsers SET {updateQuery} " +
                                      $",ChangeTime = '{DateTime.Now}' " +
                                      $",SyncChangedDate = '{DateTime.UtcNow}' " +
                                      $"where id = {existingId}" : string.Empty);

                if (!string.IsNullOrEmpty(queryToRun))
                {
                    var dbQueryExecutor = _execFactory.Create();
                    var ret = dbQueryExecutor.ExecQuery(queryToRun);


                    if (isNew)
                    {
                        inserted += $"{row.Item1}, ";
                        insertLog += queryToRun + "\r\n";
                        iCount++;
                    }
                    else
                    {
                        updated += $"{row.Item1}, ";
                        updateLog += queryToRun + "\r\n";
                        uCount++;
                    }
                }
            }

            /* if (regionsToAdd.Any())
                 CreateRegions(regionsToAdd, setting.CustomerId, setting.CreateOrganisation);*/

            inserted += string.IsNullOrEmpty(inserted) ?
            $"There was no New Initiator." : inserted;
            var insertText = $"{DateTime.Now} Number of inserted inisitator is {iCount}. UserIds are: {inserted} \r\n {insertLog}";

            updated += string.IsNullOrEmpty(updated) ?
            $"There was no Initiator to update." : updated;
            var updateText = $"{DateTime.Now} Number of updated inisitator is {uCount}. UserIds are: {updated} \r\n {updateLog}";


            logs.Add(setting.CustomerId, $"{insertText} \r\n {updateText}");
            // _logger.InfoFormat("{insertText} \r\n {updateText}");

        }

        private string GetRelatedValue(string fieldName, string forignKey, int customerId, int? regionId, int createOrganisation)
        {
            switch (fieldName)
            {

                case departmentId:
                    var depId = GetDepartmentId(forignKey, customerId, regionId, createOrganisation);
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
                    var ouId = GetOUId(forignKey, createOrganisation);
                    return ouId == 0 ? "null" : ouId.ToString();
            }

            return string.Empty;
        }

        public void DeleteInitiators(int Days2waitBeforeDelete, int customerId, ref DataLogModel logs)
        {
            var Initiators = _notifielrRepository.GetMany(n => n.Customer_Id == customerId).ToList();
            if (Initiators.Any())
            {
                var InitiatorsToDelete = Initiators.
                                            Where(i => i.SyncChangedDate != null && (i.SyncChangedDate.Value.AddDays(Days2waitBeforeDelete) <=
                                            DateTime.UtcNow)).ToList();

                // Has Domain Id
                var hasDomainId = InitiatorsToDelete.Where(i => i.Domain_Id != null).Select(i => i.Id).ToList();
                // Has ManagerComputerUserId
                var hasMCId = InitiatorsToDelete.Where(i => i.ManagerComputerUser_Id != null).Select(i => i.Id).ToList();
                var InitiatorIds = InitiatorsToDelete.Select(i => i.Id).ToList();

                var dbQueryExecutor = _execFactory.Create();
                var ret = 0;

                if (hasDomainId.Any())
                {
                    var InitHasDomainId = String.Join(",", hasDomainId);
                    var query = $"UPDATE tblComputer SET User_Id = Null " +
                                $"WHERE User_Id IN ({InitHasDomainId})";
                    ret = dbQueryExecutor.ExecQuery(query);
                }

                if (hasMCId.Any())
                {
                    var InitHasMCUId = String.Join(",", hasMCId);
                    var query = $"UPDATE tblComputerUsers SET ManagerComputerUser_Id=Null " +
                                $"WHERE ManagerComputerUser_Id IN ({InitHasMCUId})";
                    ret = dbQueryExecutor.ExecQuery(query);
                }

                foreach (var InitiatorId in InitiatorIds)
                {
                    _logger.InfoFormat("Deleting Initiator {InitiatorId} FOR CustomerId {customerId}");
                    // First delete Computer and History if there is any - this didn't work because of connection to other Inventories
                    //var selectComputerQuery = $"Select Count(*) from tblComputer where User_Id  =  {InitiatorId};";
                    //var computersCount = dbQueryExecutor.ExecuteScalar<int>(selectComputerQuery);
                    //if(computersCount > 0)
                    //{
                    //    var deleteComputerQuery = $"Delete from tblComputer_History where Computer_Id in(select Id from tblComputer where User_Id = {InitiatorId});" +
                    //            $"DELETE FROM tblComputer WHERE User_Id = {InitiatorId};";
                    //    ret = dbQueryExecutor.ExecQuery(deleteComputerQuery);
                    //}

                    var deletequery = $"Update tblComputer set User_Id = null WHERE User_Id = {InitiatorId};" +
                                $"DELETE FROM tblComputerUserLog WHERE ComputerUser_Id = {InitiatorId};" +
                                $"DELETE FROM tblComputerUser_tblCUGroup WHERE ComputerUser_Id = {InitiatorId};" +
                                $"DELETE FROM tblComputerUsers WHERE Id = {InitiatorId};";
                    ret = dbQueryExecutor.ExecQuery(deletequery);
                }
                if (ret == 1)
                {

                    logs.Add(customerId, $"Number of Initiators Deleted : {InitiatorIds.Count()}");
                }
            }
        }

        public void CreatLogFile(DataLogModel _logs)
        {
            //var _logs = new DataLogModel();

            var basePath = ConfigurationManager.AppSettings["LogPath"];
            var fileName = $"LogFile_{DateTime.Now.ToString("yyMMdd")}.txt";
            foreach (var log in _logs.RowsData)
            {
                var filePath = $"{basePath}\\{log.Id}\\{fileName}";
                var fileContent = string.Join("\n", log.Data);
                if (File.Exists(filePath))
                {
                    //Open File - Append content
                    using (var file = File.Open(filePath, FileMode.Append, FileAccess.Write))
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine("\r\n ------------------------END---------------------- \r\n ");
                        writer.Write(fileContent);
                        writer.Flush();
                        writer.Close();
                    }
                }
                else
                {
                    //Create File
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Byte[] info = new UTF7Encoding(true).GetBytes(fileContent);
                        // Add some information to the file.                                              
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }
        }

        private void CreateRegions(List<string> regionNames, int customerId, int createOrganisation)
        {
            if (createOrganisation != 0)
            {
                var dbQueryExecutor = _execFactory.Create();
                var ret = 0;
                regionNames.Distinct();
                foreach (var regionName in regionNames)
                {
                    var query = $"IF NOT EXISTS(SELECT 1 FROM tblRegion WHERE " +
                                $"Region = '{regionName}' and Customer_Id = {customerId} )" +
                                $"BEGIN " +
                                $"Insert into tblRegion (Region, Customer_Id)" +
                                $"Values ('{regionName}', {customerId})" +
                                $"END";

                    ret = dbQueryExecutor.ExecQuery(query);
                }
            }

        }
        public int GetDepartmentId(string departmentName, int customerId, int? regionId, int createOrganisation)
        {
            var depId = _departmentRepository.GetDepartmentId(departmentName.Trim().ToLower(), customerId, regionId);
            if (depId == 0 && createOrganisation != 0 && (departmentName != "" || !string.IsNullOrEmpty(departmentName)))
            {
                depId = CreateDepartment(departmentName, customerId, regionId);
            }
            return depId;
        }
        public int CreateDepartment(string departmentName, int customerId, int? regionId)
        {
            var department = new Domain.Department();

            department.DepartmentName = departmentName.Trim();
            department.Customer_Id = customerId;
            department.Region_Id = regionId;
            department.HeadOfDepartment = string.Empty;
            department.HeadOfDepartmentEMail = string.Empty;
            department.SearchKey = string.Empty;
            department.Path = string.Empty;
            department.AccountancyAmount = 0;
            department.DepartmentId = string.Empty;
            department.Charge = 0;
            department.ChargeMandatory = 0;
            department.ShowInvoice = 0;
            department.IsActive = 1;
            department.IsEMailDefault = 0;
            department.ChangedDate = DateTime.UtcNow;
            department.OverTimeAmount = 0;
            department.LanguageId = 0;
            department.DepartmentGUID = Guid.NewGuid();

            _departmentRepository.Add(department);
            _departmentRepository.Commit();

            var depId = _departmentRepository.GetDepartmentId(departmentName, customerId, regionId);

            return depId;
        }
        private int GetOUId(string OUName, int createOrganisation)
        {
            var oU = _ouService.GetOUIdByName(OUName.Trim());
            var ouId = 0;
            if (oU != null)
                ouId = oU.Id;

            if (oU == null && createOrganisation != 0 && (OUName != "" || !string.IsNullOrEmpty(OUName)))
            {
                ouId = CreateOU(OUName);
            }
            return ouId;
        }

        private int CreateOU(string OUName)
        {
            var dbQueryExecutor = _execFactory.Create();
            var ret = 0;

            var query = $"Insert into tblOU (OU, Department_Id)" +
                        $"Values ('{OUName}', NULL)";
            ret = dbQueryExecutor.ExecQuery(query);

            var ou = _ouService.GetOUIdByName(OUName.Trim());
            var ouId = 0;
            if (ou != null)
                ouId = ou.Id;


            return ouId;
        }


    }


    internal interface IImportInitiatorService
    {
        IList<ImportInitiator_JobSettings> GetJobSettings(ref DataLogModel logs);

        IList<ITrigger> GetTriggers(ref DataLogModel logs);

        CsvInputData ReadCsv(ImportInitiator_JobSettings settings, ref DataLogModel logs);

        IList<CompuerUsersFieldSetting> GetInitiatorSettings(int customerId);

        void ImportInitiator(ImportInitiator_JobSettings setting, CsvInputData inputColumns, IList<CompuerUsersFieldSetting> fieldSettings, ref DataLogModel logs);

        int CheckIfExisting(string UserId, int customerId);

        void DeleteInitiators(int Days2waitBeforeDelete, int customerId, ref DataLogModel logs);

        void CreatLogFile(DataLogModel _logs);

        int CreateDepartment(string departmentName, int customerId, int? regionId);

        int GetDepartmentId(string departmentName, int customerId, int? regionId, int createOrganisation);
    }
}
