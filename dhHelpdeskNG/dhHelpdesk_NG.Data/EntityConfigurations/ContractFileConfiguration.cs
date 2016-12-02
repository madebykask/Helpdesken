namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ContractFileConfiguration : EntityTypeConfiguration<ContractFile>
    {
        internal ContractFileConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.Contract_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.ArchivedContractFile_Id)
                .WillCascadeOnDelete(false);       

            Property(x => x.Contract_Id).IsRequired();
            Property(x => x.File).IsRequired().HasColumnName("ContractFile");
            Property(x => x.ContractFileGUID).IsRequired();
            Property(x => x.FileName).IsRequired().HasMaxLength(250); ;
            Property(x => x.ContentType).IsRequired().HasMaxLength(100); ;
            Property(x => x.ArchivedDate).IsOptional();
            Property(x => x.CreatedDate);
            Property(x => x.ArchivedContractFile_Id).IsOptional();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblContractFile");
        }
    }
}