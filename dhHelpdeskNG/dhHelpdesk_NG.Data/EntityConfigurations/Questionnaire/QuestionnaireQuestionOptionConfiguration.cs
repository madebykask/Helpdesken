namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireQuestionOptionConfiguration :
        EntityTypeConfiguration<QuestionnaireQuestionOptionEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireQuestionOptionConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.QuestionnaireQuestion_Id).IsRequired();

            this.HasRequired(o => o.QuestionnaireQuestion)
                .WithMany()
                .HasForeignKey(c => c.QuestionnaireQuestion_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(s => s.QuestionnaireQuestionResultEntities)
                .WithRequired(s => s.QuestionnaireQuestionOption)
                .HasForeignKey(s => s.QuestionnaireQuestionOption_Id);

            this.HasMany(s => s.QuestionnaireQuesOpLangEntities)
                .WithRequired(s => s.QuestionnaireQuestionOptions)
                .HasForeignKey(s => s.QuestionnaireQuestionOption_Id);

            this.Property(o => o.QuestionnaireQuestionOptionPos).IsRequired();
            this.Property(o => o.QuestionnaireQuestionOption).IsRequired().HasMaxLength(100);
            this.Property(o => o.OptionValue).IsRequired();
			this.Property(o => o.IconId).HasMaxLength(200);
			this.Property(o => o.IconSrc).HasMaxLength(2048);

			this.Property(o => o.CreatedDate);
            this.Property(o => o.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblQuestionnaireQuestionOption");
        }

        #endregion
    }
}