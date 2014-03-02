namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FormFieldConfiguration : EntityTypeConfiguration<FormField>
    {
        internal FormFieldConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Form_Id).IsRequired();
            this.Property(x => x.FormFieldName).IsRequired();
            this.Property(x => x.FormFieldType).IsRequired();
            this.Property(x => x.X).IsRequired();
            this.Property(x => x.Y).IsRequired();
            this.Property(x => x.PageNumber).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;
            this.ToTable("tblformfield");
        }
    }
}
