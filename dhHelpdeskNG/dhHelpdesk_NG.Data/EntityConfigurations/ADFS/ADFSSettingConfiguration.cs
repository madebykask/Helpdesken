namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.ADFS;

    internal sealed class ADFSSettingConfiguration : EntityTypeConfiguration<ADFSSettingEntity>
    {
        #region Constructors and Destructors

        internal ADFSSettingConfiguration()
        {            
            this.Property(a => a.ApplicationId).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrDomain).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrEmail).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrEmployeeNumber).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrFirstName).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrSurName).IsOptional().HasMaxLength(50);

            this.Property(a => a.AttrUserId).IsOptional().HasMaxLength(50);

            this.Property(a => a.SaveSSOLog).IsRequired();


            this.ToTable("tblADFSSetting");
        }

        #endregion
    }
}