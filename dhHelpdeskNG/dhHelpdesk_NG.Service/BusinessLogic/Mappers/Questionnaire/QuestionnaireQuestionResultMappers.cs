namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.Response.Questionnaire;

    public static class QuestionnaireQuestionResultMappers
    {
        public static List<OptionResult> MapToOptionResults(this IQueryable<QuestionnaireQuestionResultEntity> query)
        {

            List<OptionResult> overviews =
                query.Select(x => new Qrp {
                    CaseId = x.QuestionnaireResult.QuestionnaireCircularPart.Case_Id,
                    QuestionnaireQuestionOptionId = x.QuestionnaireQuestionOption_Id,
                    DepartmentId = x.QuestionnaireResult.QuestionnaireCircularPart.Case.Department_Id,
                    OptionNote = new OptionNote
                {
                    NoteText = x.QuestionnaireQuestionNote,
                    CaseId = x.QuestionnaireResult.QuestionnaireCircularPart.Case.CaseNumber
                },
                InitiatorEmail = x.QuestionnaireResult.QuestionnaireCircularPart.Case.PersonsEmail } )
                .GroupBy(x => x.QuestionnaireQuestionOptionId)
                    .Select(x => new
                    {
                        OptionId = x.Key,
                        Count = x.Count(),
                        CaseIds = x.Select(r => r.CaseId),
                        DepartmentIds = x.Where(r => r.DepartmentId.HasValue).Select(r => r.DepartmentId.Value),
                        Notes = x.Select(r => r.OptionNote),
                        Emails = x.Select(r => r.InitiatorEmail)
                    })
                    .ToList()
                    .Select(x => new OptionResult(x.OptionId, x.Count, x.CaseIds, x.Notes, x.Emails, x.DepartmentIds))
                    .ToList();

            return overviews;
        }

        private class Qrp
        {
            public int CaseId { get; set; }
            public int QuestionnaireQuestionOptionId { get; set; }
            public OptionNote OptionNote { get; set; }
            public string InitiatorEmail { get; set; }
            public int? DepartmentId { get; set; }
        }
    }
}