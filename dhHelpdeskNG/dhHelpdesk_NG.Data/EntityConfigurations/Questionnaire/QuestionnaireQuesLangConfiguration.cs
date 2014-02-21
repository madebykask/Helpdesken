namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuesLangConfiguration : EntityTypeConfiguration<QuestionnaireQuesLangEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuesLangConfiguration()
        {
            this.HasKey(q => new { q.QuestionnaireQuestion_Id, q.Language_Id });
            this.Property(q => q.QuestionnaireQuestion_Id).IsRequired();
            this.Property(q => q.Language_Id).IsRequired();
     
            this.HasRequired(q => q.Language)
                .WithMany()
                .HasForeignKey(q => q.Language_Id)
                .WillCascadeOnDelete(false);

            this.Property(q => q.QuestionnaireQuestion).IsRequired().HasMaxLength(1000);
            this.Property(q => q.NoteText).IsRequired().HasMaxLength(1000);
            this.Property(q => q.CreatedDate);
            this.Property(q => q.ChangedDate);//.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            this.HasRequired(q => q.QuestionnaireQuestions)
                .WithMany()
                .HasForeignKey(q => q.QuestionnaireQuestion_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireQues_tblLang");
        }

        #endregion
    }
}