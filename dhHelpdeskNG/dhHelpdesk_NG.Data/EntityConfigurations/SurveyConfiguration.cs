namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class SurveyConfiguration : EntityTypeConfiguration<Survey>
    {
        internal SurveyConfiguration()
        {
            this.HasKey(x => x.Id);
            this.ToTable("tblsurvey");
        }
    }
}
