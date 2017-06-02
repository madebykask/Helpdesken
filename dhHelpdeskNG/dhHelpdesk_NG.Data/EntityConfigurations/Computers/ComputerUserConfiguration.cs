namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public sealed class ComputerUserConfiguration : EntityTypeConfiguration<ComputerUser>
    {
        internal ComputerUserConfiguration()
        {
            this.HasKey(u => u.Id);
            this.Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(u => u.Customer_Id).IsOptional();
            this.HasOptional(u => u.Customer).WithMany().HasForeignKey(u => u.Customer_Id).WillCascadeOnDelete(false);
            this.Property(u => u.UserGUID).IsRequired().HasMaxLength(100);
            this.Property(u => u.UserId).IsOptional().HasMaxLength(50);
            this.Property(u => u.Domain_Id).IsOptional();
            this.HasOptional(u => u.Domain).WithMany().HasForeignKey(u => u.Domain_Id).WillCascadeOnDelete(false);
            this.Property(u => u.LogonName).IsRequired().HasMaxLength(50);
            this.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            this.Property(u => u.Initials).IsRequired().HasMaxLength(10);
            this.Property(u => u.SurName).IsRequired().HasMaxLength(50);
            this.Property(u => u.FullName).IsRequired().HasMaxLength(50);
            this.Property(u => u.DisplayName).IsRequired().HasMaxLength(50);
            this.Property(u => u.Location).IsRequired().HasMaxLength(100);
            this.Property(u => u.Phone).IsRequired().HasMaxLength(50);
            this.Property(u => u.Phone2).IsRequired().HasMaxLength(50);
            this.Property(u => u.Cellphone).IsRequired().HasMaxLength(50);
            this.Property(u => u.Email).IsRequired().HasMaxLength(100);
            this.Property(u => u.UserCode).IsRequired().HasMaxLength(50);
            this.Property(u => u.PostalAddress).IsRequired().HasMaxLength(50);
            this.Property(u => u.Postalcode).IsRequired().HasMaxLength(50);
            this.Property(u => u.City).IsRequired().HasMaxLength(50);
            this.Property(u => u.Title).IsRequired().HasMaxLength(50);
            this.Property(u => u.Department_Id).IsOptional();

            this.HasOptional(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.Department_Id)
                .WillCascadeOnDelete(false);

            this.Property(u => u.SOU).IsRequired().HasMaxLength(100).HasColumnName("OU");
            this.Property(u => u.OU_Id).IsOptional();
            this.HasOptional(u => u.OU).WithMany().HasForeignKey(u => u.OU_Id).WillCascadeOnDelete(false);
            this.Property(u => u.Division_Id).IsOptional();
            this.HasOptional(u => u.Division).WithMany().HasForeignKey(u => u.Division_Id).WillCascadeOnDelete(false);
            this.Property(u => u.ManagerComputerUser_Id).IsOptional();

            this.HasOptional(u => u.ManagerComputerUser)
                .WithMany()
                .HasForeignKey(u => u.ManagerComputerUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(u => u.ComputerUserGroup_Id).IsOptional();

            this.HasOptional(u => u.ComputerUserGroup)
                .WithMany()
                .HasForeignKey(u => u.ComputerUserGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(u => u.CUGs)
               .WithMany(g => g.ComputerUsers)
               .Map(m =>
               {
                   m.MapLeftKey("ComputerUser_Id");
                   m.MapRightKey("ComputerUserGroup_Id");
                   m.ToTable("tblComputerUser_tblCUGroup");
               });

            this.Property(u => u.Password).IsRequired().HasMaxLength(20);
            this.Property(u => u.Info).IsRequired().HasMaxLength(500);
            this.Property(u => u.Status).IsRequired();
            this.Property(u => u.OrderPermission).IsRequired();
            this.Property(u => u.homeDirectory).IsRequired().HasMaxLength(50);
            this.Property(u => u.homeDrive).IsRequired().HasMaxLength(5);
            this.Property(u => u.ComputerUserRole).IsRequired();
            this.Property(u => u.NDSpath).IsRequired().HasMaxLength(255);
            this.Property(u => u.Updated).IsRequired();
            this.Property(u => u.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(u => u.ChangeTime).IsRequired();
            this.Property(u => u.SyncChangedDate).IsOptional();
            this.Property(u => u.LanguageId).IsOptional();

            this.HasOptional(u => u.Language)
                .WithMany()
                .HasForeignKey(u => u.LanguageId)
                .WillCascadeOnDelete(false);


            this.ToTable("tblComputerUsers");
        }
    }
}
