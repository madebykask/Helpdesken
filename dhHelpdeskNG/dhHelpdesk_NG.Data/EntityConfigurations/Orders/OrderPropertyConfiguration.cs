namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Orders;

    internal sealed class OrderPropertyConfiguration : EntityTypeConfiguration<OrderPropertyEntity>
    {
        internal OrderPropertyConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OrderProperty).HasMaxLength(50).IsRequired();
            this.Property(o => o.OrderTypeId).HasColumnName("OrderType_Id").IsRequired();
            this.HasRequired(o => o.OrderType)
                .WithMany()
                .HasForeignKey(o => o.OrderTypeId);
            this.Property(o => o.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(o => o.ChangedDate).IsRequired();

            this.ToTable("tblOrderProperty");
        }
    }
}