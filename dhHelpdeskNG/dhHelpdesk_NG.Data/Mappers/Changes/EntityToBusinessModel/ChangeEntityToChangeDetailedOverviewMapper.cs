namespace DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeEntityToChangeDetailedOverviewMapper :
        IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
    {
        private readonly IUserRepository userRepository;

        private readonly IChangeChangeGroupRepository changeChangeGroupRepository;

        private readonly IChangeDepartmentRepository changeDepartmentRepository;

        private readonly IChangeContactRepository changeContactRepository;

        public ChangeEntityToChangeDetailedOverviewMapper(
                        IUserRepository userRepository, 
                        IChangeChangeGroupRepository changeChangeGroupRepository, 
                        IChangeDepartmentRepository changeDepartmentRepository, 
                        IChangeContactRepository changeContactRepository)
        {
            this.userRepository = userRepository;
            this.changeChangeGroupRepository = changeChangeGroupRepository;
            this.changeDepartmentRepository = changeDepartmentRepository;
            this.changeContactRepository = changeContactRepository;
        }

        public ChangeDetailedOverview Map(ChangeEntity entity)
        {
            var orderer = this.CreateOrdererFields(entity);
            var general = this.CreateGeneralFields(entity);
            var registration = this.CreateRegistrationFields(entity);
            var analyze = this.CreateAnalyzeFields(entity);
            var implementation = this.CreateImplementationFields(entity);
            var evaluation = this.CreateEvaluationFields(entity);

            return new ChangeDetailedOverview(
                entity.Id,
                orderer,
                general,
                registration,
                analyze,
                implementation,
                evaluation);
        }

        private OrdererFields CreateOrdererFields(ChangeEntity entity)
        {
            var department = entity.OrdererDepartment != null ? entity.OrdererDepartment.DepartmentName : string.Empty;

            return new OrdererFields(
                entity.OrdererId,
                entity.OrdererName,
                entity.OrdererPhone,
                entity.OrdererCellPhone,
                entity.OrdererEMail,
                department);
        }

        private GeneralFields CreateGeneralFields(ChangeEntity entity)
        {
            var status = entity.ChangeStatus != null ? entity.ChangeStatus.ChangeStatus : string.Empty;
            var system = entity.System != null ? entity.System.SystemName : string.Empty;
            var @object = entity.ChangeObject != null ? entity.ChangeObject.ChangeObject : string.Empty;
            var workingGroup = entity.WorkingGroup != null ? entity.WorkingGroup.WorkingGroupName : string.Empty;

            UserName administrator = null;

            if (entity.User_Id.HasValue)
            {
                administrator = this.userRepository.GetUserName(entity.User_Id.Value);
            }

            return new GeneralFields(
                entity.Prioritisation,
                entity.ChangeTitle,
                status,
                system,
                @object,
                entity.InventoryNumber,
                workingGroup,
                administrator,
                entity.PlannedReadyDate,
                entity.RSS.ToBool());
        }

        private RegistrationFields CreateRegistrationFields(ChangeEntity entity)
        {
            var contacts = this.changeContactRepository.FindChangeContacts(entity.Id);
            var affectedProcesses = this.changeChangeGroupRepository.FindProcessesByChangeId(entity.Id);
            var affectedDepartments = this.changeDepartmentRepository.FindDepartmensByChangeId(entity.Id);

            return new RegistrationFields(
                contacts,
                entity.ChangeGroup_Id.HasValue ? entity.ChangeGroup.ChangeGroup : null,
                affectedProcesses,
                affectedDepartments,
                entity.ChangeDescription,
                entity.ChangeBenefits,
                entity.ChangeConsequence,
                entity.ChangeImpact,
                entity.DesiredDate,
                entity.Verified.ToBool(),
                (StepStatus)entity.Approval,
                entity.ChangeExplanation);
        }

        private AnalyzeFields CreateAnalyzeFields(ChangeEntity entity)
        {
            var category = entity.ChangeCategory != null ? entity.ChangeCategory.Name : string.Empty;
            var priority = entity.ChangePriority != null ? entity.ChangePriority.ChangePriority : string.Empty;

            UserName responsible = null;

            if (entity.ResponsibleUser_Id.HasValue)
            {
                responsible = this.userRepository.GetUserName(entity.ResponsibleUser_Id.Value);
            }

            return new AnalyzeFields(
                category,
                priority,
                responsible,
                entity.ChangeSolution,
                entity.TotalCost,
                entity.YearlyCost,
                entity.TimeEstimatesHours,
                entity.ChangeRisk,
                entity.ScheduledStartTime,
                entity.ScheduledEndTime,
                entity.ImplementationPlan.ToBool(),
                entity.RecoveryPlan.ToBool(),
                (StepStatus)entity.AnalysisApproval,
                entity.ChangeRecommendation);
        }

        private ImplementationFields CreateImplementationFields(ChangeEntity entity)
        {
            var status = entity.ImplementationStatus != null ? entity.ImplementationStatus.ImplementationStatus : string.Empty;

            return new ImplementationFields(
                status,
                entity.RealStartDate,
                entity.BuildImplemented.ToBool(),
                entity.ImplementationPlanUsed.ToBool(),
                entity.ChangeDeviation,
                entity.RecoveryPlanUsed.ToBool(),
                entity.FinishingDate,
                (StepStatus)entity.ImplementationReady);
        }

        private EvaluationFields CreateEvaluationFields(ChangeEntity entity)
        {
            return new EvaluationFields(entity.ChangeEvaluation, (StepStatus)entity.EvaluationReady);
        }
    }
}