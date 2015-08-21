namespace DH.Helpdesk.Services.Services.CaseStatistic
{
    using System;

    using System.Linq;

    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;

    public class CaseStatisticService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public CaseStatisticService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
        
        public static int? ResolveIsSolvedInTime(DateTime? watchDate, DateTime finishDate, int SLA, int leadTime)
        {
            int? res = null;

            if (watchDate.HasValue)
            {
                res = (finishDate.RoundToDay() <= watchDate.Value.RoundToDay()).ToInt();
            }
            else if (SLA != 0)
            {
                res = (leadTime <= SLA).ToInt();
            }

            return res;
        }

        public CaseStatistic GetForCase(int caseId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                return uow.GetRepository<CaseStatistic>().Find(it => it.CaseId == caseId).FirstOrDefault();
            }
        }

        public void UpdateCaseStatistic(Case @case)
        {
            if (@case.Id <= 0)
            {
                throw new ArgumentException("Bad value: caseId can not be less or equal to 0");
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var r = uow.GetRepository<CaseStatistic>();
                var stat = r.Find(it => it.CaseId == @case.Id).FirstOrDefault();
                var SLA = 0;
                if (@case.Priority_Id.HasValue)
                {
                    var priorityRepository = uow.GetRepository<Priority>();
                    var priority = priorityRepository.GetById(@case.Priority_Id.Value);

                    SLA = priority != null ? priority.SolutionTime * 60 : 0;
                }
                
                if (stat == null)
                {
                    stat = new CaseStatistic { CaseId = @case.Id };
                    this.RefreshStatForCase(stat, @case, SLA);
                    r.Add(stat);
                }
                else
                {
                    this.RefreshStatForCase(stat, @case, SLA);
                    r.Update(stat);
                }

                uow.Save();
            }
        }

        private void RefreshStatForCase(CaseStatistic stat, Case @case, int SLA)
        {
            if (@case.FinishingDate.HasValue)
            {
                stat.WasSolvedInTime = CaseStatisticService.ResolveIsSolvedInTime(
                    @case.WatchDate,
                    @case.FinishingDate.Value,
                    SLA,
                    @case.LeadTime);
            }
            else
            {
                stat.WasSolvedInTime = null;
            }
        }
    }
}
