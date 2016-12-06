using System;
using System.Data.Entity;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;

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
        public UserEmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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

        public List<CaseEmailSendOverview> GetEmailsListForIntLogSend(int customerId, string searchText, bool isWgChecked, bool isInitChecked,
            bool isAdmChecked, bool isEgChecked)
        {
            var result = new List<CaseEmailSendOverview>();

            if (isWgChecked)
            {
                var wgs = DbContext.WorkingGroups
                    .Include(x => x.UserWorkingGroups.Select(u => u.User))
                    .Where(x => x.IsActive == 1 && x.Customer_Id == customerId && x.WorkingGroupName.Contains(searchText)).ToList();
                var newList = wgs.Select(x => new CaseEmailSendOverview
                {
                    Name = x.WorkingGroupName,
                    Emails = x.UserWorkingGroups.Where(r => r.User.IsActive == 1).Select(r => r.User.Email).ToList(),
                    GroupType = CaseUserSearchGroup.WorkingGroup
                }).ToList();
                result.AddRange(newList);
            }
            if (isInitChecked)
            {
                var inits = DbContext.Users
                    .Where(x => x.IsActive == 1 && x.Customer_Id == customerId)
                    .Where(x => x.UserID.Contains(searchText) || x.FirstName.Contains(searchText) || x.SurName.Contains(searchText) || x.Email.Contains(searchText))
                    .OrderBy(x => x.SurName).ToList();
                foreach (var user in inits)
                {
                    var newItem = new CaseEmailSendOverview
                    {
                        UserId = user.UserID,
                        Name = user.SurName + " " + user.FirstName,
                        Emails = new List<string>(),
                        GroupType = CaseUserSearchGroup.Initiator
                    };
                    newItem.Emails.Add(user.Email);
                    result.Add(newItem);
                }
            }

            if (isAdmChecked)
            {
                var admins = DbContext.Users
                    .Where(x => x.Performer == 1)
                    .Where(x => x.IsActive == 1 && x.CustomerUsers.Any(cu => cu.Customer_Id == customerId))
                    .Where(x => x.UserID.Contains(searchText) || x.FirstName.Contains(searchText) || x.SurName.Contains(searchText) || x.Email.Contains(searchText))
                    .OrderBy(x => x.SurName).ToList();
                foreach (var user in admins)
                {
                    var newItem = new CaseEmailSendOverview
                    {
                        UserId = user.UserID,
                        Name = user.SurName + " " + user.FirstName,
                        Emails = new List<string>(),
                        GroupType = CaseUserSearchGroup.Administaror
                    };
                    newItem.Emails.Add(user.Email);
                    result.Add(newItem);
                }
            }
            if (isEgChecked)
            {
                var emailGroups = DbContext.EMailGroups
                    .Where(x => x.IsActive == 1 && x.Customer_Id == customerId && (x.Members.Contains(searchText) || x.Name.Contains(searchText))).ToList();
                var newList = emailGroups.Select(x => new CaseEmailSendOverview
                {
                    Name = x.Name,
                    Emails = x.Members.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    GroupType = CaseUserSearchGroup.EmailGroup
                }).ToList();
                result.AddRange(newList);
            }
            result = result.Where(x => x.Emails.Any()).ToList();
            return result;
        }
    }
}