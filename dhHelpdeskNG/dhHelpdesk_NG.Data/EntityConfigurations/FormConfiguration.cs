namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class FormConfiguration : EntityTypeConfiguration<Form>
    {
        internal FormConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.FormGUID).IsRequired();
            this.Property(x => x.FormName).IsRequired();
            this.Property(x => x.FormPath).IsOptional();
            this.Property(x => x.FormLogo).IsOptional();
            this.Property(x => x.FormHeader).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.ExternalPage).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Modal).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.ToTable("tblform");
        }
    }
}
