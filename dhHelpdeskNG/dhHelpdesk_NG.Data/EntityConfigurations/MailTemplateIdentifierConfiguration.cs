namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class MailTemplateIdentifierConfiguration : EntityTypeConfiguration<MailTemplateIdentifier>
    {
        internal MailTemplateIdentifierConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Code).IsRequired().HasMaxLength(10).HasColumnName("IdentifierCode");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("IdentifierName");
            this.Property(x => x.SortOrder).IsRequired();
            this.Property(x => x.Source).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblmailtemplateidentifier");
        }            
    }
}
