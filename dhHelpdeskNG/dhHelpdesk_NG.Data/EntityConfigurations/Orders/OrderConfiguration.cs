namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        internal OrderConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.Customer_Id);
            this.HasOptional(o => o.Domain)
                .WithMany()
                .HasForeignKey(o => o.Domain_Id);
            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.UserDepartment1)
                .WithMany()
                .HasForeignKey(x => x.UserDepartment_Id)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.UserDepartment2)
                .WithMany()
                .HasForeignKey(x => x.UserDepartment_Id2)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.EmploymentType)
                .WithMany()
                .HasForeignKey(x => x.EmploymentType_Id)
                .WillCascadeOnDelete(false);
            this.HasOptional(x => x.UserOU)
                .WithMany()
                .HasForeignKey(x => x.UserOU_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.EmploymentType_Id).IsOptional();
            this.Property(o => o.OrderDate).IsOptional();
            this.Property(o => o.UserId).IsOptional().HasMaxLength(200);
            this.Property(o => o.UserFirstName).IsOptional().HasMaxLength(20);
            this.Property(o => o.UserLastName).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserPhone).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserEMail).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserInitials).IsOptional().HasMaxLength(10);
            this.Property(x => x.UserPersonalIdentityNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.UserExtension).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserTitle).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserLocation).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserRoomNumber).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserPostalAddress).IsOptional().HasMaxLength(50);
            this.Property(x => x.Responsibility).IsOptional().HasMaxLength(50);
            this.Property(x => x.Activity).IsOptional().HasMaxLength(50);
            this.Property(x => x.Manager).IsOptional().HasMaxLength(50);
            this.Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.InfoUser).IsOptional().HasMaxLength(20);
            this.Property(o => o.OrdererID).IsRequired().HasMaxLength(600);
            this.Property(o => o.Orderer).IsRequired().HasMaxLength(50);
            this.Property(o => o.OrdererAddress).IsRequired().HasMaxLength(50);
            this.Property(o => o.OrdererInvoiceAddress).IsRequired().HasMaxLength(50);
            this.Property(o => o.OrdererLocation).IsRequired().HasMaxLength(50);
            this.Property(o => o.OrdererEMail).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrdererPhone).IsRequired().HasMaxLength(50);
            this.Property(o => o.OrdererCode).IsRequired().HasMaxLength(10);
            this.Property(o => o.OrdererReferenceNumber).IsOptional().HasMaxLength(20);
            this.Property(o => o.AccountingDimension1).IsOptional().HasMaxLength(20);
            this.Property(o => o.AccountingDimension2).IsOptional().HasMaxLength(20);
            this.Property(o => o.AccountingDimension3).IsOptional().HasMaxLength(20);
            this.Property(o => o.AccountingDimension4).IsOptional().HasMaxLength(20);
            this.Property(o => o.AccountingDimension5).IsOptional().HasMaxLength(20);
            this.Property(x => x.AccountStartDate).IsOptional();
            this.Property(x => x.AccountEndDate).IsOptional();
            this.Property(x => x.EMailType).IsOptional();
            this.Property(x => x.HomeDirectory).IsRequired();
            this.Property(x => x.Profile).IsRequired();
            this.Property(x => x.InventoryNumber).IsOptional().HasMaxLength(20);
            this.Property(x => x.AccountInfo).IsOptional().HasMaxLength(500);
            this.Property(o => o.Department_Id).IsOptional();
            this.HasOptional(o => o.Ou)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.OU_Id);
            this.Property(o => o.OrderPropertyId).HasColumnName("OrderProperty_Id");
            this.HasOptional(o => o.OrderProperty)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.OrderPropertyId);
            this.Property(o => o.OrderRow).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow2).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow3).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow4).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow5).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow6).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow7).IsRequired().HasMaxLength(100);
            this.Property(o => o.OrderRow8).IsRequired().HasMaxLength(100);
            this.Property(o => o.Configuration).IsRequired().HasMaxLength(400);
            this.Property(o => o.OrderInfo).IsOptional().HasMaxLength(800);
            this.Property(o => o.OrderInfo2).IsRequired();
            this.Property(o => o.ReceiverId).IsRequired().HasMaxLength(40);
            this.Property(o => o.ReceiverName).IsRequired().HasMaxLength(50);
            this.Property(o => o.ReceiverEMail).IsRequired().HasMaxLength(100);
            this.Property(o => o.ReceiverPhone).IsRequired().HasMaxLength(50);
            this.Property(o => o.ReceiverLocation).IsRequired().HasMaxLength(50);
            this.Property(o => o.MarkOfGoods).IsRequired().HasMaxLength(100);
            this.Property(o => o.SupplierOrderNumber).IsRequired().HasMaxLength(20);
            this.Property(o => o.SupplierOrderDate).IsOptional();
            this.Property(o => o.SupplierOrderInfo).IsRequired().HasMaxLength(200);
            this.Property(o => o.User_Id).IsOptional();
            this.Property(o => o.Deliverydate).IsOptional();
            this.Property(o => o.InstallDate).IsOptional();
            this.HasOptional(o => o.OrderState)
                .WithMany()
                .HasForeignKey(o => o.OrderState_Id);
            this.HasOptional(o => o.OrderType)
                .WithMany()
                .HasForeignKey(o => o.OrderType_Id);
            this.Property(o => o.OrderFieldType_Id).IsOptional();
            this.HasOptional(x => x.OrderFieldType)
                .WithMany()
                .HasForeignKey(x => x.OrderFieldType_Id)
                .WillCascadeOnDelete(false);
            this.Property(o => o.OrderFieldType2).IsOptional().HasMaxLength(500);
            this.Property(o => o.OrderFieldType3_Id).IsOptional();
            this.HasOptional(x => x.OrderFieldType3)
                .WithMany()
                .HasForeignKey(x => x.OrderFieldType3_Id)
                .WillCascadeOnDelete(false);
            this.Property(o => o.OrderFieldType4_Id).IsOptional();
            this.HasOptional(x => x.OrderFieldType4)
                .WithMany()
                .HasForeignKey(x => x.OrderFieldType4_Id)
                .WillCascadeOnDelete(false);
            this.Property(o => o.OrderFieldType5_Id).IsOptional();
            this.HasOptional(x => x.OrderFieldType5)
                .WithMany()
                .HasForeignKey(x => x.OrderFieldType5_Id)
                .WillCascadeOnDelete(false);
            this.Property(o => o.DeliveryDepartmentId).IsOptional().HasColumnName("DeliveryDepartment_Id");
            this.HasOptional(o => o.DeliveryDepartment)
                .WithMany()
                .HasForeignKey(o => o.DeliveryDepartmentId);
            this.Property(o => o.DeliveryOu).HasColumnName("DeliveryOU").HasMaxLength(50).IsOptional();
            this.Property(o => o.DeliveryAddress).HasMaxLength(50).IsOptional();
            this.Property(o => o.DeliveryPostalCode).HasMaxLength(10).IsOptional();
            this.Property(o => o.DeliveryPostalAddress).HasMaxLength(50).IsOptional();
            this.Property(o => o.DeliveryLocation).HasMaxLength(50).IsOptional();
            this.Property(o => o.DeliveryInfo).HasMaxLength(200).IsRequired();
            this.Property(o => o.DeliveryInfo2).HasMaxLength(50).IsRequired();
            this.Property(o => o.DeliveryInfo3).HasMaxLength(50).IsRequired();
            this.Property(x => x.DeliveryName).IsOptional().HasMaxLength(50);
            this.Property(x => x.DeliveryPhone).IsOptional().HasMaxLength(50);
            this.Property(o => o.Filename).HasMaxLength(100).IsRequired();
            this.Property(o => o.CaseNumber).HasPrecision(18, 0).IsOptional();
            this.Property(o => o.Info).HasMaxLength(200).IsOptional();
            this.Property(o => o.Deleted).IsRequired();
            this.Property(o => o.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(o => o.CreatedByUser_Id).IsOptional();
            this.Property(o => o.ChangedDate).IsRequired();
            this.Property(o => o.ChangedByUser_Id).IsOptional();
            this.Property(o => o.DeliveryOuId).HasColumnName("DeliveryOU_Id").IsOptional();
            this.HasOptional(o => o.DeliveryOuEntity)
                .WithMany()
                .HasForeignKey(o => o.DeliveryOuId);
            this.HasMany(o => o.Programs)
                .WithMany(p => p.Orders)
                .Map(m =>
                {
                    m.MapLeftKey("Order_Id");
                    m.MapRightKey("Program_Id");
                    m.ToTable("tblOrder_tblProgram");
                });

            this.Property(x => x.ContactId).IsOptional().HasMaxLength(200);
            this.Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactEMail).IsOptional().HasMaxLength(50);

            this.Property(x => x.InfoProduct).IsOptional().HasMaxLength(500);

            this.HasOptional(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.User_Id);
            this.HasOptional(o => o.Department)
                .WithMany()
                .HasForeignKey(o => o.Department_Id);

            this.HasMany(o => o.Histories)
                .WithRequired(h => h.Order);

            this.ToTable("tblorder");
        }
    }
}
