namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Accounts;

    public class AccountActivityConfiguration : EntityTypeConfiguration<AccountActivity>
    {
        internal AccountActivityConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.AccountActivityGroup)
                .WithMany()
                .HasForeignKey(x => x.AccountActivityGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CreateCase)
                .WithMany()
                .HasForeignKey(x => x.CreateCase_CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AccountActivityGroup_Id).IsOptional();
            this.Property(x => x.AccountInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionAccountInfo");
            this.Property(x => x.ContactInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionContactInfo");
            this.Property(x => x.CreateCase_CaseType_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsOptional();
            this.Property(x => x.DeliveryInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionDeliveryInfo");
            this.Property(x => x.Description).IsOptional().HasMaxLength(1000).HasColumnName("AccountActivityDescription");
            this.Property(x => x.Document_Id).IsOptional();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.EMail).IsOptional().HasMaxLength(100);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("AccountActivity");
            this.Property(x => x.OrdererInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionOrdererInfo");
            this.Property(x => x.ProgramInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionProgramInfo");
            this.Property(x => x.SetAccountFinishingDate).IsRequired();
            this.Property(x => x.URL).IsOptional().HasMaxLength(100);
            this.Property(x => x.UserInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionUserInfo");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblaccountactivity");
        }
    }
}
