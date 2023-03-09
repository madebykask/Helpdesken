namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain;

    internal sealed class CaseIsAboutConfiguration : EntityTypeConfiguration<CaseIsAboutEntity>
    {
        internal CaseIsAboutConfiguration()
        {                        
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasColumnName("Case_Id");
            this.Property(f => f.CostCentre).IsOptional().HasMaxLength(50);
            this.Property(f => f.Department_Id).IsOptional();
            this.Property(f => f.OU_Id).IsOptional();
            this.Property(f => f.Person_Cellphone).IsOptional().HasMaxLength(50);            
            this.Property(f => f.Person_Email).IsOptional().HasMaxLength(100);
            this.Property(f => f.Person_Name).IsOptional().HasMaxLength(50);
            this.Property(f => f.Person_Phone).IsOptional().HasMaxLength(50);            
            this.Property(f => f.Place).IsOptional().HasMaxLength(100);
            this.Property(f => f.Region_Id).IsOptional();
            this.Property(f => f.ReportedBy).IsOptional().HasMaxLength(200);
            this.Property(f => f.UserCode).IsOptional().HasMaxLength(50);            

            this.ToTable("tblCaseIsAbout");
        }
    }
}