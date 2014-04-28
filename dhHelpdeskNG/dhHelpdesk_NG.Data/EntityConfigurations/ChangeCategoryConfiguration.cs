namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Changes;

    public class ChangeCategoryConfiguration : EntityTypeConfiguration<ChangeCategoryEntity>
    {
        internal ChangeCategoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ChangeCategory");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblchangecategory");
        }
    }
}

