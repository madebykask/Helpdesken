namespace dhHelpdesk_NG.Data.Repositories.Problem.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;

    public class ProblemLogRepository : RepositoryDecoratorBase<ProblemLog, NewProblemLogDto>, IProblemLogRepository
    {
        public ProblemLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public static ProblemLog MapProblem(NewProblemLogDto newProblemLog)
        {
            return new ProblemLog
            {
                Id = newProblemLog.Id,
                Problem_Id = newProblemLog.ProblemId,
                ChangedByUser_Id = newProblemLog.ChangedByUserId,
                LogText = newProblemLog.LogText,
                ShowOnCase = newProblemLog.ShowOnCase,
                FinishingCause_Id = newProblemLog.FinishingCauseId,
                FinishingDate = newProblemLog.FinishingDate,
                FinishConnectedCases = newProblemLog.FinishConnectedCases
            };
        }

        public static NewProblemLogDto MapProblemLog(ProblemLog newProblemLog)
        {
            return new NewProblemLogDto(newProblemLog.ChangedByUser_Id, newProblemLog.LogText, newProblemLog.ShowOnCase, newProblemLog.FinishingCause_Id, newProblemLog.FinishingDate, newProblemLog.FinishConnectedCases)
            {
                Id = newProblemLog.Id,
                ProblemId = newProblemLog.Problem_Id
            };
        }

        public static ProblemLogOverview MapProblem(ProblemLog newProblemLog)
        {
            return new ProblemLogOverview
                       {
                           Id = newProblemLog.Id,
                           ChangedByUserName = string.Format("{0} {1}", newProblemLog.ChangedByUser.FirstName, newProblemLog.ChangedByUser.SurName),
                           ChangedDate = newProblemLog.ChangedDate,
                           LogText = newProblemLog.LogText,
                       };
        }

        public override ProblemLog MapFromDto(NewProblemLogDto dto)
        {
            return MapProblem(dto);
        }

        public void DeleteByProblemId(int problemId)
        {
            var problemLogs =
                this.DataContext.ProblemLogs.Where(x => x.Problem_Id == problemId).ToList();

            problemLogs.ForEach(x => this.DataContext.ProblemLogs.Remove(x));
        }

        public NewProblemLogDto FindById(int problemLogId)
        {
            var problemLog = this.GetById(problemLogId);
            var problemLogOverview = MapProblemLog(problemLog);

            return problemLogOverview;
        }

        public List<ProblemLogOverview> FindByProblemId(int problemId)
        {
            var problemLogs = this.GetMany(x => x.Problem_Id == problemId)
                                  .OrderBy(x => x.CreatedDate)
                                  .Select(MapProblem)
                                  .ToList();

            return problemLogs;
        }
    }
}