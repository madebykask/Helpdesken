using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireCircularDepartmentEntity : Entity
    {
        public int QuestionnaireCircularId { get; set; }

        public int DepartmentId { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }

        public virtual Department Department { get; set; }
    }
}
