using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class WorkingGroupConfiguration : EntityTypeConfiguration<WorkingGroupEntity>
    {
        internal WorkingGroupConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            HasMany(c => c.UserWorkingGroups)
                .WithRequired()
                .HasForeignKey(cu => cu.WorkingGroup_Id).WillCascadeOnDelete(false);

            HasMany(x => x.Documents)
                .WithMany(x => x.WGs)
                .Map(m => m.MapLeftKey("WorkingGroup_Id")
                    .MapRightKey("Document_Id")
                    .ToTable("tblDocument_tblWorkingGroup"));

            HasOptional(x => x.StateSecondary)
                .WithMany()
                .HasForeignKey(x => x.StateSecondary_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.AllocateCaseMail).IsRequired();
            Property(x => x.Code).IsOptional().HasMaxLength(20).HasColumnName("WorkingGroupCode");
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.EMail).IsRequired().HasMaxLength(200).HasColumnName("WorkingGroupEMail");
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired();
            Property(x => x.IsDefaultBulletinBoard).IsRequired();
            Property(x => x.IsDefaultCalendar).IsRequired();
            Property(x => x.IsDefaultOperationLog).IsRequired();
            Property(x => x.WorkingGroupName).IsRequired().HasMaxLength(50).HasColumnName("WorkingGroup");
            Property(x => x.POP3Password).IsOptional().HasMaxLength(20);
            Property(x => x.POP3UserName).IsOptional().HasMaxLength(50);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StateSecondary_Id).IsOptional();

            ToTable("tblworkinggroup");
        }
    }
}
