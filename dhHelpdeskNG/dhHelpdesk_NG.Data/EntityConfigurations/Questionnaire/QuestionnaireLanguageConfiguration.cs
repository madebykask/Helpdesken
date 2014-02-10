namespace DH.Helpdesk.Dal.EntityConfigurations.Questionnaire
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Questionnaire;
    using System.ComponentModel.DataAnnotations;
    

    public sealed class QuestionnaireLanguageConfiguration : EntityTypeConfiguration<QuestionnaireLanguageEntity>
    {
        internal QuestionnaireLanguageConfiguration()
        {           
            this.Property(x => x.QuestionnaireId).IsRequired().HasColumnName("Questionnaire_Id");            
            this.Property(x => x.LanguageId).IsRequired().HasColumnName("Language_Id");            
            this.HasKey(x => new { x.QuestionnaireId, x.LanguageId });
            this.Property(x => x.QuestionnaireDescription).IsRequired();
            this.Property(x => x.QuestionnaireName).IsRequired().HasMaxLength(100);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasOptional(c => c.Questionnaire).WithMany().HasForeignKey(c => c.QuestionnaireId).WillCascadeOnDelete(false);
            this.HasOptional(c => c.Language).WithMany().HasForeignKey(c => c.LanguageId).WillCascadeOnDelete(false);            

            this.ToTable("tblquestionnaire_tbllanguage");
        }
    }
}
