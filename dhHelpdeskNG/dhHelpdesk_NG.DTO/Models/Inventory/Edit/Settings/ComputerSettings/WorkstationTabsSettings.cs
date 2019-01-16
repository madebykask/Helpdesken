using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    public class WorkstationTabsSettings : BusinessModel
    {
        public WorkstationTabsSettings(ModelStates state,
            //int tabLanguageId,
            TabSetting computersTabSetting,
            TabSetting storageTabSetting,
            TabSetting softwareTabSetting,
            TabSetting hotFixesTabSetting,
            TabSetting computerLogsTabSetting,
            TabSetting accessoriesTabSetting,
            TabSetting relatedCasesTabSetting) : base(state)
        {
            //TabLanguageId = tabLanguageId;
            ComputersTabSetting = computersTabSetting;
            StorageTabSetting = storageTabSetting;
            SoftwareTabSetting = softwareTabSetting;
            HotFixesTabSetting = hotFixesTabSetting;
            ComputerLogsTabSetting = computerLogsTabSetting;
            AccessoriesTabSetting = accessoriesTabSetting;
            RelatedCasesTabSetting = relatedCasesTabSetting;
        }
        
        //[IsId]
        //public int TabLanguageId { get; set; }
        [NotNull]
        public TabSetting ComputersTabSetting { get; set; }
        [NotNull]
        public TabSetting StorageTabSetting { get; set; }
        [NotNull]
        public TabSetting SoftwareTabSetting { get; set; }
        [NotNull]
        public TabSetting HotFixesTabSetting { get; set; }
        [NotNull]
        public TabSetting ComputerLogsTabSetting { get; set; }
        [NotNull]
        public TabSetting AccessoriesTabSetting { get; set; }
        [NotNull]
        public TabSetting RelatedCasesTabSetting { get; set; }
    }
}
