namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ContractCategoryConfiguration : EntityTypeConfiguration<ContractCategory>
    {
        internal ContractCategoryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.CreateCase_CaseType)
                .WithMany()
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.StateSecondary)
                .WithMany()
                .HasForeignKey(x => x.StateSecondary_Id1)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.StateSecondary)
               .WithMany()
               .HasForeignKey(x => x.StateSecondary_Id2)
               .WillCascadeOnDelete(false);

            this.Property(x => x.CaseType_Id).IsOptional().HasColumnName("CreateCase_CaseType_Id");
            this.Property(x => x.Case_UserId).IsOptional().HasMaxLength(40).HasColumnName("CreateCase_UserId");
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ContractCategory");
            this.Property(x => x.StateSecondary_Id1).IsOptional().HasColumnName("CreateCase_StateSecondary_Id1");
            this.Property(x => x.StateSecondary_Id2).IsOptional().HasColumnName("CreateCase_StateSecondary_Id2");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcontractcategory");
        }
    }
}