using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Dal.Infrastructure;
    using Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using Domain;

    using LinqLib.Operators;

    public interface ICaseSettingsService
    {
        IList<CaseSettings> GetCaseSettings(int customerId, int? userId = null, CaseSettingTypes? type = CaseSettingTypes.CaseOverview);
        IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? userGroupId, CaseSettingTypes type = CaseSettingTypes.CaseOverview);
        IList<CaseSettings> GetCaseSettingsWithUser(int customerId, int userId, int userGroupId);
        IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int userGroupId, CaseSettingTypes type = CaseSettingTypes.CaseOverview);
        IList<CaseSettings> GetCaseSettingsForDefaultCust();
        IEnumerable<CaseOverviewGridColumnSetting> GetAvailableCaseOverviewGridColumnSettings(int customerId, IList<CaseFieldSetting> customerCaseFieldSettings = null);
        IEnumerable<CaseOverviewGridColumnSetting> GetAvailableCaseOverviewGridColumnSettingsByUserGroup(int customerId, int userGroupId);
        IEnumerable<CaseOverviewGridColumnSetting> GetSelectedCaseOverviewGridColumnSettings(int customerId, int userId, IList<CaseFieldSetting> customerCaseFieldSettings);
        CaseSettings GetCaseSetting(int id);
        DeleteMessage DeleteCaseSetting(int id);
        string SetListCaseName(int labelId);
        void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors);
        void UpdateCaseSetting(CaseSettings updatedCaseSetting, out IDictionary<string, string> errors);
        void ReOrderCaseSetting(List<string> caseSettingIds);
        void Commit();
        void SyncSettings(CaseOverviewGridColumnSetting[] input, int customerId, int userId, int userGroupId);
    }

    public class CaseSettingsService : ICaseSettingsService
    {
        private readonly ICaseSettingRepository _caseSettingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaseFieldSettingService _caseFieldSettingService;

        public CaseSettingsService(
            ICaseSettingRepository caseSettingRepository,
            IUnitOfWork unitOfWork,
            ICaseFieldSettingService caseFieldSettingService)
        {
            _caseSettingRepository = caseSettingRepository;
            _unitOfWork = unitOfWork;
            _caseFieldSettingService = caseFieldSettingService;
        }

        public IList<CaseSettings> GetCaseSettings(int customerId, int? userId = null, CaseSettingTypes? type = CaseSettingTypes.CaseOverview)
        {
            var query = _caseSettingRepository
                .GetMany(x => x.Customer_Id == customerId && x.User_Id == userId).AsQueryable();

            if(type.HasValue)
                query = query.Where(x => x.Type == type);

            return query.OrderBy(x => x.ColOrder)
                .ToList();
        }

        public IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int usergroupId, CaseSettingTypes type = CaseSettingTypes.CaseOverview)
        {
            return _caseSettingRepository
                .GetMany(x => x.Customer_Id == customerId && x.User_Id == null && x.UserGroup == usergroupId && x.Type == type)
                .OrderBy(x => x.ColOrder)
                .ToList();
        }

        public IList<CaseSettings> GetCaseSettingsForDefaultCust()
        {
            return _caseSettingRepository
                .GetMany(x => x.Customer_Id == null)
                .ToList();
        }

        /// <summary>
        /// Returns ll available columns for case overview grid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerCaseFieldSettings"></param>
        /// <returns></returns>
        public IEnumerable<CaseOverviewGridColumnSetting> GetAvailableCaseOverviewGridColumnSettings(int customerId, IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            if (customerCaseFieldSettings == null)
            {
                customerCaseFieldSettings = _caseFieldSettingService.GetCustomerEnabledCaseFieldSettings(customerId);
            }

            var customerEnabledFields =
                customerCaseFieldSettings
                    .Where(x => x.ShowOnStartPage == 1 && !GridColumnsDefinition.NotAvailableField.Contains(x.Name))
                    .Select(it => new CaseOverviewGridColumnSetting() { Name = it.Name })
                    .ToList();

            customerEnabledFields.AddRange(CaseOverviewGridColumnSetting.GetDefaulVirtualFields());
            return customerEnabledFields;
        }

        /// <summary>
        /// Returns ll available columns for case overview grid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public IEnumerable<CaseOverviewGridColumnSetting> GetAvailableCaseOverviewGridColumnSettingsByUserGroup(int customerId, int userGroupId)
        {
            var customerEnabledFields =
                GetCaseSettingsByUserGroup(customerId, userGroupId)
                    .Where(it => !GridColumnsDefinition.NotAvailableField.Contains(it.Name))
                    .Select(it => new CaseOverviewGridColumnSetting() { Name = it.Name }).ToList();
            return customerEnabledFields;
        }

        /// <summary>
        /// Returns columns for case connect to parent grid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public IEnumerable<CaseOverviewGridColumnSetting> GetConnectToParentGridColumnSettings(int customerId, int userGroupId)
        {
            return 
                GridColumnsDefinition.CaseConnectToParentColumns.Select(
                    x => new CaseOverviewGridColumnSetting() {Name = x}).ToList();
        }

        /// <summary>
        /// Returns column settings for case overview table selected by user 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="customerCaseFieldSettings"></param>
        /// <returns></returns>
        public IEnumerable<CaseOverviewGridColumnSetting> GetSelectedCaseOverviewGridColumnSettings(int customerId, int userId, IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            var duplicates = new HashSet<string>();
            var res =
                GetAvailableCaseOverviewGridColumnSettings(customerId, customerCaseFieldSettings)
                    .Join(
                        GetAvailableCaseSettings(customerId, userId),
                        colSetting => colSetting.Name,
                        userSelection => userSelection.Name,
                        (colSetting, userSelection) =>
                        new
                            {
                                caseSettingsId = userSelection.Id,
                                Name = userSelection.Name,
                                Order = userSelection.ColOrder,
                                Style = userSelection.ColStyle
                            })
                    .OrderBy(it => it.Order)
                     // data in DB can contain same values in the Order field, so sorting also by fieldId
                    .ThenBy(it => it.caseSettingsId)
                    .Where(it => !duplicates.Contains(it.Name.ToLower()))
                    .Select(
                        it =>
                            {
                                duplicates.Add(it.Name.ToLower());
                                return new CaseOverviewGridColumnSetting()
                                           {
                                               Name = it.Name,
                                               Order = it.Order,
                                               Style = it.Style
                                           };
                            });
            return res;
        }

        public IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? userGroupId, CaseSettingTypes type = CaseSettingTypes.CaseOverview)
        {
            var query = _caseSettingRepository
                .GetMany(x => x.Customer_Id == customerId && x.Name != null && x.User_Id == null && x.Type == type)
                .AsQueryable();

            if (userGroupId.HasValue)
            {
                if (userGroupId == 1)
                {
                    query = query.Where(x => x.UserGroup == 1);
                }
                else if (userGroupId == 2)
                {
                    query = query.Where(x => x.UserGroup == 2);
                }
                else if (userGroupId == 3)
                {
                    query = query.Where(x => x.UserGroup == 3);
                }
                else if (userGroupId == 4)
                {
                    query = query.Where(x => x.UserGroup == 4);
                }
                else
                    query = query.DefaultIfEmpty();
            }

            return query.OrderBy(x => x.ColOrder).ToList();
        }

        /// <summary>
        /// Returns all case field settins for selected user and customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public IList<CaseSettings> GetCaseSettingsWithUser(int customerId, int userId, int userGroupId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var csl = GetCaseSettings(customerId, userId);

			var hasNewSetting = false;
			if (csl.Count == 0)
            {
                csl = GetCaseSettingsByUserGroup(customerId, userGroupId);

                foreach (var cfs in csl)
                {
                    var newCaseSetting = new CaseSettings();

                    newCaseSetting.Id = 0;
                    newCaseSetting.Customer_Id = customerId;
                    newCaseSetting.User_Id = userId;
                    newCaseSetting.Name = cfs.Name;
                    newCaseSetting.Line = cfs.Line;
                    newCaseSetting.MinWidth = cfs.MinWidth;
                    newCaseSetting.ColOrder = cfs.ColOrder;
                    newCaseSetting.UserGroup = cfs.UserGroup;
                    newCaseSetting.RegTime = DateTime.UtcNow;
                    newCaseSetting.ChangeTime = DateTime.UtcNow;
                    SaveCaseSetting(newCaseSetting, out errors);

					hasNewSetting = true;
				}
            }

			// If new settings have been added retrieve them again (performance check).
			if (hasNewSetting)
				csl = GetCaseSettings(customerId, userId);

			// we does not support multiline in cases overview grid
			return csl.Where(it => it.Line == 1).ToList();
        }
        
        public CaseSettings GetCaseSetting(int id)
        {
            return _caseSettingRepository.GetById(id);
        }


        public DeleteMessage DeleteCaseSetting(int id)
        {
            var caseSetting = _caseSettingRepository.GetById(id);

            if (caseSetting != null)
            {
                try
                {
                    _caseSettingRepository.Delete(caseSetting);
                    Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public string SetListCaseName(int labelId)
        {
            var name = _caseSettingRepository.SetListCaseName(labelId);

            return name;
        }

        public void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors)
        {
            if (caseSetting == null)
                throw new ArgumentNullException("caseSetting");

            errors = new Dictionary<string, string>();

            if (caseSetting.Id == 0)
                _caseSettingRepository.Add(caseSetting);
            else
                _caseSettingRepository.Update(caseSetting);

            if (errors.Count == 0)
                Commit();
        }

        public void ReOrderCaseSetting(List<string> caseSettingIds)
        {                       
            _caseSettingRepository.ReOrderCaseSetting(caseSettingIds);
            Commit();
        }

        public void UpdateCaseSetting(CaseSettings updatedCaseSetting, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            _caseSettingRepository.UpdateCaseSetting(updatedCaseSetting);
            _caseSettingRepository.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Sycnronize supplyed coulmn settings with db
        /// </summary>
        /// <param name="input"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="userGroupId"></param>
        public void SyncSettings(CaseOverviewGridColumnSetting[] input, int customerId, int userId, int userGroupId)
        {
            var inputDictionary = input.ToDictionary(it => it.Name, it => it);
            var duplicatesToDelete = new HashSet<string>();
            var currentSettings = GetAvailableCaseSettings(customerId, userId)
                .Where(
                    it =>
                        {
                            var nameLower = it.Name.ToLower();
                            if (duplicatesToDelete.Contains(nameLower))
                            {
                                _caseSettingRepository.Delete(it);
                                return false;
                            }

                            duplicatesToDelete.Add(nameLower);
                            return true;
                        })
                .ToDictionary(it => it.Name, it => it);
            // update
            currentSettings.Where(it => inputDictionary.ContainsKey(it.Key)).ForEach(
                it =>
                    {
                        var update = inputDictionary[it.Key];
                        it.Value.ColOrder = update.Order;
                        it.Value.ColStyle = update.Style;
                    });
            // delete
            currentSettings.Where(it => !inputDictionary.ContainsKey(it.Key))
                    .ForEach(it => _caseSettingRepository.Delete(it.Value));
            // add
            input.Where(it => !currentSettings.ContainsKey(it.Name)).ForEach(
                it =>
                    {
                        var entity = new CaseSettings()
                                         {
                                             Customer_Id = customerId,
                                             User_Id = userId,
                                             Name = it.Name,
                                             ColOrder = it.Order,
                                             ColStyle = it.Style,
                                             UserGroup = userGroupId,
                                             ChangeTime = DateTime.UtcNow,
                                             RegTime = DateTime.UtcNow,
                                             MinWidth = 100, // just default value taken from head
                                             Line = 1
                                             // we does not support expandable lines in casae overview talbe
                                         };
                        _caseSettingRepository.Add(entity);
                    });
            _caseSettingRepository.Commit();
        }

        private IEnumerable<CaseSettings> GetAvailableCaseSettings(int customerId, int userId)
        {   
            return
                _caseSettingRepository
                    .GetMany(it => it.Customer_Id == customerId && it.User_Id == userId && it.Line == 1 && it.Type == CaseSettingTypes.CaseOverview);
        }
    }
}