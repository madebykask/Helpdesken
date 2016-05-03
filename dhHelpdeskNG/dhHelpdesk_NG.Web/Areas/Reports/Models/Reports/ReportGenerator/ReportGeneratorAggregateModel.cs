using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator
{
    public sealed class ReportGeneratorAggregateModel
    {
        public ReportGeneratorAggregateModel(
            Dictionary<DateTime, int> aggregation)
        {
            this.Aggregation = aggregation;
        }

        [NotNull]
        public Dictionary<DateTime, int> Aggregation { get; private set; }

        [MinValue(0)]
        public int CasesFound 
        {
            get { return Aggregation.Sum(x => x.Value); }
        }
    }
}