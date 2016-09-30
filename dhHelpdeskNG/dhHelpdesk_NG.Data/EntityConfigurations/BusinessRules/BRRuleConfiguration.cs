namespace DH.Helpdesk.Dal.EntityConfigurations.BusinessRule
{
    using DH.Helpdesk.Domain.Orders;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;    

    internal sealed class BRRuleConfiguration : EntityTypeConfiguration<BRRuleEntity>
    {
        #region Constructors and Destructors

        internal BRRuleConfiguration()
        {
            this.HasKey(r => r.Id);
            this.Property(r => r.Customer_Id).IsRequired();
            this.Property(r => r.Name).IsRequired().HasMaxLength(100);
            this.Property(r => r.Event_Id).IsRequired();
            //this.Property(r => r.).IsRequired();
            
            
            this.ToTable("tblBR_Rules");
        }

        #endregion
    }
}