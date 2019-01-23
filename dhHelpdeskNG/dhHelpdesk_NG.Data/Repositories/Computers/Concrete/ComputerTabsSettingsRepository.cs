using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
using DH.Helpdesk.Dal.Attributes.Inventory;
using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Computers;
using Z.EntityFramework.Plus;

namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    public class ComputerTabsSettingsRepository : Repository<WorkstationTabSetting>, IComputerTabsSettingsRepository
    {
        public ComputerTabsSettingsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        [CreateMissingWorkstationTabSettings("customerId")]
        public List<WorkstationTabSetting> GetTabsSettingsForEdit(int customerId, int languageId)
        {
            var query = DbSet
                .IncludeFilter(x => x.WorkstationTabSettingLanguages.Where(l => l.Language_Id == languageId))
                .Where(x => x.Customer_Id == customerId);
            return query.AsNoTracking().ToList();
        }

        public void Update(TabSetting setting, int customerId, int languageId)
        {
            var settingEntity = DbSet.Where(s => s.TabField.Equals(setting.TabField, StringComparison.CurrentCultureIgnoreCase) &&
                             s.Customer_Id == customerId)
                .Include(s => s.WorkstationTabSettingLanguages)
                .FirstOrDefault();
            if (settingEntity != null)
            {
                settingEntity.Show = setting.Show;
                settingEntity.ChangedDate = DateTime.Now;
                if (settingEntity.WorkstationTabSettingLanguages == null)
                    settingEntity.WorkstationTabSettingLanguages = new List<WorkstationTabSettingLanguage>();

                var language = settingEntity.WorkstationTabSettingLanguages.FirstOrDefault(l => l.Language_Id == languageId);
                if (language != null)
                    language.Label = setting.Caption;
                else
                    settingEntity.WorkstationTabSettingLanguages.Add(new WorkstationTabSettingLanguage
                    {
                        FieldHelp = string.Empty,
                        Label = setting.Caption,
                        Language_Id = languageId,
                        WorkstationTabSetting_Id = settingEntity.Id
                    });
            }
        }
    }
}