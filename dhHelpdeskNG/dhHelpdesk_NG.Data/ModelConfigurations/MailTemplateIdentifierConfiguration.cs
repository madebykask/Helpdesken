using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class MailTemplateIdentifierConfiguration : EntityTypeConfiguration<MailTemplateIdentifier>
    {
        internal MailTemplateIdentifierConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Code).IsRequired().HasMaxLength(10).HasColumnName("IdentifierCode");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("IdentifierName");
            Property(x => x.SortOrder).IsRequired();
            Property(x => x.Source).IsRequired();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblmailtemplateidentifier");
        }            
    }
}
