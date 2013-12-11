using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class FormFieldConfiguration : EntityTypeConfiguration<FormField>
    {
        internal FormFieldConfiguration()
        {
            HasKey(x => x.Id);



            Property(x => x.Form_Id).IsRequired();
            Property(x => x.FormFieldName).IsRequired();
            Property(x => x.FormFieldType).IsRequired();
            Property(x => x.X).IsRequired();
            Property(x => x.Y).IsRequired();
            Property(x => x.PageNumber).IsRequired();

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;

            ToTable("tblformfield");
        }
    }
}
