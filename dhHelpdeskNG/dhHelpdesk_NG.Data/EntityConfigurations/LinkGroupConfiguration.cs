namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LinkGroupConfiguration : EntityTypeConfiguration<LinkGroup>
    {
        internal LinkGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);


            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.LinkGroupName).IsRequired().HasMaxLength(50).HasColumnName("LinkGroup"); 
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbllinkgroup");
        }
    }
}