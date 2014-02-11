namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuesOpLangConfiguration : EntityTypeConfiguration<QuestionnaireQuesOpLangEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuesOpLangConfiguration()
        {
            this.HasKey(q => new { q.QuestionnaireQuestionOption_Id, q.Language_Id });
            this.Property(q => q.QuestionnaireQuestionOption_Id).IsRequired();

            this.HasRequired(c => c.QuestionnaireQuestionOptions)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireQuestionOption_Id)
                .WillCascadeOnDelete(false);            
            
            this.Property(q => q.Language_Id).IsRequired();
            
            this.HasRequired(c => c.Language)
                .WithMany()
                .HasForeignKey(c => c.Language_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(q => q.QuestionnaireQuestionOption).IsRequired().HasMaxLength(100);
            this.Property(x => x.CreatedDate);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblQuestionnaireQuesOp_tblLang");
        }

        #endregion
    }
}