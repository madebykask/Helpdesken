using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Models.Shared;

    public interface ISendToDialogModelFactory
    {
        SendToDialogModel Create(
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);

        SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<CustomerUserInfo> users, Setting customerSetting,
            IEmailGroupService emailGroupService, IWorkingGroupService workingGroupService, IEmailService emailService,
            bool includeAdmins = true);
    }
}