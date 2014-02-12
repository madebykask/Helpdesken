namespace DH.Helpdesk.Dal.EntityConfigurations.Servers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ServerConfiguration : EntityTypeConfiguration<Entity>
    {
        public ServerConfiguration()
        {
            this.HasKey(x => x.Id);
        }
    }
}
