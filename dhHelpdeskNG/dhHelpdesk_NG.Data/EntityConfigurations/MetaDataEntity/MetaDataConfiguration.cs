namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.MetaData;

    internal sealed class MetaDataConfiguration : EntityTypeConfiguration<MetaDataEntity>
    {
        #region Constructors and Destructors

        internal MetaDataConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Customer_Id).IsRequired();
            Property(e => e.MetaDataGuid).IsRequired();
            Property(e => e.EntityInfo_Guid).IsRequired();
            Property(e => e.MetaDataCode).IsRequired().HasMaxLength(100);
            Property(e => e.MetaDataText).IsRequired().HasMaxLength(3500);
            Property(e => e.ExternalId).IsOptional();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsOptional();
            Property(e => e.SynchronizedDate).IsOptional();

            ToTable("tblMetaData");
        }

        #endregion
    }
}