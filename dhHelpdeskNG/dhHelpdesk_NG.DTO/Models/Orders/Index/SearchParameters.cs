namespace DH.Helpdesk.BusinessData.Models.Orders.Index
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchParameters
    {
        public SearchParameters(
                int customerId, 
                int? orderTypeId, 
                int[] administratorIds, 
                DateTime? startDate, 
                DateTime? endDate, 
                int[] statusIds, 
                string phrase, 
                int selectCount, 
                SortField sortField)
        {
            this.SortField = sortField;
            this.SelectCount = selectCount;
            this.Phrase = phrase;
            this.StatusIds = statusIds;
            this.EndDate = endDate;
            this.StartDate = startDate;
            this.AdministratorIds = administratorIds;
            this.OrderTypeId = orderTypeId;
            this.CustomerId = customerId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public int? OrderTypeId { get; private set; }

        [NotNull]
        public int[] AdministratorIds { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        [NotNull]
        public int[] StatusIds { get; private set; }

        public string Phrase { get; private set; }

        [MinValue(0)]
        public int SelectCount { get; private set; }

        public SortField SortField { get; private set; }
    }
}