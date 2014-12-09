namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public class Other
    {
        public Other()
        {
        }

        public Other(
            ConfigurableFieldModel<string> caseNumber,
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<string> fileName)
        {
            this.CaseNumber = caseNumber;
            this.Info = info;
            this.FileName = fileName;
        }

        public ConfigurableFieldModel<string> CaseNumber { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<string> FileName { get; set; }
    }
}