using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;
namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public sealed class CaseSolutionLanguageConfiguration : EntityTypeConfiguration<CaseSolutionLanguageEntity>
    {
        internal CaseSolutionLanguageConfiguration()
        {
            this.HasKey(fl => new { fl.CaseSolution_Id, fl.Language_Id });

            this.Property(fl => fl.CaseSolution_Id).IsRequired();
            this.Property(fl => fl.Language_Id).IsRequired();
            this.Property(fl => fl.CaseSolutionName).IsRequired().HasMaxLength(50);
            this.Property(fl => fl.ShortDescription).IsRequired().HasMaxLength(100);
            this.Property(fl => fl.Information).IsRequired().HasMaxLength(1000);

            this.HasRequired(f => f.CaseSolution)
                .WithMany(f => f.CaseSolutionLanguages)
                .HasForeignKey(f => f.CaseSolution_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseSolution_tblLanguage");
        }
    }
}
