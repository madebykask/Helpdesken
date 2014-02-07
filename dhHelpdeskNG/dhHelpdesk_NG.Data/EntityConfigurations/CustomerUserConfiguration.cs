namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CustomerUserConfiguration : EntityTypeConfiguration<CustomerUser>
    {
        internal CustomerUserConfiguration()
        {
            this.HasKey(x => new { x.User_Id, x.Customer_Id });

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.User)
                .WithMany(x => x.CustomerUsers)
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.CaseCategoryFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CasePerformerFilter).IsRequired().HasMaxLength(50);
            this.Property(x => x.CaseProductAreaFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseRegionFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseStateSecondaryFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseUserFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseWorkingGroupFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.ShowOnStartPage).IsOptional();
            this.Property(x => x.WatchDatePermission).IsRequired();
            this.Property(x => x.UserInfoPermission).IsRequired();

            this.ToTable("tblcustomeruser");
        }
    }
}
