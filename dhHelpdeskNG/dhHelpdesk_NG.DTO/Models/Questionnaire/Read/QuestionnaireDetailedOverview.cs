using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    public class QuestionnaireDetailedOverview
    {
        public QuestionnaireOverview Questionnaire { get; set; }

        public int CaseId { get; set; }
    }
}
