namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuestionOptionConfiguration :
        EntityTypeConfiguration<QuestionnaireQuestionOptionEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuestionOptionConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.QuestionnaireQuestion_Id).IsRequired();
            this.HasRequired(o => o.QuestionnaireQuestion)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireQuestion_Id)
                .WillCascadeOnDelete(false);

            this.Property(o => o.QuestionnaireQuestionOptionPos).IsRequired();
            this.Property(o => o.QuestionnaireQuestionOption).IsRequired().HasMaxLength(100);
            this.Property(o => o.OptionValue).IsRequired();
            
            this.Property(o => o.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(o => o.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblQuestionnaireQuestionOption");
        }

        #endregion
    }
}