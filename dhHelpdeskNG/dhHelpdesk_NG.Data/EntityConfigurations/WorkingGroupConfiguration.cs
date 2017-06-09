namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class WorkingGroupConfiguration : EntityTypeConfiguration<WorkingGroupEntity>
    {
        internal WorkingGroupConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            this.HasMany(c => c.UserWorkingGroups)
                .WithRequired()
                .HasForeignKey(cu => cu.WorkingGroup_Id).WillCascadeOnDelete(false);

            this.HasMany(x => x.Documents)
                .WithMany(x => x.WGs)
                .Map(m => m.MapLeftKey("WorkingGroup_Id")
                    .MapRightKey("Document_Id")
                    .ToTable("tblDocument_tblWorkingGroup"));

            this.HasOptional(x => x.StateSecondary)
                .WithMany()
                .HasForeignKey(x => x.StateSecondary_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AllocateCaseMail).IsRequired();
            this.Property(x => x.Code).IsOptional().HasMaxLength(20).HasColumnName("WorkingGroupCode");
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.EMail).IsRequired().HasMaxLength(200).HasColumnName("WorkingGroupEMail");
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired();
            this.Property(x => x.IsDefaultBulletinBoard).IsRequired();
            this.Property(x => x.IsDefaultCalendar).IsRequired();
            this.Property(x => x.IsDefaultOperationLog).IsRequired();
            this.Property(x => x.WorkingGroupName).IsRequired().HasMaxLength(50).HasColumnName("WorkingGroup");
            this.Property(x => x.POP3Password).IsOptional().HasMaxLength(20);
            this.Property(x => x.POP3UserName).IsOptional().HasMaxLength(50);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.StateSecondary_Id).IsOptional();
            this.Property(x => x.SendExternalEmailToWGUsers).IsOptional();
            this.Property(x => x.WorkingGroupGUID).IsOptional();
            this.Property(x => x.WorkingGroupId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); 


            this.ToTable("tblworkinggroup");
        }
    }
}
