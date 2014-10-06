namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;

    public sealed class ApplicationEditModel
    {
        public ApplicationEditModel(ApplicationData data)
        {
            this.Data = data;
        }

        public ApplicationEditModel()
        {            
        }

        public ApplicationData Data { get; set; }
    }
}