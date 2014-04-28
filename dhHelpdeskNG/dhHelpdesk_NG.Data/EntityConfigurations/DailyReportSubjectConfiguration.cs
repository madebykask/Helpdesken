namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class DailyReportSubjectConfiguration : EntityTypeConfiguration<DailyReportSubject>
    {
        internal DailyReportSubjectConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.Subject).IsRequired().HasMaxLength(50);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbldailyreportsubject");
        }
    }
}
