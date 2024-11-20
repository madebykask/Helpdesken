using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain
{
    public class BulkEditCase
    {
        public int Id { get; set; }

        //public int Priority_Id { get; set; }

        public int Performer_User_Id { get; set; }

        public int CaseType_Id { get; set; }

        public int WorkingGroup_Id { get; set; }

        public int StateSecondary_Id { get; set; }

        public int Problem_Id { get; set; }

        public string FinishDescription { get; set; }

        public int? FinishTypeId { get; set; }

        public DateTime? FinishDate { get; set; }

        public int[] CasesToBeUpdated { get; set; }

    }
}
