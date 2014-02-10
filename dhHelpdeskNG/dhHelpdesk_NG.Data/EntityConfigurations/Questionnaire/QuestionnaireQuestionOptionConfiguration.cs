
namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnaireQuestionOptionConfiguration : EntityTypeConfiguration<QuestionnaireQuestionOptionEntity>
    {
        internal QuestionnaireQuestionOptionConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(q => q.QuestionnaireQuestionId).IsRequired().HasColumnName("QuestionnaireQuestion_Id");
            this.Property(q => q.QuestionnaireQuestionOptionPos).IsRequired();
            this.Property(q => q.QuestionnaireQuestionOption).IsRequired().HasMaxLength(100);
            this.Property(q => q.OptionValue).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            
            
            this.HasOptional(c => c.QuestionnaireQuestion).WithMany().HasForeignKey(c => c.QuestionnaireQuestionId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireQuestionOption");
        }
    }
}
