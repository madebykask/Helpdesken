namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Cases;

    internal sealed class CaseFilteFavoriteConfiguration : EntityTypeConfiguration<CaseFilterFavoriteEntity>
    {
        internal CaseFilteFavoriteConfiguration()
        {            
            this.HasKey(f => f.Id);

            this.Property(f => f.Customer_Id).IsRequired();
            this.Property(f => f.User_Id).IsRequired();
            this.Property(f => f.Name).IsRequired().HasMaxLength(80);

            this.Property(f => f.InitiatorFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.InitiatorSearchScopeFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.AdministratorFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.CaseTypeFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.CategoryTypeFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.ClosingReasonFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.DepartmentFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.PriorityFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.ProductAreaFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.RegionFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.RemainingTimeFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.ResponsibleFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.StatusFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.SubStatusFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.WorkingGroupFilter).IsOptional().HasMaxLength(200);
            this.Property(f => f.RegisteredByFilter).IsOptional().HasMaxLength(200);
            

            this.Property(f => f.ClosingDateStartFilter).IsOptional();
            this.Property(f => f.ClosingDateEndFilter).IsOptional();
            this.Property(f => f.RegistrationDateStartFilter).IsOptional();
            this.Property(f => f.RegistrationDateEndFilter).IsOptional();
            this.Property(f => f.WatchDateStartFilter).IsOptional();
            this.Property(f => f.WatchDateEndFilter).IsOptional();

            this.Property(f => f.CreatedDate).IsRequired();
            
            this.ToTable("tblCaseFilterFavorite");
        }
    }
}