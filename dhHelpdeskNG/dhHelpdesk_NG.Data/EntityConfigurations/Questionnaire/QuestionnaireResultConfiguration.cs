namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireResultConfiguration : EntityTypeConfiguration<QuestionnaireResultEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireResultConfiguration()
        {
            this.HasKey(r => r.Id);            
            this.Property(r => r.QuestionnaireCircularPartic_Id).IsRequired();

            this.HasRequired(r => r.QuestionnaireCircularPart)
                .WithMany()
                .HasForeignKey(r => r.QuestionnaireCircularPartic_Id)
                .WillCascadeOnDelete(false);

            this.Property(r => r.Anonymous).IsRequired();
            this.Property(r => r.CreatedDate);

            this.ToTable("tblQuestionnaireResult");
        }

        #endregion
    }
}