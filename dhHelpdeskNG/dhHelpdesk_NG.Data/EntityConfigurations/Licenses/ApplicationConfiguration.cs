namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        internal ApplicationConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.Name).HasColumnName("Application").IsRequired().HasMaxLength(100);
            this.Property(a => a.ChangedDate).IsRequired();
            this.Property(a => a.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); 

            this.HasRequired(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.Customer_Id);


            this.ToTable("tblApplication");       
        }
    }
}