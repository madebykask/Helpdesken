using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    internal sealed class QuestionnaireCircularCaseTypeConfiguration : EntityTypeConfiguration<QuestionnaireCircularCaseTypeEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularCaseTypeConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.QuestionnaireCircularId).IsRequired();
            this.Property(c => c.CaseTypeId).IsRequired();

            this.HasRequired(c => c.QuestionnaireCircular)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireCircularId)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.CaseType)
                .WithMany()
                .HasForeignKey(c => c.CaseTypeId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireCircularCaseTypes");
        }

        #endregion
    }
}
