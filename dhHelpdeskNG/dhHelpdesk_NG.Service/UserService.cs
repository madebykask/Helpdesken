using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
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
        IList<User> SearchSortAndGenerateUsers(int customerId, int? StatusId, IUserSearch SearchUsers);
        IList<UserGroup> GetUserGroups();
        IList<UserRole> GetUserRoles();
        IList<UserWorkingGroup> GetUserWorkingGroups();

        User GetUser(int id);
        UserRole GetUserRoleById(int id);
        UserWorkingGroup GetUserWorkingGroupById(int userId, int workingGroupId);
        //String GetSystemUserOwnerId(int userId);

        DeleteMessage DeleteUser(int id);

        void SavePassword(int id, string password);
        void SaveEditUser(User user, int[] aas, int[] cs, int[] ots, int[] dus, List<UserWorkingGroup> UserWorkingGroups, out IDictionary<string, string> errors);
        void SaveNewUser(User user, int[] aas, int[] cs, int[] ots, out IDictionary<string, string> errors);
        void Commit();
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
            IUserWorkingGroupRepository userWorkingGroupRepository)
        {
            _accountActivityRepository = accountActivityRepository;
            _customerRepository = customerRepository;
            _customerUserRepository = customerUserRepository;
            _departmentRepository = departmentRepository;
            _orderTypeRepository = orderTypeRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userRoleRepository = userRoleRepository;
            _userWorkingGroupRepository = userWorkingGroupRepository;
        }

        public IEnumerable<CustomerUser> GetCustomerUserForUser(int userId)
        {
            return _customerUserRepository.GetAll().Where(x => x.User_Id == userId);
        }

        public IList<CustomerWorkingGroupForUser> GetListToUserWorkingGroup(int userId)
        {
            return _userRepository.ListForWorkingGroupsInUser(userId).ToList();
        }

        public IList<LoggedOnUsersOnIndexPage> GetListToUserLoggedOn()
        {
            return _userRepository.LoggedOnUsers();
        }

        public IList<Department> GetDepartmentsForUser(int userId, int customerId)
        {
            return _departmentRepository.GetDepartmentsForUser(userId, customerId).ToList();
        }

        public IList<User> GetAdministrators(int customerId, int active = 1)
        {
            return _userRepository.GetMany(x => x.UserGroup.Id != 1 && x.IsActive == active).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ToList();
        }

        public IList<User> GetSystemOwners(int customerId)
        {
            return _userRepository.GetMany(x => x.IsActive == 1).Where(x => x.CustomerUsers.Any(i => i.Customer_Id == customerId)).OrderBy(x => x.SurName).ToList(); //TODO: den här raden skall kanske fungera senare, men gör det inte just nu
        }

        public IList<UserLists> GetUserOnCases(int customerId)
        {
            return _userRepository.GetUserOnCases(customerId);  
        }

        public IList<User> GetUsers()
        {
            return _userRepository.GetAll().OrderBy(x => x.SurName).ToList();
        }

        public IList<User> GetUsers(int customerId)
        {
            return _userRepository.GetUsers(customerId).OrderBy(x => x.SurName).ToList();
        }

        public IList<User> SearchSortAndGenerateUsers(int customerId, int? StatusId, IUserSearch SearchUsers)
        {
            var query = (from u in _userRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select u);

            if (StatusId.HasValue)
            {
                if (StatusId == 2)
                    query = query.Where(x => x.IsActive == 0);
                else if (StatusId == 1)
                    query = query.Where(x => x.IsActive == 1);
            }

            if (!string.IsNullOrWhiteSpace(SearchUsers.SearchUs))
            {
                string s = SearchUsers.SearchUs.ToLower();
                query = query.Where(x => x.UserID.ToLower().Contains(s)
                    || x.ArticleNumber.ToLower().Contains(s)
                    || x.CellPhone.ToLower().Contains(s)
                    || x.Email.ToLower().Contains(s)
                    || x.FirstName.ToLower().Contains(s)
                    || x.Phone.ToLower().Contains(s)
                    || x.SurName.ToLower().Contains(s)
                    || x.UserGroup.Name.ToLower().ToString().Contains(s));
            }

            return query.OrderBy(x => x.SurName).ToList();
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

        public User GetUser(int id)
        {
            return _userRepository.GetById(id);
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
                    foreach (var aa in user.AAs)
                    {
                        _accountActivityRepository.Delete(aa);
                    }
                    foreach (var cc in user.Cs)
                    {
                        _customerRepository.Delete(cc);
                    }
                    foreach (var ot in user.OTs)
                    {
                        _orderTypeRepository.Delete(ot);
                    }

                    _userRepository.Delete(user);
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

        public void SavePassword(int id, string password)
        {
            var user = _userRepository.GetById(id);
            user.Password = password;
            user.PasswordChangedDate = DateTime.Now;
            _userRepository.Update(user);
            Commit();
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
                foreach (int id in cs)
                {
                    var c = _customerRepository.GetById(id);

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

            if (dus != null)
            {
                foreach (int id in dus)
                {
                    var dep = _departmentRepository.GetById(id);

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
                foreach (var uwg in UserWorkingGroups)
                {
                    if (uwg.UserRole != 0)
                        user.UserWorkingGroups.Add(uwg);
                }
            }

            #endregion

            if (user.Id == 0)
                _userRepository.Add(user);
            else
                _userRepository.Update(user);

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

            //TODO: Mia: Gör inga ändringar i o pilla INTE i customerUser härifrån.. det görs på annat håll.. det enda som user har o göra med är watchdate.. OK?????

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(user.SurName + user.FirstName + user.UserID))
                errors.Add("User.SurName" + "User.FirstName" + "User.UserID", "Du måste ange ett för- och efternamn, samt ett Id");

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

            if (user.Cs != null)
                foreach (var delete in user.Cs.ToList())
                    user.Cs.Remove(delete);
            else
                user.Cs = new List<Customer>();

            if (cs != null)
            {
                foreach (int id in cs)
                {
                    var c = _customerRepository.GetById(id);

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
                    var ot = _orderTypeRepository.GetById(id);

                    if (ot != null)
                        user.OTs.Add(ot);
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
    }
}
