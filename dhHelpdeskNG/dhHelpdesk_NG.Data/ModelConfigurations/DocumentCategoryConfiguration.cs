using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class DocumentCategoryConfiguration : EntityTypeConfiguration<DocumentCategory>
    {
        internal DocumentCategoryConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ChangedByUser_Id).IsOptional();
            Property(x => x.CreatedByUser_Id).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("DocumentCategory");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbldocumentcategory");
        }
    }
}
