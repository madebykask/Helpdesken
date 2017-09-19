namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ProductAreaConfiguration : EntityTypeConfiguration<ProductArea>
    {
        internal ProductAreaConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.MailTemplate)
                .WithMany()
                .HasForeignKey(x => x.MailID)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ParentProductArea)
                .WithMany(x => x.SubProductAreas)
                .HasForeignKey(x => x.Parent_ProductArea_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.WorkingGroups)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("ProductArea_Id")
                    .MapRightKey("WorkingGroup_Id")
                    .ToTable("tblProductArea_tblWorkingGroup");
                });

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsOptional().HasMaxLength(300);
            this.Property(x => x.InformUserText).IsOptional().HasMaxLength(300);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.MailID).IsOptional();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ProductArea");
            this.Property(x => x.Parent_ProductArea_Id).IsOptional();
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.ShowOnExternalPage).IsRequired();
            this.Property(x => x.ShowOnExtPageCases).IsRequired();
            this.Property(x => x.ProductAreaGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblproductarea");
        }
    }
}
