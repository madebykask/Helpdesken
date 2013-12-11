using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        internal DocumentConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.WGs)
                .WithMany(a => a.Documents)
                .Map(m =>
                {
                    m.MapLeftKey("Document_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblDocument_tblWorkingGroup");
                });

            HasMany(u => u.Us)
                .WithMany(a => a.Documents)
                .Map(m =>
                {
                    m.MapLeftKey("Document_Id");
                    m.MapRightKey("User_Id");
                    m.ToTable("tblDocument_tblUser");
                });

            HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.DocumentCategory)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.DocumentCategory_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ChangedByUser_Id).IsOptional();
            Property(x => x.ContentType).IsRequired().HasMaxLength(100);
            Property(x => x.CreatedByUser_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsRequired().HasMaxLength(2000).HasColumnName("DocumentDescription");
            Property(x => x.DocumentCategory_Id).IsOptional();
            Property(x => x.File).IsOptional().HasColumnName("Document");
            Property(x => x.FileName).IsRequired().HasMaxLength(100).HasColumnName("DocumentFileName");
            Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("DocumentName");
            Property(x => x.Size).IsRequired().HasColumnName("DocumentSize");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbldocument");
        }
    }
}
