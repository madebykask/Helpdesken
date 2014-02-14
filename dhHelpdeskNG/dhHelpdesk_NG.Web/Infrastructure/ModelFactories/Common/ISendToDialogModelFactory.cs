namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Common;

    public interface ISendToDialogModelFactory
    {
        SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);
    }
}