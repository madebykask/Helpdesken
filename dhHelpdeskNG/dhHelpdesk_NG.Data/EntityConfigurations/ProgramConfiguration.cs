namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ProgramConfiguration : EntityTypeConfiguration<Program>
    {
        internal ProgramConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.List).IsOptional().HasMaxLength(200).HasColumnName("EMailList");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Program");
            this.Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("Show");           
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblprogram");
        }
    }
}
