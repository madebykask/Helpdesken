using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseConfiguration : EntityTypeConfiguration<Case>
    {
        internal CaseConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Customer)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.Department)
                .WithMany(d => d.Cases)
                .HasForeignKey(c => c.Department_Id)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.RegLanguage)
                .WithMany(l => l.Cases)
                .HasForeignKey(c => c.RegLanguage_Id)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.Urgency)
                .WithMany()
                .HasForeignKey(c => c.Urgency_Id)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.Category_Id)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.Problem).WithMany().HasForeignKey(c => c.Problem_Id);

            Property(x => x.AgreedDate).IsOptional();
            Property(x => x.ApprovedDate).IsOptional();
            Property(x => x.ApprovedBy_User_Id).IsRequired();
            Property(x => x.Available).IsRequired().HasMaxLength(100);
            Property(x => x.Caption).IsRequired().HasMaxLength(60);
            Property(x => x.CaseGUID).IsRequired();
            Property(x => x.CaseNumber).IsRequired();
            Property(x => x.ContactBeforeAction).IsRequired();
            Property(x => x.Cost).IsRequired();
            Property(x => x.Currency).IsOptional().HasMaxLength(10);
            Property(x => x.Deleted).IsRequired();
            Property(x => x.Description).IsRequired();
            Property(x => x.ExternalTime).IsRequired();
            Property(x => x.FinishingDate).IsOptional();
            Property(x => x.FinishingDescription).IsOptional().HasMaxLength(200);
            Property(x => x.FollowUpDate).IsOptional();
            Property(x => x.InventoryLocation).IsRequired().HasMaxLength(100);
            Property(x => x.InventoryNumber).IsRequired().HasMaxLength(20);
            Property(x => x.InventoryType).IsRequired().HasMaxLength(50);
            Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            Property(x => x.IpAddress).IsOptional().HasMaxLength(15);
            Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            Property(x => x.OtherCost).IsRequired();
            Property(x => x.PersonsCellphone).IsRequired().HasMaxLength(30).HasColumnName("Persons_CellPhone");
            Property(x => x.PersonsEmail).IsRequired().HasMaxLength(100).HasColumnName("Persons_EMail");
            Property(x => x.PersonsName).IsRequired().HasMaxLength(50).HasColumnName("Persons_Name");
            Property(x => x.PersonsPhone).IsRequired().HasMaxLength(30).HasColumnName("Persons_Phone");
            Property(x => x.Performer_User_Id).IsRequired();
            Property(x => x.Priority_Id).IsOptional();
            Property(x => x.Place).IsRequired().HasMaxLength(100);
            Property(x => x.PlanDate).IsOptional();
            Property(x => x.ProductAreaSetDate).IsOptional();
            Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(50);
            Property(x => x.RegistrationSource).IsRequired();
            Property(x => x.RegLanguage_Id).IsRequired();
            Property(x => x.RegUserDomain).IsOptional().HasMaxLength(20);
            Property(x => x.RegUserId).IsOptional();
            Property(x => x.RelatedCaseNumber).IsRequired();
            Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            Property(x => x.SMS).IsRequired();
            Property(x => x.SolutionRate).IsOptional().HasMaxLength(10);
            Property(x => x.UserCode).IsOptional().HasMaxLength(13);
            Property(x => x.Verified).IsRequired();
            Property(x => x.VerifiedDescription).IsOptional().HasMaxLength(200);
            Property(x => x.WatchDate).IsOptional();
            Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.User_Id).IsRequired();
            Property(x => x.OU_Id).IsOptional();
            Property(x => x.CaseType_Id).IsOptional();
            Property(x => x.ProductArea_Id).IsOptional();
            Property(x => x.System_Id).IsOptional();
            Property(x => x.Urgency_Id).IsOptional();
            Property(x => x.Impact_Id).IsOptional();
            Property(x => x.Category_Id).IsOptional();
            Property(x => x.Problem_Id).IsOptional();
            Property(x => x.Project_Id).IsOptional();
            Property(x => x.Change_Id).IsOptional();
            Property(x => x.Supplier_Id).IsOptional();
            Property(x => x.WorkingGroup_Id).IsOptional();
            Property(x => x.CaseResponsibleUser_Id).IsOptional();
            Property(x => x.Status_Id).IsOptional();
            Property(x => x.StateSecondary_Id).IsOptional();
            Property(x => x.ChangeByUser_Id).IsOptional();
            Property(x => x.Unread).IsRequired().HasColumnName("Status");

            ToTable("tblcase");
        }
    }
}
