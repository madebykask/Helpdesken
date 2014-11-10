namespace DH.Helpdesk.Dal.EntityConfigurations.Orders
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class OrderEMailLogConfiguration : EntityTypeConfiguration<OrderEMailLog>
    {
        internal OrderEMailLogConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OrderEMailLogGUID).IsRequired();
            this.Property(o => o.OrderHistoryId).HasColumnName("OrderHistory_Id").IsOptional();
            this.HasOptional(o => o.OrderHistory)
                .WithMany()
                .HasForeignKey(o => o.OrderHistoryId);
            this.HasRequired(o => o.Order)
                .WithMany()
                .HasForeignKey(o => o.Order_Id);
            this.Property(o => o.EMailAddress).HasMaxLength(1000).IsOptional();
            this.Property(o => o.MailID).IsRequired();
            this.Property(o => o.MessageId).HasMaxLength(100).IsOptional();
            this.Property(o => o.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblOrderEMailLog");
        }
    }
}