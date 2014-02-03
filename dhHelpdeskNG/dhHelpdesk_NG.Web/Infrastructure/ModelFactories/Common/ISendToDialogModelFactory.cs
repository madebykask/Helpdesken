namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Web.Models;

    public interface ISendToDialogModelFactory
    {
        SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverviewDto> administrators);
    }
}