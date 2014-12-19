namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    public class HeadersFieldSettingsModel
    {
        public HeadersFieldSettingsModel()
        {
        }

        public HeadersFieldSettingsModel(
            string orderLabel,
            string userLabel,
            string accountLabel,
            string contactLabel,
            string deliveryLabel,
            string programLabel)
        {
            this.OrderLabel = orderLabel;
            this.UserLabel = userLabel;
            this.AccountLabel = accountLabel;
            this.ContactLabel = contactLabel;
            this.DeliveryLabel = deliveryLabel;
            this.ProgramLabel = programLabel;
        }

        public string OrderLabel { get; set; }

        public string UserLabel { get; set; }

        public string AccountLabel { get; set; }

        public string ContactLabel { get; set; }

        public string DeliveryLabel { get; set; }

        public string ProgramLabel { get; set; }
    }
}