namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;

    public sealed class QuestionnaireResultConfiguration: EntityTypeConfiguration<QuestionnaireResultEntity>
    {
        internal QuestionnaireResultConfiguration()
        {
            this.HasKey(q => q.Id);
            this.Property(q => q.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(q => q.QuestionnaireCircularParticId).IsRequired().HasColumnName("QuestionnaireCircularPartic_Id");
            this.Property(q => q.Anonymous).IsRequired();            
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);            
            
            this.HasOptional(c => c.QuestionnaireCircularPart).WithMany().HasForeignKey(c => c.QuestionnaireCircularParticId).WillCascadeOnDelete(false);

            this.ToTable("tblQuestionnaireResult");
        }
    }
}
