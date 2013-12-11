using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseFileConfiguration : EntityTypeConfiguration<CaseFile>
    {
        internal CaseFileConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Case)
                .WithMany(c => c.CaseFiles)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.FileName).IsRequired().HasMaxLength(200);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasefile");
        }
    }
}
