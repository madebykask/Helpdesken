namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuestionResultConfiguration : EntityTypeConfiguration<QuestionnaireQuestionResultEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuestionResultConfiguration()
        {
            this.HasKey(r => r.Id);            
            this.Property(r => r.QuestionnaireResult_Id).IsRequired();

            this.HasRequired(r => r.QuestionnaireResult)
                .WithMany()
                .HasForeignKey(r => r.QuestionnaireResult_Id)
                .WillCascadeOnDelete(false);

            this.Property(r => r.QuestionnaireQuestionOptionId).IsRequired();

            this.HasRequired(r => r.QuestionnaireQuestionOption)
                .WithMany()
                .HasForeignKey(r => r.QuestionnaireQuestionOptionId)
                .WillCascadeOnDelete(false);

            this.Property(r => r.QuestionnaireQuestionNote).IsRequired();

            this.ToTable("tblQuestionnaireQuestionResult");
        }

        #endregion
    }
}