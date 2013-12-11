using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        internal UserConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.AAs)
                .WithMany(a => a.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("AccountActivity_Id");
                    m.ToTable("tblUsers_tblAccountActivity");
                });

            HasOptional(o => o.ChangedByUser)
                .WithMany()
                .Map(conf => conf.MapKey("ChangedByUser_Id"))
                .WillCascadeOnDelete(false);

            HasMany(u => u.Departments)
                .WithMany(a => a.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Department_Id");
                    m.ToTable("tblDepartmentUser");
                });

            HasMany(x => x.Documents)
                .WithMany(x => x.Us)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("Document_Id")
                    .ToTable("tblDocument_tblUser");
                });

            HasOptional(o => o.Domain)
                .WithMany()
                .HasForeignKey(o => o.Domain_Id)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Language)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.Language_Id);

            HasRequired(o => o.UserGroup)
                .WithMany()
                .HasForeignKey(o => o.UserGroup_Id);

            HasMany(o => o.OTs)
                .WithMany(o => o.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("OrderType_Id");
                    m.ToTable("tblUsers_tblOrderType");
                });

            HasRequired(o => o.Language)
                .WithMany(o => o.Users)
                .HasForeignKey(o => o.Language_Id);

            HasMany(o => o.UserRoles)
                .WithMany(o => o.Users)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("UserRole_Id")
                    .ToTable("tblUsers_tblUserRole");
                });

            HasMany(o => o.OLs)
                .WithMany(o => o.User)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id")
                    .MapRightKey("OperationLog_Id")
                    .ToTable("tblOperationLog_tblUsers");
                });

            HasMany(o => o.UserWorkingGroups)
                .WithRequired(o => o.User)
                .HasForeignKey(o => o.User_Id).WillCascadeOnDelete(false);

            Property(x => x.Address).IsRequired().HasMaxLength(50);
            Property(x => x.AllocateCaseMail).IsRequired();
            Property(x => x.AllocateCaseSMS).IsRequired();
            Property(x => x.ArticleNumber).IsRequired().HasMaxLength(10);
            Property(x => x.BulletinBoardDate);
            Property(x => x.BulletinBoardPermission).IsRequired();
            Property(x => x.CalendarPermission).IsRequired();
            Property(x => x.CaseInfoMail).IsRequired();
            Property(x => x.CaseSolutionPermission).IsRequired();
            Property(x => x.CaseStateSecondaryColor).IsRequired().HasMaxLength(10);
            Property(x => x.CellPhone).IsRequired().HasMaxLength(20);
            Property(x => x.ChangeTime);
            Property(x => x.CloseCasePermission).IsRequired();
            Property(x => x.CopyCasePermission).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.DailyReportReminder).IsRequired();
            Property(x => x.DeleteCasePermission).IsRequired();
            Property(x => x.DeleteAttachedFilePermission).IsRequired();
            Property(x => x.Domain_Id).IsOptional();
            Property(x => x.Email).IsRequired().HasMaxLength(50);
            Property(x => x.ExternalUpdateMail).IsRequired();
            Property(x => x.FAQPermission).IsRequired();
            Property(x => x.FirstName).IsRequired().HasMaxLength(20);
            Property(x => x.FollowUpPermission).IsRequired();
            Property(x => x.InvoicePermission).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Language_Id).IsRequired();
            Property(x => x.Logo).IsRequired().HasMaxLength(50);
            Property(x => x.LogoBackColor).IsRequired().HasMaxLength(10);
            Property(x => x.MarkRequiredFields).IsRequired();
            Property(x => x.MenuSettings).IsRequired().HasMaxLength(200);
            Property(x => x.MoveCasePermission).IsRequired();
            Property(x => x.OrderPermission).IsRequired();
            Property(x => x.Password).IsRequired().HasMaxLength(20);
            Property(x => x.PasswordChangedDate);
            Property(x => x.Performer).IsRequired();
            Property(x => x.Phone).IsRequired().HasMaxLength(20);
            Property(x => x.PlanDateMail).IsRequired();
            Property(x => x.PostalAddress).IsRequired().HasMaxLength(30);
            Property(x => x.PostalCode).IsRequired().HasMaxLength(5);
            Property(x => x.RefreshContent).IsRequired();
            Property(x => x.RegTime);
            Property(x => x.ReportPermission).IsRequired();
            Property(x => x.RestrictedCasePermission).IsRequired();
            Property(x => x.SessionTimeout).IsRequired();
            Property(x => x.SetPriorityPermission).IsRequired();
            Property(x => x.ShowNotAssignedCases).IsRequired();
            Property(x => x.ShowNotAssignedWorkingGroups).IsRequired();
            Property(x => x.ShowQuickMenuOnStartPage).IsRequired();
            Property(x => x.StartPage).IsRequired();
            Property(x => x.SurName).IsRequired().HasMaxLength(30);
            Property(x => x.TimeAutoAuthorize).IsRequired();
            Property(x => x.TimeRegistration).IsRequired();
           // Property(x => x.UserGroup_Id).IsRequired();
            Property(x => x.UserID).IsRequired().HasColumnName("UserId").HasMaxLength(20);
            Property(x => x.WatchDateMail).IsRequired();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblUsers");
        }
    }
}
