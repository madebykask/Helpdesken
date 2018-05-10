using DH.Helpdesk.BusinessData.Models.User;
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

        public SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<CustomerUserInfo> users, Setting customerSetting,
            IEmailGroupService emailGroupService, IWorkingGroupService workingGroupService, IEmailService emailService, bool includeAdmins = true)
        {
            var emailGroups = emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = workingGroupService.GetWorkingGroupsWithActiveEmails(customerId, includeAdmins);
            var administrators = new List<ItemOverview>();

            if (users != null)
            {
                var filteredUsers =
                    users.Where(u => u.IsActive == 1 && 
                                     u.Performer == 1 && 
                                     u.UserGroupId > 1 && // exclude users
                                     emailService.IsValidEmail(u.Email) && 
                                     !string.IsNullOrWhiteSpace(u.Email)).ToList();

                administrators =
                    customerSetting.IsUserFirstLastNameRepresentation == 1
                        ? filteredUsers.OrderBy(it => it.FirstName)
                            .ThenBy(it => it.SurName)
                            .Select(u => new ItemOverview($"{u.FirstName} {u.SurName}", u.Email)).ToList()
                        : filteredUsers.OrderBy(it => it.SurName)
                            .ThenBy(it => it.FirstName)
                            .Select(u => new ItemOverview($"{u.SurName} {u.FirstName}", u.Email)).ToList();
            }

            return Create(emailGroups, workingGroups, administrators);
        }
    }
}