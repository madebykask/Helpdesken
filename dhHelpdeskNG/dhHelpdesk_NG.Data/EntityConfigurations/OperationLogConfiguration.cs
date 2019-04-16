using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    public class OperationLogConfiguration : EntityTypeConfiguration<OperationLog>
    {
        internal OperationLogConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.OperationObject_Id).IsRequired();
            Property(x => x.OperationObjectCategory_Id).IsOptional();
            Property(x => x.OperationLogCategory_Id).IsOptional();
            Property(x => x.User_Id).IsRequired();
            
            // long text fields
            Property(x => x.LogText).IsRequired().HasMaxLength(4000);
            Property(x => x.LogAction).IsRequired().HasMaxLength(4000);
            Property(x => x.LogTextExternal).IsRequired().HasMaxLength(4000);

            Property(x => x.PublicInformation).IsRequired();
            Property(x => x.ShowUntilDate).IsOptional();
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.WorkingTime).IsRequired();
            Property(x => x.Popup).IsRequired();
            Property(x => x.ShowDate).IsOptional();
            Property(x => x.InformUsers).IsOptional();

            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasOptional(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.OperationLogCategory_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Object)
                .WithMany()
                .HasForeignKey(x => x.OperationObject_Id)
                .WillCascadeOnDelete(false);


            HasMany(u => u.WGs).WithMany(a => a.OperationLogs).Map(
                m =>
                {
                    m.MapLeftKey("OperationLog_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblOperationLog_tblWG");
                });

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Admin)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            HasMany(o => o.EmailLogs)
                .WithOptional(e => e.OperationLog)
                .HasForeignKey(e => e.OperationLog_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblOperationLog");
        }
    }
}
