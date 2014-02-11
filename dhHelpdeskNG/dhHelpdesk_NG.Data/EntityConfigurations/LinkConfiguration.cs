namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LinkConfiguration : EntityTypeConfiguration<Link>
    {
        internal LinkConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Customer)
              .WithMany()
              .HasForeignKey(x => x.Customer_Id)
              .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.LinkUsers)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Link_Id")
                    .MapRightKey("User_Id")
                    .ToTable("tblLink_tblUsers");
                });


            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.Document_Id).IsOptional();
            this.Property(x => x.OpenInNewWindow).IsRequired();
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.URLAddress).IsRequired().HasMaxLength(100);
            this.Property(x => x.URLName).IsRequired().HasMaxLength(50);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbllink");
        }
    }
}
