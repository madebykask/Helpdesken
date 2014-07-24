namespace DH.Helpdesk.Dal.EntityConfigurations.SSO
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.SSO;

    internal sealed class SSOLogConfiguration : EntityTypeConfiguration<SSOLogEntity>
    {
        #region Constructors and Destructors

        internal SSOLogConfiguration()
        {
            this.HasKey(s => s.Id);
            this.Property(s => s.ApplicationId).IsOptional().HasMaxLength(100);
            this.Property(s => s.NetworkId).IsOptional().HasMaxLength(100);
            this.Property(s => s.ClaimData).IsOptional();            
            this.Property(s => s.CreatedDate);            

            this.ToTable("tblSSOLog");
        }

        #endregion
    }
}