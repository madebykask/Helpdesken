using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    internal sealed class QuestionnaireCircularProductAreaConfiguration : EntityTypeConfiguration<QuestionnaireCircularProductAreaEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularProductAreaConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.QuestionnaireCircularId).IsRequired();
            this.Property(c => c.ProductAreaId).IsRequired();

            this.HasRequired(c => c.QuestionnaireCircular)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireCircularId)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.ProductArea)
                .WithMany()
                .HasForeignKey(c => c.ProductAreaId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireCircularProductAreas");
        }

        #endregion
    }
}
