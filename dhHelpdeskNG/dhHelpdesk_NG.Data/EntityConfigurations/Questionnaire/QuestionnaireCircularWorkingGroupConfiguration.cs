using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    internal sealed class QuestionnaireCircularWorkingGroupConfiguration : EntityTypeConfiguration<QuestionnaireCircularWorkingGroupEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularWorkingGroupConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.QuestionnaireCircularId).IsRequired();
            this.Property(c => c.WorkingGroupId).IsRequired();
            
            this.HasRequired(c => c.QuestionnaireCircular)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireCircularId)
                .WillCascadeOnDelete(false);

            this.HasRequired(c => c.WorkingGroup)
                .WithMany()
                .HasForeignKey(c => c.WorkingGroupId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireCircularWorkingGroups");
        }

        #endregion
    }
}
