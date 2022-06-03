using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;
namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public sealed class CaseSolutionCategoryLanguageConfiguration : EntityTypeConfiguration<CaseSolutionCategoryLanguageEntity>
    {
        internal CaseSolutionCategoryLanguageConfiguration()
        {
            this.HasKey(fl => new { fl.Category_Id, fl.Language_Id });

            this.Property(fl => fl.Category_Id).IsRequired();
            this.Property(fl => fl.Language_Id).IsRequired();
            this.Property(fl => fl.CaseSolutionCategoryName).IsRequired().HasMaxLength(100).HasColumnName("CaseSolutionCategory"); 

            this.HasRequired(f => f.CaseSolutionCategory)
                .WithMany(f => f.CaseSolutionCategoryLanguages)
                .HasForeignKey(f => f.Category_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseSolutionCategory_tblLanguage");
        }
    }
}
