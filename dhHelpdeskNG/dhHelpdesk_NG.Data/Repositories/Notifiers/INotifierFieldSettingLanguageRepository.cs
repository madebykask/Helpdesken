namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public interface INotifierFieldSettingLanguageRepository : IRepository<ComputerUserFieldSettingsLanguage>
    {
        List<CaptionDto> FindByLanguageId(int customerId, int languageId);
    }
}
