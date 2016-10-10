namespace DH.Helpdesk.Dal.EntityConfigurations.BusinessRule
{
    using DH.Helpdesk.Domain.BusinessRules;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;    

    internal sealed class BRConditionConfiguration : EntityTypeConfiguration<BRConditionEntity>
    {
        #region Constructors and Destructors

        internal BRConditionConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Rule_Id).IsRequired();
            this.Property(c => c.Sequence).IsRequired();
            this.Property(c => c.Field_Id).IsRequired().HasMaxLength(50);
            this.Property(c => c.FromValue).IsRequired().HasMaxLength(4000);
            this.Property(c => c.ToValue).IsRequired().HasMaxLength(4000);

            this.HasRequired(x => x.BrRule)
                .WithMany()
                .HasForeignKey(x => x.Rule_Id)
                .WillCascadeOnDelete(false);
            
            this.ToTable("tblBR_RuleConditions");
        }

        #endregion
    }
}