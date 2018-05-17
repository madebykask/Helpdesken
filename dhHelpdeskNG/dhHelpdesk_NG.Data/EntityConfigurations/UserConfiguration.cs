namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        internal UserConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.AAs)
                .WithMany(a => a.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("AccountActivity_Id");
                    m.ToTable("tblUsers_tblAccountActivity");
                });

            this.HasOptional(o => o.ChangedByUser)
                .WithMany()
                .Map(conf => conf.MapKey("ChangedByUser_Id"))
                .WillCascadeOnDelete(false);

            this.HasMany(u => u.Departments)
                .WithMany(a => a.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Department_Id");
                    m.ToTable("tblDepartmentUser");
                });

            this.HasMany(x => x.Documents)
                .WithMany(x => x.Us)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("Document_Id")
                    .ToTable("tblDocument_tblUser");
                });

            this.HasOptional(o => o.Domain)
                .WithMany()
                .HasForeignKey(o => o.Domain_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(o => o.Language)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.Language_Id);

            this.HasRequired(o => o.UserGroup)
                .WithMany()
                .HasForeignKey(o => o.UserGroup_Id);

            this.HasMany(o => o.OTs)
                .WithMany(o => o.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("OrderType_Id");
                    m.ToTable("tblUsers_tblOrderType");
                });

            this.HasRequired(o => o.Language)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.Language_Id);

            this.HasMany(o => o.UserRoles)
                .WithMany(o => o.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("UserRole_Id")
                    .ToTable("tblUsers_tblUserRole");
                });

            this.HasMany(o => o.OLs)
                .WithMany(o => o.Us)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("OperationLog_Id")
                    .ToTable("tblOperationLog_tblUsers");
                });

            this.HasMany(o => o.UserWorkingGroups)
                .WithRequired(o => o.User)
                .HasForeignKey(o => o.User_Id).WillCascadeOnDelete(false);

            this.Property(x => x.Address).IsRequired().HasMaxLength(50);
            this.Property(x => x.AllocateCaseMail).IsRequired();
            this.Property(x => x.AllocateCaseSMS).IsRequired();
            this.Property(x => x.ArticleNumber).IsRequired().HasMaxLength(10);
            this.Property(x => x.BulletinBoardDate);
            this.Property(x => x.BulletinBoardPermission).IsRequired();
            this.Property(x => x.CalendarPermission).IsRequired();
            this.Property(x => x.CaseInfoMail).IsRequired();
            this.Property(x => x.CaseSolutionPermission).IsRequired();
            //Property(x => x.CaseStateSecondaryColor).IsRequired().HasMaxLength(10);
            this.Property(x => x.CellPhone).IsRequired().HasMaxLength(20);
            this.Property(x => x.ChangeTime);
            this.Property(x => x.CloseCasePermission).IsRequired();
            this.Property(x => x.CopyCasePermission).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.DailyReportReminder).IsRequired();
            this.Property(x => x.DeleteCasePermission).IsRequired();
            this.Property(x => x.DeleteAttachedFilePermission).IsRequired();
            this.Property(x => x.Domain_Id).IsOptional();
            this.Property(x => x.Email).IsRequired().HasMaxLength(50);
            this.Property(x => x.ExternalUpdateMail).IsRequired();
            this.Property(x => x.FAQPermission).IsRequired();
            this.Property(x => x.FirstName).IsRequired().HasMaxLength(20);
            this.Property(x => x.FollowUpPermission).IsRequired();
            this.Property(x => x.InvoicePermission).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Language_Id).IsRequired();
            this.Property(x => x.Logo).IsRequired().HasMaxLength(50);
            this.Property(x => x.LogoBackColor).IsRequired().HasMaxLength(10);
            this.Property(x => x.MarkRequiredFields).IsRequired();
            //Property(x => x.MenuSettings).IsRequired().HasMaxLength(200);
            this.Property(x => x.MoveCasePermission).IsRequired();
            this.Property(x => x.OrderPermission).IsRequired();
            this.Property(x => x.Password).IsRequired().HasMaxLength(20);
            this.Property(x => x.PasswordChangedDate);
            this.Property(x => x.Performer).IsRequired();
            this.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            this.Property(x => x.PlanDateMail).IsRequired();
            this.Property(x => x.PostalAddress).IsRequired().HasMaxLength(30);
            this.Property(x => x.PostalCode).IsRequired().HasMaxLength(5);
            this.Property(x => x.RefreshContent).IsRequired();
            this.Property(x => x.RegTime);
            this.Property(x => x.ReportPermission).IsRequired();
            this.Property(x => x.RestrictedCasePermission).IsRequired();
            this.Property(x => x.SessionTimeout).IsRequired();
            this.Property(x => x.SetPriorityPermission).IsRequired();
            this.Property(x => x.ShowNotAssignedCases).IsRequired();
            this.Property(x => x.ShowNotAssignedWorkingGroups).IsRequired();
            this.Property(x => x.ShowQuickMenuOnStartPage).IsRequired();
            this.Property(x => x.StartPage).IsRequired();
            this.Property(x => x.SurName).IsRequired().HasMaxLength(30);
            this.Property(x => x.TimeAutoAuthorize).IsRequired();
            this.Property(x => x.TimeRegistration).IsRequired();
           // Property(x => x.UserGroup_Id).IsRequired();
            this.Property(x => x.UserID).IsRequired().HasColumnName("UserId").HasMaxLength(40);
            this.Property(x => x.WatchDateMail).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(u => u.ShowSolutionTime).IsRequired();
            this.Property(u => u.TimeZoneId).IsOptional();
            this.Property(u => u.ShowCaseStatistics).IsRequired();
            this.Property(x => x.DocumentPermission).IsRequired();
            this.Property(x => x.InventoryPermission).IsRequired();
            this.Property(x => x.ContractPermission).IsRequired();
            this.Property(x => x.SettingForNoMail).IsRequired();

            this.Property(x => x.UserGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(x => x.CaseUnlockPermission).IsRequired();
            this.Property(x => x.CaseInternalLogPermission).IsRequired();

            this.ToTable("tblUsers");
        }
    }
}
