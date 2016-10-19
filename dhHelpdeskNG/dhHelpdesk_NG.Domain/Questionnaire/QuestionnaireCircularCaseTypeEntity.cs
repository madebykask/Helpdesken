using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireCircularCaseTypeEntity : Entity
    {
        public int QuestionnaireCircularId { get; set; }

        public int CaseTypeId { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }

        public virtual CaseType CaseType { get; set; }
    }
}
