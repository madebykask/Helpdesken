namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class QuestionnaireLanguageConfiguration : EntityTypeConfiguration<QuestionnaireLanguage>
    {
        internal QuestionnaireLanguageConfiguration()
        {
            this.HasKey(x => new { x.Questionnaire_Id, x.Language_Id });

            this.Property(x => x.Language_Id).IsRequired();
            this.Property(x => x.Questionnaire_Id).IsRequired();
            this.Property(x => x.QuestionnaireDescription).IsRequired();
            this.Property(x => x.QuestionnaireName).IsRequired().HasMaxLength(1000);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblquestionnaire_tbllanguage");
        }
    }
}
