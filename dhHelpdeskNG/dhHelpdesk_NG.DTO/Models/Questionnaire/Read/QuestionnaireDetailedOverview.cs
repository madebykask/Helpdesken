namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    public class QuestionnaireDetailedOverview
    {
        public QuestionnaireOverview Questionnaire { get; set; }

        public int CaseId { get; set; }

        public string Caption { get; set; }

        public decimal CaseNumber { get; set; }
    }
}
