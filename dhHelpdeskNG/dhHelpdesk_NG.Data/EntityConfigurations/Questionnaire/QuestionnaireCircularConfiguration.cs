namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;
    
    public sealed class QuestionnaireCircularConfiguration: EntityTypeConfiguration<QuestionnaireCircularEntity>
    {
        internal QuestionnaireCircularConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.QuestionnaireId).IsRequired().HasColumnName("Questionnaire_Id");
            this.Property(x => x.CircularName).IsRequired().HasMaxLength(50);
            this.Property(x => x.Status).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasOptional(c => c.Questionnaire).WithMany().HasForeignKey(c => c.QuestionnaireId).WillCascadeOnDelete(false);            

            this.ToTable("tblQuestionnaireCircular");
        }
    }

}
