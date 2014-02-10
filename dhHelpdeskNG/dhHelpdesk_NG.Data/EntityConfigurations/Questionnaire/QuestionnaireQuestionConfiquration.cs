namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnaireQuestionConfiquration:EntityTypeConfiguration<QuestionnaireQuestionEntity>
    {
        internal QuestionnaireQuestionConfiquration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.QuestionnaireId).IsRequired().HasColumnName("Questionnaire_Id");
            this.Property(x => x.QuestionnaireQuestionNumber).IsRequired().HasMaxLength(10);
            this.Property(x => x.QuestionnaireQuestion).IsRequired().HasMaxLength(1000);
            this.Property(x => x.ShowNote).IsRequired();
            this.Property(x => x.NoteText).IsRequired().HasMaxLength(1000);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasOptional(c => c.Questionnaire).WithMany().HasForeignKey(c => c.QuestionnaireId).WillCascadeOnDelete(false);            

            this.ToTable("tblQuestionnaireQuestion");
        }
    }
}
