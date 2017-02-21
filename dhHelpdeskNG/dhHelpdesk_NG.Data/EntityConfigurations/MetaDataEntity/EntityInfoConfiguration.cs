namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.MetaDataEntity;

    internal sealed class EntityInfoConfiguration : EntityTypeConfiguration<EntityInfo>
    {
        #region Constructors and Destructors

        internal EntityInfoConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.EntityGuid).IsRequired();
            Property(e => e.EntityName).IsRequired().HasMaxLength(50);
            Property(e => e.EntityType).IsRequired().HasMaxLength(50);                        

            ToTable("tblEntityInfo");
        }

        #endregion
    }
}