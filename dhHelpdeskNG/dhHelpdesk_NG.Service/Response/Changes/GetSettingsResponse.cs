namespace DH.Helpdesk.Services.Response.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GetSettingsResponse
    {
        public GetSettingsResponse(ChangeFieldSettings settings, List<ItemOverview> languages)
        {
            this.Settings = settings;
            this.Languages = languages;
        }

        [NotNull]
        public List<ItemOverview> Languages { get; private set; }

        [NotNull]
        public ChangeFieldSettings Settings { get; private set; }
    }
}