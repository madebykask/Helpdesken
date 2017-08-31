namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class RegionConfiguration : EntityTypeConfiguration<Region>
    {
        internal RegionConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Region");
            this.Property(x => x.SearchKey).IsOptional().HasMaxLength(20);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.SearchKey).IsOptional();
            this.Property(x => x.Code).IsOptional().HasMaxLength(20);
            this.Property(x => x.RegionGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //this.Property(x => x.SynchronizedDate).IsOptional();
            this.Property(x => x.LanguageId).IsOptional();
            this.ToTable("tblregion");
        }
    }
}
