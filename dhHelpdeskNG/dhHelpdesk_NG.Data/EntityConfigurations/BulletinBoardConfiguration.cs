namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class BulletinBoardConfiguration : EntityTypeConfiguration<BulletinBoard>
    {
        internal BulletinBoardConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.WGs)
                .WithMany(a => a.BulletinBoards)
                .Map(m =>
                {
                    m.MapLeftKey("BulletinBoard_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblBulletinBoard_tblWG");
                });

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.PublicInformation).IsRequired();
            this.Property(x => x.ShowDate).IsOptional();
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.ShowUntilDate).IsOptional();
            this.Property(x => x.Text).IsRequired().HasMaxLength(4000).HasColumnName("BulletinBoardText");
            this.Property(x => x.ChangedDate).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblbulletinboard");
        }
    }
}
