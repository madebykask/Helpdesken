namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OperationObjectConfiguration : EntityTypeConfiguration<OperationObject>
    {
        internal OperationObjectConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);            

            this.HasOptional(x => x.WorkingGroup)
                 .WithMany()
                 .HasForeignKey(x => x.WorkingGroup_Id)
                 .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsRequired().HasMaxLength(200).HasColumnName("OperationObjectDescription");
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OperationObject");
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.ShowPI).IsRequired().HasColumnName("ShowInPrinterInventory");
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbloperationobject");
        }
    }
}
