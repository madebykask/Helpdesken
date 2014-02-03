using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Domain
{
    public class ProductAreaWorkingGroup
    {
        public int ProductArea_Id { get; set; }
        public int WorkingGroup_Id { get; set; }

        public virtual WorkingGroupEntity WorkingGroup { get; set; }
    }
}
