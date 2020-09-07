using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedCase.Dal.Data
{
    public class CaseFile
    {
        public int Case_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? UserId { get; set; }
    }
}
