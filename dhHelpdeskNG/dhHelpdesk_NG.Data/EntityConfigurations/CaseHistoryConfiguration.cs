namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseHistoryConfiguration : EntityTypeConfiguration<CaseHistory>
    {
        internal CaseHistoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Case)
                .WithMany(x => x.CaseHistories)
                .HasForeignKey(x => x.Case_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Category)
                            .WithMany()
                            .HasForeignKey(x => x.Category_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.StateSecondary)
                            .WithMany()
                            .HasForeignKey(x => x.StateSecondary_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Priority)
                            .WithMany()
                            .HasForeignKey(x => x.Priority_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                            .WithMany()
                            .HasForeignKey(x => x.Department_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Region)
                            .WithMany()
                            .HasForeignKey(x => x.Region_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.IsAbout_Region)
                            .WithMany()
                            .HasForeignKey(x => x.IsAbout_Region_Id)
                            .WillCascadeOnDelete(false);


            this.HasOptional(x => x.OU)
                            .WithMany()
                            .HasForeignKey(x => x.OU_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.IsAbout_OU)
                            .WithMany()
                            .HasForeignKey(x => x.IsAbout_OU_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.IsAbout_Department)
                            .WithMany()
                            .HasForeignKey(x => x.IsAbout_Department_Id)
                            .WillCascadeOnDelete(false);


            this.HasOptional(c => c.Problem)
                            .WithMany()
                            .HasForeignKey(c => c.Problem_Id);

            this.HasOptional(c => c.Project)
                            .WithMany()
                            .HasForeignKey(c => c.Project_Id);

            this.HasRequired(x => x.CaseType)
                            .WithMany()
                            .HasForeignKey(x => x.CaseType_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ProductArea)
                            .WithMany()
                            .HasForeignKey(x => x.ProductArea_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CausingPart)
                            .WithMany()
                            .HasForeignKey(x => x.CausingPartId)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.UserPerformer)
                            .WithMany()
                            .HasForeignKey(x => x.Performer_User_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.UserResponsible)
                            .WithMany()
                            .HasForeignKey(x => x.CaseResponsibleUser_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.WorkingGroup)
                            .WithMany()
                            .HasForeignKey(x => x.WorkingGroup_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.System)
                            .WithMany()
                            .HasForeignKey(x => x.System_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Urgency)
                            .WithMany()
                            .HasForeignKey(x => x.Urgency_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Impact)
                            .WithMany()
                            .HasForeignKey(x => x.Impact_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Status)
                            .WithMany()
                            .HasForeignKey(x => x.Status_Id)
                            .WillCascadeOnDelete(false);

            this.HasOptional(x => x.RegistrationSourceCustomer)
                           .WithMany()
                           .HasForeignKey(x => x.RegistrationSourceCustomer_Id)
                           .WillCascadeOnDelete(false);            

            this.Property(x => x.AgreedDate).IsOptional();
            this.Property(x => x.Performer_User_Id).IsOptional();
            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.ApprovedDate).IsOptional();
            this.Property(x => x.ApprovedBy_User_Id).IsOptional();
            this.Property(x => x.Available).IsRequired().HasMaxLength(100);
            this.Property(x => x.Caption).IsRequired().HasMaxLength(100);
            this.Property(x => x.CaseHistoryGUID).IsRequired();
            this.Property(x => x.CaseNumber).IsRequired();
            this.Property(x => x.ContactBeforeAction).IsRequired();
            this.Property(x => x.Cost).IsRequired();
            this.Property(x => x.Currency).IsOptional().HasMaxLength(10);
            this.Property(x => x.Description).IsRequired();
            this.Property(x => x.ExternalTime).IsRequired();
            this.Property(x => x.FinishingDate).IsOptional();
            this.Property(x => x.FinishingDescription).IsOptional().HasMaxLength(200);
            this.Property(x => x.FollowUpDate).IsOptional();
            this.Property(x => x.InventoryLocation).IsRequired().HasMaxLength(100);
            this.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(60);
            this.Property(x => x.InventoryType).IsRequired().HasMaxLength(50);
            this.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.IpAddress).IsRequired().HasMaxLength(15);
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.OtherCost).IsRequired();
            this.Property(x => x.PersonsCellphone).IsRequired().HasMaxLength(50).HasColumnName("Persons_CellPhone");
            this.Property(x => x.PersonsEmail).IsRequired().HasMaxLength(100).HasColumnName("Persons_EMail");
            this.Property(x => x.PersonsName).IsRequired().HasMaxLength(50).HasColumnName("Persons_Name");
            this.Property(x => x.PersonsPhone).IsRequired().HasMaxLength(50).HasColumnName("Persons_Phone");
            this.Property(x => x.Place).IsRequired().HasMaxLength(100);
            this.Property(x => x.PlanDate).IsOptional();
            this.Property(x => x.ProductAreaSetDate).IsOptional();
            this.Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.RegistrationSource).IsRequired();
            this.Property(x => x.RelatedCaseNumber).IsRequired();
            this.Property(x => x.RegUserDomain).IsOptional().HasMaxLength(20);
            this.Property(x => x.RegUserId).IsOptional();
            this.Property(x => x.ReportedBy).IsOptional().HasMaxLength(200);
            this.Property(x => x.SMS).IsRequired();
            this.Property(x => x.SolutionRate).IsOptional().HasMaxLength(10);
            this.Property(x => x.UserCode).IsOptional().HasMaxLength(50);
            this.Property(x => x.Verified).IsOptional();
            this.Property(x => x.VerifiedDescription).IsOptional().HasMaxLength(200);
            this.Property(x => x.WatchDate).IsOptional();
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Unread).IsRequired().HasColumnName("Status");
            this.Property(x => x.Problem_Id).IsOptional();
            this.Property(x => x.CausingPartId).IsOptional();
            this.Property(x => x.DefaultOwnerWG_Id).IsOptional();
            this.Property(x => x.CaseFile).IsOptional();
            this.Property(x => x.LogFile).IsOptional();
            this.Property(x => x.CaseLog).IsOptional();
            this.Property(x => x.ClosingReason).IsOptional().HasMaxLength(300);
            this.Property(x => x.CostCentre).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_Persons_Name).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_ReportedBy).IsOptional().HasMaxLength(200);
            this.Property(x => x.IsAbout_Persons_Phone).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_UserCode).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_Department_Id).IsOptional();
            this.Property(x => x.CreatedByApp).IsOptional();
            this.Property(x => x.LatestSLACountDate).IsOptional();
            this.Property(x => x.LeadTime).IsRequired();
            this.Property(x => x.ActionLeadTime).IsRequired();
            this.Property(x => x.ActionExternalTime).IsRequired();
            this.Property(x => x.CaseExtraFollowers).IsRequired();
            this.Property(x => x.IsAbout_Persons_EMail).IsOptional().HasMaxLength(100);
            this.Property(x => x.IsAbout_Persons_CellPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_Region_Id).IsOptional();
            this.Property(x => x.IsAbout_CostCentre).IsOptional().HasMaxLength(50);
            this.Property(x => x.IsAbout_Place).IsOptional().HasMaxLength(100);

            this.ToTable("tblcasehistory");
        }
    }
}
