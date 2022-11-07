using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Gdpr
{
    public class DataPrivacyResult
    {
        public IList<int> CaseIdsResult { get; set; }
        public IList<decimal> CaseNumbersErrorResult { get; set; }
    }
}
