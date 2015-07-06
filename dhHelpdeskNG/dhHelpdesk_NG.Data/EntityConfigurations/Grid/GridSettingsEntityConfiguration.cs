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
            this.Property(x => x.CustomerId).HasColumnName("Customer_Id").IsRequired();
            this.Property(x => x.UserId).HasColumnName("User_Id").IsRequired();
            this.Property(x => x.GridId).HasColumnName("Grid_Id").IsRequired();
            this.Property(x => x.FieldId).HasColumnName("Field_Id").IsOptional();
            this.Property(x => x.Parameter).IsRequired();
            this.Property(x => x.Value).IsRequired();
            

            this.ToTable("tblUserGridSettings");
        }
    }
}
