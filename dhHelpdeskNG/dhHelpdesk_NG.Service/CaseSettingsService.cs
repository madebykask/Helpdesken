using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseSettingsService
    {
        IList<CaseSettings> GetCaseSettings(int customerId);
        IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? UserGroupId);
        IList<CaseSettings> GetCaseSettingsWithUser(int customerId, int? UserId);
        IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int UserGroupId);

        CaseSettings GetCaseSetting(int id);

        DeleteMessage DeleteCaseSetting(int id);

        string SetListCaseName(int labelId);

        void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors);
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
            _caseSettingRepository = caseSettingRepository;
            _unitOfWork = unitOfWork;
            _userGroupRepository = userGroupRepository;
        }

        public IList<CaseSettings> GetCaseSettings(int customerId)
        {
            return _caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == null).OrderBy(x => x.ColOrder).ToList();
        }

        public IList<CaseSettings> GetCaseSettingsByUserGroup(int customerId, int usergroupId)
        {
            return _caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == null && x.UserGroup == usergroupId).OrderBy(x => x.ColOrder).ToList();
        }

        public IList<CaseSettings> GenerateCSFromUGChoice(int customerId, int? UserGroupId)
        {
            var query = (from cs in _caseSettingRepository.GetAll().Where(x => x.Customer_Id == customerId && x.Name != null && x.User_Id == null)
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

        public IList<CaseSettings> GetCaseSettingsWithUser(int customerId, int? UserId)
        {
            IList<CaseSettings> csl;
                
            csl = _caseSettingRepository.GetMany(x => x.Customer_Id == customerId && x.User_Id == UserId).OrderBy(x => x.ColOrder).ToList();
            if (csl == null)
                csl = GetCaseSettings(customerId);
            else if(csl.Count == 0)    
                csl = GetCaseSettings(customerId);

            return csl;
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
            var name = _caseSettingRepository.SetListCaseName(labelId);

            return name;
        }

        public void SaveCaseSetting(CaseSettings caseSetting, out IDictionary<string, string> errors)
        {
            if (caseSetting == null)
                throw new ArgumentNullException("caseSettings");

            errors = new Dictionary<string, string>();

            if (caseSetting.Id == 0)
                _caseSettingRepository.Add(caseSetting);
            else
                _caseSettingRepository.Update(caseSetting);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}