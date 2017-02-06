using System;
using System.Data.Entity;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.Dal.Repositories.Notifiers;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class UserEmailRepository : Repository, IUserEmailRepository
    {
        private readonly INotifierRepository _notifierRepository;

        public UserEmailRepository(IDatabaseFactory databaseFactory, INotifierRepository notifierRepository)
            : base(databaseFactory)
        {
            _notifierRepository = notifierRepository;
        }

        public List<MailAddress> FindUserEmails(int userId)
        {
            var emails = this.DbContext.Users.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefault();
            
            if (string.IsNullOrEmpty(emails))
            {
                return new List<MailAddress>();
            }

            return emails.Split(";")
                .Where(EmailHelper.IsValid)
                .Select(e => new MailAddress(e))
                .ToList();
        }

        public List<CaseEmailSendOverview> GetUserEmailsListForCaseSend(int customerId, string searchText, bool searchInWorkingGrs, bool searchInInitiators, bool searchInAdmins, bool searchInEmailGrs)
        {
            var result = new List<CaseEmailSendOverview>();

            if (searchInInitiators)
            {
                var inits = _notifierRepository.Search(customerId, searchText).Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => new CaseEmailSendOverview
                {
                    UserId = x.UserId,
                    Name = x.SurName + " " + x.FirstName,
                    Emails = new List<string>
                    {
                        x.Email
                    },
                    GroupType = CaseUserSearchGroup.Initiator,
                    DepartmentName = string.IsNullOrEmpty(x.DepartmentName) ? string.Empty : x.DepartmentName
                }).ToList();
                result.AddRange(inits);
            }

            if (searchInAdmins)
            {
                var admins = DbContext.Users
                    .Where(x => x.Performer == 1)
                    .Where(x => x.IsActive == 1 && x.CustomerUsers.Any(cu => cu.Customer_Id == customerId))
                    .Where(x => x.UserID.Contains(searchText) || x.FirstName.Contains(searchText) || x.SurName.Contains(searchText) || x.Email.Contains(searchText))
                    .Where(x => !string.IsNullOrEmpty(x.Email))
                    .OrderBy(x => x.SurName)
                    .Select(x => new CaseEmailSendOverview
                    {
                        UserId = x.UserID,
                        Name = x.SurName + " " + x.FirstName,
                        Emails = new List<string>
                        {
                            x.Email
                        },
                        GroupType = CaseUserSearchGroup.Administaror,
                        DepartmentName = string.Empty
                    }).ToList();
                result.AddRange(admins);
            }
            if (searchInWorkingGrs)
            {
                var wgs = DbContext.WorkingGroups
                    .Include(x => x.UserWorkingGroups.Select(u => u.User))
                    .Where(x => x.IsActive == 1 && x.Customer_Id == customerId && x.WorkingGroupName.Contains(searchText)).ToList();
                var newList = wgs.Select(x => new CaseEmailSendOverview
                {
                    Name = x.WorkingGroupName,
                    Emails = x.UserWorkingGroups.Where(r => r.User.IsActive == 1).Select(r => r.User.Email).ToList(),
                    GroupType = CaseUserSearchGroup.WorkingGroup,
                    DepartmentName = string.Empty
                }).Where(x => x.Emails.Any()).ToList();
                result.AddRange(newList);
            }
            if (searchInEmailGrs)
            {
                var emailGroups = DbContext.EMailGroups
                    .Where(x => x.IsActive == 1 && x.Customer_Id == customerId && (x.Members.Contains(searchText) || x.Name.Contains(searchText))).ToList();
                var newList = emailGroups.Select(x => new CaseEmailSendOverview
                {
                    Name = x.Name,
                    Emails = x.Members.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    GroupType = CaseUserSearchGroup.EmailGroup,
                    DepartmentName = string.Empty
                }).Where(x => x.Emails.Any()).ToList();
                result.AddRange(newList);
            }
            return result.ToList();
        }
    }
}