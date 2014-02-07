namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ComputerUsersBlackListConfiguration : EntityTypeConfiguration<ComputerUsersBlackList>
    {
        internal ComputerUsersBlackListConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.User_Id).IsRequired().HasMaxLength(50).HasColumnName("UserId");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcomputerusersblackList");
        }
    }
}
