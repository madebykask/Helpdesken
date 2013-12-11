using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class BulletinBoardConfiguration : EntityTypeConfiguration<BulletinBoard>
    {
        internal BulletinBoardConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.WGs)
                .WithMany(a => a.BulletinBoards)
                .Map(m =>
                {
                    m.MapLeftKey("BulletinBoard_Id");
                    m.MapRightKey("WorkingGroup_Id");
                    m.ToTable("tblBulletinBoard_tblWG");
                });

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.PublicInformation).IsRequired();
            Property(x => x.ShowDate).IsOptional();
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.ShowUntilDate).IsOptional();
            Property(x => x.Text).IsRequired().HasMaxLength(4000).HasColumnName("BulletinBoardText");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblbulletinboard");
        }
    }
}
