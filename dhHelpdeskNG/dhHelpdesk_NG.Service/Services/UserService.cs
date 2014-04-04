using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Repositories.Users;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IUserService
    {
        IEnumerable<CustomerUser> GetCustomerUserForUser(int userId);

        IList<UserLists> GetUserOnCases(int customer);
        IList<CustomerWorkingGroupForUser> GetListToUserWorkingGroup(int userId);
        IList<LoggedOnUsersOnIndexPage> GetListToUserLoggedOn();
        IList<Department> GetDepartmentsForUser(int userId, int customerId = 0);
        IList<User> GetAdministrators(int customerId, int active = 1);
        IList<User> GetSystemOwners(int customerId);
        IList<User> GetUsers();
        IList<User> GetUsers(int customerId);
        IList<User> SearchSortAndGenerateUsers(int StatusId, UserSearch SearchUsers);
        IList<UserGroup> GetUserGroups();
        IList<UserRole> GetUserRoles();
        IList<UserWorkingGroup> GetUserWorkingGroups();
        IList<User> GetUsersForWorkingGroup(int customerId, int workingGroupId);

        User GetUser(int id);
        UserRole GetUserRoleById(int id);
        UserWorkingGroup GetUserWorkingGroupById(int userId, int workingGroupId);

        DeleteMessage DeleteUser(int id);

        void SavePassword(int id, string password);
        void SaveEditUser(User user, int[] aas, int[] cs, int[] ots, int[] dus, List<UserWorkingGroup> UserWorkingGroups, out IDictionary<string, string> errors);
        void SaveNewUser(User user, int[] aas, int[] cs, int[] ots, out IDictionary<string, string> errors);
        void SaveProfileUser(User user, out IDictionary<string, string> errors);
        void Commit();

        UserOverview Login(string name, string password);

        IEnumerable<ModuleOverview> GetModules();
        IEnumerable<UserModuleOverview> GetUserModules(int user);
        void UpdateUserModules(IEnumerable<UserModule> modules);
        void InitializeUserModules(IEnumerable<UserModuleOverview> modules);
    }

    public class UserService : IUserService
    {
        private readonly IAccountActivityRepository _accountActivityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IOrderTypeRepository _orderTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public readonly IUserRepository _userRepository;
        public readonly IUserGroupRepository _userGroupRepository;
        public readonly IUserRoleRepository _userRoleRepository;
        public readonly IUserWorkingGroupRepository _userWorkingGroupRepository;
        public readonly IDepartmentUserRepository _departmentUserRepository;
        public readonly ILogProgramRepository _logprogramRepository;
        public readonly ICaseSettingRepository _casesettingRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IUserModuleRepository _userModuleRepository;

        public UserService(
            IAccountActivityRepository accountActivityRepository,
            ICustomerRepository customerRepository,
            ICustomerUserRepository customerUserRepository,
            IDepartmentRepository departmentRepository,
            IOrderTypeRepository orderTypeRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserGroupRepository userGroupRepository,
            IUserRoleRepository userRoleRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IDepartmentUserRepository departmentUserRepository,
            ILogProgramRepository logprogramRepository,
            ICaseSettingRepository casesettingRepository,
            IModuleRepository moduleRepository,
            IUserModuleRepository userModuleRepository)
        {
            this._accountActivityRepository = accountActivityRepository;
            this._customerRepository = customerRepository;
            this._customerUserRepository = customerUserRepository;
            this._departmentRepository = departmentRepository;
            this._orderTypeRepository = orderTypeRepository;
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._userGroupRepository = userGroupRepository;
            this._userRoleRepository = userRoleRepository;
            this._userWorkingGroupRepository = userWorkingGroupRepository;
            this._departmentUserRepository = departmentUserRepository;
            this._logprogramRepository = logprogramRepository;
            this._casesettingRepository = casesettingRepository;
            _moduleRepository = moduleRepository;
            _userModuleRepository = userModuleRepository;
        }

        public IEnumerable<CustomerUser> GetCustomerUserForUser(int userId)
        {
            return this._customerUserRepository.GetAll().Where(x => x.User_Id == userId);
        }

        public IList<CustomerWorkingGroupForUser> GetListToUserWorkingGroup(int userId)
        {
            return this._userRepository.ListForWorkingGroupsInUser(userId).ToList();
        }

        public IList<LoggedOnUsersOnIndexPage> GetListToUserLoggedOn()
        {
            return this._userRepository.LoggedOnUsers();
        }

        public IList<Department> GetDepartmentsForUser(int userId, int customerId)
        {
            return this._departmentRepository.GetDepartmentsForUser(userId, customerId).ToList();
        }

        public IList<User> GetAdministrators(int customerId, int active = 1)
        {
            return this._userRepository.GetMany(x => x.UserGroup.Id != 1 && x.IsActive == active).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ToList();
        }

        public IList<User> GetSystemOwners(int customerId)
        {
            return this._userRepository.GetMany(x => x.IsActive == 1).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ToList(); //TODO: den här raden skall kanske fungera senare, men gör det inte just nu
        }

        public IList<User> GetUsersForWorkingGroup(int customerId, int workingGroupId)
        {
            return this._userRepository.GetUsersForWorkingGroup(customerId, workingGroupId).OrderBy(x => x.SurName).ToList();    
        }

        public IList<UserLists> GetUserOnCases(int customerId)
        {
            return this._userRepository.GetUserOnCases(customerId);  
        }

        public IList<User> GetUsers()
        {
            return this._userRepository.GetAll().OrderBy(x => x.SurName).ToList();
        }

        public IList<User> GetUsers(int customerId)
        {
            return this._userRepository.GetUsers(customerId).OrderBy(x => x.SurName).ToList();
        }

        public IList<User> SearchSortAndGenerateUsers(int statusId, UserSearch searchUsers)
        {
            return this._userRepository.GetUsersForUserSettingList(statusId, searchUsers);

        }

        public IList<UserGroup> GetUserGroups()
        {
            return this._userGroupRepository.GetAll().OrderBy(x => x.Id).ToList();
        }

        public IList<UserRole> GetUserRoles()
        {
            return this._userRoleRepository.GetAll().ToList();
        }

        public IList<UserWorkingGroup> GetUserWorkingGroups()
        {
            return this._userWorkingGroupRepository.GetAll().ToList();
        }

        public User GetUser(int id)
        {
            return this._userRepository.GetById(id);
        }

        public UserRole GetUserRoleById(int id)
        {
            return this._userRoleRepository.GetById(id);
        }

        public UserWorkingGroup GetUserWorkingGroupById(int userId, int workingGroupId)
        {
            return this._userWorkingGroupRepository.Get(x => x.User_Id == userId && x.WorkingGroup_Id == workingGroupId);
        }

        //public User GetSystemUserOwnerId(int userId)
        //{
        //    return _userRepository.GetUser(userId);
        //}
        public DeleteMessage DeleteUser(int id)
        {
            var user = this._userRepository.GetById(id);

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
                   

                    this._userRepository.Delete(user);
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
            var user = this._userRepository.GetById(id);
            user.Password = password;
            user.PasswordChangedDate = DateTime.Now;
            this._userRepository.Update(user);
            this.Commit();
        }

        public void SaveProfileUser(User user, out IDictionary<string, string> errors)
        {
            //var user = this._userRepository.GetById(id);

            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.BulletinBoardDate = user.BulletinBoardDate ?? DateTime.Now;
            //user.CaseStateSecondaryColor = user.CaseStateSecondaryColor ?? string.Empty;
            user.ChangeTime = DateTime.Now;
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;
            user.PasswordChangedDate = DateTime.Now;
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;
            user.RegTime = DateTime.Now;

            errors = new Dictionary<string, string>();

            this._userRepository.Update(user);
            this.Commit();
        }

        public void SaveEditUser(User user, int[] aas, int[] cs, int[] ots, int[] dus, List<UserWorkingGroup> UserWorkingGroups, out IDictionary<string, string> errors)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.BulletinBoardDate = user.BulletinBoardDate ?? DateTime.Now;
            //user.CaseStateSecondaryColor = user.CaseStateSecondaryColor ?? string.Empty;
            user.ChangeTime = DateTime.Now;
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;
            user.PasswordChangedDate = DateTime.Now;
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;
            user.RegTime = DateTime.Now;
            user.ShowQuickMenuOnStartPage = user.ShowQuickMenuOnStartPage;
            user.Password = user.Password ?? string.Empty;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(user.SurName + user.FirstName + user.UserID))
                errors.Add("User.SurName" + "User.FirstName" + "User.UserID", "Du måste ange ett för- och efternamn, samt ett Id");

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
                    var aa = this._accountActivityRepository.GetById(id);

                    if (aa != null)
                        user.AAs.Add(aa);
                }
            }


            if (user.Cs != null)
                foreach (var delete in user.Cs.ToList())
                    user.Cs.Remove(delete);
            else
                user.Cs = new List<Customer>();


            if (user.Id == 0)
            {
                if (user.CustomerUsers != null)
                    foreach (var delete in user.CustomerUsers.ToList())
                        user.CustomerUsers.Remove(delete);
                else
                    user.CustomerUsers = new List<CustomerUser>();
            }



            if (cs != null)
            {
                foreach (int id in cs)
                {
                    var c = this._customerRepository.GetById(id);

                    if (c != null)
                        user.Cs.Add(c);
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
                    var ot = this._orderTypeRepository.GetById(id);

                    if (ot != null)
                        user.OTs.Add(ot);
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
                    var dep = this._departmentRepository.GetById(id);

                    if (dep != null)
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
                if (UserWorkingGroups != null)
                {
                    foreach (var uwg in UserWorkingGroups)
                    {
                        if (uwg.UserRole != 0)
                            user.UserWorkingGroups.Add(uwg);
                    }
                }
                
            }

            #endregion

            if (user.Id == 0)
                this._userRepository.Add(user);
            else
                this._userRepository.Update(user);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveNewUser(User user, int[] aas, int[] cs, int[] ots, out IDictionary<string, string> errors)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Address = user.Address ?? string.Empty;
            user.ArticleNumber = user.ArticleNumber ?? string.Empty;
            user.BulletinBoardDate = user.BulletinBoardDate ?? DateTime.Now;
            //user.CaseStateSecondaryColor = user.CaseStateSecondaryColor ?? string.Empty;
            user.ChangeTime = DateTime.Now;
            user.CellPhone = user.CellPhone ?? string.Empty;
            user.Email = user.Email ?? string.Empty;
            user.Logo = user.Logo ?? string.Empty;
            user.LogoBackColor = user.LogoBackColor ?? string.Empty;
            user.PasswordChangedDate = DateTime.Now;
            user.Phone = user.Phone ?? string.Empty;
            user.PostalAddress = user.PostalAddress ?? string.Empty;
            user.PostalCode = user.PostalCode ?? string.Empty;
            user.RegTime = DateTime.Now;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(user.UserID))
                errors.Add("User.UserID", "Du måste ange ett Id");

            if (string.IsNullOrEmpty(user.SurName))
                errors.Add("User.SurName", "Du måste ange ett efternamn");

            if (string.IsNullOrEmpty(user.FirstName))
                errors.Add("User.FirstName", "Du måste ange ett förnamn");

            if (user.AAs != null)
                foreach (var delete in user.AAs.ToList())
                    user.AAs.Remove(delete);
            else
                user.AAs = new List<AccountActivity>();

            if (aas != null)
            {
                foreach (int id in aas)
                {
                    var aa = this._accountActivityRepository.GetById(id);

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
                foreach (int id in cs)
                {
                    var c = this._customerRepository.GetById(id);

                    if (c != null)
                        user.Cs.Add(c);
                }
            }

            if (user.UserWorkingGroups != null)
                foreach (var delete in user.UserWorkingGroups.ToList())
                    user.UserWorkingGroups.Remove(delete);
            else
                user.UserWorkingGroups = new List<UserWorkingGroup>();

            if (user.OTs != null)
                foreach (var delete in user.OTs.ToList())
                    user.OTs.Remove(delete);
            else
                user.OTs = new List<OrderType>();

            if (ots != null)
            {
                foreach (int id in ots)
                {
                    var ot = this._orderTypeRepository.GetById(id);

                    if (ot != null)
                        user.OTs.Add(ot);
                }
            }

            if (user.Id == 0)
                this._userRepository.Add(user);
            else
                this._userRepository.Update(user);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public UserOverview Login(string name, string password)
        {
            var user = this._userRepository.Login(name, password);
            
            return user;
        }

        public IEnumerable<ModuleOverview> GetModules()
        {
            return _moduleRepository.GetModules();
        }

        public IEnumerable<UserModuleOverview> GetUserModules(int user)
        {
            var userModules = _userModuleRepository.GetUserModules(user);

            if (userModules != null && userModules.Any())
                return userModules;

            return _moduleRepository
                .GetModules()
                .ToList()
                .Select(m => new UserModuleOverview()
                {
                    User_Id = user,
                    Module_Id = m.Id,
                    isVisible = true,  
                    NumberOfRows = 3,
                    Module = new ModuleOverview()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description
                    }
                });
        }

        public void UpdateUserModules(IEnumerable<UserModule> modules)
        {
            _userModuleRepository.UpdateUserModules(modules);
            Commit();
        }

        public void InitializeUserModules(IEnumerable<UserModuleOverview> modules)
        {
            if (modules == null)
                return;

            if (modules.Any(m => m.NotSaved()))
            {
                var toSave = modules
                    .Select(m => new UserModule()
                    {
                        User_Id = m.User_Id,
                        Module_Id = m.Module_Id,
                        Position = GetInitializePosition((Module)m.Module_Id),
                        NumberOfRows = m.NumberOfRows,
                        isVisible = m.isVisible
                    });
                _userModuleRepository.UpdateUserModules(toSave);
                Commit();
            }
        }

        private int GetInitializePosition(Module module)
        {
            switch (module)
            {
                case Module.Customers:
                    return 101;
                case Module.Problems:
                    return 102;
                case Module.Statistics:
                    return 103;
                case Module.BulletinBoard:
                    return 201;
                case Module.Calendar:
                    return 202;
                case Module.Faq:
                    return 203;
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
    }
}
