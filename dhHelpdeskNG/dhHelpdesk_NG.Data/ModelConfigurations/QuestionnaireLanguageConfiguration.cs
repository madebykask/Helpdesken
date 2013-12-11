using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class QuestionnaireLanguageConfiguration : EntityTypeConfiguration<QuestionnaireLanguage>
    {
        internal QuestionnaireLanguageConfiguration()
        {
            HasKey(x => new { x.Questionnaire_Id, x.Language_Id });

            Property(x => x.Language_Id).IsRequired();
            Property(x => x.Questionnaire_Id).IsRequired();
            Property(x => x.QuestionnaireDescription).IsRequired();
            Property(x => x.QuestionnaireName).IsRequired().HasMaxLength(1000);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            ToTable("tblquestionnaire_tbllanguage");
        }
    }
}
