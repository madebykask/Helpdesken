namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FormFieldValueConfiguration : EntityTypeConfiguration<FormFieldValue>
    {
        internal FormFieldValueConfiguration()
        {
         



            this.Property(x => x.Case_Id).IsRequired();
            this.Property(x => x.FormField_Id).IsRequired();
            this.Property(x => x.FormFieldValues).IsRequired();



            this.ToTable("tblformfieldvalue");
        }
    }
}
