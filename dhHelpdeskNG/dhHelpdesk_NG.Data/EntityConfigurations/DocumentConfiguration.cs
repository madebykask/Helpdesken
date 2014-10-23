namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        internal DocumentConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.WGs)
                .WithMany(a => a.Documents)
                .Map(m =>
                {
                    m.MapLeftKey("Document_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblDocument_tblWorkingGroup");
                });

            this.HasMany(u => u.Us)
                .WithMany(a => a.Documents)
                .Map(m =>
                {
                    m.MapLeftKey("Document_Id");
                    m.MapRightKey("User_Id");
                    m.ToTable("tblDocument_tblUser");
                });

            this.HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.DocumentCategory)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.DocumentCategory_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(d => d.AccountActivities)
                .WithOptional(a => a.Document)
                .HasForeignKey(a => a.Document_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(d => d.OrderTypes)
                .WithOptional(a => a.Document)
                .HasForeignKey(a => a.Document_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(d => d.Links)
                .WithOptional(a => a.Document)
                .HasForeignKey(a => a.Document_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ChangedByUser_Id).IsOptional();
            this.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
            this.Property(x => x.CreatedByUser_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsRequired().HasMaxLength(2000).HasColumnName("DocumentDescription");
            this.Property(x => x.DocumentCategory_Id).IsOptional();
            this.Property(x => x.File).IsOptional().HasColumnName("Document");
            this.Property(x => x.FileName).IsRequired().HasMaxLength(100).HasColumnName("DocumentFileName");
            this.Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("DocumentName");
            this.Property(x => x.Size).IsRequired().HasColumnName("DocumentSize");
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbldocument");
        }
    }
}
