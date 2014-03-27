namespace DH.Helpdesk.Dal.EntityConfigurations.MailTemplates
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.MailTemplates;

    public sealed class MailTemplateIdentifierConfiguration : EntityTypeConfiguration<MailTemplateIdentifierEntity>
    {
        internal MailTemplateIdentifierConfiguration()
        {
            this.HasKey(i => i.Id);
            this.Property(i => i.IdentifierName).IsRequired().HasMaxLength(50);
            this.Property(i => i.IdentifierCode).IsRequired().HasMaxLength(10);
            this.Property(i => i.Source).IsRequired();
            this.Property(i => i.SortOrder).IsRequired();

            this.ToTable("tblMailTemplateIdentifier");
        }
    }
}