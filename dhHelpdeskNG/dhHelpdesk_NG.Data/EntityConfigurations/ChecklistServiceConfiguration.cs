using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    

    using DH.Helpdesk.Domain;

    public class ChecklistServiceConfiguration : EntityTypeConfiguration<ChecklistService>
    {
        internal ChecklistServiceConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CheckList_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ChecklistService");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblchecklistservice");
        }
    }
}

