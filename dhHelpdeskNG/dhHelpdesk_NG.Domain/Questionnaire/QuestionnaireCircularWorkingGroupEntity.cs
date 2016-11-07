using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireCircularWorkingGroupEntity : Entity
    {
        public int QuestionnaireCircularId { get; set; }

        public int WorkingGroupId { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }

        public virtual WorkingGroupEntity WorkingGroup { get; set; }
    }
}
