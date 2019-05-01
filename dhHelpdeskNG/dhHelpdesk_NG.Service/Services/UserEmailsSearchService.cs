using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Notifiers;

namespace DH.Helpdesk.Services.Services
{
    public interface IUserEmailsSearchService
    {
        IList<CaseEmailSendOverview> GetUserEmailsForCaseSend(int customerId, string searchText, IEmailSearchScope searchScope);
    }

    public class UserEmailsSearchService : IUserEmailsSearchService
    {
        private readonly INotifierRepository _notifierRepository;
        private readonly IUserEmailRepository _userEmailRepository;
        private readonly IEmailGroupRepository _emailGroupRepository;
        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        public UserEmailsSearchService(
            INotifierRepository notifierRepository, 
            IUserEmailRepository userEmailRepository, 
            IEmailGroupRepository emailGroupRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IWorkingGroupRepository workingGroupRepository)
        {
            _notifierRepository = notifierRepository;
            _userEmailRepository = userEmailRepository;
            _emailGroupRepository = emailGroupRepository;
            _userWorkingGroupRepository = userWorkingGroupRepository;
            _workingGroupRepository = workingGroupRepository;
        }

        public IList<CaseEmailSendOverview> GetUserEmailsForCaseSend(int customerId, string searchText, IEmailSearchScope searchScope)
        {
            var result = new List<CaseEmailSendOverview>();

            #region SearchInInitiators

            if (searchScope.SearchInInitiators)
            {
                var inits = _notifierRepository.Search(customerId, searchText)
                    .Where(x => !string.IsNullOrEmpty(x.Email))
                    .Select(x => new CaseEmailSendOverview
                    {
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        SurName = x.SurName,
                        Emails = new List<string> { x.Email },
                        GroupType = CaseUserSearchGroup.Initiator,
                        DepartmentName = string.IsNullOrEmpty(x.DepartmentName) ? string.Empty : x.DepartmentName
                    })
                    .ToList();

                result.AddRange(inits);
            }

            #endregion

            #region SearchInAdmins

            if (searchScope.SearchInAdmins)
            {
                //admin
                var items = _userEmailRepository.SearchUsersEmails(searchText, customerId, true, false);
                var admins = items // todo: shall we exclude Users?
                    .Select(x => new CaseEmailSendOverview
                    {
                        UserId = x.UserID,
                        FirstName = x.FirstName,
                        SurName = x.SurName,
                        Emails = new List<string> { x.Email },
                        GroupType = CaseUserSearchGroup.Administaror,
                        DepartmentName = string.Empty
                    })
                    .ToList();

                result.AddRange(admins);
            }

            #endregion

            #region SearchInUsers

            if (searchScope.SearchInUsers)
            {
                var items = _userEmailRepository.SearchUsersEmails(searchText, customerId, false, true);
                var users = items // todo: shall we exclude performers?
                    .Select(x => new CaseEmailSendOverview
                    {
                        UserId = x.UserID,
                        FirstName = x.FirstName,
                        SurName = x.SurName,
                        Emails = new List<string> { x.Email },
                        GroupType = CaseUserSearchGroup.Users,
                        DepartmentName = string.Empty
                    })
                    .ToList();

                result.AddRange(users);
            }

            #endregion

            #region SearchInWorkingGroups

            if (searchScope.SearchInWorkingGrs)
            {
                var newList = _userEmailRepository.SearchWorkingGroupEmails(customerId, searchText);
                result.AddRange(newList);
            }

            #endregion

            #region SearchInEmailGrs

            if (searchScope.SearchInEmailGrs)
            {
                var emailGroups =  
                    _emailGroupRepository.GetAll().AsQueryable()
                        .Where(x => x.IsActive == 1 && 
                                    x.Customer_Id == customerId &&
                                    (x.Members.Contains(searchText) || x.Name.Contains(searchText))).ToList();

                var newList = 
                    emailGroups.Select(x => new CaseEmailSendOverview
                    {
                        FirstName = x.Name,
                        SurName = string.Empty,
                        Emails = x.Members.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList(),
                        GroupType = CaseUserSearchGroup.EmailGroup,
                        DepartmentName = string.Empty
                    })
                    .Where(x => x.Emails.Any())
                    .ToList();

                result.AddRange(newList);
            }

            #endregion

            return result.ToList();
        }
    }
}