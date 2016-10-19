using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireCircularProductAreaEntity : Entity
    {
        public int QuestionnaireCircularId { get; set; }

        public int ProductAreaId { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }

        public virtual ProductArea ProductArea { get; set; }
    }
}
