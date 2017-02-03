namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettingsOverview
    {
        public UserFieldSettingsOverview(FieldOverviewSetting userId,
            FieldOverviewSetting userFirstName,
            FieldOverviewSetting userLastName,
            FieldOverviewSetting userPhone,
            FieldOverviewSetting userEMail,
            FieldOverviewSetting userInitials,
            FieldOverviewSetting userPersonalIdentityNumber,
            FieldOverviewSetting userExtension,
            FieldOverviewSetting userTitle,
            FieldOverviewSetting userLocation,
            FieldOverviewSetting userRoomNumber,
            FieldOverviewSetting userPostalAddress,
            FieldOverviewSetting responsibility,
            FieldOverviewSetting activity,
            FieldOverviewSetting manager,
            FieldOverviewSetting referenceNumber,
            FieldOverviewSetting infoUser,
            FieldOverviewSetting userOuId,
            FieldOverviewSetting employmentType,
            FieldOverviewSetting userDepartmentId1,
            FieldOverviewSetting userDepartmentId2)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
            UserInitials = userInitials;
            UserPersonalIdentityNumber = userPersonalIdentityNumber;
            UserExtension = userExtension;
            UserTitle = userTitle;
            UserLocation = userLocation;
            UserRoomNumber = userRoomNumber;
            UserPostalAddress = userPostalAddress;
            Responsibility = responsibility;
            Activity = activity;
            Manager = manager;
            ReferenceNumber = referenceNumber;
            InfoUser = infoUser;
            UserOU_Id = userOuId;
            EmploymentType = employmentType;
            UserDepartment_Id1 = userDepartmentId1;
            UserDepartment_Id2 = userDepartmentId2;
        }

        [NotNull]
        public FieldOverviewSetting UserId { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserFirstName { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserLastName { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserEMail { get; private set; }


        [NotNull]
        public FieldOverviewSetting UserInitials { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserPersonalIdentityNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserExtension { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserTitle { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserLocation { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserRoomNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserPostalAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting Responsibility { get; private set; }

        [NotNull]
        public FieldOverviewSetting Activity { get; private set; }

        [NotNull]
        public FieldOverviewSetting Manager { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReferenceNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting InfoUser { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserOU_Id { get; private set; }

        [NotNull]
        public FieldOverviewSetting EmploymentType { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserDepartment_Id1 { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserDepartment_Id2 { get; private set; }

    }
}