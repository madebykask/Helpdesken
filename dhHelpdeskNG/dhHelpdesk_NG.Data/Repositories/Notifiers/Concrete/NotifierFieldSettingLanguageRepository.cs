namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Notifiers;

    public sealed class NotifierFieldSettingLanguageRepository : RepositoryBase<ComputerUserFieldSettingsLanguage>, INotifierFieldSettingLanguageRepository
    {
        public NotifierFieldSettingLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<FieldCaptionDto> FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId);

            var captions =
                settings.Join(
                    this.DataContext.ComputerUserFieldSettingsLanguages,
                    s => new { SettingId = s.Id, LanguageId = languageId },
                    t => new { SettingId = t.ComputerUserFieldSettings_Id, LanguageId = t.Language_Id },
                    (s, t) => new { FieldName = s.ComputerUserField, Text = t.Label }).ToList();

            return captions.Select(c => new FieldCaptionDto(c.FieldName, c.Text)).ToList();
        }
    }
}
