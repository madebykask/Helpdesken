using DH.Helpdesk.BusinessData.Models.ComputerUsers;
using DH.Helpdesk.BusinessData.Models.Inventory;
using DH.Helpdesk.Dal.Repositories.Inventory;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DHDomain = DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using BusinessData.Models.Notifiers;
	using BusinessData.Models.Case;


	public interface IComputerService
    {
        IDictionary<string, string> Validate(ComputerUsersBlackList computerUsersBlackList);

        IList<ComputerUser> GetComputerUsers(int customerId);
        IList<ComputerUser> SearchSortAndGenerateComputerUsers(int customerId, IComputerUserSearch searchComputerUsers);
		IList<UserSearchResults> SearchComputerUsers(int customerId, string searchFor, int? categoryID = null);
		IList<UserSearchResults> SearchComputerUsersByDepartments(int customerId, string query, List<int> departmentIds, int? categoryID);
		IList<ComputerUserFieldSettings> GetComputerUserFieldSettings(int customerId);
        IList<ComputerUserFieldSettings> GetComputerUserFieldSettingsForDefaultCust();
        IList<ComputerUserGroup> GetComputerUserGroups(int customerId);
        IList<ComputerUsersBlackList> GetComputerUsersBlackLists();
        IList<ComputerUserFieldSettingsLanguage> GetComputerUserFieldSettingsWithLanguages(int customerId, int languageId);
        IList<ComputerUserFieldSettingsLanguageModel> GetComputerUserFieldSettingsWithLanguagesForDefaultCust(int languageId);
                                                 

        IList<ComputerResults> SearchComputer(int customerId, string searchFor);
        ComputerUser GetComputerUser(int id);
        ComputerUserGroup GetComputerUserGroup(int id);
        ComputerUsersBlackList GetComputerUsersBlackList(int id);

        DeleteMessage DeleteComputerUser(int id);
        DeleteMessage DeleteComputerUserGroup(int id);

        void DeleteComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        void NewComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        void SaveComputerUser(ComputerUser computerUser, int[] cugs, out IDictionary<string, string> errors);
        void SaveComputerUserFieldSetting(IList<ComputerUserFieldSettings> computerUserFieldSetting, out IDictionary<string, string> errors);
        void SaveComputerUserFieldSettingForCustomerCopy(ComputerUserFieldSettings computerUserFieldSetting, out IDictionary<string, string> errors);
        void SaveComputerUserFieldSettingLangForCustomerCopy(ComputerUserFieldSettingsLanguage computerUserFieldSettingLanguage, out IDictionary<string, string> errors);
        void SaveComputerUserGroup(ComputerUserGroup computerUserGroup, int[] ous, out IDictionary<string, string> errors);
        void UpdateComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList);
        ComputerUserCategoryOverview GetEmptyComputerUserCategory(int customerId);
        Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly = true);
        List<InventorySearchResult> SearchPcNumber(int customerId, string query);
        ComputerUserCategory GetComputerUserCategoryByID(int computerUserCategoryID);
        int SaveComputerUserCategory(ComputerUserCategoryData data);


        void Commit();

		IList<ComputerUserCategoryOverview> GetComputerUserCategoriesByCustomerID(int customerId, bool includeEmpty = false);
		ComputerUser GetComputerUserByUserID(string userID);
    }

    public class ComputerService : IComputerService
    {
        private readonly INotifierFieldSettingRepository _computerUserFieldSettingsRepository;
        private readonly INotifierGroupRepository _computerUserGroupRepository;
        private readonly INotifierRepository _computerUserRepository;
        private readonly IComputerUsersBlackListRepository _computerUsersBlackListRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly IOrganizationUnitRepository _ouRespository;
        private readonly INotifierFieldSettingLanguageRepository _computerUserFieldSettingLanguageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryRepository _inventoryRepository;
		private readonly IComputerUserCategoryRepository _computerUserCategoryRepository;



		public ComputerService(
            INotifierFieldSettingRepository computerUserFieldSettingsRepository,
            INotifierGroupRepository computerUserGroupRepository,
            INotifierRepository computerUserRepository,
            IComputerUsersBlackListRepository computerUsersBlackListRepository,
            IComputerRepository computerRepository,
            IOrganizationUnitRepository ouRespository,
            INotifierFieldSettingLanguageRepository computerUserFieldSettingLanguageRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
			IComputerUserCategoryRepository computerUserCategoryRepository)

		{
			this._computerUserFieldSettingsRepository = computerUserFieldSettingsRepository;
            this._computerUserGroupRepository = computerUserGroupRepository;
            this._computerUserRepository = computerUserRepository;
            this._computerUsersBlackListRepository = computerUsersBlackListRepository;
            this._computerRepository = computerRepository;
            this._ouRespository = ouRespository;
            this._unitOfWork = unitOfWork;
            this._computerUserFieldSettingLanguageRepository = computerUserFieldSettingLanguageRepository;
			this._computerUserCategoryRepository = computerUserCategoryRepository;
            this._inventoryRepository = inventoryRepository;

		}

		public IDictionary<string, string> Validate(ComputerUsersBlackList computerUsersBlackListToValidate)
        {
            if (computerUsersBlackListToValidate == null)
                throw new ArgumentNullException("computeruersblacklisttovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ComputerUser> GetComputerUsers(int customerId)
        {
            return this._computerUserRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SyncChangedDate).ToList();
        }

        public IList<ComputerUserFieldSettingsLanguage> GetComputerUserFieldSettingsWithLanguages(int customerId, int languageId)
        {
            return this._computerUserFieldSettingLanguageRepository.GetComputerUserFieldSettingsLanguage(customerId, languageId).ToList();
        }

        public IList<ComputerUserFieldSettingsLanguageModel> GetComputerUserFieldSettingsWithLanguagesForDefaultCust(int languageId)
        {
            return this._computerUserFieldSettingLanguageRepository.GetComputerUserFieldSettingsWithLanguagesForDefaultCust(languageId).ToList();
        }

        public IList<ComputerUser> SearchSortAndGenerateComputerUsers(int customerId, IComputerUserSearch searchComputerUsers)
        {
            var query = (from cu in this._computerUserRepository.GetMany(x => x.Customer_Id == customerId && x.Updated != 2)
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

		public IList<UserSearchResults> SearchComputerUsers(int customerId, string searchFor, int? categoryID = null)
		{
			return this._computerUserRepository.Search(customerId, searchFor, categoryID);
        }
        public IList<UserSearchResults> SearchComputerUsersByDepartments(int customerId, string searchFor, List<int> departmentIds, int? categoryID)
        {
            var results = this._computerUserRepository.Search(customerId, searchFor, categoryID).Where(x=>departmentIds.Contains(x.Department_Id ?? 0));
            return results.Where(x => x.Department_Id != 0).ToList();
        }

        public IList<ComputerResults> SearchComputer(int customerId, string searchFor)
        {
            return this._computerRepository.Search(customerId, searchFor);
        }

        public IList<ComputerUserFieldSettings> GetComputerUserFieldSettings(int customerId)
        {
            return this._computerUserFieldSettingsRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<ComputerUserFieldSettings> GetComputerUserFieldSettingsForDefaultCust()
        {
            var list = this._computerUserFieldSettingsRepository.GetMany(x => x.Customer_Id == null).ToList();

            return list;
        }

        public IList<ComputerUserGroup> GetComputerUserGroups(int customerId)
        {
            return this._computerUserGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<ComputerUsersBlackList> GetComputerUsersBlackLists()
        {
            return this._computerUsersBlackListRepository.GetAll().OrderBy(x => x.User_Id).ToList();
        }

        public ComputerUser GetComputerUser(int id)
        {
            return this._computerUserRepository.GetById(id);
        }

        public ComputerUserGroup GetComputerUserGroup(int id)
        {
            return this._computerUserGroupRepository.GetById(id);
        }

        public ComputerUsersBlackList GetComputerUsersBlackList(int id)
        {
            return this._computerUsersBlackListRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteComputerUser(int id)
        {
            var computerUser = this._computerUserRepository.GetById(id);

            if (computerUser != null)
            {
                try
                {
                    this._computerUserRepository.Delete(computerUser);
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

        public DeleteMessage DeleteComputerUserGroup(int id)
        {
            var computerUserGroup = this._computerUserGroupRepository.GetById(id);

            if (computerUserGroup != null)
            {
                try
                {
                    this._computerUserGroupRepository.Delete(computerUserGroup);
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

        public void DeleteComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            this._computerUsersBlackListRepository.Delete(computerUsersBlackList);
        }

        public void NewComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            computerUsersBlackList.ChangedDate = DateTime.UtcNow;
            this._computerUsersBlackListRepository.Add(computerUsersBlackList);
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
                    var cg = this._computerUserGroupRepository.GetById(id);

                    if (cg != null)
                        computerUser.CUGs.Add(cg);
                }
            }

            if (computerUser.Id == 0)
                this._computerUserRepository.Add(computerUser);
            else
                this._computerUserRepository.Update(computerUser);

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
                    this._computerUserFieldSettingsRepository.Add(setting);
                else
                    this._computerUserFieldSettingsRepository.Update(setting);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveComputerUserFieldSettingForCustomerCopy(ComputerUserFieldSettings computerUserFieldSettings, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            if (computerUserFieldSettings.Id == 0)
                _computerUserFieldSettingsRepository.Add(computerUserFieldSettings);
            else
                _computerUserFieldSettingsRepository.Update(computerUserFieldSettings);


            _computerUserFieldSettingsRepository.Commit();

        }

        public void SaveComputerUserFieldSettingLangForCustomerCopy(ComputerUserFieldSettingsLanguage computerUserFieldSettingsLanguage, out IDictionary<string, string> errors)
        {

            errors = new Dictionary<string, string>();

            var upd = new ComputerUserFieldSettingsLanguage
            {
                ComputerUserFieldSettings_Id = computerUserFieldSettingsLanguage.ComputerUserFieldSettings_Id,
                Language_Id = computerUserFieldSettingsLanguage.Language_Id,
                Label = computerUserFieldSettingsLanguage.Label,
                FieldHelp = computerUserFieldSettingsLanguage.FieldHelp
            };

            _computerUserFieldSettingLanguageRepository.Add(upd);


            _computerUserFieldSettingLanguageRepository.Commit();





            //errors = new Dictionary<string, string>();

            //if (computerUserFieldSettingsLanguage.ComputerUserFieldSettings_Id == 0)
            //    _computerUserFieldSettingLanguageRepository.Add(computerUserFieldSettingsLanguage);
            //else
            //    _computerUserFieldSettingLanguageRepository.Update(computerUserFieldSettingsLanguage);


            //_computerUserFieldSettingsRepository.Commit();

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
                computerUserGroup.OUs = new List<DHDomain.OU>();

            if (ous != null)
            {
                foreach (int id in ous)
                {
                    var ou = this._ouRespository.GetById(id);

                    if (ou != null)
                        computerUserGroup.OUs.Add(ou);
                }
            }

            computerUserGroup.ChangedDate = DateTime.UtcNow;

            if (computerUserGroup.Id == 0)
                this._computerUserGroupRepository.Add(computerUserGroup);
            else
                this._computerUserGroupRepository.Update(computerUserGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void UpdateComputerUsersBlackList(ComputerUsersBlackList computerUsersBlackList)
        {
            computerUsersBlackList.ChangedDate = DateTime.UtcNow;
            this._computerUsersBlackListRepository.Update(computerUsersBlackList);
        }

        public Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly = true)
        {
            return _computerUserRepository.GetInitiatorByUserId(userId, customerId, activeOnly);
        }

		public ComputerUserCategory GetComputerUserCategoryByID(int computerUserCategoryID)
		{
			var category = _computerUserCategoryRepository.GetByID(computerUserCategoryID);
			return category;
		}

        public int SaveComputerUserCategory(ComputerUserCategoryData data)
        {
            ComputerUserCategory entity = null;
            if (data.Id > 0)
            {
                entity = _computerUserCategoryRepository.GetByID(data.Id);
            }
            else
            {
                entity = new ComputerUserCategory
                {
                    CustomerID = data.CustomerId,
                    ComputerUsersCategoryGuid = Guid.NewGuid(),
                    IsEmpty = data.IsEmpty
                };
            }

            entity.Name = data.Name;

            if (data.Id > 0)
                _computerUserCategoryRepository.Update(entity);
            else
                _computerUserCategoryRepository.Add(entity);

            _computerUserCategoryRepository.Commit();

            return entity.ID;
        }

		public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public List<InventorySearchResult> SearchPcNumber(int customerId, string query)
        {
            return _inventoryRepository.SearchPcNumber(customerId, query);
        }

        public ComputerUserCategoryOverview GetEmptyComputerUserCategory(int customerId)
        {
            var emptyCategory = _computerUserCategoryRepository.GetEmptyCategoryOverview(customerId);
            if (emptyCategory == null)
                emptyCategory = CreateEmptyCategory();

            return emptyCategory;
        }

        public IList<ComputerUserCategoryOverview> GetComputerUserCategoriesByCustomerID(int customerId, bool includeEmpty = false)
		{
			var categories = _computerUserCategoryRepository.GetAllByCustomerID(customerId);
		    if (!includeEmpty)
		    {
		        categories = categories.Where(o => !o.IsEmpty).ToList();
		    }
		    else
		    {
                //check if empty category exists and if not create it
		        var emptyCategory = categories.FirstOrDefault(c => c.IsEmpty);
		        if (emptyCategory == null)
		        {
		            emptyCategory = CreateEmptyCategory();
		            categories.Insert(0, emptyCategory);
		        }
		        
		        //empty shall always have fixed Id
		        emptyCategory.Id = ComputerUserCategory.EmptyCategoryId;
		        
            }

            return categories;
		}

        private ComputerUserCategoryOverview CreateEmptyCategory()
        {
            var emptyCategory = new ComputerUserCategoryOverview()
            {
                Id = ComputerUserCategory.EmptyCategoryId,
                Name = ComputerUserCategory.EmptyCategoryDefaultName,
                IsEmpty = true
            };
            return emptyCategory;
        }

        public ComputerUser GetComputerUserByUserID(string reportedBy)
		{
			var computerUser = _computerUserRepository.Get(o => o.UserId == reportedBy);
			return computerUser;
		}
	}
}

