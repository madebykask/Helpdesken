namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseTypeConfiguration : EntityTypeConfiguration<CaseType>
    {
        internal CaseTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Administrator)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ParentCaseType)
                .WithMany(x => x.SubCaseTypes)
                .HasForeignKey(x => x.Parent_CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AutomaticApproveTime).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Form_Id).IsOptional();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired().HasColumnName("isDefault");
            this.Property(x => x.IsEMailDefault).IsRequired().HasColumnName("isEMailDefault");
            this.Property(x => x.ITILProcess).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseType");
            this.Property(x => x.Parent_CaseType_Id).IsOptional();
            this.Property(x => x.RequireApproving).IsRequired();
            this.Property(x => x.Selectable).IsRequired();
            this.Property(x => x.ShowOnExternalPage).IsRequired();
            this.Property(x => x.User_Id).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.RelatedField).IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;
            this.Property(x => x.CaseTypeGUID).IsOptional();
            this.Property(x => x.ShowOnExtPageCases).IsRequired();
            this.Property(x => x.WorkingGroup_Id).IsOptional();

            this.ToTable("tblcasetype");
        }
    }
}
