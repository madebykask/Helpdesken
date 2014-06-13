namespace DH.Helpdesk.Dal.EntityConfigurations
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
   
    using DH.Helpdesk.Domain;

    public class ActionSettingConfiguration : EntityTypeConfiguration<ActionSettingEntity>
    {
        internal ActionSettingConfiguration()
        {
            this.HasKey(a => new {a.Customer_Id , a.ObjectId});
            
            this.Property(a => a.ObjectValue).IsRequired().HasMaxLength(800);
            this.Property(a => a.ObjectClass).IsOptional();
            this.Property(a => a.Visibled);
            
            this.ToTable("tblActionSetting");
        }
    }
    
}
