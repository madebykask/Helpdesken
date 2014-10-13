namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Questionnaire;

    internal sealed class QuestionnaireCircularConfiguration : EntityTypeConfiguration<QuestionnaireCircularEntity>
    {
        #region Constructors and Destructors

        internal QuestionnaireCircularConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Questionnaire_Id).IsRequired();

            this.HasRequired(c => c.Questionnaire)
                .WithMany()
                .HasForeignKey(c => c.Questionnaire_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(s => s.QuestionnaireCircularPartEntities)
                .WithRequired(s => s.QuestionnaireCircular)
                .HasForeignKey(s => s.QuestionnaireCircular_Id);

            this.Property(c => c.CircularName).IsRequired().HasMaxLength(50);
            this.Property(c => c.Status).IsRequired();

            // this.Property(c => c.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblQuestionnaireCircular");
        }

        #endregion
    }
}