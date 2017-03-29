namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    internal sealed class WorkFlowConfiguration : EntityTypeConfiguration<WorkFlowEntity>
    {
        internal WorkFlowConfiguration()
        {            
            HasKey(e => e.Id);
            Property(e => e.Customer_Id).IsRequired();
            Property(e => e.ItemCaption).HasMaxLength(200).IsRequired();            
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();

            ToTable("tblWorkFlow");
        }
    }
}