namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireConfiguration : EntityTypeConfiguration<QuestionnaireEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.QuestionnaireName).IsRequired().HasMaxLength(100);
            this.Property(q => q.QuestionnaireDescription);
	        this.Property(q => q.Identifier).HasMaxLength(100);
            this.Property(q => q.Customer_Id).IsOptional();
            this.Property(q => q.ExcludeAdministrators).IsOptional();
            this.Property(q => q.UseBase64Images).IsOptional();

            this.HasRequired(q => q.Customer)
                .WithMany()
                .HasForeignKey(q => q.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(s => s.QuestionnaireQuestionEntities)
                .WithRequired(s => s.Questionnaire)
                .HasForeignKey(s => s.Questionnaire_Id);

            this.HasMany(s => s.QuestionnaireLanguageEntities)
                .WithRequired(s => s.Questionnaire)
                .HasForeignKey(s => s.Questionnaire_Id);

            this.Property(q => q.CreatedDate);
            this.Property(q => q.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblQuestionnaire");
        }

        #endregion
    }
}