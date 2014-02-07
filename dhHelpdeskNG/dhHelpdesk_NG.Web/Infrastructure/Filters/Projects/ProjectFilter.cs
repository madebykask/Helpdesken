namespace DH.Helpdesk.Web.Infrastructure.Filters.Projects
{
    public class ProjectFilter
    {
        public Enums.Show State { get; set; }

        public int? ProjectManagerId { get; set; }

        public string ProjectNameLikeString { get; set; }
    }
}