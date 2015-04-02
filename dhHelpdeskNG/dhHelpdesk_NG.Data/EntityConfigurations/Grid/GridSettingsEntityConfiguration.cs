namespace DH.Helpdesk.Dal.EntityConfigurations.Grid
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain.Grid;

    public class GridSettingsEntityConfiguration : EntityTypeConfiguration<GridSettingsEntity>
    {
        internal GridSettingsEntityConfiguration()
        {
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CustomerId).IsRequired();
            this.Property(x => x.UserId).IsRequired();
            this.Property(x => x.GridId).IsRequired();
            this.Property(x => x.FieldId).IsOptional();
            this.Property(x => x.Parameter).IsRequired();
            this.Property(x => x.Value).IsRequired();
            

            this.ToTable("UserGridSettings");
        }
    }
}
