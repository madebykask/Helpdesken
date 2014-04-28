namespace DH.Helpdesk.Dal.EntityConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    internal sealed class ChangeHistoryConfiguration : EntityTypeConfiguration<ChangeHistoryEntity>
    {
        internal ChangeHistoryConfiguration()
        {
            this.HasKey(h => h.Id);
            this.Property(h => h.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(h => h.Change_Id).IsRequired();
            this.HasRequired(h => h.Change).WithMany().HasForeignKey(h => h.Change_Id).WillCascadeOnDelete(false);
            this.Property(h => h.ChangeHistoryGUID).IsRequired();
            this.Property(h => h.OrdererId).IsOptional().HasMaxLength(20);
            this.Property(h => h.OrdererName).IsOptional().HasMaxLength(50);
            this.Property(h => h.OrdererPhone).IsOptional().HasMaxLength(50);
            this.Property(h => h.OrdererCellPhone).IsOptional().HasMaxLength(50);
            this.Property(h => h.OrdererEMail).IsOptional().HasMaxLength(50);
            this.Property(h => h.OrdererDepartment_Id).IsOptional();

            this.HasOptional(h => h.OrdererDepartment)
                .WithMany()
                .HasForeignKey(h => h.OrdererDepartment_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.ChangeTitle).IsOptional().HasMaxLength(50);
            this.Property(h => h.ChangeStatus_Id).IsOptional();

            this.HasOptional(h => h.ChangeStatus)
                .WithMany()
                .HasForeignKey(h => h.ChangeStatus_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.System_Id).IsOptional();
            this.HasOptional(h => h.System).WithMany().HasForeignKey(h => h.System_Id).WillCascadeOnDelete(false);
            this.Property(h => h.ChangeObject_Id).IsOptional();

            this.HasOptional(h => h.ChangeObject)
                .WithMany()
                .HasForeignKey(h => h.ChangeObject_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.InventoryNumber).IsOptional().HasMaxLength(100);
            this.Property(h => h.ChangeGroup_Id).IsOptional();

            this.HasOptional(h => h.ChangeGroup)
                .WithMany()
                .HasForeignKey(h => h.ChangeGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.ChangeCategory_Id).IsOptional();

            this.HasOptional(h => h.ChangeCategory)
                .WithMany()
                .HasForeignKey(h => h.ChangeCategory_Id)
                .WillCascadeOnDelete();

            this.Property(h => h.ChangeDescription).IsOptional();
            this.Property(h => h.ChangeBenefits).IsOptional();
            this.Property(h => h.ChangeConsequence).IsOptional();
            this.Property(h => h.ChangeImpact).IsOptional().HasMaxLength(100);
            this.Property(h => h.DesiredDate).IsOptional();
            this.Property(h => h.ChangePriority_Id).IsOptional();

            this.HasOptional(h => h.ChangePriority)
                .WithMany()
                .HasForeignKey(h => h.ChangePriority_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.WorkingGroup_Id).IsOptional();

            this.HasOptional(h => h.WorkingGroup)
                .WithMany()
                .HasForeignKey(h => h.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.User_Id).IsOptional();
            this.HasOptional(h => h.User).WithMany().HasForeignKey(h => h.User_Id).WillCascadeOnDelete(false);
            this.Property(h => h.Verified).IsRequired();
            this.Property(h => h.Approval).IsRequired();
            this.Property(h => h.ApprovalDate).IsOptional();
            this.Property(h => h.ApprovedByUser_Id).IsOptional();

            this.HasOptional(h => h.ApprovedByUser)
                .WithMany()
                .HasForeignKey(h => h.ApprovedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.ChangeExplanation).IsOptional();
            this.Property(h => h.ResponsibleUser_Id).IsOptional();

            this.HasOptional(h => h.ResponsibleUser)
                .WithMany()
                .HasForeignKey(h => h.ResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.ChangeSolution).IsOptional();
            this.Property(h => h.TotalCost).IsRequired();
            this.Property(h => h.YearlyCost).IsRequired();
            this.Property(h => h.Currency).IsOptional();
            this.Property(h => h.TimeEstimatesHours).IsOptional();
            this.Property(h => h.ChangeRisk).IsOptional();
            this.Property(h => h.ImplementationPlan).IsRequired();
            this.Property(h => h.RecoveryPlan).IsRequired();
            this.Property(h => h.ChangeRecommendation).IsOptional();
            this.Property(h => h.AnalysisApproval).IsOptional();
            this.Property(h => h.AnalysisApprovalDate).IsOptional();
            this.Property(h => h.AnalysisApprovedByUser_Id).IsOptional();

            this.HasOptional(h => h.AnalysysApprovedByUser)
                .WithMany()
                .HasForeignKey(h => h.AnalysisApprovedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.Prioritisation).IsOptional();
            this.Property(h => h.ScheduledStartTime).IsOptional();
            this.Property(h => h.ScheduledEndTime).IsOptional();
            this.Property(h => h.ImplementationStatus_Id).IsOptional();

            this.HasOptional(h => h.ImplementationStatus)
                .WithMany()
                .HasForeignKey(h => h.ImplementationStatus_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.RealStartDate).IsOptional();
            this.Property(h => h.ImplementationPlanUsed).IsRequired();
            this.Property(h => h.RecoveryPlanUsed).IsRequired();
            this.Property(h => h.BuildImplemented).IsRequired();
            this.Property(h => h.ChangeDeviation).IsOptional();
            this.Property(h => h.FinishingDate).IsOptional();
            this.Property(h => h.ImplementationReady).IsRequired();
            this.Property(h => h.ChangeEvaluation).IsOptional();
            this.Property(h => h.EvaluationReady).IsRequired();
            this.Property(h => h.RSS).IsRequired();
            this.Property(h => h.CreatedDate).IsRequired();
            this.Property(h => h.CreatedByUser_Id).IsOptional();

            this.HasOptional(h => h.CreatedByUser)
                .WithMany()
                .HasForeignKey(h => h.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(h => h.PlannedReadyDate).IsOptional();

            this.ToTable("tblChangeHistory");
        }
    }
}
