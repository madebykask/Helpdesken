namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewLogModelFactory : INewLogModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public NewLogModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        public LogModel Create(string temporaryId, ChangeEditData editData, LogEditSettings settings)
        {
            var sendToDialog = this.sendToDialogModelFactory.Create(
                editData.EmailGroups,
                editData.WorkingGroupsWithEmails,
                editData.Administrators);

            return new LogModel(sendToDialog);
        }
    }
}