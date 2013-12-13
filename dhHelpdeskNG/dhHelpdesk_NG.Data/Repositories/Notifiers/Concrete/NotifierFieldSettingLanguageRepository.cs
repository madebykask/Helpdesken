namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class NotifierFieldSettingLanguageRepository : RepositoryBase<ComputerUserFieldSettingsLanguage>, INotifierFieldSettingLanguageRepository
    {
        public NotifierFieldSettingLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<CaptionDto> FindByLanguageId(int customerId, int languageId)
        {
            var settings = this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId);

            return
                settings.Join(
                    this.DataContext.ComputerUserFieldSettingsLanguages,
                    s => new { SettingId = s.Id, LanguageId = languageId },
                    t => new { SettingId = t.ComputerUserFieldSettings_Id, LanguageId = t.Language_Id },
                    (s, t) => new CaptionDto { FieldName = s.ComputerUserField, Text = t.Label }).ToList();
        }
    }
}
