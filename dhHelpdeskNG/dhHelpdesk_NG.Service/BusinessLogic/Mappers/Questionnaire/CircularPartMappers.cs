namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.MapperData;

    public static class CircularPartMappers
    {
        public static List<ConnectedCase> MapToConnectedCases(this IQueryable<QuestionnaireCircularPartEntity> query)
        {
            List<ConnectedCase> overviews =
                query.Select(
                    x => new { x.Case.Id, x.Case.CaseNumber, x.Case.Caption, x.Case.PersonsEmail, x.Guid, x.SendDate })
                    .ToList()
                    .Select(
                        c =>
                        new ConnectedCase(
                            c.Id,
                            (int)c.CaseNumber,
                            c.Caption,
                            c.PersonsEmail,
                            c.Guid,
                            c.SendDate != null))
                    .ToList();

            return overviews;
        }

        public static List<AvailableCase> MapToAvilableCases(
            this IQueryable<QuestionnaireCircularPartEntity> query,
            IQueryable<Case> cases,
            bool isUniqueEmail)
        {
            var questionnaireCirculars =
                query.Select(x => new { x.Case_Id, x.SendDate })
                    .GroupBy(x => x.Case_Id)
                    .Select(x => new { Case_Id = x.Key, SendDate = x.Max(c => c.SendDate) });

            var anonymus = from @case in cases
                           join questionnaireCircular in questionnaireCirculars on @case.Id equals
                               questionnaireCircular.Case_Id into splits
                           from split in splits.DefaultIfEmpty()
                           select new { @case.Id, @case.CaseNumber, @case.Caption, @case.PersonsEmail, split.SendDate };

            if (isUniqueEmail)
            {
                anonymus = from element in anonymus
                           group element by element.PersonsEmail
                           into groups
                           select groups.OrderBy(p => p.CaseNumber).FirstOrDefault();
            }

            List<AvailableCase> businessModels =
                anonymus.ToList()
                    .Select(
                        c => new AvailableCase(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail, c.SendDate != null))
                    .OrderBy(x => x.CaseNumber)
                    .ToList();

            return businessModels;
        }

        public static List<Participant> MapToParticipants(this IQueryable<QuestionnaireCircularPartEntity> query)
        {
            var businessModels =
                query.Select(
                    x =>
                    new Participant
                        {
                            Guid = x.Guid,
                            CaseNumber = x.Case.CaseNumber,
                            CaseId = x.Case.Id,
                            CaseDescription = x.Case.Description,
                            Caption = x.Case.Caption,
                            Email = x.Case.PersonsEmail
                        }).ToList();

            return businessModels;
        }

        public static Participant MapToParticipant(this QuestionnaireCircularPartEntity entity)
        {
            var businessModels =
                        new Participant
                        {
                            Guid = entity.Guid,
                            CaseNumber = entity.Case.CaseNumber,
                            CaseId = entity.Case.Id,
                            CaseDescription = entity.Case.Description,
                            Caption = entity.Case.Caption,
                            Email = entity.Case.PersonsEmail
                        };

            return businessModels;
        }
    }
}
