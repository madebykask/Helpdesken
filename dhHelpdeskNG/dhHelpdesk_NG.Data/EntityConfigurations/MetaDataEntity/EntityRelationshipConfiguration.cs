namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.MetaData;

    internal sealed class EntityRelationshipConfiguration : EntityTypeConfiguration<EntityRelationship>
    {
        #region Constructors and Destructors

        internal EntityRelationshipConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.ParentEntity_Guid).IsRequired();
            Property(e => e.ChildEntity_Guid).IsRequired();
            Property(e => e.ParentItem_Guid).IsRequired();
            Property(e => e.ChildItem_Guid).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();
            
            ToTable("tblEntityRelationship");
        }

        #endregion
    }
}