namespace dhHelpdesk_NG.Web.Models.Problem.Input
{
    using dhHelpdesk_NG.Web.Infrastructure;

    public class SearchInputModel
    {
        public int CustomerId { get; set; }

        public Enums.Show Show { get; set; }
    }
}