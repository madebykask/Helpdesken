namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class OUConfiguration : EntityTypeConfiguration<OU>
    {
        internal OUConfiguration()
        {
            this.HasKey(x => x.Id);

            //HasMany(x => x.ComputerUsers)
            //    .WithOptional()
            //    .HasForeignKey(x => x.OU_Id)
            //    .WillCascadeOnDelete(false);

            this.HasMany(x => x.ComputerUserGroups)
                .WithMany(x => x.OUs)
                .Map(m => m.MapLeftKey("OU_Id")
                    .MapRightKey("ComputerUserGroup_Id")
                    .ToTable("tblComputerUserGroup_tblOU"));

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.SubOUs)
                .HasForeignKey(x => x.Parent_OU_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ADSync).IsRequired();
            this.Property(x => x.HomeDirectory).IsRequired().HasMaxLength(200);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("OU");
            this.Property(x => x.OUId).IsRequired().HasMaxLength(20);
            this.Property(x => x.Path).IsRequired().HasMaxLength(200).HasColumnName("LDAPPath");
            this.Property(x => x.ScriptPath).IsRequired().HasMaxLength(100);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Code).IsOptional().HasMaxLength(20);
            this.Property(x => x.SearchKey).IsOptional().HasMaxLength(200);
            this.Property(x => x.ShowInvoice).IsRequired();

            //this.Property(x => x.OUGUID).IsOptional();
            //this.Property(x => x.SynchronizedDate).IsOptional();

            this.ToTable("tblou");
        }
    }
}
