namespace DH.Helpdesk.Dal.EntityConfigurations.BusinessRule
{    
    using DH.Helpdesk.Domain.BusinessRules;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;    

    internal sealed class BRActionParamConfiguration : EntityTypeConfiguration<BRActionParamEntity>
    {
        #region Constructors and Destructors

        internal BRActionParamConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.RuleAction_Id).IsRequired();
            this.Property(a => a.ParamType_Id).IsRequired();            
            this.Property(a => a.ParamValue).IsRequired().HasMaxLength(4000);
            
            this.ToTable("tblBR_ActionParams");
        }

        #endregion
    }
}