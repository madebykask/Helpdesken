namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewChangeModel
    {
        public NewChangeModel()
        {
        }

        public NewChangeModel(string id, InputModel.InputModel input)
        {
            this.Id = id;
            this.Input = input;
        }

        public string Id { get; set; }

        [NotNull]
        public InputModel.InputModel Input { get; set; }
    }
}