using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http.Results;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Exceptions;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services;

using log4net;
using File = DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields.File;
using IUnitOfWork = DH.Helpdesk.Dal.NewInfrastructure.IUnitOfWork;

namespace DH.Helpdesk.Services.BusinessLogic.Gdpr
{
    public interface IGDPRDataPrivacyProcessor
    {
        void Process(int customerId, int userId, DataPrivacyParameters p, int batchSize);
    }

    public class GDPRDataPrivacyProcessor : IGDPRDataPrivacyProcessor
    {
        private readonly ILog _log = LogManager.GetLogger("DataPrivacyLogger");

        private readonly IFilesStorage _filesStorage;
        private readonly IGDPROperationsAuditRespository _gdprOperationsAuditRespository;
        private readonly ISettingRepository _settingRepository;
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IJsonSerializeService _jsonSerializeService;
        private readonly IDataPrivacyTaskProgress _taskProgress;
        private readonly ISettingsLogic _settingsLogic;
        private readonly IGDPRDataPrivacyCasesService _gDPRDataPrivacyCasesService;
        private readonly ICaseDeletionService _caseDeletionService;
        const int sqlTimeout = 300; //seconds

        #region CaseFileEntity

        private class CaseFileEntity
        {
            #region ctor()

            public CaseFileEntity(int caseId, string caseNumber, string fileName)
                : this(caseId, caseNumber, 0, fileName, LogFileType.External)
            {
            }

            public CaseFileEntity(int caseId, string caseNumber, int logId, string fileName, LogFileType logType = LogFileType.External)
            {
                CaseId = caseId;
                CaseNumber = caseNumber;
                LogId = logId;
                FileName = fileName;
                LogType = logType;
            }

            #endregion

            public int CaseId { get; private set; }
            public string CaseNumber { get; private set; }
            public int LogId { get; private set; }
            public string FileName { get; private set; }
            public LogFileType LogType { get; private set; }

            public int GetCaseNumberOrId()
            {
                int res;
                if (int.TryParse(CaseNumber, out res))
                    return res;
                else
                    return CaseId;
            }
        }

        #endregion

        #region ctor()

        public GDPRDataPrivacyProcessor(
            IFilesStorage filesStorage,
            IGDPROperationsAuditRespository gdprOperationsAuditRespository,
            IGlobalSettingRepository globalSettingRepository,
            ISettingRepository settingRepository,
            IJsonSerializeService jsonSerializeService,
            IDataPrivacyTaskProgress taskProgress,
            IUnitOfWorkFactory unitOfWorkFactory,
            ISettingsLogic settingsLogic,
            IGDPRDataPrivacyCasesService gDPRDataPrivacyCasesService,
            ICaseDeletionService caseDeletionService)
        {
            _jsonSerializeService = jsonSerializeService;
            _taskProgress = taskProgress;
            _filesStorage = filesStorage;
            _gdprOperationsAuditRespository = gdprOperationsAuditRespository;
            _settingRepository = settingRepository;
            _globalSettingRepository = globalSettingRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _settingsLogic = settingsLogic;
            _gDPRDataPrivacyCasesService = gDPRDataPrivacyCasesService;
            _caseDeletionService = caseDeletionService;
        }

        #endregion

        public void Process(int customerId, int userId, DataPrivacyParameters p, int batchSize)
        {
            var processedCasesIds = new List<int>();
            var processedCaseNumbers = new List<decimal>();
            var caseNumbersToExclude = new List<decimal>();
            var filesToDelete = new List<CaseFileEntity>();
            var deletionStatus = new DeletionStatus();
            string errorMessage = String.Empty;

            _log.Debug($"GDPR process has been called. CustomerId: {customerId}, FavoriteId: {p.SelectedFavoriteId}, TaskId: {p.TaskId}");

            try
            {
                var totalCount = _gDPRDataPrivacyCasesService.GetCasesCount(customerId, p);
                var fetchNext = totalCount > 0;
                var step = 0;
                _log.Debug($"Total cases found to process: {totalCount}. TaskId: {p.TaskId}.");
                IList<int> parentIds = new List<int>();

                using (var uow = _unitOfWorkFactory.Create(sqlTimeout))
                {
                    parentIds = _gDPRDataPrivacyCasesService.GetCaseParents(customerId, p, uow);
                }

                while (fetchNext)
                {
                    step++;
                    var processed = processedCasesIds.Count;
                    var take = processed + batchSize > totalCount ? totalCount - processed : batchSize;

                    _log.Debug($"Step {step}. Fetching next {take} cases. Skip: {processed}. TaskId: {p.TaskId}.");

                    var casesIds = new List<int>();
                    var batchProcessedCaseIds = new List<int>();
                    var batchProcessedCaseNumbers = new List<decimal>();
                    var batchCaseNumbersToExclude = new List<decimal>();
                    var batchErrorMessage = String.Empty;

                    using (var uow = _unitOfWorkFactory.Create(sqlTimeout))
                    {
                        uow.AutoDetectChangesEnabled = false;

                        //GET query
                        var casesQueryable = _gDPRDataPrivacyCasesService.GetCasesQuery(customerId, p, uow).OrderBy(c => parentIds.Contains(c.Id) ? 1 : 0);

                        List<Case> cases = new List<Case>();
                        //fetch next cases
                        if (p.GDPRType == 2)
                        {
                            cases = casesQueryable.OrderBy(x => parentIds.Contains(x.Id) ? 1 : 0).Take(batchSize).ToList();
                        }
                        else
                        {
                            cases = casesQueryable.Skip(processed).Take(take).ToList();
                        }

                        cases.RemoveAll(c => caseNumbersToExclude.Contains(c.CaseNumber));

                        if (cases.Any())
                        {
                            _log.Debug($"{cases.Count} cases will be processed. TaskId: {p.TaskId}.");

                            var pos = 1;

                            if (p.GDPRType == 2) //Deletion
                            {
                                casesIds = cases.Select(c => c.Id).ToList();
                                foreach (var c in cases.OrderBy(x => parentIds.Contains(x.Id) ? 1 : 0))
                                {
                                    List<int> caseId = new List<int>() { c.Id };

                                    if (caseId != null)
                                    {

                                        deletionStatus = _caseDeletionService.DeleteCases(caseId, customerId, null);
                                        errorMessage += string.IsNullOrEmpty(deletionStatus.ErrorMessage) ? String.Empty : deletionStatus.ErrorMessage + "//";
                                        batchErrorMessage += string.IsNullOrEmpty(deletionStatus.ErrorMessage) ? String.Empty : deletionStatus.ErrorMessage + "//";
                                        batchCaseNumbersToExclude.AddRange(deletionStatus.CaseNumbersToExclude);
                                        batchProcessedCaseNumbers.AddRange(deletionStatus.ProcessedCaseNumbers);
                                        batchProcessedCaseIds.AddRange(deletionStatus.ProcessedCaseIds.ToList());
                                    }
                                    UpdateProgress(p.TaskId, processed + pos, totalCount);
                                    pos++;
                                }

                                _log.Debug($"{cases.Count} cases have been deleted. TaskId: {p.TaskId}.");
                            }
                            else //Anonymization
                            {
                                p.ReplaceDataWith = p.ReplaceDataWith ?? string.Empty;

                                foreach (var c in cases)
                                {
                                    ProcessReplaceCasesData(c, p, uow, filesToDelete);
                                    ProcessReplaceCasesHistoryData(c, p);
                                    ProcessExtededCaseData(c, p, uow);

                                    UpdateProgress(p.TaskId, processed + pos, totalCount);
                                    pos++;
                                }

                                _log.Debug($"{cases.Count} cases have been processed. TaskId: {p.TaskId}.");
                                ProccessFormPlusCaseData(cases.Select(c => c.Id).ToList(), uow);

                                _log.Debug($"Saving cases changes. TaskId: {p.TaskId}.");
                                uow.DetectChanges();
                                uow.Save();

                                _log.Debug($"Cases changes have been saved sucessfully. TaskId: {p.TaskId}.");

                                _log.Debug($"Deleting cases files.");
                                DeleteCaseFiles(customerId, filesToDelete);

                                casesIds = cases.Select(c => c.Id).ToList();
                                batchProcessedCaseIds = cases.Select(c => c.Id).ToList();
                            }
                        }
                        else
                        {
                            fetchNext = false;
                        }
                    }


                    //save processed cases
                    processedCasesIds.AddRange(casesIds);
                    processedCaseNumbers.AddRange(batchProcessedCaseNumbers);
                    caseNumbersToExclude.AddRange(batchCaseNumbersToExclude);

                    //save step audit
                    SaveStepSuccessOperationAudit(customerId, userId, p, new DataPrivacyResult { CaseIdsResult = batchProcessedCaseIds, CaseNumbersErrorResult = batchCaseNumbersToExclude, CaseNumbersResult = batchProcessedCaseNumbers, ErrorMessage = batchErrorMessage }, step);

                    if (processedCasesIds.Count() >= totalCount)
                    {
                        fetchNext = false;
                    }

                }

                caseNumbersToExclude = caseNumbersToExclude.Except(processedCaseNumbers).ToList();
                SaveSuccessOperationAudit(customerId, userId, p, new DataPrivacyResult { CaseIdsResult = processedCasesIds, CaseNumbersErrorResult = caseNumbersToExclude, CaseNumbersResult = processedCaseNumbers, ErrorMessage = errorMessage });

                _log.Debug($"Processing has been completed successfully. TaskId: {p.TaskId}.");
            }
            catch (Exception e)
            {
                var errMsg = $"Unknown error. {e.Message}";
                SaveFailedOperationAudit(customerId, userId, p, errMsg);
                throw;
            }
        }

        private void UpdateProgress(int taskId, int pos, int totalCount)
        {
            var step = totalCount / 100;// take 1 percent as step
            step = step == 0 ? 10 : step;
            if (pos % step == 0)
            {
                var percentProgress = Math.Ceiling(((float)pos / (float)totalCount) * 100);
                var progress =  percentProgress > 100 ? 100 : percentProgress;

                _taskProgress.Update(taskId, Convert.ToInt32(progress));
            }
        }

        private void DeleteCaseFiles(int customerId, List<CaseFileEntity> filesToDelete)
        {
            if (!filesToDelete.Any())
                return;

            var caseFiles = filesToDelete.Where(f => f.LogId == 0).ToList();
            var logFiles = filesToDelete.Where(f => f.LogId > 0).ToList();

            var baseDirPath = GetFilesDirPath(customerId);

            //delete case files
            foreach (var fileEntity in caseFiles)
            {
                var caseId = fileEntity.GetCaseNumberOrId();
                try
                {
                    this._filesStorage.DeleteFile(ModuleName.Cases, caseId, baseDirPath, fileEntity.FileName);
                }
                catch (IOException e)
                {
                    _log.Error("Error deleting log file.", e);
                }

            }

            //delete log files
            foreach (var fileEntity in logFiles)
            {
                _log.Debug($"Deleting fileEntity {fileEntity.FileName}: {fileEntity.LogType}.");
                try
                {
                    this._filesStorage.DeleteFile(
                        fileEntity.LogType.GetFolderPrefix(),
                        fileEntity.LogId, baseDirPath, fileEntity.FileName);
                }
                catch (IOException e)
                {
                    _log.Error("Error deleting log file.", e);
                }
            }
        }

        private string GetFilesDirPath(int customerId)
        {
            return _settingsLogic.GetFilePath(customerId);
        }

        #region Replace case data

        private void ProcessReplaceCasesData(Case c, DataPrivacyParameters parameters, IUnitOfWork uow, List<CaseFileEntity> caseFilesToDelete)
        {
            var replaceDataWith = parameters.ReplaceDataWith;
            var replaceDatesWith = parameters.ReplaceDatesWith;

            var caseFiles = uow.GetRepository<CaseFile>();
            var logFiles = uow.GetRepository<LogFile>();
            var followers = uow.GetRepository<CaseExtraFollower>();

            foreach (var fieldName in parameters.FieldsNames)
            {
                if (fieldName == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.Description.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString() ||
                    fieldName == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString()
                )
                {
                    var property = c.GetType().GetProperty(fieldName);
                    if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                    property.SetValue(c, replaceDataWith);
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Region_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Department_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.OU_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.System_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Category_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                         fieldName == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()

                )
                {
                    var property = c.GetType().GetProperty(fieldName);
                    if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                    property.SetValue(c, null);

                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                {
                    c.PersonsName = replaceDataWith; //first name last name?
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                {
                    c.PersonsEmail = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                {
                    c.PersonsPhone = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                {
                    c.PersonsCellphone = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.ReportedBy = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Person_Name = replaceDataWith; //first last name?
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Person_Email = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Person_Phone = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Person_Cellphone = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Region_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Department_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.OU_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.CostCentre = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.Place = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
                {
                    if (c.IsAbout != null)
                        c.IsAbout.UserCode = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                {
                    c.InventoryType = replaceDataWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                {
                    c.User_Id = null;
                    c.RegUserId = ReplaceWith(c.RegUserId, replaceDataWith);
                    c.RegUserDomain = ReplaceWith(c.RegUserDomain, replaceDataWith);
                    c.RegUserName = ReplaceWith(c.RegUserName, replaceDataWith);
                    c.IpAddress = ReplaceWith(c.IpAddress, replaceDataWith);

                    //clean logs
                    foreach (var log in c.Logs.ToList())
                    {
                        log.RegUser = replaceDataWith;
                        log.User_Id = null;
                    }
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                {
                    c.RegistrationSourceCustomer_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                {
                    c.AgreedDate = replaceDatesWith;
                }
                //else if (fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString())
                //{
                //    c.Cost = 0;
                //    c.OtherCost = 0;
                //    c.Currency = replaceDataWith;
                //}
                else if (fieldName == GlobalEnums.TranslationCaseFields.Project.ToString())
                {
                    c.Project_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Problem.ToString())
                {
                    c.Problem_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                {
                    c.CausingPartId = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.Change.ToString())
                {
                    c.Change_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                {
                    c.PlanDate = replaceDatesWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                {
                    c.WatchDate = replaceDatesWith;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                {
                    c.SolutionRate = string.Empty;
                }
                else if (fieldName == "tblLog.Text_External")
                {
                    foreach (var log in c.Logs.ToList())
                    {
                        log.Text_External = replaceDataWith;
                    }
                }
                else if (fieldName == "tblLog.Text_Internal")
                {
                    foreach (var log in c.Logs.ToList())
                    {
                        log.Text_Internal = replaceDataWith;
                    }
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
                {
                    foreach (var log in c.Logs.ToList())
                    {
                        log.FinishingType = null;
                    }
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                {
                    c.FinishingDate = replaceDatesWith;
                }
                else if (fieldName == AdditionalDataPrivacyFields.SelfService_RegUser.ToString())
                {
                    foreach (var log in c.Logs.ToList())
                    {
                        log.RegUser = replaceDataWith;
                    }
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString())
                {
                    if (c.CaseFollowers.Any())
                    {
                        followers.DeleteRange(c.CaseFollowers);
                    }
                }
            }

            if (parameters.RemoveCaseAttachments && c.CaseFiles.Any())
            {
                var items = c.CaseFiles.Select(f => new CaseFileEntity(f.Case_Id, c.CaseNumber.ToString(), f.FileName)).ToList();
                if (items.Any())
                {
                    //store to delete from disk
                    caseFilesToDelete.AddRange(items);

                    //delete from db
                    caseFiles.DeleteRange(c.CaseFiles);
                }
            }

            if (parameters.RemoveLogAttachments && c.Logs.Any())
            {
                var logFilesEnitites = c.Logs.SelectMany(l => l.LogFiles).ToArray();
                if (logFilesEnitites.Any())
                {
                    //store to delete from disk
                    var items = logFilesEnitites.Select(l => new CaseFileEntity(c.Id, c.CaseNumber.ToString(), l.Log_Id, l.FileName, l.LogType)).ToList();
                    caseFilesToDelete.AddRange(items);

                    //delete from db
                    logFiles.DeleteRange(logFilesEnitites);
                }
            }

            if (parameters.RemoveFileViewLogs)
            {
                var fileAccessLog = uow.GetRepository<FileViewLogEntity>();
                fileAccessLog.DeleteWhere(f => f.Case_Id == c.Id);
            }
        }

        private void ProcessReplaceCasesHistoryData(Case c, DataPrivacyParameters parameters)
        {
            if (c.CaseHistories.Any())
            {
                var replaceDataWith = parameters.ReplaceDataWith;
                var replaceDatesWith = parameters.ReplaceDatesWith;

                foreach (var caseHistory in c.CaseHistories.ToList())
                {
                    foreach (var fieldName in parameters.FieldsNames)
                    {
                        if (fieldName == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Description.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.ClosingReason.ToString()
                        )
                        {
                            var property = caseHistory.GetType().GetProperty(fieldName);
                            if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                            property.SetValue(caseHistory, replaceDataWith);

                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Region_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Department_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.OU_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.System_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Category_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()
                        )
                        {
                            var property = caseHistory.GetType().GetProperty(fieldName);
                            if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                            property.SetValue(caseHistory, null);

                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                        {
                            caseHistory.PersonsName = replaceDataWith; //first name last name?
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                        {
                            caseHistory.PersonsEmail = replaceDataWith;
                        }
                        if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                        {
                            caseHistory.PersonsPhone = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                        {
                            caseHistory.PersonsCellphone = replaceDataWith;
                        }
                        // No such fields in Case History
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                        //{
                        //    //ch.IsAbout_Persons_EMail = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                        //{
                        //    //ch.IsAbout_Persons_CellPhone = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                        //{
                        //    //ch.IsAbout_Region_Id = null;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                        //{
                        //    //ch.IsAbout_OU_Id = null;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                        //{
                        //    //ch.IsAbout_CostCentre = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                        //{
                        //    //ñh.IsAbout_Place = replaceDataWith;
                        //}
                        else if (fieldName == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                        {
                            caseHistory.InventoryType = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                        {
                            caseHistory.User_Id = null;
                            caseHistory.RegUserId = ReplaceWith(caseHistory.RegUserId, replaceDataWith);
                            caseHistory.RegUserDomain = ReplaceWith(caseHistory.RegUserDomain, replaceDataWith);
                            caseHistory.IpAddress = ReplaceWith(caseHistory.IpAddress, replaceDataWith);
                            caseHistory.CreatedByUser = ReplaceWith(caseHistory.CreatedByUser, replaceDataWith);
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                        {
                            caseHistory.RegistrationSourceCustomer_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                        {
                            caseHistory.AgreedDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString())
                        {
                            caseHistory.Cost = 0;
                            caseHistory.OtherCost = 0;
                            caseHistory.Currency = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Project.ToString())
                        {
                            caseHistory.Project_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Problem.ToString())
                        {
                            caseHistory.Problem_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                        {
                            caseHistory.CausingPartId = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Change.ToString())
                        {
                            caseHistory.Change_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                        {
                            caseHistory.PlanDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                        {
                            caseHistory.WatchDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                        {
                            caseHistory.SolutionRate = string.Empty;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                        {
                            caseHistory.FinishingDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString())
                        {
                            caseHistory.CaseExtraFollowers = replaceDataWith;
                        }
                    }

                    if (parameters.ReplaceEmails)
                    {
                        var toReplace = c.CaseHistories.SelectMany(ch => ch.Emaillogs).ToList();//emailLogs.GetAll().Where(x => x.CaseHistory_Id == caseHistory.Id).ToList();
                        if (toReplace.Any())
                        {
                            toReplace.ForEach(e => e.EmailAddress = e.Bcc = e.Cc = replaceDataWith);
                        }

                        if (c.Mail2Tickets.Any())
                        {
                            foreach (var mail2Ticket in c.Mail2Tickets)
                            {
                                mail2Ticket.EMailAddress = replaceDataWith;
                                mail2Ticket.CreatedDate = replaceDatesWith ?? DateTime.Now; ;
                            }
                        }
                    }
                }
            }
        }

        private string ReplaceWith(string curValue, string resetValue)
        {
            if (string.IsNullOrEmpty(curValue))
            {
                return curValue;
            }
            return resetValue;
        }

        private void ProcessExtededCaseData(Case c, DataPrivacyParameters parameters, IUnitOfWork uow)
        {
            var replaceDataWith = parameters.ReplaceDataWith;
            var replaceDatesWith = parameters.ReplaceDatesWith;

            var caseExtendedCaseRep = uow.GetRepository<Case_ExtendedCaseEntity>();
            var caseSectionExtendedCaseRep = uow.GetRepository<Case_CaseSection_ExtendedCase>();
            var extendedCaseValuesRep = uow.GetRepository<ExtendedCaseValueEntity>();

            if (c.CaseExtendedCaseDatas != null && c.CaseExtendedCaseDatas.Any())
            {
                foreach (var caseData in c.CaseExtendedCaseDatas.ToList())
                {
                    CleanExtendedCaseData(extendedCaseValuesRep, caseData.ExtendedCaseData, replaceDataWith, replaceDatesWith);
                    if (caseData.ExtendedCaseForm != null)
                    {
                        caseData.ExtendedCaseForm.CreatedBy = replaceDataWith;
                        caseData.ExtendedCaseForm.CreatedOn = replaceDatesWith ?? DateTime.Now;
                        caseData.ExtendedCaseForm.UpdatedBy = replaceDataWith;
                        caseData.ExtendedCaseForm.UpdatedOn = replaceDatesWith;
                    }
                }
                caseExtendedCaseRep.DeleteRange(c.CaseExtendedCaseDatas);
            }

            if (c.CaseSectionExtendedCaseDatas != null && c.CaseSectionExtendedCaseDatas.Any())
            {
                foreach (var caseData in c.CaseSectionExtendedCaseDatas.ToList())
                {
                    CleanExtendedCaseData(extendedCaseValuesRep, caseData.ExtendedCaseData, replaceDataWith, replaceDatesWith);
                }
                caseSectionExtendedCaseRep.DeleteRange(c.CaseSectionExtendedCaseDatas);
            }
        }

        private static void CleanExtendedCaseData(
            Dal.NewInfrastructure.IRepository<ExtendedCaseValueEntity> extendedCaseValuesRep,
            ExtendedCaseDataEntity caseData,
            string replaceDataWith,
            DateTime? replaceDatesWith)
        {
            if (caseData != null)
            {
                if (caseData.ExtendedCaseValues != null && caseData.ExtendedCaseValues.Any())
                {
                    extendedCaseValuesRep.DeleteRange(caseData.ExtendedCaseValues);
                }
                caseData.CreatedBy = replaceDataWith;
                caseData.CreatedOn = replaceDatesWith ?? DateTime.Now;
                caseData.UpdatedBy = replaceDataWith;
                caseData.UpdatedOn = replaceDatesWith;
            }
        }

        private void ProccessFormPlusCaseData(IList<int> casesIds, IUnitOfWork uow)
        {
            if (!casesIds.Any()) return;

            var formFieldValueHistoryRep = uow.GetRepository<FormFieldValueHistory>();
            var formFieldValueRep = uow.GetRepository<FormFieldValue>();

            formFieldValueRep.DeleteWhere(x => casesIds.Contains(x.Case_Id));
            formFieldValueHistoryRep.DeleteWhere(x => casesIds.Contains(x.Case_Id));
        }

        #endregion

        #region Audit Methods

        private void SaveSuccessOperationAudit(int customerId, int userId, DataPrivacyParameters dataPrivacyParameters, DataPrivacyResult result)
        {
            var operationName = GDPROperations.DataPrivacy.ToString();
            SaveSuccessOperationAuditInner(customerId, userId, operationName, dataPrivacyParameters, result);
        }

        private void SaveStepSuccessOperationAudit(int customerId, int userId, DataPrivacyParameters dataPrivacyParameters, DataPrivacyResult result, int step)
        {
            var operationName = $"{GDPROperations.DataPrivacy}_{step}";
            SaveSuccessOperationAuditInner(customerId, userId, operationName, dataPrivacyParameters, result);
        }

        private void SaveSuccessOperationAuditInner(int customerId, int userId, string operationName, DataPrivacyParameters dataPrivacyParameters, DataPrivacyResult result)
        {
            var audiData = new GDPROperationsAudit()
            {
                User_Id = userId,
                Customer_Id = customerId,
                Parameters = _jsonSerializeService.Serialize(dataPrivacyParameters),
                Operation = operationName,
                Application = ApplicationType.Helpdesk.ToString(),
                CreatedDate = DateTime.UtcNow,
                Result = result.CaseIdsResult.Any() ? string.Join(",", result.CaseIdsResult.ToArray()) : null,
                ErrorResultCaseNumbers = result.CaseNumbersErrorResult.Any() ? string.Join(",", result.CaseNumbersErrorResult.ToArray()) : null,
                ResultCaseNumbers = result.CaseNumbersResult.Any() ? string.Join(",", result.CaseNumbersResult.ToArray()) : null,
                Error = result.ErrorMessage,
                Success = true
            };

            _gdprOperationsAuditRespository.Add(audiData);
            _gdprOperationsAuditRespository.Commit();
        }

        private void SaveFailedOperationAudit(int customerId, int userId, DataPrivacyParameters dataPrivacyParameters, string error)
        {
            var audiData = new GDPROperationsAudit()
            {
                User_Id = userId,
                Customer_Id = customerId,
                Parameters = _jsonSerializeService.Serialize(dataPrivacyParameters),
                Operation = GDPROperations.DataPrivacy.ToString(),
                Application = ApplicationType.Helpdesk.ToString(),
                CreatedDate = DateTime.UtcNow,
                Success = false,
                Error = error
            };

            _gdprOperationsAuditRespository.Add(audiData);
            _gdprOperationsAuditRespository.Commit();
        }

        #endregion
    }
}