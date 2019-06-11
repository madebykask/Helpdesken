using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Services.Utils;

namespace DH.Helpdesk.Services.Services.CaseStatistic
{
    using System;

    using System.Linq;

    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;


    public interface ICaseStatisticService
    {
        DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate);
        CaseStatistic GetForCase(int caseId);
        void UpdateCaseStatistic(Case @case);
    }

    public class CaseStatisticService : ICaseStatisticService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ISettingService _settingService;
        private readonly IStateSecondaryService _stateSecondaryService;

        public CaseStatisticService(IUnitOfWorkFactory unitOfWorkFactory, ISettingService settingService, IStateSecondaryService stateSecondaryService)
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._settingService = settingService;
            _stateSecondaryService = stateSecondaryService;
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
            using (var uow = this._unitOfWorkFactory.Create())
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

            using (var uow = this._unitOfWorkFactory.Create())
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

        public DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate)
        {
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;

            if (oldSubStateId.HasValue)
            {
                var oldSubStatus = _stateSecondaryService.GetStateSecondary(oldSubStateId.Value);
                if (oldSubStatus != null)
                    oldSubStateMode = oldSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            DateTime? ret = null;
            if ((oldSubStateMode == -1 && newSubStateMode == 0) || 
                (oldSubStateMode == 1 && newSubStateMode == 0))
                ret = DateTime.UtcNow;
            else if ((oldSubStateMode == 1 && newSubStateMode == -1) ||
                     (oldSubStateMode == 1 && newSubStateMode == 1) ||
                     (oldSubStateMode == 0 && newSubStateMode == 0))
                ret = oldSLADate;

            return ret;
        }

        private void RefreshStatForCase(CaseStatistic stat, Case @case, int SLA)
        {            
            if (@case.FinishingDate.HasValue)
            {
                var baseCalculationTime = @case.FinishingDate.Value;
                var cs = _settingService.GetCustomerSetting(@case.Customer_Id);
                if (cs != null && cs.CalcSolvedInTimeByLatestSLADate != 0 && @case.LatestSLACountDate.HasValue)
                    baseCalculationTime = @case.LatestSLACountDate.Value;

                stat.WasSolvedInTime = ResolveIsSolvedInTime(
                    @case.WatchDate,
                    baseCalculationTime,
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
