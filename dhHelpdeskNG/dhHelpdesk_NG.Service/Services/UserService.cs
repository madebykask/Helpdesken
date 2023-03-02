using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.Models.Users;
    using DH.Helpdesk.BusinessData.Models.Users.Input;
    using DH.Helpdesk.BusinessData.Models.Users.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Dal.Repositories.Users;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;
    using LinqLib.Operators;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using UserGroup = DH.Helpdesk.Domain.UserGroup;

    public interface IUserService
    {
        IList<CustomerUser> GetCustomerUserForUser(int userId);

        IList<UserLists> GetUserOnCases(int customer, bool isTakeOnlyActive = false);
        IList<CustomerWorkingGroupForUser> GetListToUserWorkingGroup(int userId);
        List<CustomerWorkingGroupForUser> GetWorkinggroupsForUserAndCustomer(int userId, int customerId);
        IList<LoggedOnUsersOnIndexPage> GetListToUserLoggedOn();
        IList<Department> GetDepartmentsForUser(int userId, int customerId = 0);
        IList<Customer> GetCustomersConnectedToUser(int userId);
        IList<User> GetAdministrators(int customerId, int active = 1);
        IList<User> GetSystemOwners(int customerId);
        IList<User> GetUsers();
        IList<User> GetUsers(int customerId);
        IList<User> GetUsersByUserGroup(int customerId);
        IList<User> GetAdminstratorsForSMS(int customerId, int active = 1);

        IList<CustomerUserInfo> GetCustomerUserInfos(int customerId);

        /// <summary>
        /// Fetches active users with performer flag.
        /// If userId is supplied appends to list user with this id without checking first condition
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<CustomerUserInfo> GetAvailablePerformersOrUserId(int customerId, int? userId = null, bool includeWorkingGroups = false);

        IList<User> GetPerformersOrUserId(int customerId, int? userId = null);
        IList<User> GetAllPerformers(int customerId);
        IList<CustomerUserInfo> GetAvailablePerformersForWorkingGroup(int customerId, int? workingGroup = null);

        IList<User> SearchSortAndGenerateUsers(UserSearch searchUsers, IList<int> customersIds = null, bool excludeSystemAdministrator = false);
        IList<UserGroup> GetUserGroups();
        IList<UserRole> GetUserRoles();
        IList<UserWorkingGroup> GetUserWorkingGroups();
        IList<UserWorkingGroup> GetUserWorkingGroupsByWorkgroup(int workingGroupId);
        IList<int> GetUserCustomersIds(int userId);
        bool UserHasCustomerId(int customerId);

        //IList<User> GetUsersForWorkingGroup(int customerId, int workingGroupId, bool includeWorkingGroups = false);
        IList<CustomerUserInfo> GetUsersForWorkingGroup(int workingGroupId);

        bool UserHasActiveCase(int customerId, int userId, List<int> workingGroups);

        User GetUserForCopy(int id);
        CustomerUserInfo GetUserInfo(int id);
        User GetUser(int id); //not perf effecient - user info method
        Task<User> GetUserAsync(int id);
        string GetUserTimeZoneId(int userId);
        UserRole GetUserRoleById(int id);
        UserWorkingGroup GetUserWorkingGroupById(int userId, int workingGroupId);
        UserOverview GetUserByLogin(string userId, int? customerId);

        DeleteMessage DeleteUser(int id);

        void SavePassword(int id, string password);

        void SaveEditUser(
            User user,
            int[] aas,
            int[] customersSelected,
            int[] customersAvailable,
            int[] ots,
            int[] ccs,
            int[] dus,
            IList<UserWorkingGroup> userWorkingGroups,
            IList<CustomerUserForEdit> customerUsers,
            out IDictionary<string, string> errors);
        
        void SaveNewUser(User user, int[] aas, int[] cs, int[] ots, List<UserWorkingGroup> UserWorkingGroups, int[] departments, out IDictionary<string, string> errors, string confirmpassword = "");
        void SaveProfileUser(User user, out IDictionary<string, string> errors);
        void Commit();

        UserOverview Login(string name, string password);
        Task<UserOverview> LoginAsync(string name, string password);
        DateTime GetUserPasswordChangedDate(int id);

        /// <summary>
        /// The get modules.
        /// </summary>
        /// <returns>
        /// The modules.
        /// </returns>
        IEnumerable<ModuleOverview> GetModules();

        /// <summary>
        /// The get user modules.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The modules.
        /// </returns>
        IEnumerable<UserModuleOverview> GetUserModules(int user);

        /// <summary>
        /// The update user modules.
        /// </summary>
        /// <param name="modules">
        /// The modules.
        /// </param>
        void UpdateUserModules(IEnumerable<UserModule> modules);

        /// <summary>
        /// The get user module.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// The <see cref="UserModule"/>.
        /// </returns>
        UserModule GetUserModule(int userId, int moduleId);

        /// <summary>
        /// The get user overview.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="UserOverview"/>.
        /// </returns>
        UserOverview GetUserOverview(int userId);
        Task<UserOverview> GetUserOverviewAsync(int userId);

        List<ItemOverview> FindActiveOverviews(int customerId);

        ItemOverview FindActiveOverview(int userId);

        List<User> GetActiveUsers();
        List<User> GetAllUsers();
        List<User> GetCustomerUsers(int customerId, bool activeOnly = true);

        List<CustomerSettings> GetUserCustomersSettings(int userId);

        List<ItemOverview> GetWorkingGroupUsers(int customerId, int? workingGroupId);

        List<UserProfileCustomerSettings> GetUserProfileCustomersSettings(int userId);

        void UpdateUserProfileCustomerSettings(int userId, List<UserProfileCustomerSettings> customersSettings);

        List<Customer> GetCustomersForUser(int userId);

        int? GetUserDefaultWorkingGroupId(int userId, int customerId);

        WorkingGroupEntity GetUserDefaultWorkingGroup(int userId, int customerId);

        bool IsUserValidAdmin(string userId, string pass);

        bool VerifyUserCasePermissions(UserOverview user, int caseId);
        Expression<Func<Case, bool>> GetCasePermissionFilter(UserOverview user, int customerId);
        string GetUserEmail(int id);
        User GetUserByEmail(string emailAddress);
    }

    public class UserService : IUserService
    {
        private readonly IAccountActivityRepository _accountActivityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IOrderTypeRepository _orderTypeRepository;
        private readonly IContractCategoryRepository _contractCategoryRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;
        private readonly ICaseSettingRepository _casesettingRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IUserModuleRepository _userModuleRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ITranslator _translator;
        private readonly IUsersPasswordHistoryRepository _userPasswordHistoryRepository;
        private readonly ISettingRepository _settingRepository;

        private readonly IEntityToBusinessModelMapper<Setting, CustomerSettings> _customerSettingsToBusinessModelMapper;


#pragma warning disable 0618
        public UserService(
            IAccountActivityRepository accountActivityRepository,
            ICustomerRepository customerRepository,
            ICustomerUserRepository customerUserRepository,
            IDepartmentRepository departmentRepository,
            IOrderTypeRepository orderTypeRepository,
            IContractCategoryRepository contractCategoryRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserGroupRepository userGroupRepository,
            IUserRoleRepository userRoleRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            ICaseSettingRepository casesettingRepository,
            IModuleRepository moduleRepository,
            IUserModuleRepository userModuleRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IUserPermissionsChecker userPermissionsChecker, 
            ITranslator translator,
            IUsersPasswordHistoryRepository userPasswordHistoryRepository,
            IEntityToBusinessModelMapper<Setting, CustomerSettings> customerSettingsToBusinessModelMapper,
            ISettingRepository settingRepository)
        {
            _accountActivityRepository = accountActivityRepository;
            _customerRepository = customerRepository;
            _customerUserRepository = customerUserRepository;
            _departmentRepository = departmentRepository;
            _orderTypeRepository = orderTypeRepository;
            _contractCategoryRepository = contractCategoryRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userRoleRepository = userRoleRepository;
            _workingGroupRepository = workingGroupRepository;
            _userWorkingGroupRepository = userWorkingGroupRepository;
            _casesettingRepository = casesettingRepository;
            _moduleRepository = moduleRepository;
            _userModuleRepository = userModuleRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _userPermissionsChecker = userPermissionsChecker;
            _translator = translator;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
            _customerSettingsToBusinessModelMapper = customerSettingsToBusinessModelMapper;
            _settingRepository = settingRepository;
        }
#pragma warning restore 0618

        public User GetUserByEmail(string emailAddress)
        {
            return this._userRepository.GetUserByEmail(emailAddress);
        }
        public bool UserHasActiveCase(int customerId, int userId, List<int> workingGroups)
        {
            return _userRepository.UserHasActiveCase(customerId, userId, workingGroups);
        }

        public IList<CustomerUser> GetCustomerUserForUser(int userId)
        {
            return _customerUserRepository.GetMany(x => x.User_Id == userId).ToList();
        }

        public IList<CustomerWorkingGroupForUser> GetListToUserWorkingGroup(int userId)
        {
            return _userRepository.ListForWorkingGroupsInUser(userId).ToList();
        }

        public UserOverview GetUserByLogin(string userId, int? customerId)
        {
            return _userRepository.GetUserByLogin(userId, customerId);
        }

        public List<CustomerWorkingGroupForUser> GetWorkinggroupsForUserAndCustomer(int userId, int customerId)
        {
            return _userRepository.GetWorkinggroupsForUserAndCustomer(userId, customerId).ToList();
        }

        public IList<LoggedOnUsersOnIndexPage> GetListToUserLoggedOn()
        {
            return _userRepository.LoggedOnUsers();
        }

        public IList<Department> GetDepartmentsForUser(int userId, int customerId)
        {
            return _departmentRepository.GetDepartmentsForUser(userId, customerId).OrderBy(x => x.Customer_Id).ThenBy(x => x.DepartmentName).ToList();
            //return _departmentRepository.GetDepartmentsForUser(userId, customerId).ToList();
        }

        /// <summary>
        /// Returns Ilist of Customer with all the customers that the user is assigned to
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Customer> GetCustomersConnectedToUser(int userId)
        {
            return _customerRepository.CustomersForUser(userId).ToList();
        }

        public IList<User> GetAdministrators(int customerId, int active = 1)
        {
            return _userRepository.GetMany(x => x.UserGroup.Id != 1 && x.IsActive == active).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToList();
        }

        public IList<User> GetAdminstratorsForSMS(int customerId, int active = 1)
        {
            return _userRepository.GetMany(x => x.UserGroup.Id != 1 && x.IsActive == active && x.CellPhone.Length > 5).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToList();
        }

        public IList<User> GetSystemOwners(int customerId)
        {
            return _userRepository.GetMany(x => x.IsActive == 1).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ToList(); //TODO: den här raden skall kanske fungera senare, men gör det inte just nu
        }

        #region GetUsersForWorkingGroupOverview

        public IList<CustomerUserInfo> GetUsersForWorkingGroup(int customerId, int workingGroupId)
        {
            var query = _userRepository.GetUsersForWorkingGroupQuery(workingGroupId, customerId, true);
            var users = MapToCustomerUserInfo(query).ToList();
            return users;
        }

        public IList<CustomerUserInfo> GetUsersForWorkingGroup(int workingGroupId)
        {
            var query = _userRepository.GetUsersForWorkingGroupQuery(workingGroupId);
            var users = MapToCustomerUserInfo(query).ToList();
            return users;
        }

        //its important to keep IQuerable
        private IQueryable<CustomerUserInfo> MapToCustomerUserInfo(IQueryable<User> query, bool includeWorkingGroups = false)
        {
            if (includeWorkingGroups)
            {
                return query.Select(x => new CustomerUserInfo()
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    Performer = x.Performer,
                    UserGroupId = x.UserGroup_Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    SurName = x.SurName,
                    WorkingGroups =
                        x.UserWorkingGroups.Select(w => new UserWorkingGroupOverview()
                        {
                            WorkingGroupId = w.WorkingGroup_Id,
                            IsDefault = w.IsDefault,
                            IsMemberOfGroup = w.IsMemberOfGroup,
                            UserRole = w.UserRole
                        }).ToList()
                });
            }

            //by default without groups
            return query.Select(x => new CustomerUserInfo()
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Performer = x.Performer,
                UserGroupId = x.UserGroup_Id,
                Email = x.Email,
                FirstName = x.FirstName,
                SurName = x.SurName
            });
        }

        #endregion

        public IList<UserLists> GetUserOnCases(int customerId, bool isTakeOnlyActive = false)
        {
            return _userRepository.GetUserOnCases(customerId, isTakeOnlyActive);
        }

        // NOTE: do not use if you have conditions!!!
        public IList<User> GetUsers()
        {
            return _userRepository.GetAll().OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToList();
        }

        public IList<User> GetUsers(int customerId)
        {
            return _userRepository.GetUsers(customerId)
                .Where(x => x.IsActive == 1)
                .OrderBy(x => x.SurName)
                .ThenBy(x => x.FirstName).ToList();
        }

        // perf optimised for dropdowns - returns only basic info
        public IList<CustomerUserInfo> GetCustomerUserInfos(int customerId)
        {
            var query = 
                _userRepository.GetUsers(customerId)
                            .Where(x => x.IsActive == 1)
                            .OrderBy(x => x.SurName)
                            .ThenBy(x => x.FirstName)
                            .Select(x => new CustomerUserInfo
                            {
                               Id = x.Id,
                               FirstName =  x.FirstName,
                               SurName = x.SurName,
                               Email =  x.Email,
                               IsActive = x.IsActive
                            });

            return query.ToList();
        }

        public IList<User> GetUsersByUserGroup(int customerId)
        {
            return _userRepository.GetUsersByUserGroup(customerId).OrderBy(x => x.SurName).ThenBy(x => x.FirstName).ToList();
        }
        
        public IList<CustomerUserInfo> GetAvailablePerformersOrUserId(int customerId, int? userId = null, bool includeWorkingGroups = false)
        {
            var query =
                _userRepository.GetUsers(customerId)
                    .Where(e => e.IsActive == 1 && (e.UserGroup_Id > UserGroups.User && e.Performer == 1 || (userId.HasValue && e.Id == userId)))
                    .OrderBy(e => e.SurName)
                    .ThenBy(e => e.FirstName);

            var items = MapToCustomerUserInfo(query, includeWorkingGroups).ToList();
            return items;
        }

        public IList<User> GetPerformersOrUserId(int customerId, int? userId = null)
        {
            return
                _userRepository.GetUsers(customerId)
                    .Where(e => e.Performer == 1 && e.UserGroup_Id > UserGroups.User || (userId.HasValue && e.Id == userId))
                    .OrderBy(e => e.SurName)
                    .ToList();
        }

        public IList<User> GetAllPerformers(int customerId)
        {
            return
                _userRepository.GetUsers(customerId)
                    .Where(e => e.Performer == 1 && e.UserGroup_Id > UserGroups.User)
                    .ToList();
        }

        public IList<CustomerUserInfo> GetAvailablePerformersForWorkingGroup(int customerId, int? workingGroup = null)
        {
            if (workingGroup.HasValue)
            {
                var query = 
                    _userRepository.GetUsersForWorkingGroupQuery(workingGroup.Value, customerId, true) 
                        .Where(it => it.IsActive == 1 && it.Performer == 1 && it.UserGroup_Id > UserGroups.User); 

                return MapToCustomerUserInfo(query).ToList(); //todo: check working group flag
            }
            return GetAvailablePerformersOrUserId(customerId);
        }

        public IList<User> SearchSortAndGenerateUsers(UserSearch searchUsers, IList<int> customersIds = null, bool excludeSystemAdministrator = false)
        {
            customersIds = customersIds ?? new List<int> {searchUsers.CustomerId};
            return _userRepository.GetUsersForUserSettingList(searchUsers, customersIds, excludeSystemAdministrator).OrderBy(x => x.FirstName).ToList();
        }

        public IList<UserGroup> GetUserGroups()
        {
            return _userGroupRepository.GetAll().OrderBy(x => x.Id).ToList();
        }

        public IList<UserRole> GetUserRoles()
        {
            return _userRoleRepository.GetAll().ToList();
        }

        public IList<UserWorkingGroup> GetUserWorkingGroups()
        {
            return _userWorkingGroupRepository.GetAll().ToList();
        }

        public IList<UserWorkingGroup> GetUserWorkingGroupsByWorkgroup(int workingGroupId)
        {
            return _userWorkingGroupRepository.GetMany(uw => uw.WorkingGroup_Id == workingGroupId)
                .AsQueryable()
                .AsNoTracking()
                .Include(ur => ur.User.Departments)
                .ToList();
        }
        
        public CustomerUserInfo GetUserInfo(int id)
        {
            return _userRepository.GetUserInfo(id);
        }

        public User GetUser(int id)
        {
            return _userRepository.GetById(id);
        }

        public Task<User> GetUserAsync(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public User GetUserForCopy(int id)
        {
            return _userRepository.GetUserForCopy(id);
        }

        public string GetUserTimeZoneId(int userId)
        {
            return _userRepository.GetUserTimeZoneId(userId);
        }
        public UserRole GetUserRoleById(int id)
        {
            return _userRoleRepository.GetById(id);
        }

        public UserWorkingGroup GetUserWorkingGroupById(int userId, int workingGroupId)
        {
            return _userWorkingGroupRepository.Get(x => x.User_Id == userId && x.WorkingGroup_Id == workingGroupId);
        }

        //public User GetSystemUserOwnerId(int userId)
        //{
        //    return _userRepository.GetUser(userId);
        //}
        public DeleteMessage DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);

            if (user != null)
            {
                try
                {
                   
                    user.AAs.Clear();
                    user.UserWorkingGroups.Clear();
                    user.CustomerUsers.Clear();
                    user.Departments.Clear();
                    user.OLs.Clear();                    
                    user.OTs.Clear();
                    user.UserRoles.Clear();

                    //Remove Case Settings
                    var userCaseSettings = _casesettingRepository.GetMany(cs => cs.User_Id == id).ToList();
                    if (userCaseSettings.Any())
                    {
                        foreach (var caseSetting in userCaseSettings)
                            _casesettingRepository.Delete(caseSetting);
                    }

                    // Remove User Modules 
                    var userModules = _userModuleRepository.GetMany(um => um.User_Id == id).ToList();
                    if (userModules.Any())
                    {
                        foreach (var userModule in userModules)
                            _userModuleRepository.Delete(userModule);
                    }

                    // Remove User Password History 
                    var passwordHistories = _userPasswordHistoryRepository.GetMany(up=> up.User_Id == id).ToList();
                    if (passwordHistories.Any())
                    {
                        foreach (var passHistory in passwordHistories)
                        {
                            _userPasswordHistoryRepository.Delete(passHistory);
                            this.Commit();
                        }
                    }
                    
                    _userRepository.Delete(user);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch(Exception e)
                {
                    var inner = e.InnerException.Message;
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SavePassword(int id, string password)
        {
            var user = _userRepository.GetById(id);
            user.Password = password;
            user.PasswordChangedDate = DateTime.Now;
            _userRepository.Update(user);
            this.Commit();
        }

        public void SaveProfileUser(User user, out IDictionary<string, string> errors)
        {
            //var user = _userRepository.GetById(id);
            var curTime = DateTime.Now;

            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.ChangeTime = curTime;
            user.BulletinBoardDate = user.BulletinBoardDate ?? curTime;
            //user.CaseStateSecondaryColor = user.CaseStateSecondaryColor ?? string.Empty;            
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;            
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;                        

            errors = new Dictionary<string, string>();

            _userRepository.Update(user);
            Commit();
        }

        public void SaveEditUser(
            User user,
            int[] aas, 
            int[] customersSelected, 
            int[] customersAvailable, 
            int[] ots, 
            int[] ccs,
            int[] dus, 
            IList<UserWorkingGroup> userWorkingGroups, 
            IList<CustomerUserForEdit> customerUsers,
            out IDictionary<string, string> errors)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            errors = new Dictionary<string, string>();

            user.Email = user.Email.TrimStart().TrimEnd();

            var curTime = DateTime.Now;

            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.BulletinBoardDate = user.BulletinBoardDate ?? curTime;
            user.ChangeTime = curTime;
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;
            user.ShowQuickMenuOnStartPage = user.ShowQuickMenuOnStartPage;
            user.Password = user.Password ?? string.Empty;

            //if (string.IsNullOrEmpty(user.SurName + user.FirstName + user.UserID))
            //    errors.Add("User.SurName" + "User.FirstName" + "User.UserID", "Du måste ange ett för- och efternamn, samt ett Id");

            # region handling <ILists>

            if (user.AAs != null)
                foreach (var delete in user.AAs.ToList())
                    user.AAs.Remove(delete);
            else
                user.AAs = new List<AccountActivity>();

            if (aas != null)
            {
                foreach (int id in aas)
                {
                    var aa = _accountActivityRepository.GetById(id);

                    if (aa != null)
                        user.AAs.Add(aa);
                }
            }

            if (user.CusomersAvailable != null)
            {
                foreach (var delete in user.CusomersAvailable.ToArray())
                {
                    user.CusomersAvailable.Remove(delete);
                }
            }
            else
            {
                user.CusomersAvailable = new List<Customer>();
            }

            if (user.Cs != null)
                foreach (var delete in user.Cs.ToList())
                    user.Cs.Remove(delete);
            else
                user.Cs = new List<Customer>();
            
            if (user.Id != 0)
            {
                var allCustomersMap = _customerRepository.GetAll().ToDictionary(it => it.Id, it => it);
                if (customersAvailable != null)
                {
                    customersAvailable.Where(allCustomersMap.ContainsKey)
                        .Select(it => allCustomersMap[it])
                        .ForEach(it => user.CusomersAvailable.Add(it));
                }

                if (customersSelected != null)
                {
                    customersSelected.Where(allCustomersMap.ContainsKey)
                        .Select(it => allCustomersMap[it])
                        .ForEach(it => user.Cs.Add(it));
                }
            }

            errors = ValidateUserFields(user, true);

            if (user.Id == 0)
            {
                if (user.CustomerUsers != null)
                    foreach (var delete in user.CustomerUsers.ToList())
                        user.CustomerUsers.Remove(delete);
                else
                    user.CustomerUsers = new List<CustomerUser>();
            }

            if (user.OTs != null)
                foreach (var delete in user.OTs.ToList())
                    user.OTs.Remove(delete);
            else
                user.OTs = new List<OrderType>();

            if (ots != null)
            {
                foreach (int id in ots)
                {
                    var ot = _orderTypeRepository.GetById(id);

                    if (ot != null)
                        user.OTs.Add(ot);
                }
            }
            if (user.CCs != null)
                foreach (var delete in user.CCs.ToList())
                    user.CCs.Remove(delete);
            else
                user.CCs = new List<ContractCategory>();

            if (ccs != null)
            {
                foreach (int id in ccs)
                {
                    var cc = _contractCategoryRepository.GetById(id);

                    if (cc != null)
                        user.CCs.Add(cc);
                }
            }
            if (user.Departments != null)
                foreach (var delete in user.Departments.ToList())
                    user.Departments.Remove(delete);
            else
                user.Departments = new List<Department>();

            if (dus != null)
            {
                foreach (int id in dus)
                {
                    var dep = _departmentRepository.GetById(id);

                    if (dep != null && customersSelected.Contains(dep.Customer_Id))
                        user.Departments.Add(dep);
                }
            }
            
            if (user.UserWorkingGroups != null)
                foreach (var delete in user.UserWorkingGroups.ToList())
                    user.UserWorkingGroups.Remove(delete);
            else
                user.UserWorkingGroups = new List<UserWorkingGroup>();

            if (user != null)
            {
                if (userWorkingGroups != null)
                {
                    foreach (var uwg in userWorkingGroups)
                    {
                        // http://redmine.fastdev.se/issues/10997
                        //Filter 0 because problem in Case
                        var wg = _workingGroupRepository.GetById(uwg.WorkingGroup_Id);

                        if (uwg.UserRole != WorkingGroupUserPermission.NO_ACCESS && wg != null && customersSelected.Contains(wg.Customer_Id))
                            user.UserWorkingGroups.Add(uwg);

                        //user.userWorkingGroups.Add(uwg);
                    }
                }
                
            }

            #endregion

            UpdateCustomerUserSettings(user, customerUsers);

            if (user.Id == 0)
                _userRepository.Add(user);
            else
                _userRepository.Update(user);

            if (errors.Count == 0)
                Commit();
        }

        private void UpdateCustomerUserSettings(User user, IList<CustomerUserForEdit> customerUsers)
        {
            foreach (var customerUser in user.CustomerUsers)
            {
                if (customerUser.CasePerformerFilter == null)
                {
                    customerUser.CasePerformerFilter = string.Empty;
                }

                var cusData = customerUsers.FirstOrDefault(x => x.CustomerId == customerUser.Customer_Id && x.UserId == customerUser.User_Id);
                if (cusData != null)
                {
                    customerUser.UserInfoPermission = cusData.UserInfoPermission.ToInt();
                    customerUser.CaptionPermission = cusData.CaptionPermission.ToInt();
                    customerUser.ContactBeforeActionPermission = cusData.ContactBeforeActionPermission.ToInt();
                    customerUser.PriorityPermission = cusData.PriorityPermission.ToInt();
                    customerUser.StateSecondaryPermission = cusData.StateSecondaryPermission.ToInt();
                    customerUser.WatchDatePermission = cusData.WatchDatePermission.ToInt();
                    customerUser.RestrictedCasePermission = cusData.RestrictedCasePermission;
                }
            }
        }

        public void SaveNewUser(User user, int[] aas, int[] cs, int[] ots, List<UserWorkingGroup> userWorkingGroups, int[] departments, out IDictionary<string, string> errors, string confirmpassword = "")
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            errors = new Dictionary<string, string>();

            var curTime = DateTime.Now;

            user.Email = user.Email?.TrimStart().TrimEnd();
            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.BulletinBoardDate = user.BulletinBoardDate ?? curTime;
            //user.CaseStateSecondaryColor = user.CaseStateSecondaryColor ?? string.Empty;
            user.ChangeTime = curTime;
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;
            user.PasswordChangedDate = curTime;
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;
            user.RegTime = curTime;
            user.TimeZoneId = user.TimeZoneId;

            if (user.AAs != null)
                foreach (var delete in user.AAs.ToList())
                    user.AAs.Remove(delete);
            else
                user.AAs = new List<AccountActivity>();

            if (aas != null)
            {
                foreach (var id in aas)
                {
                    var aa = _accountActivityRepository.GetById(id);

                    if (aa != null)
                        user.AAs.Add(aa);
                }
            }

            if (user.Cs != null)
                foreach (var delete in user.Cs.ToList())
                    user.Cs.Remove(delete);
            else
                user.Cs = new List<Customer>();

            if (cs != null)
            {
                var customerIdsHash = cs.ToDictionary(it => it, it => true);
                _customerRepository.GetAll()
                    .Where(it => customerIdsHash.ContainsKey(it.Id))
                    .ForEach(it => user.Cs.Add(it));
            }

            errors = ValidateUserFields(user, false, confirmpassword);

            if (user.UserWorkingGroups != null)
                foreach (var delete in user.UserWorkingGroups.ToList())
                    user.UserWorkingGroups.Remove(delete);
            else
                user.UserWorkingGroups = new List<UserWorkingGroup>();

            if (userWorkingGroups != null)
            {
                foreach (var uwg in userWorkingGroups)
                {
                    if (uwg.UserRole != WorkingGroupUserPermission.NO_ACCESS)
                        user.UserWorkingGroups.Add(uwg);
                }
            }

            if (user.OTs != null)
                foreach (var delete in user.OTs.ToList())
                    user.OTs.Remove(delete);
            else
                user.OTs = new List<OrderType>();

            if (ots != null)
            {
                foreach (int id in ots)
                {
                    var ot = _orderTypeRepository.GetById(id);

                    if (ot != null)
                        user.OTs.Add(ot);
                }
            }

            if (user.Departments != null)
                foreach (var delete in user.Departments.ToList())
                    user.Departments.Remove(delete);
            else
                user.Departments = new List<Department>();

            if (departments != null)
            {
                foreach (int id in departments)
                {
                    var dep = _departmentRepository.GetById(id);

                    if (dep != null)
                        user.Departments.Add(dep);
                }
            }

            
            if (user.Id == 0)
                _userRepository.Add(user);
            else
                _userRepository.Update(user);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public UserOverview Login(string name, string password)
        {
            var user = _userRepository.GetUserLoginInfo(name);

            if (user != null && user.Password.Equals(password, StringComparison.CurrentCulture)) // case sensetive
                return _userRepository.GetUser(user.Id);

            return null;
        }

        public async Task<UserOverview> LoginAsync(string name, string password)
        {
            return await _userRepository.GetByUserIdAsync(name, password);
        }

        public DateTime GetUserPasswordChangedDate(int id)
        {
            return _userRepository.GetPasswordChangedDate(id);
        }

        /// <summary>
        /// The get modules.
        /// </summary>
        /// <returns>
        /// The modules.
        /// </returns>
        public IEnumerable<ModuleOverview> GetModules()
        {
            return _moduleRepository.GetModules();
        }

        /// <summary>
        /// The get user modules.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<UserModuleOverview> GetUserModules(int user)
        {
            var userModules = _userModuleRepository.GetUserModules(user).ToList();

            if (userModules.Any())
            {
                return userModules;
            }

            var init = _moduleRepository
                .GetModules()
                .Select(m => new UserModuleOverview()
                {
                    User_Id = user,
                    Module_Id = m.Id,
                    isVisible = this.GetDefaultVisibility((Module)m.Id),
                    NumberOfRows = this.GetDefaultNumberOfRows((Module)m.Id),
                    Position = this.GetInitializePosition((Module)m.Id),
                    Module = new ModuleOverview()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description
                    }
                }).ToList();

            _userModuleRepository.UpdateUserModules(init.Select(m => new UserModule()
                                                                              {
                                                                                  Id = m.Id,
                                                                                  User_Id = m.User_Id,
                                                                                  Module_Id = m.Module_Id,
                                                                                  isVisible = m.isVisible,
                                                                                  NumberOfRows = m.NumberOfRows,
                                                                                  Position = m.Position                                                                                  
                                                                              }));
            this.Commit();

            return init;
        }

        public bool IsUserValidAdmin(string userId, string pass)
        {
            var user = _userRepository.GetUserLoginInfo(userId);
            if (user != null && user.Password == pass && user.UserGroupId == UserGroups.SystemAdministrator)
                return true;
            
            return false;
        }

        public bool VerifyUserCasePermissions(UserOverview user, int caseId)
        {
            var caseCustomerId = _customerRepository.GetCustomerByCaseId(caseId);
            var customerSettings = _customerUserRepository.GetCustomerSettings(caseCustomerId, user.Id);

            Expression<Func<Case, bool>> casePermissionFilter = null;

            if (customerSettings != null && customerSettings.RestrictedCasePermission)
            {
                switch (user.UserGroupId)
                {
                    case (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator:
                        casePermissionFilter = CaseSpecifications.GetByAdministratorOrResponsibleUserExpression(user.Id, user.Id);
                        break;
                    case (int)BusinessData.Enums.Admin.Users.UserGroup.User:
                        casePermissionFilter = CaseSpecifications.GetByReportedByOrUserId(user.UserId, user.Id);
                        break;
                }
            }

            var isAuthorised = _customerUserRepository.CheckUserCasePermissions(user.Id, caseId, casePermissionFilter);
            return isAuthorised;
        }

        public Expression<Func<Case, bool>> GetCasePermissionFilter(UserOverview user, int customerId)
        {
            var customerSettings = _customerUserRepository.GetCustomerSettings(customerId, user.Id);

            Expression<Func<Case, bool>> casePermissionFilter = null;

            if (customerSettings != null && customerSettings.RestrictedCasePermission)
            {
                switch (user.UserGroupId)
                {
                    case (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator:
                        return casePermissionFilter = CaseSpecifications.GetByAdministratorOrResponsibleUserExpression(user.Id, user.Id);
                    case (int)BusinessData.Enums.Admin.Users.UserGroup.User:
                        return casePermissionFilter = CaseSpecifications.GetByReportedByOrUserId(user.UserId, user.Id);

                }
            }

            return casePermissionFilter;
        }

        /// <summary>
        /// The update user modules.
        /// </summary>
        /// <param name="modules">
        /// The modules.
        /// </param>
        public void UpdateUserModules(IEnumerable<UserModule> modules)
        {
            _userModuleRepository.UpdateUserModules(modules);
            this.Commit();
        }

        /// <summary>
        /// The get user module.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// The <see cref="UserModule"/>.
        /// </returns>
        public UserModule GetUserModule(int userId, int moduleId)
        {
            return _userModuleRepository.GetUserModule(userId, moduleId);
        }

        /// <summary>
        /// The get user overview.
        /// </summary>
        /// <param name="userId">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="UserOverview"/>.
        /// </returns>
        public UserOverview GetUserOverview(int userId)
        {
            return _userRepository.GetUser(userId);
        }

        public async Task<UserOverview> GetUserOverviewAsync(int userId)
        {
            return await _userRepository.GetUserAsync(userId);
        }

        public List<ItemOverview> FindActiveOverviews(int customerId)
        {
            return _userRepository.FindActiveOverviews(customerId);
        }

        public ItemOverview FindActiveOverview(int userId)
        {
            return _userRepository.FindActiveOverview(userId);
        }

        public List<User> GetActiveUsers()
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userRep = uow.GetRepository<User>();

                var users = userRep.GetAll()
                        .GetActive()
                        .GetOrderedByName()
                        .ToList();

                return users;
            }
        }

        public List<User> GetAllUsers()
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userRep = uow.GetRepository<User>();

                var users = userRep.GetAll()
                        .GetOrderedByName()
                        .ToList();

                return users;
            }
        }

        public List<User> GetCustomerUsers(int customerId, bool activeOnly = true)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerRep = uow.GetRepository<Customer>();
                var customerUserRep = uow.GetRepository<CustomerUser>();
                var userRep = uow.GetRepository<User>();

                var customers = customerRep.GetAll().GetById(customerId);
                var customerUsers = customerUserRep.GetAll();
                var users = userRep.GetAll();
                if (activeOnly)
                    users = users.GetActive();

                return UsersMapper.MapToCustomerUsers(customers, users, customerUsers);
            }
        }

        public List<CustomerSettings> GetUserCustomersSettings(int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerRep = uow.GetRepository<Customer>();
                var customerUserRep = uow.GetRepository<CustomerUser>();
                var userRep = uow.GetRepository<User>();
                var customerSettingsRep = uow.GetRepository<Setting>();

                var customers = customerRep.GetAll();
                var customerUsers = customerUserRep.GetAll();
                var users = userRep.GetAll().GetById(userId);
                var customerSettings = customerSettingsRep.GetAll();

                return UsersMapper.MapToUserCustomersSettings(
                                customers, 
                                users, 
                                customerUsers, 
                                customerSettings,
                                _customerSettingsToBusinessModelMapper);
            }
        }

        public IList<int> GetUserCustomersIds(int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerUserRep = uow.GetRepository<CustomerUser>();
                return customerUserRep.GetAll().Where(cu => cu.User_Id == userId).Select(cu => cu.Customer_Id).ToList();
            }
        }

        public bool UserHasCustomerId(int customerId)
        {
            return GetUserCustomersIds(customerId).Any(x => x == customerId);
        }

        public List<ItemOverview> GetWorkingGroupUsers(int customerId, int? workingGroupId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userRep = uow.GetRepository<User>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var userWorkingGroupRep = uow.GetRepository<UserWorkingGroup>();
                var customerRep = uow.GetRepository<Customer>();
                var customerUserRep = uow.GetRepository<CustomerUser>();

                var users = userRep.GetAll().GetActive();
                var workingGroups = workingGroupRep.GetAll().GetById(workingGroupId);
                var userWorkingGroups = userWorkingGroupRep.GetAll();
                var customers = customerRep.GetAll().GetById(customerId);
                var customerUsers = customerUserRep.GetAll();

                return UsersMapper.MapToWorkingGroupUsers(
                                users,
                                workingGroups,
                                userWorkingGroups,
                                customers,
                                customerUsers,
                                workingGroupId);
            }
        }

        public List<UserProfileCustomerSettings> GetUserProfileCustomersSettings(int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerRep = uow.GetRepository<Customer>();
                var customerUserRep = uow.GetRepository<CustomerUser>();
                var userRep = uow.GetRepository<User>();
                var customerSettingsRep = uow.GetRepository<Setting>();

                var customers = customerRep.GetAll();
                var customerUsers = customerUserRep.GetAll();
                var users = userRep.GetAll().GetById(userId);
                var customerSettings = customerSettingsRep.GetAll();

                return UsersMapper.MapToUserProfileCustomersSettings(
                                customers,
                                users,
                                customerUsers,
                                customerSettings);
            }
        }

        public void UpdateUserProfileCustomerSettings(int userId, List<UserProfileCustomerSettings> customersSettings)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customerUserRep = uow.GetRepository<CustomerUser>();

                var customerUsers = customerUserRep.GetAll()
                                .Where(cu => cu.User_Id == userId)
                                .ToList();

                foreach (var customerUser in customerUsers)
                {
                    var settings = customersSettings.FirstOrDefault(s => s.CustomerId == customerUser.Customer_Id);
                    if (settings != null)
                    {
                        customerUser.ShowOnStartPage = settings.ShowOnStartPage.ToInt();
                    }
                }

                uow.Save();
            }
        }

        public List<Customer> GetCustomersForUser(int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var customersRep = uow.GetRepository<Customer>();
                var usersRep = uow.GetRepository<User>();
                var userCustomersRep = uow.GetRepository<CustomerUser>();

                var customers = customersRep.GetAll();
                var users = usersRep.GetAll().GetById(userId);
                var userCustomers = userCustomersRep.GetAll();

                return UsersMapper.MapToUserCustomers(customers, users, userCustomers);
            }
        }

        public int? GetUserDefaultWorkingGroupId(int userId, int customerId)
        {
            return _userRepository.GetUserDefaultWorkingGroupId(userId, customerId);
        }

        public WorkingGroupEntity GetUserDefaultWorkingGroup(int userId, int customerId)
        {
            return _userRepository.GetUserDefaultWorkingGroup(userId, customerId);
        }

        /// <summary>
        /// The get initialize position.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetInitializePosition(Module module)
        {
            switch (module)
            {
                case Module.Customers:
                    return 101;
                case Module.ChangeManagement:
                    return 102;
                case Module.Problems:
                    return 103;
                case Module.Statistics:
                    return 104;
                case Module.BulletinBoard:
                    return 201;
                case Module.Calendar:
                    return 202;
                case Module.Faq:
                    return 203;
                case Module.Cases:
                    return 204;
                case Module.OperationalLog:
                    return 301;
                case Module.DailyReport:
                    return 302;
                case Module.QuickLinks:
                    return 303;
                case Module.Documents:
                    return 304;
            }

            return 101;
        }

        private int? GetDefaultNumberOfRows(Module module)
        {
            switch (module)
            {
                case Module.Customers:
                case Module.QuickLinks:
                    return null;
            }

            return 3;
        }

        private bool GetDefaultVisibility(Module module)
        {
            switch (module)
            {
                case Module.ChangeManagement:
                    return false;
            }

            return true;
        }

        private IDictionary<string, string> ValidateUserFields(User user, bool isExistingUser = false, string confirmpassword = "")
        {
            var errors = new Dictionary<string, string>();

            List<UserPermission> wrongPermissions;
            if (!_userPermissionsChecker.CheckPermissions(user, out wrongPermissions))
            {
                errors.Add("User permissions", _translator.Translate("There are wrong permissions for this user group."));
            }
            var hasDublicate = _userRepository.FindUsersByUserId(user.UserID).Count() > (isExistingUser ? 1 : 0);
            if (hasDublicate)
            {
                errors.Add("User.UserID", "Det här användarnamnet är upptaget. Var vänlig använd något annat.");
            }

            if (!isExistingUser)
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    errors.Add("NewPassWord", "Du måste ange ett lösenord");
                }
                else if (!user.Password.Equals(confirmpassword, StringComparison.CurrentCulture))
                {
                    errors.Add("NewPassword",
                        "Det nya lösenordet bekräftades inte korrekt. Kontrollera att nytt lösenord och bekräftat lösenord stämmer överens");
                }
            }

            if (user.Email != null)
            {
                var userEMail = user.Email.TrimStart().TrimEnd();
                if (userEMail.Contains(' ') || !EmailHelper.IsValid(userEMail))
                    errors.Add("User.Email", "E-postadress är inte giltig.");
            }
            else
                errors.Add("User.Email", "E-postadress är inte giltig.");

            if (string.IsNullOrEmpty(user.UserID))
                errors.Add("User.UserID", "Du måste ange ett Id");

            if (string.IsNullOrEmpty(user.SurName))
                errors.Add("User.SurName", "Du måste ange ett efternamn");

            if (string.IsNullOrEmpty(user.FirstName))
                errors.Add("User.FirstName", "Du måste ange ett förnamn");

            if (!user.Cs.Any(it => it.Id == user.Customer_Id))
            {
                errors.Add("User.Customer_Id", "Du måste ange en standardkund");
            }

            // Get passwordlength for Customer
            var minPasswordLength = 0;
            var customerSetting = _settingRepository.GetCustomerSetting(user.Customer_Id);
            if (customerSetting != null)
                minPasswordLength = customerSetting.MinPasswordLength;

            if (!string.IsNullOrEmpty(user.Password))
            {
                if (customerSetting != null)
                { 
                    if (customerSetting.ComplexPassword != 0)
                    {
                        if (!PasswordHelper.IsValid(user.Password))
                            errors.Add("NewPassword", "Lösenord är inte giltigt. Minst 8 tecken, varav en stor bokstav, en liten bokstav, en siffra och ett special tecken (!@#=$&?*).");
                    }
                    else if (user.Password.Length < minPasswordLength)
                        errors.Add("NewPassword", "Lösenord är inte giltigt. Minst antal tecken är: " + minPasswordLength);
                }
            }

            return errors;
        }

        public string GetUserEmail(int id)
        {
            var user = _userRepository.GetUser(id);
            return user.Email;
        }
    }
}
