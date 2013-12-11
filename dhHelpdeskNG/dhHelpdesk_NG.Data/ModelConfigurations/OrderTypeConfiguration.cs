using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OrderTypeConfiguration : EntityTypeConfiguration<OrderType>
    {
        internal OrderTypeConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.CreateCase_CaseType)
                .WithMany()
                .HasForeignKey(x => x.CreateCase_CaseType_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ParentOrderType)
                .WithMany(x => x.SubOrderTypes)
                .HasForeignKey(x => x.Parent_OrderType_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.CaptionOrdererInfo).IsOptional().HasMaxLength(30);
            Property(x => x.CaptionUserInfo).IsOptional().HasMaxLength(30);
            Property(x => x.CreateCase_CaseType_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsOptional().HasMaxLength(500).HasColumnName("OrderTypeDescription");
            Property(x => x.Document_Id).IsOptional();
            Property(x => x.EMail).IsOptional().HasMaxLength(100);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OrderType");
            Property(x => x.Parent_OrderType_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblordertype");
        }
    }
}
