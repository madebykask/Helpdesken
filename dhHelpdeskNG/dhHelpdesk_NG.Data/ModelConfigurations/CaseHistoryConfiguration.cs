using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseHistoryConfiguration : EntityTypeConfiguration<CaseHistory>
    {
        internal CaseHistoryConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Case)
                .WithMany(x => x.CaseHistories)
                .HasForeignKey(x => x.Case_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Category)
                            .WithMany()
                            .HasForeignKey(x => x.Category_Id)
                            .WillCascadeOnDelete(false);

            HasOptional(x => x.StateSecondary)
                            .WithMany()
                            .HasForeignKey(x => x.StateSecondary_Id)
                            .WillCascadeOnDelete(false);

            HasOptional(x => x.Priority)
                            .WithMany()
                            .HasForeignKey(x => x.Priority_Id)
                            .WillCascadeOnDelete(false);

            HasOptional(x => x.Department)
                            .WithMany()
                            .HasForeignKey(x => x.Department_Id)
                            .WillCascadeOnDelete(false);

            //HasOptional(x => x.UserPerformer)
            //    .WithMany()
            //    .HasForeignKey(x => x.Performer_User_Id)
            //    .WillCascadeOnDelete(false);

            Property(x => x.AgreedDate).IsOptional();
            Property(x => x.ApprovedDate).IsOptional();
            Property(x => x.Available).IsRequired().HasMaxLength(100);
            Property(x => x.Caption).IsRequired().HasMaxLength(60);
            Property(x => x.CaseHistoryGUID).IsRequired();
            Property(x => x.CaseNumber).IsRequired();
            Property(x => x.ContactBeforeAction).IsRequired();
            Property(x => x.Cost).IsRequired();
            Property(x => x.Currency).IsOptional().HasMaxLength(10);
            Property(x => x.Description).IsRequired();
            Property(x => x.ExternalTime).IsRequired();
            Property(x => x.FinishingDate).IsOptional();
            Property(x => x.FinishingDescription).IsOptional().HasMaxLength(200);
            Property(x => x.FollowUpDate).IsOptional();
            Property(x => x.InventoryLocation).IsRequired().HasMaxLength(100);
            Property(x => x.InventoryNumber).IsRequired().HasMaxLength(20);
            Property(x => x.InventoryType).IsRequired().HasMaxLength(50);
            Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            Property(x => x.IpAddress).IsRequired().HasMaxLength(15);
            Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            Property(x => x.MOSS_DocId).IsOptional();
            Property(x => x.MOSS_DocUrl).IsRequired().HasMaxLength(300);
            Property(x => x.MOSS_DocUrlText).IsRequired().HasMaxLength(100);
            Property(x => x.MOSS_DocVersion).IsRequired();
            Property(x => x.OtherCost).IsRequired();
            Property(x => x.PersonsCellphone).IsRequired().HasMaxLength(30).HasColumnName("Persons_CellPhone");
            Property(x => x.PersonsEmail).IsRequired().HasMaxLength(100).HasColumnName("Persons_EMail");
            Property(x => x.PersonsName).IsRequired().HasMaxLength(50).HasColumnName("Persons_Name");
            Property(x => x.PersonsPhone).IsRequired().HasMaxLength(30).HasColumnName("Persons_Phone");
            Property(x => x.Place).IsRequired().HasMaxLength(100);
            Property(x => x.PlanDate).IsOptional();
            Property(x => x.ProductAreaSetDate).IsOptional();
            Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(50);
            Property(x => x.RegistrationSource).IsRequired();
            Property(x => x.RelatedCaseNumber).IsRequired();
            Property(x => x.RegUserDomain).IsOptional().HasMaxLength(20);
            Property(x => x.RegUserId).IsOptional();
            Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            Property(x => x.SMS).IsRequired();
            Property(x => x.SolutionRate).IsOptional().HasMaxLength(10);
            Property(x => x.UserCode).IsOptional().HasMaxLength(13);
            Property(x => x.Verified).IsRequired();
            Property(x => x.VerifiedDescription).IsOptional().HasMaxLength(200);
            Property(x => x.WatchDate).IsOptional();
            Property(x => x.CreatedDate).IsRequired(); 
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Unread).IsRequired().HasColumnName("Status");

            ToTable("tblcasehistory");
        }
    }
}
