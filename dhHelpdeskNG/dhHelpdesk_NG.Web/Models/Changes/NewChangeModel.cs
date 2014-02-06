namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class NewChangeModel
    {
        public NewChangeModel()
        {
        }

        public NewChangeModel(string id, InputModel input)
        {
            this.Id = id;
            this.Input = input;
        }

        public string Id { get; set; }

        [NotNull]
        public InputModel Input { get; set; }
    }
}