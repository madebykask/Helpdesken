namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ContractHistoryConfiguration : EntityTypeConfiguration<ContractHistory>
    {
        internal ContractHistoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.Contract_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ContractCategory)
                .WithMany()
                .HasForeignKey(x => x.ContractCategory_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Supplier)
               .WithMany()
               .HasForeignKey(x => x.Supplier_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ResponsibleUser)
               .WithMany()
               .HasForeignKey(x => x.ResponsibleUser_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FollowUpResponsibleUser)
               .WithMany()
               .HasForeignKey(x => x.FollowUpResponsibleUser_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Contract_Id).IsRequired();
            this.Property(x => x.CreatedByUser_Id).IsOptional();
            this.Property(x => x.ContractCategory_Id).IsRequired();
            this.Property(x => x.ContractEndDate).IsOptional();
            this.Property(x => x.ContractStartDate).IsOptional();
            this.Property(x => x.Department_Id).IsOptional();
            this.Property(x => x.Finished).IsRequired();
            this.Property(x => x.FollowUpInterval).IsRequired();
            this.Property(x => x.FollowUpResponsibleUser_Id).IsOptional();
            this.Property(x => x.NoticeDate).IsOptional();
            this.Property(x => x.ResponsibleUser_Id).IsOptional();
            this.Property(x => x.Supplier_Id).IsOptional();
            this.Property(x => x.NoticeTime).IsRequired();
            this.Property(x => x.Running).IsRequired();
            this.Property(x => x.Info).IsOptional();
            this.Property(x => x.Files).IsOptional();
            this.Property(x => x.ContractNumber).IsRequired();
            this.Property(x => x.CreatedDate);

            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblContractHistory");
        }
    }
}