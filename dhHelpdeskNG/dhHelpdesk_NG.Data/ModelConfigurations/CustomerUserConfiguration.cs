using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CustomerUserConfiguration : EntityTypeConfiguration<CustomerUser>
    {
        internal CustomerUserConfiguration()
        {
            HasKey(x => new { x.User_Id, x.Customer_Id });

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.User)
                .WithMany(x => x.CustomerUsers)
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.CaseCategoryFilter).IsOptional().HasMaxLength(50);
            Property(x => x.CasePerformerFilter).IsRequired().HasMaxLength(50);
            Property(x => x.CaseProductAreaFilter).IsOptional().HasMaxLength(50);
            Property(x => x.CaseRegionFilter).IsOptional().HasMaxLength(50);
            Property(x => x.CaseStateSecondaryFilter).IsOptional().HasMaxLength(50);
            Property(x => x.CaseUserFilter).IsOptional().HasMaxLength(50);
            Property(x => x.CaseWorkingGroupFilter).IsOptional().HasMaxLength(50);
            Property(x => x.ShowOnStartPage).IsOptional();
            Property(x => x.WatchDatePermission).IsRequired();
            Property(x => x.UserInfoPermission).IsRequired();

            ToTable("tblcustomeruser");
        }
    }
}
