namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OrderTypeConfiguration : EntityTypeConfiguration<OrderType>
    {
        internal OrderTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.CreateCase_CaseType)
                .WithMany()
                .HasForeignKey(x => x.CreateCase_CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ParentOrderType)
                .WithMany(x => x.SubOrderTypes)
                .HasForeignKey(x => x.Parent_OrderType_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.CaptionOrdererInfo).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionUserInfo).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionReceiverInfo).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionGeneral).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionOrderInfo).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionOrder).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionDeliveryInfo).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionProgram).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaptionOther).IsOptional().HasMaxLength(50);
            this.Property(x => x.CreateCase_CaseType_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsOptional().HasMaxLength(1500).HasColumnName("OrderTypeDescription");
            this.Property(x => x.Document_Id).IsOptional();
            this.Property(x => x.EMail).IsOptional().HasMaxLength(100);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OrderType");
            this.Property(x => x.Parent_OrderType_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblordertype");
        }
    }
}
