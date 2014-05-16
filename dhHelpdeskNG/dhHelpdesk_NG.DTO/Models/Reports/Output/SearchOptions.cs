namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchOptions
    {        
        public SearchOptions(
            IEnumerable<ItemOverview> reports)
        {
            this.Reports = reports;
        }

        [NotNull]
        public IEnumerable<ItemOverview> Reports { get; private set; } 
    }
}