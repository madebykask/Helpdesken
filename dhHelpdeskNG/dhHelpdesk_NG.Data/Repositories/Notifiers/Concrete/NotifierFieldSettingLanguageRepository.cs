namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public sealed class NotifierFieldSettingLanguageRepository : RepositoryBase<ComputerUserFieldSettingsLanguage>, INotifierFieldSettingLanguageRepository
    {
        public NotifierFieldSettingLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<Caption> FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId);

            var captions =
                settings.Join(
                    this.DataContext.ComputerUserFieldSettingsLanguages,
                    s => new { SettingId = s.Id, LanguageId = languageId },
                    t => new { SettingId = t.ComputerUserFieldSettings_Id, LanguageId = t.Language_Id },
                    (s, t) => new { FieldName = s.ComputerUserField, Text = t.Label }).ToList();

            return captions.Select(c => new Caption(c.FieldName, c.Text)).ToList();
        }

        public IEnumerable<ComputerUserFieldSettingsLanguage> GetComputerUserFieldSettingsLanguage(int? customerId, int? languageId)
        {
            var query = from cfsl in this.DataContext.ComputerUserFieldSettingsLanguages
                        join cfs in this.DataContext.ComputerUserFieldSettings on cfsl.ComputerUserFieldSettings_Id equals cfs.Id
                        where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId
                        group cfsl by new { cfsl.ComputerUserFieldSettings_Id, cfsl.Label, cfsl.Language_Id, cfsl.FieldHelp } into grouped
                        select new ComputerUserFieldSettingsLanguage
                        {
                            ComputerUserFieldSettings_Id = grouped.Key.ComputerUserFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Language_Id = grouped.Key.Language_Id,
                            FieldHelp = grouped.Key.FieldHelp
                        };

            return query.OrderBy(x => x.ComputerUserFieldSettings_Id);
        }
    }
}
