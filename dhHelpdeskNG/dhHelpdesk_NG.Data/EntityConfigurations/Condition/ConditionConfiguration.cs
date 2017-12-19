namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class ConditionConfiguration : EntityTypeConfiguration<ConditionEntity>
    {
        #region Constructors and Destructors

        internal ConditionConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Parent_Id).IsRequired();
            Property(e => e.GUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.Property_Name).IsRequired().HasMaxLength(100);
            Property(e => e.Values).IsRequired();
            Property(e => e.Name).IsOptional().HasMaxLength(200);
            Property(e => e.Description).IsOptional().HasMaxLength(200);
            Property(e => e.Operator).IsRequired();
            Property(e => e.CreatedByUser_Id).IsOptional();
            Property(e => e.ChangedByUser_Id).IsOptional();
            Property(e => e.SortOrder).IsRequired();
            Property(e => e.Status).IsRequired();

            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();

            ToTable("tblCondition");
        }

        #endregion
    }
}