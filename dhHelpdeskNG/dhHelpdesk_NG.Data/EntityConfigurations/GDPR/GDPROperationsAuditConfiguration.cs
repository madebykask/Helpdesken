using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.EntityConfigurations.GDPR
{
    public class GDPROperationsAuditConfiguration : EntityTypeConfiguration<GDPROperationsAudit>
    {
        public GDPROperationsAuditConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsOptional();

            this.Property(x => x.Operation).IsRequired().HasMaxLength(50); 
            this.Property(x => x.Parameters).IsOptional();
            this.Property(x => x.Result).IsOptional();
            this.Property(x => x.ErrorResult).IsOptional();
            this.Property(x => x.Application).IsRequired().HasMaxLength(50);
            this.Property(x => x.Success).IsRequired();
            this.Property(x => x.Error).IsOptional();
            this.Property(x => x.CreatedDate).IsRequired();

            this.ToTable("tblGDPROperationsAudit");
        }
    }
}