using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionRegistration : Entity
    {
        public int CreatedByUser_Id { get; set; }
        public int Department_Id { get; set; }
        public int Question_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual Question Question { get; set; }
    }
}
