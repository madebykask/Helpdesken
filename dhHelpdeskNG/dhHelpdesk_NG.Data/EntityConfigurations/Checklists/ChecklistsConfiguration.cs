using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations.Checklists
{
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class ChecklistsConfiguration : EntityTypeConfiguration<Domain.Checklists>
    {
        internal ChecklistsConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.WorkingGroup_Id);
            Property(x => x.ChecklistName).IsRequired();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ChangedDate).IsRequired();

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);
           

            ToTable("dbo.tblChecklists");
        }
    }
}
