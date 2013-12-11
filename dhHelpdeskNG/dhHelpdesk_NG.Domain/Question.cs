using System;

namespace dhHelpdesk_NG.Domain
{
    public class Question : Entity
    {
        public int QuestionCategory_Id { get; set; }
        public string Name { get; set; }
        public string Pos { get; set; }
        public string QuestionHelp { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionCategory QuestionCategory { get; set; }
    }
}
