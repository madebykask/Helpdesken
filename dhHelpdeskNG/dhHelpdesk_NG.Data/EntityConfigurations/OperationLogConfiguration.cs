namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OperationLogConfiguration : EntityTypeConfiguration<OperationLog>
    {
        internal OperationLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.WGs)
                .WithMany(a => a.OperationLogs)
                .Map(m =>
                {
                    m.MapLeftKey("OperationLog_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblOperationLog_tblWG");
                }
                );

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Admin)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.OperationLogCategory_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.PublicInformation).IsRequired();
            //Property(x => x.ShowDate).IsOptional();
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.ShowUntilDate).IsOptional();
            this.Property(x => x.LogText).IsRequired().HasMaxLength(4000);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasOptional(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.OperationLogCategory_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblOperationLog");
        }
    }
}
