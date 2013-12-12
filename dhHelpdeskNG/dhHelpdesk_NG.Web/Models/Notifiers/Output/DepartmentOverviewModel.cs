namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class DepartmentOverviewModel
    {
        public DepartmentOverviewModel(int id, string name)
        {
            // Test comment
            ArgumentsValidator.IsId(id, "id");
            ArgumentsValidator.NotNullAndEmpty(name, "name");

            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
    }
}