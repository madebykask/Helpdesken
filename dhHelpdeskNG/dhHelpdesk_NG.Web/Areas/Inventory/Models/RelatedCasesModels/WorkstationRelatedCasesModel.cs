namespace DH.Helpdesk.Web.Areas.Inventory.Models.RelatedCasesModels
{
    public class WorkstationRelatedCasesModel : BaseEditWorkstationModel
    {
        public WorkstationRelatedCasesModel(int id) : base(id)
        {
            Id = id;
        }

        public RelatedCasesModel RelatedCases { get; set; }

        public override WorkstationEditTabs Tab
        {
            get { return WorkstationEditTabs.Cases; }
        }
    }
}