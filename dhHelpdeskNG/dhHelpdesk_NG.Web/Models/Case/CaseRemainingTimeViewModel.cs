namespace DH.Helpdesk.Web.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;

    public sealed class CaseRemainingTimeViewModel
    {
        public CaseRemainingTimeViewModel(CaseRemainingTimeData data)
        {
            this.Data = data;
        }

        public CaseRemainingTimeData Data { get; private set; }
    }
}