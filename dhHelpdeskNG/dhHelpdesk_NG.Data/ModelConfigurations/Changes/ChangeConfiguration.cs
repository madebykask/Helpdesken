namespace dhHelpdesk_NG.Data.ModelConfigurations.Changes
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeConfiguration : EntityTypeConfiguration<Change>
    {
        internal ChangeConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.Customer_Id).IsOptional();
            this.HasOptional(c => c.Customer).WithMany().HasForeignKey(c => c.Customer_Id).WillCascadeOnDelete(false);
            this.Property(c => c.ChangeGUID).IsRequired();
            this.Property(c => c.OrdererId).IsOptional().HasMaxLength(20);
            this.Property(c => c.OrdererName).IsOptional().HasMaxLength(50);
            this.Property(c => c.OrdererPhone).IsOptional().HasMaxLength(50);
            this.Property(c => c.OrdererCellPhone).IsOptional().HasMaxLength(50);
            this.Property(c => c.OrdererEMail).IsOptional().HasMaxLength(50);
            this.Property(c => c.OrdererDepartment_Id).IsOptional();
            
            this.HasOptional(c => c.OrdererDepartment)
                .WithMany()
                .HasForeignKey(c => c.OrdererDepartment_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangeTitle).IsOptional().HasMaxLength(50);
            this.Property(c => c.ChangeStatus_Id).IsOptional();
            
            this.HasOptional(c => c.ChangeStatus)
                .WithMany()
                .HasForeignKey(c => c.ChangeStatus_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.System_Id).IsOptional();
            this.HasOptional(c => c.System).WithMany().HasForeignKey(c => c.System_Id).WillCascadeOnDelete(false);
            this.Property(c => c.ChangeObject_Id).IsOptional();

            this.HasOptional(c => c.ChangeObject)
                .WithMany()
                .HasForeignKey(c => c.ChangeObject_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.InventoryNumber).IsOptional().HasMaxLength(100);
            this.Property(c => c.ChangeGroup_Id).IsOptional();
            
            this.HasOptional(c => c.ChangeGroup)
                .WithMany()
                .HasForeignKey(c => c.ChangeGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangeCategory_Id).IsOptional();
            
            this.HasOptional(c => c.ChangeCategory)
                .WithMany()
                .HasForeignKey(c => c.ChangeCategory_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangeDescription).IsOptional();
            this.Property(c => c.ChangeBenefits).IsOptional();
            this.Property(c => c.ChangeConsequence).IsOptional();
            this.Property(c => c.ChangeImpact).IsOptional().HasMaxLength(100);
            this.Property(c => c.DesiredDate).IsOptional();
            this.Property(c => c.ChangePriority_Id).IsOptional();

            this.HasOptional(c => c.ChangePriority)
                .WithMany()
                .HasForeignKey(c => c.ChangePriority_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.WorkingGroup_Id).IsOptional();

            this.HasOptional(c => c.WorkingGroup)
                .WithMany()
                .HasForeignKey(c => c.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.User_Id).IsOptional();
            this.Property(c => c.PlannedReadyDate).IsOptional();
            this.Property(c => c.Verified).IsRequired();
            this.Property(c => c.Approval).IsRequired();
            this.Property(c => c.ApprovalDate).IsOptional();
            this.Property(c => c.ApprovedByUser_Id).IsOptional();

            this.HasOptional(c => c.ApprovedByUser)
                .WithMany()
                .HasForeignKey(c => c.ApprovedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangeExplanation).IsOptional();
            this.Property(c => c.ResponsibleUser_Id).IsOptional();

            this.HasOptional(c => c.ResponsibleUser)
                .WithMany()
                .HasForeignKey(c => c.ResponsibleUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangeSolution).IsOptional();
            this.Property(c => c.TotalCost).IsRequired();
            this.Property(c => c.YearlyCost).IsRequired();
            this.Property(c => c.Currency).IsOptional();
            this.Property(c => c.TimeEstimatesHours).IsOptional();
            this.Property(c => c.ChangeRisk).IsOptional();
            this.Property(c => c.ImplementationPlan).IsRequired();
            this.Property(c => c.RecoveryPlan).IsRequired();
            this.Property(c => c.ChangeRecommendation).IsOptional();
            this.Property(c => c.AnalysisApproval).IsRequired();
            this.Property(c => c.AnalysisApprovalDate).IsOptional();
            this.Property(c => c.AnalysisApprovedByUser_Id).IsOptional();

            this.HasOptional(c => c.AnalysisApprovedByUser)
                .WithMany()
                .HasForeignKey(c => c.AnalysisApprovedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.Prioritisation).IsOptional();
            this.Property(c => c.ScheduledStartTime).IsOptional();
            this.Property(c => c.ScheduledEndTime).IsOptional();
            this.Property(c => c.ImplementationStatus_Id).IsOptional();

            this.HasOptional(c => c.ImplementationStatus)
                .WithMany()
                .HasForeignKey(c => c.ImplementationStatus_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.RealStartDate).IsOptional();
            this.Property(c => c.ImplementationPlanUsed).IsRequired();
            this.Property(c => c.RecoveryPlanUsed).IsRequired();
            this.Property(c => c.BuildImplemented).IsRequired();
            this.Property(c => c.ChangeDeviation).IsOptional();
            this.Property(c => c.FinishingDate).IsOptional();
            this.Property(c => c.ImplementationReady).IsRequired();
            this.Property(c => c.ChangeEvaluation).IsOptional();
            this.Property(c => c.EvaluationReady).IsRequired();
            this.Property(c => c.RegLanguage_Id).IsRequired();

            this.HasRequired(c => c.RegLanguage)
                .WithMany()
                .HasForeignKey(c => c.RegLanguage_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.SourceCase_Id).IsOptional();

            this.HasOptional(c => c.SourceCase)
                .WithMany()
                .HasForeignKey(c => c.SourceCase_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.RSS).IsRequired();
            this.Property(c => c.CreatedDate).IsRequired();
            this.Property(c => c.ChangedByUser_Id).IsOptional();
            
            this.HasOptional(c => c.ChangedByUser)
                .WithMany()
                .HasForeignKey(c => c.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ChangedDate).IsRequired();

            this.ToTable("tblChange");
        }
    }
}

