namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Domain;

    public class GlobalSettingConfiguration : EntityTypeConfiguration<GlobalSetting>
    {
        internal GlobalSettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.DefaultLanguage)
                .WithMany()
                .HasForeignKey(x => x.DefaultLanguage_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ApplicationName).IsRequired().HasMaxLength(50);
            this.Property(x => x.AttachedFileFolder).IsRequired().HasMaxLength(200);
            this.Property(x => x.DBType).IsRequired();
            this.Property(x => x.DBVersion).IsRequired().HasMaxLength(10);
            this.Property(x => x.HelpdeskDBVersion).IsOptional().HasMaxLength(20);
            this.Property(x => x.DefaultLanguage_Id).IsRequired();
            this.Property(x => x.EMailBodyEncoding).IsOptional().HasMaxLength(20);
            this.Property(x => x.FullTextSearch).IsRequired();
            this.Property(x => x.GlobalStartPage).IsRequired();
            this.Property(x => x.LockCaseToWorkingGroup).IsRequired();
            this.Property(x => x.LoginOption).IsRequired();
            this.Property(x => x.OrderNumber).IsRequired();
            this.Property(x => x.PDFPrint).IsRequired();
            this.Property(x => x.PDFPrintPassword).IsOptional().HasMaxLength(20);
            this.Property(x => x.PDFPrintUserName).IsOptional().HasMaxLength(50);
            this.Property(x => x.ServerName).IsRequired().HasMaxLength(50);
            this.Property(x => x.ServerPort).IsRequired();
            this.Property(x => x.SMTPServer).IsRequired().HasMaxLength(50);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CaseLockTimer).IsRequired();
            this.Property(x => x.CaseLockBufferTime).IsRequired();
            this.Property(x => x.CaseLockExtendTime).IsRequired();
            this.Property(x => x.ExtendedCasePath).IsOptional().HasMaxLength(500);
            this.Property(x => x.MultiCustomersSearch).IsRequired();

			this.Property(x => x.PerformanceLogActive).IsRequired();
			this.Property(x => x.PerformanceLogFrequency).IsRequired();
			this.Property(x => x.PerformanceLogSettingsCache).IsRequired();
            this.Property(x => x.NewAdvancedSearch).IsRequired();

            this.ToTable("tblglobalsettings");
        }
    }
}
