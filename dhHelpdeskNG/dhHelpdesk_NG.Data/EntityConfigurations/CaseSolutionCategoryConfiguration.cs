namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseSolutionCategoryConfiguration : EntityTypeConfiguration<CaseSolutionCategory>
    {
        internal CaseSolutionCategoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(f => f.CaseSolutionCategoryLanguages)
                .WithRequired(f => f.CaseSolutionCategory)
                .HasForeignKey(f => f.Category_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.Name).IsRequired().HasColumnName("CaseSolutionCategory");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcasesolutioncategory");
        }
    }
}
