namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseConfiguration : EntityTypeConfiguration<Case>
    {
        internal CaseConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.Category_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.Customer)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Department)
                .WithMany(d => d.Cases)
                .HasForeignKey(c => c.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.ProductArea)
                .WithMany()
                .HasForeignKey(c => c.ProductArea_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.RegLanguage)
                .WithMany(l => l.Cases)
                .HasForeignKey(c => c.RegLanguage_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Urgency)
                .WithMany()
                .HasForeignKey(c => c.Urgency_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Workinggroup)
                .WithMany()
                .HasForeignKey(c => c.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.LastChangedByUser)
                           .WithMany()
                           .HasForeignKey(c => c.ChangeByUser_Id)
                           .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Administrator)
                           .WithMany()
                           .HasForeignKey(c => c.Performer_User_Id)
                           .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Priority)
                           .WithMany()
                           .HasForeignKey(c => c.Priority_Id)
                           .WillCascadeOnDelete(false);

            this.HasOptional(c => c.StateSecondary)
                           .WithMany()
                           .HasForeignKey(c => c.StateSecondary_Id)
                           .WillCascadeOnDelete(false);

            this.HasRequired(c => c.CaseType)
                .WithMany()
                .HasForeignKey(c => c.CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Problem)
                .WithMany()
                .HasForeignKey(c => c.Problem_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Region)
                .WithMany()
                .HasForeignKey(c => c.Region_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.CausingPart)
                .WithMany()
                .HasForeignKey(c => c.CausingPartId)
                .WillCascadeOnDelete(false);

            this.HasOptional(s => s.IsAbout) 
                .WithRequired(ad => ad.Case)
                .WillCascadeOnDelete(true);

            this.Property(x => x.AgreedDate).IsOptional();
            this.Property(x => x.ApprovedDate).IsOptional();
            this.Property(x => x.ApprovedBy_User_Id).IsOptional();
            this.Property(x => x.Available).IsRequired().HasMaxLength(100);
            this.Property(x => x.Caption).IsRequired().HasMaxLength(100);
            this.Property(x => x.CaseGUID).IsRequired();
            this.Property(x => x.CaseNumber).IsRequired();
            this.Property(x => x.ContactBeforeAction).IsRequired();
            this.Property(x => x.Cost).IsRequired();
            this.Property(x => x.Currency).IsOptional().HasMaxLength(10);
            this.Property(x => x.Deleted).IsRequired();
            this.Property(x => x.Description).IsRequired();
            this.Property(x => x.ExternalTime).IsRequired();
            this.Property(x => x.FinishingDate).IsOptional();
            this.Property(x => x.FinishingDescription).IsOptional().HasMaxLength(200);
            this.Property(x => x.FollowUpDate).IsOptional();
            this.Property(x => x.InventoryLocation).IsRequired().HasMaxLength(100);
            this.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(60);
            this.Property(x => x.InventoryType).IsRequired().HasMaxLength(50);
            this.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.IpAddress).IsOptional().HasMaxLength(15);
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.OtherCost).IsRequired();
            this.Property(x => x.PersonsCellphone).IsRequired().HasMaxLength(50).HasColumnName("Persons_CellPhone");
            this.Property(x => x.PersonsEmail).IsRequired().HasMaxLength(100).HasColumnName("Persons_EMail");
            this.Property(x => x.PersonsName).IsRequired().HasMaxLength(50).HasColumnName("Persons_Name");
            this.Property(x => x.PersonsPhone).IsRequired().HasMaxLength(50).HasColumnName("Persons_Phone");
            this.Property(x => x.Performer_User_Id).IsOptional();
            this.Property(x => x.Priority_Id).IsOptional();
            this.Property(x => x.Place).IsRequired().HasMaxLength(100);
            this.Property(x => x.PlanDate).IsOptional();
            this.Property(x => x.ProductAreaSetDate).IsOptional();
            this.Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.RegistrationSource).IsRequired();
            this.Property(x => x.RegLanguage_Id).IsRequired();
            this.Property(x => x.RegUserDomain).IsOptional().HasMaxLength(20);
            this.Property(x => x.RegUserId).IsOptional().HasMaxLength(200);
            this.Property(x => x.RegUserName).IsOptional();
            this.Property(x => x.RelatedCaseNumber).IsRequired();
            this.Property(x => x.ReportedBy).IsOptional().HasMaxLength(200);
            this.Property(x => x.SMS).IsRequired();
            this.Property(x => x.SolutionRate).IsOptional().HasMaxLength(10);
            this.Property(x => x.UserCode).IsOptional().HasMaxLength(50);
            this.Property(x => x.Verified).IsRequired();
            this.Property(x => x.VerifiedDescription).IsOptional().HasMaxLength(200);
            this.Property(x => x.WatchDate).IsOptional();
            this.Property(x => x.ChangeTime).IsRequired();
            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CaseNumber).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.Ou)
                .WithMany()
                .HasForeignKey(x => x.OU_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.CaseType_Id).IsOptional();
            this.Property(x => x.ProductArea_Id).IsOptional();
            this.HasOptional(x => x.System)
                .WithMany()
                .HasForeignKey(x => x.System_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.Urgency_Id).IsOptional();
            this.HasOptional(x => x.Impact)
                .WithMany()
                .HasForeignKey(x => x.Impact_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.Category_Id).IsOptional();
            this.Property(x => x.Problem_Id).IsOptional();
            this.Property(x => x.Project_Id).IsOptional();
            this.Property(x => x.Change_Id).IsOptional();
            this.Property(x => x.MovedFromCustomer_Id).IsOptional();
            this.HasOptional(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.Supplier_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.HasOptional(x => x.CaseResponsibleUser)
                .WithMany()
                .HasForeignKey(x => x.CaseResponsibleUser_Id)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.Status_Id)
                .WillCascadeOnDelete(false);
            this.Property(x => x.StateSecondary_Id).IsOptional();
            this.Property(x => x.ChangeByUser_Id).IsOptional();
            this.Property(x => x.Unread).IsRequired().HasColumnName("Status");
            this.Property(x => x.CausingPartId).IsOptional();
            this.Property(x => x.DefaultOwnerWG_Id).IsOptional();

            this.Property(x => x.Status_Id).IsOptional();

            this.HasMany(x => x.Logs)
                .WithRequired(x => x.Case)
                .HasForeignKey(x => x.Case_Id);

            this.HasMany(x => x.Mail2Tickets)
                .WithOptional(x => x.Case)
                .HasForeignKey(x => x.Case_Id);

            this.Property(x => x.RegistrationSourceCustomer_Id).IsOptional();
            this.HasOptional(c => c.RegistrationSourceCustomer)
                .WithMany()
                .HasForeignKey(c => c.RegistrationSourceCustomer_Id)
                .WillCascadeOnDelete(true);

            this.Property(x => x.LatestSLACountDate).IsOptional();

            this.HasMany(x => x.InvoiceRows)
                .WithOptional(x => x.Case)
                .HasForeignKey(x => x.Case_Id);

            this.HasMany(x => x.CaseFollowers)
                .WithRequired(x => x.Case)
                .HasForeignKey(x => x.Case_Id);

            this.HasOptional(x => x.CaseSolution)
               .WithMany()
               .HasForeignKey(x => x.CaseSolution_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CurrentCaseSolution)
               .WithMany()
               .HasForeignKey(x => x.CurrentCaseSolution_Id)
               .WillCascadeOnDelete(false);

            //this.Property(x => x.ActiveTab).IsOptional().HasMaxLength(100);

            //this.HasMany(c => c.ExtendedCaseDatas)
            //    .WithMany(ecd => ecd.Cases)                
            //    .Map(m =>
            // {
            //     m.MapLeftKey("Case_Id");
            //     m.MapRightKey("ExtendedCaseData_Id");
            //     m.ToTable("tblCase_ExtendedCaseData");
            // });


            this.ToTable("tblcase");
        }
    }
}
