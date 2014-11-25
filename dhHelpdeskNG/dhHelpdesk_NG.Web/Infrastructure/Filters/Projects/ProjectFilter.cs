namespace DH.Helpdesk.Web.Infrastructure.Filters.Projects
{
    using DH.Helpdesk.Web.Models.Shared;

    public class ProjectFilter
    {
        public ProjectFilter()
        {
            this.State = Enums.Show.Active;
            this.SortField = new SortFieldModel();
        }

        public Enums.Show State { get; set; }

        public int? ProjectManagerId { get; set; }

        public string ProjectNameLikeString { get; set; }

        public SortFieldModel SortField { get; set; }
    }
}