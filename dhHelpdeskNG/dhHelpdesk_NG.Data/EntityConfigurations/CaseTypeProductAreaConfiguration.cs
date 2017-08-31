using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    internal sealed class CaseTypeProductAreaConfiguration : EntityTypeConfiguration<CaseTypeProductArea>
    {
        internal CaseTypeProductAreaConfiguration()
        {
            this.HasKey(x => new { x.CaseType_Id, x.ProductArea_Id });
            this.Property(x => x.CaseType_Id).IsRequired();
            this.Property(x => x.ProductArea_Id).IsRequired();

            this.HasRequired(x => x.CaseType)
                .WithMany(x => x.CaseTypeProductAreas)
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ProductArea)
                .WithMany(x => x.CaseTypeProductAreas)
                .HasForeignKey(x => x.ProductArea_Id)
                .WillCascadeOnDelete(false);


            this.ToTable("tblCaseType_tblProductArea");
        }
    }
}
