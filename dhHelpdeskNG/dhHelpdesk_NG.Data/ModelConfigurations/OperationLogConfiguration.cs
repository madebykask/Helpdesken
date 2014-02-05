using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OperationLogConfiguration : EntityTypeConfiguration<OperationLog>
    {
        internal OperationLogConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.WGs)
                .WithMany(a => a.OperationLogs)
                .Map(m =>
                {
                    m.MapLeftKey("OperationLog_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblOperationLog_tblWG");
                }
                );

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            Property(x => x.OperationLogCategory_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.PublicInformation).IsRequired();
            //Property(x => x.ShowDate).IsOptional();
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.ShowUntilDate).IsOptional();
            Property(x => x.LogText).IsRequired().HasMaxLength(4000);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblOperationLog");
        }
    }
}
