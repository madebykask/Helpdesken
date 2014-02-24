namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LinkUserConfiguration : EntityTypeConfiguration<LinkUser>
    {
        internal LinkUserConfiguration()
        {
            this.HasKey(x => new { x.Link_Id, x.User_Id });

            this.ToTable("tblLink_tblUsers");
        }

    }
}