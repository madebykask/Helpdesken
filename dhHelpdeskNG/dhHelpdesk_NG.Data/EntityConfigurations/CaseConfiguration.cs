namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseConfiguration : EntityTypeConfiguration<Case>
    {
        internal CaseConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Customer)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Department)
                .WithMany(d => d.Cases)
                .HasForeignKey(c => c.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.RegLanguage)
                .WithMany(l => l.Cases)
                .HasForeignKey(c => c.RegLanguage_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Urgency)
                .WithMany()
                .HasForeignKey(c => c.Urgency_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.Category_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Problem).WithMany().HasForeignKey(c => c.Problem_Id);

            this.Property(x => x.AgreedDate).IsOptional();
            this.Property(x => x.ApprovedDate).IsOptional();
            this.Property(x => x.ApprovedBy_User_Id).IsRequired();
            this.Property(x => x.Available).IsRequired().HasMaxLength(100);
            this.Property(x => x.Caption).IsRequired().HasMaxLength(60);
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
            this.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(20);
            this.Property(x => x.InventoryType).IsRequired().HasMaxLength(50);
            this.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.IpAddress).IsOptional().HasMaxLength(15);
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.OtherCost).IsRequired();
            this.Property(x => x.PersonsCellphone).IsRequired().HasMaxLength(30).HasColumnName("Persons_CellPhone");
            this.Property(x => x.PersonsEmail).IsRequired().HasMaxLength(100).HasColumnName("Persons_EMail");
            this.Property(x => x.PersonsName).IsRequired().HasMaxLength(50).HasColumnName("Persons_Name");
            this.Property(x => x.PersonsPhone).IsRequired().HasMaxLength(30).HasColumnName("Persons_Phone");
            this.Property(x => x.Performer_User_Id).IsRequired();
            this.Property(x => x.Priority_Id).IsOptional();
            this.Property(x => x.Place).IsRequired().HasMaxLength(100);
            this.Property(x => x.PlanDate).IsOptional();
            this.Property(x => x.ProductAreaSetDate).IsOptional();
            this.Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(50);
            this.Property(x => x.RegistrationSource).IsRequired();
            this.Property(x => x.RegLanguage_Id).IsRequired();
            this.Property(x => x.RegUserDomain).IsOptional().HasMaxLength(20);
            this.Property(x => x.RegUserId).IsOptional();
            this.Property(x => x.RelatedCaseNumber).IsRequired();
            this.Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            this.Property(x => x.SMS).IsRequired();
            this.Property(x => x.SolutionRate).IsOptional().HasMaxLength(10);
            this.Property(x => x.UserCode).IsOptional().HasMaxLength(13);
            this.Property(x => x.Verified).IsRequired();
            this.Property(x => x.VerifiedDescription).IsOptional().HasMaxLength(200);
            this.Property(x => x.WatchDate).IsOptional();
            this.Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.User_Id).IsRequired();
            this.Property(x => x.OU_Id).IsOptional();
            this.Property(x => x.CaseType_Id).IsOptional();
            this.Property(x => x.ProductArea_Id).IsOptional();
            this.Property(x => x.System_Id).IsOptional();
            this.Property(x => x.Urgency_Id).IsOptional();
            this.Property(x => x.Impact_Id).IsOptional();
            this.Property(x => x.Category_Id).IsOptional();
            this.Property(x => x.Problem_Id).IsOptional();
            this.Property(x => x.Project_Id).IsOptional();
            this.Property(x => x.Change_Id).IsOptional();
            this.Property(x => x.Supplier_Id).IsOptional();
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.Property(x => x.CaseResponsibleUser_Id).IsOptional();
            this.Property(x => x.Status_Id).IsOptional();
            this.Property(x => x.StateSecondary_Id).IsOptional();
            this.Property(x => x.ChangeByUser_Id).IsOptional();
            this.Property(x => x.Unread).IsRequired().HasColumnName("Status");

            this.ToTable("tblcase");
        }
    }
}
