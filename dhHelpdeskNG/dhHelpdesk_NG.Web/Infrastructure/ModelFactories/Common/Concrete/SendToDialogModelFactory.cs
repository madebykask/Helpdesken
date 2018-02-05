using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class SendToDialogModelFactory : ISendToDialogModelFactory
    {
        public SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators)
        {
            if (emailGroups == null)
            {
                emailGroups = new List<GroupWithEmails>();
            }

            if (workingGroups == null)
            {
                workingGroups = new List<GroupWithEmails>();
            }

            if (administrators == null)
            {
                administrators = new List<ItemOverview>();
            }

            var emailGroupList = new MultiSelectList(emailGroups, "Id", "Name");
            var emailGroupEmails = emailGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var workingGroupList = new MultiSelectList(workingGroups, "Id", "Name");
            var workingGroupEmails = workingGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var administratorList = new MultiSelectList(administrators, "Value", "Name");

            return new SendToDialogModel(
                emailGroupList,
                emailGroupEmails,
                workingGroupList,
                workingGroupEmails,
                administratorList);
        }

        public SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<User> users, Setting customerSetting,
            IEmailGroupService emailGroupService, IWorkingGroupService workingGroupService, IEmailService emailService, bool includeAdmins = true)
        {
            var emailGroups = emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = workingGroupService.GetWorkingGroupsWithActiveEmails(customerId, includeAdmins);
            var administrators = new List<ItemOverview>();

            if (users != null)
            {
                if (customerSetting.IsUserFirstLastNameRepresentation == 1)
                {
                    foreach (var u in users.OrderBy(it => it.FirstName).ThenBy(it => it.SurName))
                        if (u.IsActive == 1 && u.Performer == 1 && emailService.IsValidEmail(u.Email) && !string.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.FirstName, u.SurName), u.Email));
                }
                else
                {
                    foreach (var u in users.OrderBy(it => it.SurName).ThenBy(it => it.FirstName))
                        if (u.IsActive == 1 && u.Performer == 1 && emailService.IsValidEmail(u.Email) && !string.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.SurName, u.FirstName), u.Email));
                }
            }

            return Create(emailGroups, workingGroups, administrators);
        }
    }
}