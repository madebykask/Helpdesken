namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishingCauseCustomerModel
    {
        public FinishingCauseCustomerModel(FinishingCauseCustomerData data)
        {
            this.Data = data;
        }

        [NotNull]
        public FinishingCauseCustomerData Data { get; private set; }
    }
}