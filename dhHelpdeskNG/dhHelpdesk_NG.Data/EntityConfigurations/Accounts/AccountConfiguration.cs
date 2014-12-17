namespace DH.Helpdesk.Dal.EntityConfigurations.Accounts
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Accounts;

    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        internal AccountConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.AccountActivity)
                .WithMany()
                .HasForeignKey(x => x.AccountActivity_Id)
                .WillCascadeOnDelete(false);

            //this.HasRequired(x => x.AccountTypeEntity2)
            //    .WithMany()
            //    .HasForeignKey(x => x.AccountType2)
            //    .WillCascadeOnDelete(false);

            //this.HasRequired(x => x.AccountTypeEntity3)
            //    .WithMany()
            //    .HasForeignKey(x => x.AccountType3)
            //    .WillCascadeOnDelete(false);

            //this.HasRequired(x => x.AccountTypeEntity4)
            //    .WithMany()
            //    .HasForeignKey(x => x.AccountType4)
            //    .WillCascadeOnDelete(false);

            this.HasOptional(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department2)
                .WithMany()
                .HasForeignKey(x => x.Department_Id2)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.OU)
                .WithMany()
                .HasForeignKey(x => x.OU_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.EmploymentType).IsRequired();

            this.Property(x => x.CreatedByUser_Id).IsOptional();

            this.Property(x => x.ChangedByUser_Id).IsRequired();

            this.HasMany(o => o.Programs)
               .WithMany(p => p.Accounts)
               .Map(m =>
               {
                   m.MapLeftKey("Account_Id");
                   m.MapRightKey("Program_Id");
                   m.ToTable("tblAccount_tblProgram");
               });

            this.HasMany(s => s.AccountEMailLogs)
                .WithRequired(s => s.Account)
                .HasForeignKey(s => s.Account_Id);

            this.Property(x => x.OrdererId).IsOptional().HasMaxLength(20);
            this.Property(x => x.OrdererFirstName).IsOptional().HasMaxLength(20);
            this.Property(x => x.OrdererLastName).IsOptional().HasMaxLength(50);
            this.Property(x => x.OrdererPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.OrdererEmail).IsOptional().HasMaxLength(50);

            this.Property(x => x.UserId).IsOptional().HasMaxLength(200);
            this.Property(x => x.UserFirstName).IsOptional().HasMaxLength(40);
            this.Property(x => x.UserInitials).IsOptional().HasMaxLength(10);
            this.Property(x => x.UserLastName).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserPersonalIdentityNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.UserPhone).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserExtension).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserEMail).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserTitle).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserLocation).IsOptional().HasMaxLength(50);
            this.Property(x => x.UserRoomNumber).IsOptional().HasMaxLength(20);
            this.Property(x => x.UserPostalAddress).IsOptional().HasMaxLength(50);

            this.Property(x => x.Responsibility).IsOptional().HasMaxLength(50);
            this.Property(x => x.Activity).IsOptional().HasMaxLength(50);
            this.Property(x => x.Manager).IsOptional().HasMaxLength(50);
            this.Property(x => x.ReferenceNumber).IsOptional().HasMaxLength(200);
            this.Property(x => x.InfoUser).IsOptional().HasMaxLength(20);
            this.Property(x => x.AccountStartDate).IsOptional();
            this.Property(x => x.AccountEndDate).IsOptional();

            this.Property(x => x.EMailType).IsRequired();
            this.Property(x => x.HomeDirectory).IsRequired();
            this.Property(x => x.Profile).IsRequired();

            this.Property(x => x.InventoryNumber).IsOptional().HasMaxLength(20);
            this.Property(x => x.AccountType2).IsOptional().HasMaxLength(50);
            this.Property(x => x.AccountType3).IsOptional();
            this.Property(x => x.AccountType4).IsOptional();
            this.Property(x => x.AccountType5).IsOptional();

            this.Property(x => x.Info).IsOptional().HasMaxLength(500);
            this.Property(x => x.ContactId).IsOptional().HasMaxLength(200);
            this.Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactEMail).IsOptional().HasMaxLength(50);
            this.Property(x => x.InfoProduct).IsOptional().HasMaxLength(500);

            this.Property(x => x.CaseNumber).IsRequired();

            this.Property(x => x.FinishingDate).IsOptional();
            this.Property(x => x.AccountFile).IsOptional();
            this.Property(x => x.AccountFileName).IsOptional().HasMaxLength(100);
            this.Property(x => x.AccountFileContentType).IsOptional().HasMaxLength(100);
            this.Property(x => x.InfoOther).IsOptional().HasMaxLength(200);

            this.Property(x => x.DeliveryName).IsOptional().HasMaxLength(50);
            this.Property(x => x.DeliveryPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.DeliveryAddress).IsOptional().HasMaxLength(50);
            this.Property(x => x.DeliveryPostalAddress).IsOptional().HasMaxLength(50);

            this.Property(x => x.Info).IsOptional().HasMaxLength(500);
            this.Property(x => x.Info).IsOptional().HasMaxLength(500);
            this.Property(x => x.Info).IsOptional().HasMaxLength(500);
            this.Property(x => x.Info).IsOptional().HasMaxLength(500);

            this.Property(x => x.Export).IsRequired();

            this.Property(x => x.Deleted).IsRequired();

            this.Property(x => x.ChangedDate).IsRequired();
            this.Property(x => x.CreatedDate).IsRequired();

            this.ToTable("tblaccount");
        }
    }
}
