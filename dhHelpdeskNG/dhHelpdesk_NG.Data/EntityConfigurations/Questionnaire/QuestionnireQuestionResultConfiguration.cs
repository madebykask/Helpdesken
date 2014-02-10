namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnireQuestionResultConfiguration:EntityTypeConfiguration<QuestionnaireQuestionResultEntity>
    {
        internal QuestionnireQuestionResultConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(q => q.QuestionnaireResultId).IsRequired().HasColumnName("QuestionnaireResult_Id");
            this.Property(q => q.QuestionnaireQuestionOptionId).IsRequired().HasColumnName("QuestionnaireQuestionOption_Id");
            this.Property(x => x.QuestionnaireQuestionNote).IsRequired();            
            
            this.HasOptional(c => c.QuestionnaireQuestionOption).WithMany().HasForeignKey(c => c.QuestionnaireQuestionOptionId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.QuestionnaireResult).WithMany().HasForeignKey(c => c.QuestionnaireResultId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireQuestionResult");
        }
    }
}
