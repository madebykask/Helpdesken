namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifierModel
    {
        public NotifierModel(int id, NotifierInputModel inputModel)
        {
            this.Id = id;
            this.InputModel = inputModel;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public NotifierInputModel InputModel { get; private set; }
    }
}