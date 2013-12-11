using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseQuestionCategoryConfiguration : EntityTypeConfiguration<CaseQuestionCategory>
    {
        internal CaseQuestionCategoryConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.CaseQuestionHeader)
                .WithMany(c => c.CaseQuestionCategories)
                .HasForeignKey(c => c.CaseQuestionHeader_Id)
                .WillCascadeOnDelete(false);

            HasOptional(c => c.QuestionGroup)
                .WithMany(c => c.CaseQuestionCategories)
                .HasForeignKey(c => c.QuestionGroup_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.CaseQuestionCategoryGUID).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("CaseQuestionCategory");
            Property(x => x.Pos).IsOptional().HasMaxLength(5);
            Property(x => x.Weight).IsOptional();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasequestioncategory");
        }
    }
}
