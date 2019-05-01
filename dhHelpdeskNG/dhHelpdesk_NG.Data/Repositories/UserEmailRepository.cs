using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Domain;

using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

namespace DH.Helpdesk.Dal.Repositories
{
    public sealed class UserEmailRepository : RepositoryBase<User>, IUserEmailRepository
    {
        public UserEmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<MailAddress> FindUserEmails(int userId)
        {
            var emails = Table.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefault();
            
            if (string.IsNullOrEmpty(emails))
            {
                return new List<MailAddress>();
            }

            return emails.Split(";")
                .Where(EmailHelper.IsValid)
                .Select(e => new MailAddress(e))
                .ToList();
        }

        public IList<UserBasicOvierview> SearchUsersEmails(string searchText, int customerId, bool performersOnly, bool usersOnly)
        {
            var query = Table
                .Where(x => x.IsActive == 1 && x.CustomerUsers.Any(cu => cu.Customer_Id == customerId))
                .Where(x => x.UserID.Contains(searchText) || x.FirstName.Contains(searchText) ||
                            x.SurName.Contains(searchText) || x.Email.Contains(searchText)
                            || (x.SurName + " " + x.FirstName).Contains(searchText)
                            || (x.FirstName + " " + x.SurName).Contains(searchText))
                .Where(x => !string.IsNullOrEmpty(x.Email));

            if (performersOnly)
            {
                var usersGroupId = (int)UserGroup.User;
                query = query.Where(x => x.Performer == 1 && x.UserGroup_Id > usersGroupId);
            }

            if (usersOnly)
            {
                var usersGroupId = (int)UserGroup.User;
                query = query.Where(x => x.UserGroup_Id == usersGroupId);
            }

            var items = query
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.SurName)
                .ThenBy(x => x.Id)
                .Select(x => new UserBasicOvierview()
                {
                    UserID = x.UserID,
                    FirstName = x.FirstName,
                    SurName = x.SurName,
                    Email = x.Email
                })
                .Take(25)
                .ToList();

            return items;
        }

        public List<CaseEmailSendOverview> SearchWorkingGroupEmails(int customerId, string searchText)
        {
            var wgs = DataContext.WorkingGroups.Where(x => x.IsActive == 1 &&
                                                           x.Customer_Id == customerId &&
                                                           x.WorkingGroupName.Contains(searchText))
                .AsQueryable();

            var usWkgs = DataContext.UserWorkingGroups
                .Include(x => x.User)
                .Where(x => x.User.IsActive == 1)
                .Where(x => x.IsMemberOfGroup)
                .AsQueryable();

            var workGs = wgs.GroupJoin(usWkgs, wg => wg.Id, uwg => uwg.WorkingGroup_Id, (wg, uwgs) => new
            {
                WorkingGroup = wg,
                UserWorkingGroups = uwgs
            }).ToList();

            var newList = workGs.Select(x => new CaseEmailSendOverview
                {
                    FirstName = x.WorkingGroup.WorkingGroupName,
                    SurName = string.Empty,
                    Emails = string.IsNullOrWhiteSpace(x.WorkingGroup.EMail)
                        ? x.UserWorkingGroups.Select(r => r.User.Email).ToList()
                        : new List<string> { x.WorkingGroup.EMail },
                    GroupType = CaseUserSearchGroup.WorkingGroup,
                    DepartmentName = string.Empty
                })
                .Where(x => x.Emails.Any())
                .OrderBy(x => x.FirstName)
                .ToList();

            return newList;
        }
    }
}