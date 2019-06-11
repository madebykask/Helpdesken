namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    /// <summary>
    /// Represents user settins for the "Case overview" grid.
    /// </summary>
    public class CaseSettingConfiguration : EntityTypeConfiguration<CaseSettings>
    {
        internal CaseSettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Customer_Id).HasColumnName("CustomerId");
            this.Property(x => x.ColOrder).IsRequired();
            this.Property(x => x.Line).IsRequired();
            this.Property(x => x.MinWidth).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("tblCaseName");
            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.UserGroup).IsRequired();
            this.Property(x => x.Type).IsRequired();
            this.Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ColStyle).IsOptional();

            this.ToTable("tblCaseSettings");
        }
    }
}
