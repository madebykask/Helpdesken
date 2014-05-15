namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchOptions
    {        
        public SearchOptions(
            IEnumerable<ItemOverview> reports)
        {
            this.Reports = reports;
        }

        private SearchOptions()
        {
        }

        [NotNull]
        public IEnumerable<ItemOverview> Reports { get; private set; } 
    }
}