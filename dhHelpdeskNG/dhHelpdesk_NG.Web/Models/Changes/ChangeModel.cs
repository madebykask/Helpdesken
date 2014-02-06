namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class ChangeModel
    {
        public ChangeModel()
        {
        }

        public ChangeModel(int id, InputModel input)
        {
            this.Id = id;
            this.Input = input;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public InputModel Input { get; set; }
    }
}