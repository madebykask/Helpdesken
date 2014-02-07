namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseQuestionCategoryConfiguration : EntityTypeConfiguration<CaseQuestionCategory>
    {
        internal CaseQuestionCategoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.CaseQuestionHeader)
                .WithMany(c => c.CaseQuestionCategories)
                .HasForeignKey(c => c.CaseQuestionHeader_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.QuestionGroup)
                .WithMany(c => c.CaseQuestionCategories)
                .HasForeignKey(c => c.QuestionGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.CaseQuestionCategoryGUID).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("CaseQuestionCategory");
            this.Property(x => x.Pos).IsOptional().HasMaxLength(5);
            this.Property(x => x.Weight).IsOptional();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcasequestioncategory");
        }
    }
}
