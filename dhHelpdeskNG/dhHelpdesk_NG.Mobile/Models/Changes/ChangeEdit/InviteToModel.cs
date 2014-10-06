namespace DH.Helpdesk.Mobile.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Models.Shared;

    public sealed class InviteToModel
    {
        #region Constructors and Destructors

        public InviteToModel()
        {
        }

        public InviteToModel(SendToDialogModel sendToDialog)
        {
            this.SendToDialog = sendToDialog;
        }

        #endregion

        #region Public Properties

        public string Emails { get; set; }

        [NotNull]
        public SendToDialogModel SendToDialog { get; set; }

        #endregion
    }
}