namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldSettings : BusinessModel
    {
        private FieldSettings()
        {
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [NotNull]
        public FieldSetting UserId { get; private set; }

        [NotNull]
        public FieldSetting Domain { get; private set; }

        [NotNull]
        public FieldSetting LoginName { get; private set; }

        [NotNull]
        public FieldSetting FirstName { get; private set; }

        [NotNull]
        public FieldSetting Initials { get; private set; }

        [NotNull]
        public FieldSetting LastName { get; private set; }

        [NotNull]
        public FieldSetting DisplayName { get; private set; }

        [NotNull]
        public FieldSetting Place { get; private set; }

        [NotNull]
        public FieldSetting Phone { get; private set; }

        [NotNull]
        public FieldSetting CellPhone { get; private set; }

        [NotNull]
        public FieldSetting Email { get; private set; }

        [NotNull]
        public FieldSetting Code { get; private set; }

        [NotNull]
        public FieldSetting PostalAddress { get; private set; }

        [NotNull]
        public FieldSetting PostalCode { get; private set; }

        [NotNull]
        public FieldSetting City { get; private set; }

        [NotNull]
        public FieldSetting Title { get; private set; }

        [NotNull]
        public FieldSetting Region { get; private set; }

        [NotNull]
        public FieldSetting Department { get; private set; }

        [NotNull]
        public FieldSetting Unit { get; private set; }

        [NotNull]
        public FieldSetting OrganizationUnit { get; private set; }

        public FieldSetting CostCentre { get; private set; }

        [NotNull]
        public FieldSetting Division { get; private set; }

        [NotNull]
        public FieldSetting Manager { get; private set; }

        [NotNull]
        public FieldSetting Group { get; private set; }

        [NotNull]
        public FieldSetting Other { get; private set; }

        [NotNull]
        public FieldSetting Ordered { get; private set; }

        [NotNull]
        public FieldSetting CreatedDate { get; private set; }

        [NotNull]
        public FieldSetting ChangedDate { get; private set; }

        [NotNull]
        public FieldSetting SynchronizationDate { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDateAndTime { get; private set; }

        public bool IsEmpty { get; private set; }

        public static FieldSettings CreateForEdit(
            FieldSetting userId,
            FieldSetting domain,
            FieldSetting loginName,
            FieldSetting firstName,
            FieldSetting initials,
            FieldSetting lastName,
            FieldSetting displayName,
            FieldSetting place,
            FieldSetting phone,
            FieldSetting cellPhone,
            FieldSetting email,
            FieldSetting code,
            FieldSetting postalAddress,
            FieldSetting postalCode,
            FieldSetting city,
            FieldSetting title,
            FieldSetting region,
            FieldSetting department,
            FieldSetting unit,
            FieldSetting organizationUnit,
            FieldSetting costCentre,
            FieldSetting division,
            FieldSetting manager,
            FieldSetting group,
            FieldSetting other,
            FieldSetting ordered,
            FieldSetting createdDate,
            FieldSetting changedDate,
            FieldSetting synchronizationDate)
        {
            var settings = new FieldSettings
                           {
                               UserId = userId,
                               Domain = domain,
                               LoginName = loginName,
                               FirstName = firstName,
                               Initials = initials,
                               LastName = lastName,
                               DisplayName = displayName,
                               Place = place,
                               Phone = phone,
                               CellPhone = cellPhone,
                               Email = email,
                               Code = code,
                               PostalAddress = postalAddress,
                               PostalCode = postalCode,
                               City = city,
                               Title = title,
                               Region = region,
                               Department = department,
                               Unit = unit,
                               OrganizationUnit = organizationUnit,
                               CostCentre = costCentre,
                               Division = division,
                               Manager = manager,
                               Group = group,
                               Other = other,
                               Ordered = ordered,
                               CreatedDate = createdDate,
                               ChangedDate = changedDate,
                               SynchronizationDate = synchronizationDate
                           };

            settings.State = ModelStates.ForEdit;
            return settings;
        }

        public static FieldSettings CreateUpdated(
            int customerId,
            int languageId,
            FieldSetting userId,
            FieldSetting domain,
            FieldSetting loginName,
            FieldSetting firstName,
            FieldSetting initials,
            FieldSetting lastName,
            FieldSetting displayName,
            FieldSetting place,
            FieldSetting phone,
            FieldSetting cellPhone,
            FieldSetting email,
            FieldSetting code,
            FieldSetting postalAddress,
            FieldSetting postalCode,
            FieldSetting city,
            FieldSetting title,
            FieldSetting region,
            FieldSetting department,
            FieldSetting unit,
            FieldSetting organizationUnit,
            FieldSetting division,
            FieldSetting manager,
            FieldSetting group,
            FieldSetting other,
            FieldSetting ordered,
            FieldSetting createdDate,
            FieldSetting changedDate,
            FieldSetting synchronizationDate,
            DateTime changedDateAndTime)
        {
            var settings = new FieldSettings
                           {
                               CustomerId = customerId,
                               LanguageId = languageId,
                               UserId = userId,
                               Domain = domain,
                               LoginName = loginName,
                               FirstName = firstName,
                               Initials = initials,
                               LastName = lastName,
                               DisplayName = displayName,
                               Place = place,
                               Phone = phone,
                               CellPhone = cellPhone,
                               Email = email,
                               Code = code,
                               PostalAddress = postalAddress,
                               PostalCode = postalCode,
                               City = city,
                               Title = title,
                               Region = region,
                               Department = department,
                               Unit = unit,
                               OrganizationUnit = organizationUnit,
                               Division = division,
                               Manager = manager,
                               Group = group,
                               Other = other,
                               Ordered = ordered,
                               CreatedDate = createdDate,
                               ChangedDate = changedDate,
                               SynchronizationDate = synchronizationDate,
                               ChangedDateAndTime = changedDateAndTime
                           };

            settings.State = ModelStates.Updated;
            return settings;
        }

        public static FieldSettings CreateEmpty()
        {
            var empty = new FieldSettings();
            empty.MarkAsEmpty();
            return empty;
        }

        public void MarkAsEmpty()
        {
            this.IsEmpty = true;
        }
    }
}