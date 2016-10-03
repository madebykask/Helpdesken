namespace DH.Helpdesk.Dal.EntityConfigurations.BusinessRule
{    
    using DH.Helpdesk.Domain.BusinessRules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;    

    internal sealed class BRActionConfiguration : EntityTypeConfiguration<BRActionEntity>
    {
        #region Constructors and Destructors

        internal BRActionConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Rule_Id).IsRequired();
            this.Property(a => a.ActionType_Id).IsRequired();
            this.Property(a => a.Sequence).IsRequired();
            
            this.ToTable("tblBR_RuleActions");
        }

        #endregion
    }
}