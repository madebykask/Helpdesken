namespace DH.Helpdesk.Services.Response.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GetNewChangeEditDataResponse
    {
        public GetNewChangeEditDataResponse(ChangeEditSettings editSettings, ChangeEditOptions editOptions)
        {
            this.EditSettings = editSettings;
            this.EditOptions = editOptions;
        }

        [NotNull]
        public ChangeEditSettings EditSettings { get; private set; }

        [NotNull]
        public ChangeEditOptions EditOptions { get; private set; }
    }
}
