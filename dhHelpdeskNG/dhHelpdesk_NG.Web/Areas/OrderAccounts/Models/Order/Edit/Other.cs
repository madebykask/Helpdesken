namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public class Other
    {
        public Other()
        {
        }

        public Other(
            ConfigurableFieldModel<decimal?> caseNumber,
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<FilesModel> fileName)
        {
            this.CaseNumber = caseNumber;
            this.Info = info;
            this.FileName = fileName;
        }

        public ConfigurableFieldModel<decimal?> CaseNumber { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<FilesModel> FileName { get; set; }
    }
}