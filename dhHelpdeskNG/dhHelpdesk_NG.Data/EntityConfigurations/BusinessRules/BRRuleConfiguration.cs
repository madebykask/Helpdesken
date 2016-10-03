namespace DH.Helpdesk.Dal.EntityConfigurations.BusinessRule
{
    using DH.Helpdesk.Domain.BusinessRules;
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
            this.Property(r => r.Sequence).IsRequired();
            this.Property(r => r.ContinueOnSuccess).IsRequired();
            this.Property(r => r.ContinueOnError).IsRequired();
            this.Property(r => r.Status).IsRequired();
            this.Property(r => r.CreatedTime).IsRequired();
            this.Property(r => r.CreatedByUser_Id).IsRequired();
            this.Property(r => r.CreatedTime).IsRequired();
            this.Property(r => r.ChangedByUser_Id).IsRequired();

            this.HasRequired(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblBR_Rules");
        }

        #endregion
    }
}