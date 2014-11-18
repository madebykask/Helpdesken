namespace DH.Helpdesk.Dal.EntityConfigurations.Orders
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OrderLogConfiguration : EntityTypeConfiguration<OrderLog>
    {
        internal OrderLogConfiguration()
        {
            this.HasKey(l => l.Id);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(l => l.Order)
                .WithMany(o => o.Logs)
                .HasForeignKey(l => l.Order_Id);
            this.Property(l => l.OrderHistoryId).HasColumnName("OrderHistory_Id").IsOptional();
            this.HasOptional(l => l.OrderHistory)
                .WithMany()
                .HasForeignKey(l => l.OrderHistoryId);
            this.Property(l => l.LogNote).HasMaxLength(1000).IsRequired();
            this.HasRequired(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.User_Id);
            this.Property(l => l.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(l => l.ChangedDate).IsRequired();

            this.ToTable("tblOrderLog");
        }
    }
}