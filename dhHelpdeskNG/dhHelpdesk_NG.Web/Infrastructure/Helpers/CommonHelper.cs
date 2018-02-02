using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Infrastructure.Helpers
{
    internal static class CommonHelper
    {
        public static string GetFinishingCauseFullPath(FinishingCauseInfo[] finishingCauses, int? finishingCauseId)
        {
            if (!finishingCauseId.HasValue)
            {
                return string.Empty;
            }

            var finishingCause = finishingCauses.FirstOrDefault(f => f.Id == finishingCauseId);
            if (finishingCause == null)
            {
                return string.Empty;
            }

            var list = new List<FinishingCauseInfo>();
            var parent = finishingCause;
            do
            {
                list.Add(parent);
                parent = finishingCauses.FirstOrDefault(c => c.Id == parent.ParentId);
            }
            while (parent != null);

            return string.Join(" - ", list.Select(c => c.Name).Reverse());
        }

        public static SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<User> users, Setting customerSetting,
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
    }
}