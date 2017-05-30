using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Response.Questionnaire
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OptionResult
    {
        public OptionResult(int optionId, int count, IEnumerable<int> caseIds, IEnumerable<OptionNote> notes = null)
        {
            this.OptionId = optionId;
            this.Count = count;
            this.CaseIds = caseIds;
            this.Notes = notes?.ToList() ?? new List<OptionNote>();
        }

        [IsId]
        public int OptionId { get; private set; }

        [MinValue(0)]
        public int Count { get; private set; }
        public IEnumerable<int> CaseIds { get; private set; }

        public List<OptionNote> Notes { get; set; }
    }
}
