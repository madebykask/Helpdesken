namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierModel
    {
        public NotifierModel(int id, NotifierInputModel inputModel)
        {
            ArgumentsValidator.IsId(id, "id");
            ArgumentsValidator.NotNull(inputModel, "inputModel");

            this.Id = id;
            this.InputModel = inputModel;
        }

        public int Id { get; private set; }

        public NotifierInputModel InputModel { get; private set; }
    }
}