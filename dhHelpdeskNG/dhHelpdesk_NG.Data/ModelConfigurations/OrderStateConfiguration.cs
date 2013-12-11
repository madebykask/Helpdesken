using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OrderStateConfiguration : EntityTypeConfiguration<OrderState>
    {
        internal OrderStateConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            //HasRequired(x => x.Customer)
            //    .WithMany(x => x.OrderStates)
            //    .HasForeignKey(x => x.Customer_Id)
            //    .WillCascadeOnDelete(false);

            Property(x => x.ChangedByUser_Id).IsOptional();
            Property(x => x.CreateCase).IsRequired();
            Property(x => x.CreatedByUser_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.EMailList).IsRequired().HasMaxLength(500);
            Property(x => x.EnableToOrderer).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OrderState");
            Property(x => x.NotifyOrderer).IsRequired();
            Property(x => x.NotifyReceiver).IsRequired();
            Property(x => x.SelectedInSearchCondition).IsRequired();
            Property(x => x.SortOrder).IsRequired();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblorderstate");
        }
    }
}
