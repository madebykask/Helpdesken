using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    public class WorkstationTabsSettingsModel
    {
        public WorkstationTabsSettingsModel()
        {
        }

        public WorkstationTabsSettingsModel(
            //int tabLanguageId,
            TabSettingModel computersTabSettingModel,
            TabSettingModel storageTabSettingModel,
            TabSettingModel softwareTabSettingModel,
            TabSettingModel hotFixesTabSettingModel,
            TabSettingModel computerLogsTabSettingModel,
            TabSettingModel accessoriesTabSettingModel,
            TabSettingModel relatedCasesTabSettingModel)
        {
            //TabLanguageId = tabLanguageId;
            ComputersTabSettingModel = computersTabSettingModel;
            StorageTabSettingModel = storageTabSettingModel;
            SoftwareTabSettingModel = softwareTabSettingModel;
            HotFixesTabSettingModel = hotFixesTabSettingModel;
            ComputerLogsTabSettingModel = computerLogsTabSettingModel;
            AccessoriesTabSettingModel = accessoriesTabSettingModel;
            RelatedCasesTabSettingModel = relatedCasesTabSettingModel;
        }

        //[IsId]
        //public int TabLanguageId { get; set; }
        [NotNull]
        [LocalizedDisplay("Arbetsstation")]
        public TabSettingModel ComputersTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Lagring")]
        public TabSettingModel StorageTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Program")]
        public TabSettingModel SoftwareTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Hotfix")]
        public TabSettingModel HotFixesTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Logg")]
        public TabSettingModel ComputerLogsTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Tillbehör")]
        public TabSettingModel AccessoriesTabSettingModel { get; set; }
        [NotNull]
        [LocalizedDisplay("Ärenden")]
        public TabSettingModel RelatedCasesTabSettingModel { get; set; }

    }
}