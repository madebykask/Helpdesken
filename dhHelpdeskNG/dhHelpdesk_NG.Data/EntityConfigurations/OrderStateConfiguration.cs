namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OrderStateConfiguration : EntityTypeConfiguration<OrderState>
    {
        internal OrderStateConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            //HasRequired(x => x.Customer)
            //    .WithMany(x => x.OrderStates)
            //    .HasForeignKey(x => x.Customer_Id)
            //    .WillCascadeOnDelete(false);

            this.Property(x => x.ChangedByUser_Id).IsOptional();
            this.Property(x => x.CreateCase).IsRequired();
            this.Property(x => x.CreatedByUser_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.EMailList).IsRequired().HasMaxLength(500);
            this.Property(x => x.EnableToOrderer).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OrderState");
            this.Property(x => x.NotifyOrderer).IsRequired();
            this.Property(x => x.NotifyReceiver).IsRequired();
            this.Property(x => x.SelectedInSearchCondition).IsRequired();
            this.Property(x => x.SortOrder).IsRequired();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblorderstate");
        }
    }
}
