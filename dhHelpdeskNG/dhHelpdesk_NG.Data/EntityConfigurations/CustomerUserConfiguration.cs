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
            this.Property(x => x.CaseRegionFilter).IsOptional().HasMaxLength(100);
            this.Property(x => x.CaseDepartmentFilter).IsOptional().HasMaxLength(100);
            this.Property(x => x.CaseStateSecondaryFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseUserFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseWorkingGroupFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseCaseTypeFilter).IsOptional().HasMaxLength(50); 
            this.Property(x => x.CaseStatusFilter).IsOptional().HasMaxLength(50); 
            this.Property(x => x.CaseResponsibleFilter).IsOptional().HasMaxLength(50); 
            this.Property(x => x.ShowOnStartPage).IsOptional();
            this.Property(x => x.WatchDatePermission).IsRequired();
            this.Property(x => x.UserInfoPermission).IsRequired();
            this.Property(x => x.CasePriorityFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseRegistrationDateStartFilter).IsOptional();
            this.Property(x => x.CaseRegistrationDateEndFilter).IsOptional();
            this.Property(x => x.CaseWatchDateStartFilter).IsOptional();
            this.Property(x => x.CaseWatchDateEndFilter).IsOptional();
            this.Property(x => x.CaseClosingDateStartFilter).IsOptional();
            this.Property(x => x.CaseClosingDateEndFilter).IsOptional();
            this.Property(x => x.CaseRegistrationDateFilterShow).IsRequired();
            this.Property(x => x.CaseWatchDateFilterShow).IsRequired();
            this.Property(x => x.CaseClosingDateFilterShow).IsRequired();
            this.Property(x => x.CaseClosingReasonFilter).IsOptional().HasMaxLength(50);
            this.Property(x => x.CaseInitiatorFilterShow).IsRequired();
            this.Property(x => x.CaseRemainingTimeFilter).IsOptional().HasMaxLength(50);

            this.ToTable("tblcustomeruser");
        }
    }
}
