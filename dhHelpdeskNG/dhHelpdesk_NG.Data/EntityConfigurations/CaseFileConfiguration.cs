namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseFileConfiguration : EntityTypeConfiguration<CaseFile>
    {
        internal CaseFileConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Case)
                .WithMany(c => c.CaseFiles)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.FileName).IsRequired().HasMaxLength(200);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.UserId).IsOptional();

            this.ToTable("tblcasefile");
        }
    }
}
