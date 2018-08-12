using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    public class CaseSectionsConfiguration : EntityTypeConfiguration<CaseSection>
    {
        internal CaseSectionsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.IsEditCollapsed).IsRequired();
            this.Property(x => x.IsNewCollapsed).IsRequired();
            this.Property(x => x.SectionType).IsRequired();
            this.Property(x => x.UpdatedDate).IsOptional();

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblCaseSections");
        }
    }

    public class CaseSectionsLanguageConfiguration : EntityTypeConfiguration<CaseSectionLanguage>
    {
        internal CaseSectionsLanguageConfiguration()
        {
            this.HasKey(x => new { x.CaseSection_Id, x.Language_Id });

            this.HasRequired(x => x.CaseSection)
                .WithMany(x => x.CaseSectionLanguages)
                .HasForeignKey(x => x.CaseSection_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(x => x.Label).IsOptional().HasMaxLength(50);

            this.ToTable("tblCaseSections_tblLang");
        }
    }
}
