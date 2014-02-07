namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models;

    public sealed class SendToDialogModelFactory : ISendToDialogModelFactory
    {
        public SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators)
        {
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