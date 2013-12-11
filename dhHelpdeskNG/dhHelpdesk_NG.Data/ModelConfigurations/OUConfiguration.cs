using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OUConfiguration : EntityTypeConfiguration<OU>
    {
        internal OUConfiguration()
        {
            HasKey(x => x.Id);

            //HasMany(x => x.ComputerUsers)
            //    .WithOptional()
            //    .HasForeignKey(x => x.OU_Id)
            //    .WillCascadeOnDelete(false);

            HasMany(x => x.ComputerUserGroups)
                .WithMany(x => x.OUs)
                .Map(m => m.MapLeftKey("OU_Id")
                    .MapRightKey("ComputerUserGroup_Id")
                    .ToTable("tblComputerUserGroup_tblOU"));

            HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Parent)
                .WithMany(x => x.SubOUs)
                .HasForeignKey(x => x.Parent_OU_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ADSync).IsRequired();
            Property(x => x.HomeDirectory).IsRequired().HasMaxLength(200);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OU");
            Property(x => x.OUId).IsRequired().HasMaxLength(20);
            Property(x => x.Path).IsRequired().HasMaxLength(200).HasColumnName("LDAPPath");
            Property(x => x.ScriptPath).IsRequired().HasMaxLength(100);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblou");
        }
    }
}
