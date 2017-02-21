namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.MetaDataEntity;

    internal sealed class MetaDataConfiguration : EntityTypeConfiguration<MetaData>
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
            Property(e => e.MetaDataText).IsRequired().HasMaxLength(250);
            Property(e => e.MetaDataDescription).IsOptional().HasMaxLength(3000);
            Property(e => e.ExtenalId).IsOptional();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();
            Property(e => e.SynchronizedDate).IsOptional();

            ToTable("tblMetaData");
        }

        #endregion
    }
}