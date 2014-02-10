namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnaireQuesOpLangConfiguration:EntityTypeConfiguration<QuestionnaireQuesOpLangEntity>
    {
        internal QuestionnaireQuesOpLangConfiguration()
        {
            this.Property(q => q.QuestionnaireQuestionOptionId).IsRequired().HasColumnName("QuestionnaireQuestionOption_Id");
            this.Property(q => q.LanguageId).IsRequired().HasColumnName("Language_Id");
            this.HasKey(q=> new{q.QuestionnaireQuestionOptionId, q.LanguageId});
            this.Property(q => q.QuestionnaireQuestionOption).IsRequired().HasMaxLength(100);            
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            
            
            this.HasOptional(c => c.QuestionnaireQuestionOptions).WithMany().HasForeignKey(c => c.QuestionnaireQuestionOptionId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.Language).WithMany().HasForeignKey(c => c.LanguageId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireQuesOp_tblLang");
        }

    }
}
