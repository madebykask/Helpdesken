namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class InfoTextConfiguration : EntityTypeConfiguration<InfoText>
    {
        internal InfoTextConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.Language_Id).IsOptional();
            this.Property(x => x.Name).IsRequired().HasColumnName("InfoText");
            this.Property(x => x.Type).IsOptional().HasColumnName("InfoTextType");
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblinfotext");
        }
    }
}
