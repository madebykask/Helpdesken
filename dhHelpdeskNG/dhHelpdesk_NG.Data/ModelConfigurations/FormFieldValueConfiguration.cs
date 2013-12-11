using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class FormFieldValueConfiguration : EntityTypeConfiguration<FormFieldValue>
    {
        internal FormFieldValueConfiguration()
        {
         



            Property(x => x.Case_Id).IsRequired();
            Property(x => x.FormField_Id).IsRequired();
            Property(x => x.FormFieldValues).IsRequired();



            ToTable("tblformfieldvalue");
        }
    }
}
