namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseSolutionConfiguration : EntityTypeConfiguration<CaseSolution>
    {
        internal CaseSolutionConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.CaseSolutionCategory)
                .WithMany()
                .HasForeignKey(x => x.CaseSolutionCategory_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.CaseSolutionSchedule)
               .WithRequiredPrincipal()
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CaseType)
                .WithMany()
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CaseWorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.CaseWorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.Category_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FinishingCause)
                .WithMany()
                .HasForeignKey(x => x.FinishingCause_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.PerformerUser)
                .WithMany()
                .HasForeignKey(x => x.PerformerUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ProductArea)
                .WithMany()
                .HasForeignKey(x => x.ProductArea_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.Project_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);


			this.HasOptional(x => x.SplitToCaseSolution)
				.WithMany()
				.HasForeignKey(x => x.SplitToCaseSolution_Id)
				.WillCascadeOnDelete(false);

            this.HasMany(cs => cs.ExtendedCaseForms).WithMany(ecf => ecf.CaseSolutions).Map(m =>
             {
                 m.MapLeftKey("CaseSolution_Id");
                 m.MapRightKey("ExtendedCaseForms_Id");
                 m.ToTable("tblCaseSolution_ExtendedCaseForms");
             });

            this.Property(x => x.Caption).IsRequired().HasMaxLength(60);
            this.Property(x => x.CaseSolutionCategory_Id).IsOptional();
            this.Property(x => x.CaseWorkingGroup_Id).IsOptional();
            this.Property(x => x.CaseType_Id).IsOptional();
            this.Property(x => x.Category_Id).IsOptional();
            this.Property(x => x.Department_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsOptional();
            this.Property(x => x.FinishingCause_Id).IsOptional();
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseSolutionName");
            this.Property(x => x.NoMailToNotifier).IsRequired();
            this.Property(x => x.PerformerUser_Id).IsOptional();
            this.Property(x => x.Priority_Id).IsOptional();
            this.Property(x => x.ProductArea_Id).IsOptional();
            this.Property(x => x.Project_Id).IsOptional();
            this.Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            this.Property(x => x.Text_External).IsRequired();
            this.Property(x => x.Text_Internal).IsRequired();
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.TemplatePath).IsOptional();
            this.Property(x => x.ShowInSelfService).IsRequired();
            this.Property(x => x.OrderNum).IsOptional();
            this.Property(x => x.FormGUID).IsOptional();
            this.Property(x => x.PersonsName);
            this.Property(x => x.PersonsPhone);
            this.Property(x => x.PersonsCellPhone);
            this.Property(x => x.PersonsEmail);
            this.Property(x => x.Region_Id).IsOptional();
            this.Property(x => x.OU_Id).IsOptional();
            this.Property(x => x.Place);
            this.Property(x => x.UserCode).IsOptional().HasMaxLength(50);
            this.Property(x => x.System_Id).IsOptional();
            this.Property(x => x.Urgency_Id).IsOptional();
            this.Property(x => x.Impact_Id).IsOptional();
            this.Property(x => x.InvoiceNumber);
            this.Property(x => x.ReferenceNumber);
            this.Property(x => x.Status_Id).IsOptional();
            this.Property(x => x.StateSecondary_Id).IsOptional();
            this.Property(x => x.Verified).IsRequired();
            this.Property(x => x.VerifiedDescription);
            this.Property(x => x.SolutionRate);
            this.Property(x => x.InventoryNumber);
            this.Property(x => x.InventoryType);
            this.Property(x => x.InventoryLocation);
            this.Property(x => x.Supplier_Id).IsOptional();
            this.Property(x => x.SMS).IsRequired();
            this.Property(x => x.AgreedDate);
            this.Property(x => x.Available);
            this.Property(x => x.Cost).IsRequired();
            this.Property(x => x.OtherCost).IsRequired();
            this.Property(x => x.Currency);
            this.Property(x => x.ContactBeforeAction).IsRequired();
            this.Property(x => x.Problem_Id).IsOptional();
            this.Property(x => x.Change_Id).IsOptional();
            this.Property(x => x.WatchDate);
            this.Property(x => x.FinishingDate);
            this.Property(x => x.FinishingDescription);
            this.Property(x => x.UpdateNotifierInformation).IsOptional();
            this.Property(x => x.Status).IsRequired();
            this.Property(x => x.CostCentre).IsOptional();

            this.Property(x => x.IsAbout_CostCentre).IsOptional();
            this.Property(x => x.IsAbout_Department_Id).IsOptional();
            this.Property(x => x.IsAbout_OU_Id).IsOptional();
            this.Property(x => x.IsAbout_PersonsCellPhone).IsOptional();
            this.Property(x => x.IsAbout_PersonsEmail).IsOptional();
            this.Property(x => x.IsAbout_PersonsName).IsOptional();
            this.Property(x => x.IsAbout_PersonsPhone).IsOptional();
            this.Property(x => x.IsAbout_Place).IsOptional();
            this.Property(x => x.IsAbout_Region_Id).IsOptional();
            this.Property(x => x.IsAbout_ReportedBy).IsOptional();
            this.Property(x => x.IsAbout_UserCode).IsOptional().HasMaxLength(50);
            this.Property(x => x.ShowOnCaseOverview).IsRequired();
            this.Property(x => x.ShowInsideCase).IsRequired();
            this.Property(x => x.SetCurrentUserAsPerformer).IsOptional();
            this.Property(x => x.SetCurrentUsersWorkingGroup).IsOptional();            
            this.Property(x => x.OverWritePopUp).IsRequired();
            this.Property(x => x.ConnectedButton).IsOptional();
            this.Property(x => x.SaveAndClose).IsOptional();
            this.Property(x => x.SortOrder).IsRequired();
            this.Property(x => x.ShortDescription).IsOptional().HasMaxLength(100);
            this.Property(x => x.Information).IsOptional();
            this.Property(x => x.DefaultTab).IsRequired().HasMaxLength(100);
            this.Property(x => x.CaseSolutionDescription).IsOptional();
            this.Property(x => x.ValidateOnChange).IsOptional().HasMaxLength(100);

            this.ToTable("tblcasesolution");
        }
    }
}
