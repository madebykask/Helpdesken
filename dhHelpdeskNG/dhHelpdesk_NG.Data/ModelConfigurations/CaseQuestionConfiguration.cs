using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseQuestionConfiguration : EntityTypeConfiguration<CaseQuestion>
    {
        internal CaseQuestionConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.CaseQuestionCategory)
                .WithMany(c => c.CaseQuestions)
                .HasForeignKey(c => c.CaseQuestionCategory_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Answer).IsRequired();
            Property(x => x.Note).IsOptional().HasMaxLength(100);
            Property(x => x.Question).IsOptional().HasMaxLength(2000);
            Property(x => x.QuestionHelp).IsOptional().HasMaxLength(500);
            Property(x => x.Weight).IsRequired();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasequestion");
        }
    }
}
