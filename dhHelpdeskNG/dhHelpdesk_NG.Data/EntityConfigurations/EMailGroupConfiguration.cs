namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class EMailGroupConfiguration : EntityTypeConfiguration<EmailGroupEntity>
    {
        internal EMailGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Members).IsRequired().HasMaxLength(3000);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("EMailGroup");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblemailgroup");
        }
    }
}
