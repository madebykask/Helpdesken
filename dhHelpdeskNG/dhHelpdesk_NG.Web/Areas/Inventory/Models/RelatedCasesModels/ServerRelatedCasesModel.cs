namespace DH.Helpdesk.Web.Areas.Inventory.Models.RelatedCasesModels
{
    public class ServerRelatedCasesModel : BaseEditServerModel
    {
        public ServerRelatedCasesModel(int id) : base(id)
        {
            Id = id;
        }

        public RelatedCasesModel RelatedCases { get; set; }

        public override ServerEditTabs Tab { get { return ServerEditTabs.Cases; } }
    }
}