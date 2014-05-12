namespace DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain.Changes;

    public sealed class UpdatedChangeToChangeEntityMapper : IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity>
    {
        private readonly ICurrencyRepository currencyRepository;

        public UpdatedChangeToChangeEntityMapper(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public void Map(UpdatedChange businessModel, ChangeEntity entity)
        {
            this.MapOrdererFields(entity, businessModel);
            this.MapGeneralFields(entity, businessModel);
            this.MapRegistrationFields(entity, businessModel);
            this.MapAnalyzeFields(entity, businessModel);
            this.MapImplementationFields(entity, businessModel);
            this.MapEvaluationFields(entity, businessModel);
        }

        private void MapOrdererFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            entity.OrdererId = businessModel.Orderer.Id;
            entity.OrdererName = businessModel.Orderer.Name;
            entity.OrdererPhone = businessModel.Orderer.Phone;
            entity.OrdererCellPhone = businessModel.Orderer.CellPhone;
            entity.OrdererEMail = businessModel.Orderer.Email;
            entity.OrdererDepartment_Id = businessModel.Orderer.DepartmentId;
        }

        private void MapGeneralFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            entity.Prioritisation = businessModel.General.Priority;
            entity.ChangeTitle = businessModel.General.Title;
            entity.ChangeStatus_Id = businessModel.General.StatusId;
            entity.System_Id = businessModel.General.SystemId;
            entity.ChangeObject_Id = businessModel.General.ObjectId;
            entity.InventoryNumber = string.Join(";", businessModel.General.Inventories);
            entity.WorkingGroup_Id = businessModel.General.WorkingGroupId;
            entity.User_Id = businessModel.General.AdministratorId;
            entity.PlannedReadyDate = businessModel.General.FinishingDate;
            entity.ChangedDate = businessModel.General.ChangedDate;
            entity.ChangedByUser_Id = businessModel.General.ChangedByUserId;
            entity.RSS = businessModel.General.Rss.ToInt();
        }

        private void MapRegistrationFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            entity.ChangeGroup_Id = businessModel.Registration.OwnerId;
            entity.ChangeDescription = businessModel.Registration.Description;
            entity.ChangeBenefits = businessModel.Registration.BusinessBenefits;
            entity.ChangeConsequence = businessModel.Registration.Consequence;
            entity.ChangeImpact = businessModel.Registration.Impact;
            entity.DesiredDate = businessModel.Registration.DesiredDate;
            entity.Verified = businessModel.Registration.Verified.ToInt();
            entity.Approval = (int)businessModel.Registration.Approval;
            entity.ApprovalDate = businessModel.Registration.ApprovedDateAndTime;
            entity.ApprovedByUser_Id = businessModel.Registration.ApprovedByUserId;
            entity.ChangeExplanation = businessModel.Registration.RejectExplanation;
        }

        private void MapAnalyzeFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            string currencyCode = null;

            if (businessModel.Analyze.CurrencyId.HasValue)
            {
                currencyCode = this.currencyRepository.GetCurrencyCode(businessModel.Analyze.CurrencyId.Value);
            }

            entity.ChangeCategory_Id = businessModel.Analyze.CategoryId;
            entity.ChangePriority_Id = businessModel.Analyze.PriorityId;
            entity.ResponsibleUser_Id = businessModel.Analyze.ResponsibleId;
            entity.ChangeSolution = businessModel.Analyze.Solution;
            entity.TotalCost = businessModel.Analyze.Cost;
            entity.YearlyCost = businessModel.Analyze.YearlyCost;
            entity.Currency = currencyCode;
            entity.TimeEstimatesHours = businessModel.Analyze.EstimatedTimeInHours;
            entity.ChangeRisk = businessModel.Analyze.Risk;
            entity.ScheduledStartTime = businessModel.Analyze.StartDate;
            entity.ScheduledEndTime = businessModel.Analyze.FinishDate;
            entity.ImplementationPlan = businessModel.Analyze.HasImplementationPlan.ToInt();
            entity.RecoveryPlan = businessModel.Analyze.HasRecoveryPlan.ToInt();
            entity.AnalysisApproval = (int)businessModel.Analyze.Approval;
            entity.AnalysisApprovalDate = businessModel.Analyze.ApprovedDateAndTime;
            entity.AnalysisApprovedByUser_Id = businessModel.Analyze.ApprovedByUserId;
            entity.ChangeRecommendation = businessModel.Analyze.RejectExplanation;
        }

        private void MapImplementationFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            entity.ImplementationStatus_Id = businessModel.Implementation.StatusId;
            entity.RealStartDate = businessModel.Implementation.RealStartDate;
            entity.FinishingDate = businessModel.Implementation.FinishingDate;
            entity.BuildImplemented = businessModel.Implementation.BuildImplemented.ToInt();
            entity.ImplementationPlanUsed = businessModel.Implementation.ImplementationPlanUsed.ToInt();
            entity.ChangeDeviation = businessModel.Implementation.Deviation;
            entity.RecoveryPlanUsed = businessModel.Implementation.RecoveryPlanUsed.ToInt();
            entity.ImplementationReady = businessModel.Implementation.ImplementationReady.ToInt();
        }

        private void MapEvaluationFields(ChangeEntity entity, UpdatedChange businessModel)
        {
            entity.ChangeEvaluation = businessModel.Evaluation.ChangeEvaluation;
            entity.EvaluationReady = businessModel.Evaluation.EvaluationReady.ToInt();
        }
    }
}
