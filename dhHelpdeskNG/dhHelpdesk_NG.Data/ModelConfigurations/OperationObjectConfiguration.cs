using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OperationObjectConfiguration : EntityTypeConfiguration<OperationObject>
    {
        internal OperationObjectConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);            

            HasOptional(x => x.WorkingGroup)
                 .WithMany()
                 .HasForeignKey(x => x.WorkingGroup_Id)
                 .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsRequired().HasMaxLength(200).HasColumnName("OperationObjectDescription");
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OperationObject");
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.ShowPI).IsRequired().HasColumnName("ShowInPrinterInventory");
            Property(x => x.WorkingGroup_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbloperationobject");
        }
    }
}
