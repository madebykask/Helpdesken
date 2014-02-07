namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models;

    public interface ISendToDialogModelFactory
    {
        SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverviewDto> administrators);
    }
}