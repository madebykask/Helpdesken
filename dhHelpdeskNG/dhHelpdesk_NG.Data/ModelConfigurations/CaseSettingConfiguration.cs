using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseSettingConfiguration : EntityTypeConfiguration<CaseSettings>
    {
        internal CaseSettingConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Customer_Id).IsRequired().HasColumnName("CustomerId");
            Property(x => x.ColOrder).IsRequired();
            Property(x => x.Line).IsRequired();
            Property(x => x.MinWidth).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("tblCaseName");
            Property(x => x.User_Id).IsOptional();
            Property(x => x.UserGroup).IsRequired();
            Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            ToTable("tblCaseSettings");
        }
    }
}
