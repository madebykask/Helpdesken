namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
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

            this.HasOptional(x => x.CaseSolution)
                .WithMany()
                .HasForeignKey(x => x.CaseSolution_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.LinkGroup)
                .WithMany()
                .HasForeignKey(x => x.LinkGroup_Id)
                .WillCascadeOnDelete(false);



            this.HasMany(x => x.Us)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Link_Id")
                    .MapRightKey("User_Id")
                    .ToTable("tblLink_tblUsers");
                });

            this.HasMany(u => u.Wg)
                .WithMany(a => a.Links)
                .Map(m =>
                {
                    m.MapLeftKey("Link_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblLink_tblWorkingGroup");
                });

            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.Document_Id).IsOptional();
            this.Property(x => x.OpenInNewWindow).IsRequired();
            this.Property(x => x.NewWindowHeight).IsRequired();
            this.Property(x => x.NewWindowWidth).IsRequired();
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.URLAddress).IsRequired().HasMaxLength(300);
            this.Property(x => x.URLName).IsRequired().HasMaxLength(50);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CaseSolution_Id).IsOptional();
            this.Property(x => x.SortOrder).IsOptional().HasMaxLength(2);
            //this.Property(x => x.LinkGUID).IsOptional();

            this.ToTable("tbllink");
        }
    }
}
