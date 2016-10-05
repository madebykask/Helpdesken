namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ContractFieldSettingConfiguration : EntityTypeConfiguration<ContractFieldSettings>
    {
        internal ContractFieldSettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.ContractField).IsRequired();
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.ShowInList).IsRequired();
            this.Property(x => x.ShowExternal).IsRequired();
            this.Property(x => x.Label).IsRequired();
            this.Property(x => x.Label_ENG).IsRequired();
            this.Property(x => x.FieldHelp).IsOptional();
            this.Property(x => x.Required).IsRequired();        
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            //this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblContractFieldSettings");
        }
    }
}