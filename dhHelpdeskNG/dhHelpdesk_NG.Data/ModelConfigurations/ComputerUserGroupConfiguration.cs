using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ComputerUserGroupConfiguration : EntityTypeConfiguration<ComputerUserGroup>
    {
        internal ComputerUserGroupConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
              .WithMany()
              .HasForeignKey(x => x.Customer_Id)
              .WillCascadeOnDelete(false);

            HasMany(u => u.OUs)
                .WithMany(a => a.ComputerUserGroups)
                .Map(m => {
                        m.MapLeftKey("ComputerUserGroup_Id");
                        m.MapRightKey("OU_Id");
                        m.ToTable("tblComputerUserGroup_tblOU");
                    });

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Department_Id).IsOptional();
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ComputerUserGroup");
            Property(x => x.Path).IsRequired().HasMaxLength(200).HasColumnName("LDAPPath");
            Property(x => x.ShowOnStartPage).IsRequired().HasColumnName("Show");
            Property(x => x.Type).IsRequired().HasColumnName("ComputerUserGroupType");
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcomputerusergroup");
        }
    }
}
