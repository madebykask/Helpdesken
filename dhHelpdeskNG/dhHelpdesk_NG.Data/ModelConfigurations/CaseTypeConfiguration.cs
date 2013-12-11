using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseTypeConfiguration : EntityTypeConfiguration<CaseType>
    {
        internal CaseTypeConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Administrator)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ParentCaseType)
                .WithMany(x => x.SubCaseTypes)
                .HasForeignKey(x => x.Parent_CaseType_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.AutomaticApproveTime).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Form_Id).IsOptional();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            Property(x => x.IsEMailDefault).IsRequired().HasColumnName("isEMailDefault");
            Property(x => x.ITILProcess).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseType");
            Property(x => x.Parent_CaseType_Id).IsOptional();
            Property(x => x.RequireApproving).IsRequired();
            Property(x => x.Selectable).IsRequired();
            Property(x => x.ShowOnExternalPage).IsRequired();
            Property(x => x.User_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;

            ToTable("tblcasetype");
        }
    }
}
