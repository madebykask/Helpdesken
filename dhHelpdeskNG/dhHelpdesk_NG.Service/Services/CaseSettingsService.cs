namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICaseSettingsService
    {
        IList<CaseSettings> GetCaseSettings(int customerId);
        IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? UserGroupId);
        IList<CaseSettings> GetCaseSettingsWithUser(int customerId, int userId, int userGroupId);
        IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int UserGroupId);
        IList<CaseSettings> GetCaseSettingsForDefaultCust();
        //IList<CaseSettings> GetCaseSettingsByCopyUserId(int userId);

        CaseSettings GetCaseSetting(int id);

        DeleteMessage DeleteCaseSetting(int id);

        string SetListCaseName(int labelId);

        void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors);

        void UpdateCaseSetting(CaseSettings updatedCaseSetting, out IDictionary<string, string> errors);

        void ReOrderCaseSetting(List<string> caseSettingIds);

        void Commit();
    }

    public class CaseSettingsService : ICaseSettingsService
    {
        private readonly ICaseSettingRepository _caseSettingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserGroupRepository _userGroupRepository;

        public CaseSettingsService(
            ICaseSettingRepository caseSettingRepository,
            IUnitOfWork unitOfWork,
            IUserGroupRepository userGroupRepository)
        {
            this._caseSettingRepository = caseSettingRepository;
            this._unitOfWork = unitOfWork;
            this._userGroupRepository = userGroupRepository;
        }

        public IList<CaseSettings> GetCaseSettings(int customerId)
        {
            return this._caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == null).OrderBy(x => x.ColOrder).ToList();
        }

        public IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int usergroupId)
        {
            return this._caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == null && x.UserGroup == usergroupId).OrderBy(x => x.ColOrder).ToList();
        }

        public IList<CaseSettings> GetCaseSettingsForDefaultCust()
        {
            var list = this._caseSettingRepository.GetAll().Where(x => x.Customer_Id == null).ToList();

            return list;

        }

        public IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? UserGroupId)
        {
            var query = (from cs in this._caseSettingRepository.GetAll().Where(x => x.Customer_Id == customerId && x.Name != null && x.User_Id == null)
                         select cs);

            if (UserGroupId.HasValue)
            {
                if (UserGroupId == 1)
                {
                    query = query.Where(x => x.UserGroup == 1);
                }
                else if (UserGroupId == 2)
                {
                    query = query.Where(x => x.UserGroup == 2);
                }
                else if (UserGroupId == 3)
                {
                    query = query.Where(x => x.UserGroup == 3);
                }
                else if (UserGroupId == 4)
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

            IList<CaseSettings> csl = this._caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == userId)
                .OrderBy(x => x.ColOrder)
                .ToList();
            if (csl.Count == 0)
            {
                csl = this.GetCaseSettingsByUserGroup(customerId, userGroupId);

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
                    this.SaveCaseSetting(newCaseSetting, out errors);
                }
            }

            csl = this._caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == userId).OrderBy(x => x.ColOrder).ToList();

            //// we does not support multiline in cases overview
            return csl.Where(it => it.Line == 1).ToList();
        }
        
        public CaseSettings GetCaseSetting(int id)
        {
            return this._caseSettingRepository.GetById(id);
        }


        public DeleteMessage DeleteCaseSetting(int id)
        {
            var caseSetting = this._caseSettingRepository.GetById(id);

            if (caseSetting != null)
            {
                try
                {
                    this._caseSettingRepository.Delete(caseSetting);
                    this.Commit();

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
            var name = this._caseSettingRepository.SetListCaseName(labelId);

            return name;
        }

        public void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors)
        {
            if (caseSetting == null)
                throw new ArgumentNullException("caseSettings");

            errors = new Dictionary<string, string>();

            if (caseSetting.Id == 0)
                this._caseSettingRepository.Add(caseSetting);
            else
                this._caseSettingRepository.Update(caseSetting);

            if (errors.Count == 0)
                this.Commit();
        }

        public void ReOrderCaseSetting(List<string> caseSettingIds)
        {                       
            this._caseSettingRepository.ReOrderCaseSetting(caseSettingIds);                        
            this.Commit();
        }

        public void UpdateCaseSetting(CaseSettings updatedCaseSetting, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            _caseSettingRepository.UpdateCaseSetting(updatedCaseSetting);
            _caseSettingRepository.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}