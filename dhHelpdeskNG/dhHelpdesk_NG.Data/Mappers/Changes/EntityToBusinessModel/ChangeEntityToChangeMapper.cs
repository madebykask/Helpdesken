namespace DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeEntityToChangeMapper : IEntityToBusinessModelMapper<ChangeEntity, Change>
    {
        private readonly IUserRepository userRepository;

        private readonly ICurrencyRepository currencyRepository;

        public ChangeEntityToChangeMapper(IUserRepository userRepository, ICurrencyRepository currencyRepository)
        {
            this.userRepository = userRepository;
            this.currencyRepository = currencyRepository;
        }

        public Change Map(ChangeEntity entity)
        {
            var orderer = this.CreateOrdererFields(entity);
            var general = this.CreateGeneralFields(entity);
            var registration = this.CreateRegistrationFields(entity);
            var analyze = this.CreateAnalyzeFields(entity);
            var implementation = this.CreateImplementationFields(entity);
            var evaluation = this.CreateEvaluationFields(entity);

            return new Change(entity.Id, orderer, general, registration, analyze, implementation, evaluation);
        }

        private OrdererFields CreateOrdererFields(ChangeEntity entity)
        {
            return new OrdererFields(
                entity.OrdererId,
                entity.OrdererName,
                entity.OrdererPhone,
                entity.OrdererCellPhone,
                entity.OrdererEMail,
                entity.OrdererDepartment_Id);
        }

        private GeneralFields CreateGeneralFields(ChangeEntity entity)
        {
            var inventories = !string.IsNullOrEmpty(entity.InventoryNumber)
                ? entity.InventoryNumber.Split(";").ToList()
                : new List<string>(0);

            UserName changedByUser = null;

            if (entity.ChangedByUser_Id != null && entity.ChangedByUser_Id.Value > 0)
            {
                changedByUser = new UserName(entity.ChangedByUser.FirstName, entity.ChangedByUser.SurName);
            }

            return new GeneralFields(
                entity.Prioritisation ?? 0,
                entity.ChangeTitle,
                entity.ChangeStatus_Id,
                entity.System_Id,
                entity.ChangeObject_Id,
                inventories,
                entity.WorkingGroup_Id,
                entity.User_Id,
                entity.PlannedReadyDate,
                entity.CreatedDate,
                entity.ChangedDate,
                changedByUser,
                entity.RSS.ToBool());
        }

        private RegistrationFields CreateRegistrationFields(ChangeEntity entity)
        {
            UserName approvedByUser = null;

            if (entity.ApprovedByUser_Id.HasValue)
            {
                approvedByUser = this.userRepository.GetUserName(entity.ApprovedByUser_Id.Value);
            }

            return new RegistrationFields(
                entity.ChangeGroup_Id,
                entity.ChangeDescription,
                entity.ChangeBenefits,
                entity.ChangeConsequence,
                entity.ChangeImpact,
                entity.DesiredDate,
                entity.Verified.ToBool(),
                (StepStatus)entity.Approval,
                entity.ApprovalDate,
                approvedByUser,
                entity.ChangeExplanation);
        }

        private AnalyzeFields CreateAnalyzeFields(ChangeEntity entity)
        {
            int? currencyId = null;

            if (!string.IsNullOrEmpty(entity.Currency))
            {
                currencyId = this.currencyRepository.GetCurrencyIdByCode(entity.Currency);
            }

            var estimatedTimeInHours = entity.TimeEstimatesHours ?? 0;

            UserName approvedByUser = null;

            if (entity.AnalysisApprovedByUser_Id.HasValue)
            {
                approvedByUser = this.userRepository.GetUserName(entity.AnalysisApprovedByUser_Id.Value);
            }

            return new AnalyzeFields(
                entity.ChangeCategory_Id,
                entity.ChangePriority_Id,
                entity.ResponsibleUser_Id,
                entity.ChangeSolution,
                entity.TotalCost,
                entity.YearlyCost,
                currencyId,
                estimatedTimeInHours,
                entity.ChangeRisk,
                entity.ScheduledStartTime,
                entity.ScheduledEndTime,
                entity.ImplementationPlan.ToBool(),
                entity.RecoveryPlan.ToBool(),
                (StepStatus)entity.AnalysisApproval,
                entity.AnalysisApprovalDate,
                approvedByUser,
                entity.ChangeRecommendation);
        }

        private ImplementationFields CreateImplementationFields(ChangeEntity entity)
        {
            return new ImplementationFields(
                entity.ImplementationStatus_Id,
                entity.RealStartDate,
                entity.FinishingDate,
                entity.BuildImplemented.ToBool(),
                entity.ImplementationPlanUsed.ToBool(),
                entity.ChangeDeviation,
                entity.RecoveryPlanUsed.ToBool(),
                entity.ImplementationReady.ToBool());
        }

        private EvaluationFields CreateEvaluationFields(ChangeEntity entity)
        {
            return new EvaluationFields(entity.ChangeEvaluation, entity.EvaluationReady.ToBool());
        }
    }
}
