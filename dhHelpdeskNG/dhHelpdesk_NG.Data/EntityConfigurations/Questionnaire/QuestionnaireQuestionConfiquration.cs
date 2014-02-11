namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuestionConfiquration : EntityTypeConfiguration<QuestionnaireQuestionEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuestionConfiquration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Questionnaire_Id).IsRequired();
            
            this.HasRequired(q => q.Questionnaire)
                .WithMany()
                .HasForeignKey(q => q.Questionnaire_Id)
                .WillCascadeOnDelete(false);
            
            this.Property(q => q.QuestionnaireQuestionNumber).IsRequired().HasMaxLength(10);
            this.Property(q => q.QuestionnaireQuestion).IsRequired().HasMaxLength(1000);
            this.Property(q => q.ShowNote).IsRequired();
            this.Property(q => q.NoteText).IsRequired().HasMaxLength(1000);
            this.Property(q => q.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            //this.Property(q => q.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            this.ToTable("tblQuestionnaireQuestion");
        }

        #endregion
    }
}