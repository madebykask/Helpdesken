using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class GlobalSettingConfiguration : EntityTypeConfiguration<GlobalSetting>
    {
        internal GlobalSettingConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.DefaultLanguage)
                .WithMany()
                .HasForeignKey(x => x.DefaultLanguage_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ApplicationName).IsRequired().HasMaxLength(50);
            Property(x => x.AttachedFileFolder).IsRequired().HasMaxLength(50);
            Property(x => x.DBType).IsRequired();
            Property(x => x.DBVersion).IsRequired().HasMaxLength(10);
            Property(x => x.DefaultLanguage_Id).IsRequired();
            Property(x => x.EMailBodyEncoding).IsOptional().HasMaxLength(20);
            Property(x => x.FullTextSearch).IsRequired();
            Property(x => x.GlobalStartPage).IsRequired();
            Property(x => x.LockCaseToWorkingGroup).IsRequired();
            Property(x => x.LoginOption).IsRequired();
            Property(x => x.OrderNumber).IsRequired();
            Property(x => x.PDFPrint).IsRequired();
            Property(x => x.PDFPrintPassword).IsOptional().HasMaxLength(20);
            Property(x => x.PDFPrintUserName).IsOptional().HasMaxLength(50);
            Property(x => x.ServerName).IsRequired().HasMaxLength(50);
            Property(x => x.ServerPort).IsRequired();
            Property(x => x.SMTPServer).IsRequired().HasMaxLength(20);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblglobalsettings");
        }
    }
}
