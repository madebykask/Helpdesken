using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Domain.Notifiers;

    public interface IComputerService
    {
        IDictionary<string, string> Validate(ComputerUsersBlackList computerUsersBlackList);

        IList<ComputerUser> GetComputerUsers(int customerId);
        IList<ComputerUser> SearchSortAndGenerateComputerUsers(int customerId, IComputerUserSearch searchComputerUsers);
        IList<UserSearchResults> SearchComputerUsers(int customerId, string searchFor);
        IList<ComputerUserFieldSettings> GetComputerUserFieldSettings(int customerId);
        IList<ComputerUserGroup> GetComputerUserGroups(int customerId);
        IList<ComputerUsersBlackList> GetComputerUsersBlackLists();

        IList<ComputerResults> SearchComputer(int customerId, string searchFor);
        ComputerResults GetComputerInventory(string computername, bool join);
        ComputerUser GetComputerUser(int id);
        ComputerUserGroup GetComputerUserGroup(int id);
        ComputerUsersBlackList GetComputerUsersBlackList(int id);

        DeleteMessage DeleteComputerUser(int id);
        DeleteMessage DeleteComputerUserGroup(int id);

        void DeleteComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        void NewComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        void SaveComputerUser(ComputerUser computerUser, int[] cugs, out IDictionary<string, string> errors);
        void SaveComputerUserFieldSetting(IList<ComputerUserFieldSettings> computerUserFieldSetting, out IDictionary<string, string> errors);
        void SaveComputerUserGroup(ComputerUserGroup computerUserGroup, int[] ous, out IDictionary<string, string> errors);
        void UpdateComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        void Commit();
    }

    public class ComputerService : IComputerService
    {
        private readonly INotifierFieldSettingRepository _computerUserFieldSettingsRepository;
        private readonly INotifierGroupRepository _computerUserGroupRepository;
        private readonly INotifierRepository _computerUserRepository;
        private readonly IComputerUsersBlackListRepository _computerUsersBlackListRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly IOrganizationUnitRepository _ouRespository;
        private readonly IUnitOfWork _unitOfWork;

        public ComputerService(
            INotifierFieldSettingRepository computerUserFieldSettingsRepository,
            INotifierGroupRepository computerUserGroupRepository,
            INotifierRepository computerUserRepository,
            IComputerUsersBlackListRepository computerUsersBlackListRepository,
            IComputerRepository computerRepository,
            IOrganizationUnitRepository ouRespository,
            IUnitOfWork unitOfWork)
        {
            _computerUserFieldSettingsRepository = computerUserFieldSettingsRepository;
            _computerUserGroupRepository = computerUserGroupRepository;
            _computerUserRepository = computerUserRepository;
            _computerUsersBlackListRepository = computerUsersBlackListRepository;
            _computerRepository = computerRepository;
            _ouRespository = ouRespository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ComputerUsersBlackList computerUsersBlackListToValidate)
        {
            if (computerUsersBlackListToValidate == null)
                throw new ArgumentNullException("computeruersblacklisttovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public ComputerResults GetComputerInventory(string computername, bool join)
        {
            return _computerRepository.GetComputerInventory(computername, join);
        }

        public IList<ComputerUser> GetComputerUsers(int customerId)
        {
            return _computerUserRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SyncChangedDate).ToList();
        }

        public IList<ComputerUser> SearchSortAndGenerateComputerUsers(int customerId, IComputerUserSearch searchComputerUsers)
        {
            var query = (from cu in _computerUserRepository.GetAll().Where(x => x.Customer_Id == customerId && x.Updated != 2)
                         select cu);

            if (searchComputerUsers.DomainId.HasValue)
                query = query.Where(x => x.Domain_Id == searchComputerUsers.DomainId);
            if (searchComputerUsers.RegionId.HasValue)
                query = query.Where(x => x.Department != null && x.Department.Region_Id == searchComputerUsers.RegionId);
            if (searchComputerUsers.DepartmentId.HasValue)
                query = query.Where(x => x.Department_Id == searchComputerUsers.DepartmentId);
            if (searchComputerUsers.OuId.HasValue)
                query = query.Where(x => x.OU_Id == searchComputerUsers.OuId);
            if (searchComputerUsers.DivisionId.HasValue)
                query = query.Where(x => x.Division_Id == searchComputerUsers.DivisionId);

            if (searchComputerUsers.StatusId.HasValue)
            {
                if (searchComputerUsers.StatusId == 2)
                    query = query.Where(x => x.Status == 0);
                else if (searchComputerUsers.StatusId == 1)
                    query = query.Where(x => x.Status == 1);
            }

            if (!string.IsNullOrEmpty(searchComputerUsers.SearchCompUs))
            {
                string s = searchComputerUsers.SearchCompUs.ToLower();
                query = query.Where(x => x.UserId.ToLower().Contains(s)
                    || x.LogonName.ToLower().Contains(s)
                    || x.FirstName.ToLower().Contains(s)
                    || x.SurName.ToLower().Contains(s)
                    || x.Location.ToLower().Contains(s)
                    || x.Phone.ToLower().Contains(s)
                    || x.Cellphone.ToLower().Contains(s)
                    || x.Email.ToLower().Contains(s)
                    || x.UserCode.ToLower().Contains(s));
            }

            return query.OrderBy(x => x.SurName).ToList();
        }

        public IList<UserSearchResults> SearchComputerUsers(int customerId, string searchFor)
        {
            return _computerUserRepository.Search(customerId, searchFor);
        }

        public IList<ComputerResults> SearchComputer(int customerId, string searchFor)
        {
            return _computerRepository.Search(customerId, searchFor);
        }

        public IList<ComputerUserFieldSettings> GetComputerUserFieldSettings(int customerId)
        {
            return _computerUserFieldSettingsRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<ComputerUserGroup> GetComputerUserGroups(int customerId)
        {
            return _computerUserGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<ComputerUsersBlackList> GetComputerUsersBlackLists()
        {
            return _computerUsersBlackListRepository.GetAll().OrderBy(x => x.User_Id).ToList();
        }

        public ComputerUser GetComputerUser(int id)
        {
            return _computerUserRepository.GetById(id);
        }

        public ComputerUserGroup GetComputerUserGroup(int id)
        {
            return _computerUserGroupRepository.GetById(id);
        }

        public ComputerUsersBlackList GetComputerUsersBlackList(int id)
        {
            return _computerUsersBlackListRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteComputerUser(int id)
        {
            var computerUser = _computerUserRepository.GetById(id);

            if (computerUser != null)
            {
                try
                {
                    _computerUserRepository.Delete(computerUser);
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

        public DeleteMessage DeleteComputerUserGroup(int id)
        {
            var computerUserGroup = _computerUserGroupRepository.GetById(id);

            if (computerUserGroup != null)
            {
                try
                {
                    _computerUserGroupRepository.Delete(computerUserGroup);
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

        public void DeleteComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            _computerUsersBlackListRepository.Delete(computerUsersBlackList);
        }

        public void NewComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            computerUsersBlackList.ChangedDate = DateTime.UtcNow;
            _computerUsersBlackListRepository.Add(computerUsersBlackList);
        }

        public void SaveComputerUser(ComputerUser computerUser, int[] cugs, out IDictionary<string, string> errors)
        {
            if (computerUser == null)
                throw new ArgumentNullException("computeruser");

            errors = new Dictionary<string, string>();

            computerUser.Cellphone = computerUser.Cellphone ?? string.Empty;
            computerUser.City = computerUser.City ?? string.Empty;
            computerUser.DisplayName = computerUser.DisplayName ?? string.Empty;
            computerUser.Email = computerUser.Email ?? string.Empty;
            computerUser.FirstName = computerUser.FirstName ?? string.Empty;
            computerUser.FullName = computerUser.FullName ?? string.Empty;
            computerUser.homeDirectory = computerUser.homeDirectory ?? string.Empty;
            computerUser.homeDrive = computerUser.homeDrive ?? string.Empty;
            computerUser.Info = computerUser.Info ?? string.Empty;
            computerUser.Initials = computerUser.Initials ?? string.Empty;
            computerUser.Location = computerUser.Location ?? string.Empty;
            computerUser.LogonName = computerUser.LogonName ?? string.Empty;
            computerUser.NDSpath = computerUser.NDSpath ?? string.Empty;
            computerUser.Password = computerUser.Password ?? string.Empty;
            computerUser.Phone = computerUser.Phone ?? string.Empty;
            computerUser.Phone2 = computerUser.Phone2 ?? string.Empty;
            computerUser.PostalAddress = computerUser.PostalAddress ?? string.Empty;
            computerUser.Postalcode = computerUser.Postalcode ?? string.Empty;
            computerUser.SOU = computerUser.SOU ?? string.Empty;
            computerUser.SurName = computerUser.SurName ?? string.Empty;
            computerUser.Title = computerUser.Title ?? string.Empty;
            computerUser.UserCode = computerUser.UserCode ?? string.Empty;
            computerUser.UserGUID = computerUser.UserGUID ?? string.Empty;
            computerUser.UserId = computerUser.UserId ?? string.Empty;

            //if (string.IsNullOrEmpty(computerUser.FirstName))
            //    errors.Add("ComputerUser.FirstName", "Du måste ange ett förnamn");
            //if (string.IsNullOrEmpty(computerUser.SurName))
            //    errors.Add("ComputerUser.SurName", "Du måste ange ett efternamn");
            //if (string.IsNullOrEmpty(computerUser.Phone))
            //    errors.Add("ComputerUser.Phone", "Du måste ange ett telefonnummer");
            //if (string.IsNullOrEmpty(computerUser.Email))
            //    errors.Add("ComputerUser.Email", "Du måste ange en e-postadress");

            if (computerUser.CUGs != null)
                foreach (var delete in computerUser.CUGs.ToList())
                    computerUser.CUGs.Remove(delete);
            else
                computerUser.CUGs = new List<ComputerUserGroup>();

            if (cugs != null)
            {
                foreach (int id in cugs)
                {
                    var cg = _computerUserGroupRepository.GetById(id);

                    if (cg != null)
                        computerUser.CUGs.Add(cg);
                }
            }

            if (computerUser.Id == 0)
                _computerUserRepository.Add(computerUser);
            else
                _computerUserRepository.Update(computerUser);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveComputerUserFieldSetting(IList<ComputerUserFieldSettings> computerUserFieldSetting, out IDictionary<string, string> errors)
        {
            if (computerUserFieldSetting == null)
                throw new ArgumentNullException("computeruserfieldsetting");

            errors = new Dictionary<string, string>();

            foreach (var setting in computerUserFieldSetting.ToList())
            {
                setting.LDAPAttribute = setting.LDAPAttribute ?? string.Empty;

                if (setting.Id == 0)
                    _computerUserFieldSettingsRepository.Add(setting);
                else
                    _computerUserFieldSettingsRepository.Update(setting);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveComputerUserGroup(ComputerUserGroup computerUserGroup, int[] ous, out IDictionary<string, string> errors)
        {
            if (computerUserGroup == null)
                throw new ArgumentNullException("computerusergroup");

            computerUserGroup.Path = computerUserGroup.Path == null ? string.Empty : computerUserGroup.Path;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(computerUserGroup.Name))
                errors.Add("ComputerUserGroup.Name", "Du måste ange en AD-grupp");

            if (string.IsNullOrEmpty(computerUserGroup.Path))
                errors.Add("ComputerUserGroup.Path", "Du måste ange en LDAP-sökväg");

            if (computerUserGroup.OUs != null)
                foreach (var delete in computerUserGroup.OUs.ToList())
                    computerUserGroup.OUs.Remove(delete);
            else
                computerUserGroup.OUs = new List<OU>();

            if (ous != null)
            {
                foreach (int id in ous)
                {
                    var ou = _ouRespository.GetById(id);

                    if (ou != null)
                        computerUserGroup.OUs.Add(ou);
                }
            }

            computerUserGroup.ChangedDate = DateTime.UtcNow;

            if (computerUserGroup.Id == 0)
                _computerUserGroupRepository.Add(computerUserGroup);
            else
                _computerUserGroupRepository.Update(computerUserGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void UpdateComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            computerUsersBlackList.ChangedDate = DateTime.UtcNow;
            _computerUsersBlackListRepository.Update(computerUsersBlackList);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}

