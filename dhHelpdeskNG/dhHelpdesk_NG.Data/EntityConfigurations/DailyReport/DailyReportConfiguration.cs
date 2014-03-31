using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations.DailyReport
{
    public sealed class DailyReportConfiguration : EntityTypeConfiguration<Domain.DailyReport>
    {
        internal DailyReportConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.User_Id).IsRequired();
            Property(x => x.DailyReportSubject_Id).IsRequired();
            Property(x => x.DailyReportText).IsRequired().HasMaxLength(2000);
            Property(x => x.MailSent).IsRequired();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ChangedDate).IsRequired();

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);
            HasRequired(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.User_Id)
               .WillCascadeOnDelete(false);
            HasRequired(x => x.DailyReportSubject)
               .WithMany()
               .HasForeignKey(x => x.DailyReportSubject_Id)
               .WillCascadeOnDelete(false);

            ToTable("dbo.tblDailyReport");
        }
    }
}