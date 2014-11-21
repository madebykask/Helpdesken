namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings
{
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GetSettingsResponse
    {
        public GetSettingsResponse(FullFieldSettings settings)
        {
            this.Settings = settings;
        }

        [NotNull]
        public FullFieldSettings Settings { get; private set; }
    }
}