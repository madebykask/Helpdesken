namespace DH.Helpdesk.Web.Models.Changes
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Changes.Edit;

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