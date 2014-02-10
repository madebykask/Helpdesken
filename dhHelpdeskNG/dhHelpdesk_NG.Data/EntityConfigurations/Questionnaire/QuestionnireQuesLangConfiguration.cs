namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;
    public sealed  class QuestionnireQuesLangConfiguration:EntityTypeConfiguration<QuestionnaireQuesLangEntity>
    {
        internal QuestionnireQuesLangConfiguration()
        {
            this.HasKey(q => new {q.QuestionnaireQuestionId,q.LanguageId});
            this.Property(q => q.QuestionnaireQuestionId).IsRequired().HasColumnName("QuestionnaireQuestion_Id");
            this.Property(q => q.LanguageId).IsRequired().HasColumnName("Language_Id");
            this.Property(x => x.QuestionnaireQuestion).IsRequired().HasMaxLength(1000);
            this.Property(x => x.NoteText).IsRequired().HasMaxLength(1000);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            

            this.HasOptional(c => c.QuestionnaireQuestions).WithMany().HasForeignKey(c => c.QuestionnaireQuestionId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.Language).WithMany().HasForeignKey(c => c.LanguageId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireQues_tblLang");
        }

    }
}
