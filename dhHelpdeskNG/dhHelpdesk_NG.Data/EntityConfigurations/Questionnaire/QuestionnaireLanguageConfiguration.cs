namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireLanguageConfiguration : EntityTypeConfiguration<QuestionnaireLanguageEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireLanguageConfiguration()
        {
            this.HasKey(l => new { l.Questionnaire_Id, l.Language_Id });

            this.HasRequired(l => l.Questionnaire)
                .WithMany()
                .HasForeignKey(l => l.Questionnaire_Id)
                .WillCascadeOnDelete(false);

            this.Property(l => l.Language_Id).IsRequired();

            this.HasRequired(l => l.Language)
                .WithMany()
                .HasForeignKey(l => l.Language_Id).WillCascadeOnDelete(false);

            this.Property(l => l.QuestionnaireDescription);
            this.Property(l => l.QuestionnaireName).IsRequired().HasMaxLength(100);
            this.Property(l => l.CreatedDate);
            this.Property(l => l.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblquestionnaire_tbllanguage");
        }

        #endregion
    }
}