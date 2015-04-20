namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishingCauseCategoryCustomerModel
    {
        public FinishingCauseCategoryCustomerModel(FinishingCauseCategoryCustomerData data)
        {
            this.Data = data;
        }

        [NotNull]
        public FinishingCauseCategoryCustomerData Data { get; private set; }
    }
}