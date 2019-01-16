using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.Repositories.Computers
{
    public interface IComputerTabsSettingsRepository : INewRepository
    {
        void Update(TabSetting businessModel, int customerId, int languageId);
        List<WorkstationTabSetting> GetTabsSettingsForEdit(int customerId, int languageId);
        
    }
}