namespace DH.Helpdesk.Services.Response.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FindChangeResponse
    {
        #region Constructors and Destructors

        public FindChangeResponse(ChangeEditData editData, ChangeEditSettings editSettings, ChangeEditOptions editOptions)
        {
            this.EditOptions = editOptions;
            this.EditSettings = editSettings;
            this.EditData = editData;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ChangeEditData EditData { get; private set; }

        [NotNull]
        public ChangeEditOptions EditOptions { get; private set; }

        [NotNull]
        public ChangeEditSettings EditSettings { get; private set; }

        #endregion
    }
}