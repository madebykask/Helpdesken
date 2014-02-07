namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Notifiers;

    public interface INotifierFieldSettingLanguageRepository : IRepository<ComputerUserFieldSettingsLanguage>
    {
        List<FieldCaptionDto> FindByCustomerIdAndLanguageId(int customerId, int languageId);
    }
}
