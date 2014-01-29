using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeStatusConfiguration : EntityTypeConfiguration<ChangeStatusEntity>
    {
        internal ChangeStatusConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.CompletionStatus).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.InformOrderer).IsRequired();
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ChangeStatus");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblchangestatus");
        }
    }
}
