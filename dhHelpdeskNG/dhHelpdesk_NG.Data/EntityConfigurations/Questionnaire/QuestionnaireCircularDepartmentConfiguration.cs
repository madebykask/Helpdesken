using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    internal sealed class QuestionnaireCircularDepartmentConfiguration : EntityTypeConfiguration<QuestionnaireCircularDepartmentEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularDepartmentConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.QuestionnaireCircularId).IsRequired();
            this.Property(c => c.DepartmentId).IsRequired();

            this.HasRequired(c => c.QuestionnaireCircular)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireCircularId)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.Department)
                .WithMany()
                .HasForeignKey(c => c.DepartmentId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireCircularDepartments");
        }

        #endregion
    }
}
