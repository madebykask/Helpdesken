using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using DH.Helpdesk.Domain;
namespace DH.Helpdesk.Dal.EntityConfigurations.CheckLists
{        
    public sealed class CheckListsConfiguration : EntityTypeConfiguration<CheckListsEntity>
    {
        internal CheckListsConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.WorkingGroup_Id).IsOptional();
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
