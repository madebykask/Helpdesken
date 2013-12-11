using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class AccountActivityConfiguration : EntityTypeConfiguration<AccountActivity>
    {
        internal AccountActivityConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.AccountActivityGroup)
                .WithMany()
                .HasForeignKey(x => x.AccountActivityGroup_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CreateCase)
                .WithMany()
                .HasForeignKey(x => x.CreateCase_CaseType_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            HasOptional(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.Document_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.AccountActivityGroup_Id).IsOptional();
            Property(x => x.AccountInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionAccountInfo");
            Property(x => x.ContactInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionContactInfo");
            Property(x => x.CreateCase_CaseType_Id).IsOptional();
            Property(x => x.Customer_Id).IsOptional();
            Property(x => x.DeliveryInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionDeliveryInfo");
            Property(x => x.Description).IsOptional().HasMaxLength(1000).HasColumnName("AccountActivityDescription");
            Property(x => x.Document_Id).IsOptional();
            Property(x => x.EMail).IsOptional().HasMaxLength(100);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("AccountActivity");
            Property(x => x.OrdererInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionOrdererInfo");
            Property(x => x.ProgramInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionProgramInfo");
            Property(x => x.SetAccountFinishingDate).IsRequired();
            Property(x => x.URL).IsOptional().HasMaxLength(100);
            Property(x => x.UserInfo).IsOptional().HasMaxLength(30).HasColumnName("CaptionUserInfo");
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblaccountactivity");
        }
    }

    public class AccountActivityGroupConfiguration : EntityTypeConfiguration<AccountActivityGroup>
    {
        internal AccountActivityGroupConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("AccountActivityGroup");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblaccountactivitygroup");
        }
    }
}
