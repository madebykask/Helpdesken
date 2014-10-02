namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.BusinessData.Models;

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

        public IEnumerable<ComputerUserFieldSettingsLanguageModel> GetComputerUserFieldSettingsWithLanguagesForDefaultCust(int languageId)
        {
            var query = from cfsl in this.DataContext.ComputerUserFieldSettingsLanguages
                        join cfs in this.DataContext.ComputerUserFieldSettings on cfsl.ComputerUserFieldSettings_Id equals cfs.Id
                        where (cfsl.Language_Id == languageId) && (cfs.Show == 1)
                        group cfsl by new { cfs.Customer_Id, cfsl.ComputerUserFieldSettings_Id, cfsl.Label, cfsl.Language_Id, cfsl.FieldHelp, cfs.ComputerUserField } into grouped
                        select new CustomComputerUserFieldSettingsLanguage
                        {
                            CustomerId = grouped.Key.Customer_Id == null ? null : grouped.Key.Customer_Id,
                            Id = grouped.Key.ComputerUserFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Language_Id = grouped.Key.Language_Id,
                            FieldHelp = grouped.Key.FieldHelp,
                            Name = grouped.Key.ComputerUserField
                        };

            var res1 = query.Select(q => new { q.Id, q.Label, q.Language_Id, q.FieldHelp, q.CustomerId, q.Name}).ToList();
            var res2 = res1.Where(q => (int?)q.CustomerId == null)
                           .Select(q => new ComputerUserFieldSettingsLanguageModel
                           {
                               Id = q.Id,
                               Label = q.Label,
                               Language_Id = q.Language_Id,
                               FieldHelp = q.FieldHelp,
                               Name = q.Name
                               
                           })
                           .OrderBy(x => x.Id)
                           .ToList();
            return res2;
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
