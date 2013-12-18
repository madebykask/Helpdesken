namespace dhHelpdesk_NG.Data.ModelConfigurations.Notifiers
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain;

    public sealed class ComputerUserGroupConfiguration : EntityTypeConfiguration<ComputerUserGroup>
    {
        internal ComputerUserGroupConfiguration()
        {
            this.HasKey(g => g.Id);
            this.Property(g => g.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(g => g.Name).IsRequired().HasMaxLength(50).HasColumnName("ComputerUserGroup");
            this.Property(g => g.Path).IsRequired().HasMaxLength(200).HasColumnName("LDAPPath");
            this.Property(g => g.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(g => g.ShowOnStartPage).IsRequired().HasColumnName("Show");
            this.Property(g => g.Type).IsRequired().HasColumnName("ComputerUserGroupType");
            this.Property(g => g.Department_Id).IsOptional();
            this.Property(g => g.Customer_Id).IsRequired();

            this.HasRequired(g => g.Customer)
              .WithMany()
              .HasForeignKey(g => g.Customer_Id)
              .WillCascadeOnDelete(false);

            this.Property(g => g.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(g => g.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.HasMany(g => g.OUs).WithMany(g => g.ComputerUserGroups).Map(
                m =>
                    {
                        m.MapLeftKey("ComputerUserGroup_Id");
                        m.MapRightKey("OU_Id");
                        m.ToTable("tblComputerUserGroup_tblOU");
                    });

            this.ToTable("tblcomputerusergroup");
        }
    }
}
