using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ContractCategoryConfiguration : EntityTypeConfiguration<ContractCategory>
    {
        internal ContractCategoryConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.CreateCase_CaseType)
                .WithMany()
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.StateSecondary)
                .WithMany()
                .HasForeignKey(x => x.StateSecondary_Id1)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.StateSecondary)
               .WithMany()
               .HasForeignKey(x => x.StateSecondary_Id2)
               .WillCascadeOnDelete(false);

            Property(x => x.CaseType_Id).IsOptional().HasColumnName("CreateCase_CaseType_Id");
            Property(x => x.Case_UserId).IsOptional().HasMaxLength(40).HasColumnName("CreateCase_UserId");
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("ContractCategory");
            Property(x => x.StateSecondary_Id1).IsOptional().HasColumnName("CreateCase_StateSecondary_Id1");
            Property(x => x.StateSecondary_Id2).IsOptional().HasColumnName("CreateCase_StateSecondary_Id2");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcontractcategory");
        }
    }
}