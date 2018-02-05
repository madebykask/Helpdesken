namespace DH.Helpdesk.Web.Areas.Inventory.Models.RelatedCasesModels
{
    public class PrinterRelatedCasesModel : BaseEditPrinterModel
    {
        public PrinterRelatedCasesModel(int id) : base(id)
        {
            Id = id;
        }

        public RelatedCasesModel RelatedCases { get; set; }

        public override PrinterEditTabs Tab { get { return PrinterEditTabs.Cases; } }
    }
}