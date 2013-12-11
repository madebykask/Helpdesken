using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class FormConfiguration : EntityTypeConfiguration<Form>
    {
        internal FormConfiguration()
        {
            HasKey(x => x.Id);

           

            Property(x => x.FormGUID).IsRequired();
            Property(x => x.FormName).IsRequired();
            Property(x => x.FormPath).IsOptional();
            Property(x => x.FormLogo).IsOptional();
            Property(x => x.FormHeader).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;

            ToTable("tblform");
        }
    }
}
