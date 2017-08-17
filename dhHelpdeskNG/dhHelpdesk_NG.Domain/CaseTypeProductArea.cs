using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain
{
    public class CaseTypeProductArea
    {
        public int CaseType_Id { get; set; }

        public virtual CaseType CaseType { get; set; }

        public int ProductArea_Id { get; set; }

        public virtual ProductArea ProductArea { get; set; }
    }
}
