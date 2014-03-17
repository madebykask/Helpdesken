namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers;

    public sealed class NotifierModelFactory : INotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        public InputModel Create(
            DisplayFieldSettings settings,
            int? selectedRegionId,
            NotifierDetails notifier,
            List<ItemOverview> domains,
            List<ItemOverview> regions,
            List<ItemOverview> departments,
            List<ItemOverview> organizationUnits,
            List<ItemOverview> divisions,
            List<ItemOverview> managers,
            List<ItemOverview> groups)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.UserId, notifier.UserId);

            ConfigurableFieldModel<DropDownContent> domain;

            if (domains != null)
            {
                var domainItems = domains.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var domainValue = notifier.DomainId.HasValue
                    ? notifier.DomainId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                domain = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Domain,
                    domainItems,
                    domainValue);
            }
            else
            {
                domain = new ConfigurableFieldModel<DropDownContent>(false);
            }

            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.LoginName,
                notifier.LoginName);

            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.FirstName,
                notifier.FirstName);

            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.Initials,
                notifier.Initials);

            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.LastName,
                notifier.LastName);

            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.DisplayName,
                notifier.DisplayName);

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Place, notifier.Place);

            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Phone, notifier.Phone);

            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.CellPhone,
                notifier.CellPhone);

            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Email, notifier.Email);

            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Code, notifier.Code);

            var postalAddress = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.PostalAddress,
                notifier.PostalAddress);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.PostalCode,
                notifier.PostalCode);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.City, notifier.City);

            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Title, notifier.Title);

            ConfigurableFieldModel<DropDownContent> region;
            ConfigurableFieldModel<DropDownContent> department;

            if (regions != null)
            {
                var regionItems = regions.Select(r => new KeyValuePair<string, string>(r.Value, r.Name)).ToList();
                var regionValue = selectedRegionId.HasValue ? selectedRegionId.ToString() : null;

                region = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Region,
                    regionItems,
                    regionValue);
            }
            else
            {
                region = new ConfigurableFieldModel<DropDownContent>(false);
            }

            if (departments != null)
            {
                var departmentItems =
                    departments.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var departmentValue = notifier.DepartmentId.HasValue
                    ? notifier.DepartmentId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                department = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Department,
                    departmentItems,
                    departmentValue);
            }
            else
            {
                department = new ConfigurableFieldModel<DropDownContent>(false);
            }

            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Unit, notifier.Unit);

            ConfigurableFieldModel<DropDownContent> organizationUnit;

            if (organizationUnits != null)
            {
                var organizationUnitItems =
                    organizationUnits.Select(u => new KeyValuePair<string, string>(u.Value, u.Name)).ToList();

                var organizationUnitValue = notifier.OrganizationUnitId.HasValue
                    ? notifier.OrganizationUnitId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                organizationUnit = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.OrganizationUnit,
                    organizationUnitItems,
                    organizationUnitValue);
            }
            else
            {
                organizationUnit = new ConfigurableFieldModel<DropDownContent>(false);
            }

            ConfigurableFieldModel<DropDownContent> division;

            if (divisions != null)
            {
                var divisionItems = divisions.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var divisionValue = notifier.DivisionId.HasValue
                    ? notifier.DivisionId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                division = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Division,
                    divisionItems,
                    divisionValue);
            }
            else
            {
                division = new ConfigurableFieldModel<DropDownContent>(false);
            }

            ConfigurableFieldModel<DropDownContent> manager;

            if (managers != null)
            {
                var managerItems = managers.Select(m => new KeyValuePair<string, string>(m.Value, m.Name)).ToList();

                var managerValue = notifier.ManagerId.HasValue
                    ? notifier.ManagerId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                manager = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Manager,
                    managerItems,
                    managerValue);
            }
            else
            {
                manager = new ConfigurableFieldModel<DropDownContent>(false);
            }

            ConfigurableFieldModel<DropDownContent> group;

            if (groups != null)
            {
                var groupItems = groups.Select(g => new KeyValuePair<string, string>(g.Value, g.Name)).ToList();

                var groupValue = notifier.GroupId.HasValue
                    ? notifier.GroupId.Value.ToString(CultureInfo.InvariantCulture)
                    : null;

                group = this.notifierInputFieldModelFactory.CreateDropDownModel(settings.Group, groupItems, groupValue);
            }
            else
            {
                group = new ConfigurableFieldModel<DropDownContent>(false);
            }

            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Other, notifier.Other);

            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(
                settings.Ordered,
                notifier.Ordered);

            var createdDate = this.notifierInputFieldModelFactory.CreateLabelModel(
                settings.CreatedDate,
                notifier.CreatedDate.ToString(CultureInfo.InvariantCulture));

            var changedDate = this.notifierInputFieldModelFactory.CreateLabelModel(
                settings.ChangedDate,
                notifier.ChangedDate.ToString(CultureInfo.InvariantCulture));

            string synchronizationDateValue;

            if (notifier.SynchronizationDate.HasValue)
            {
                synchronizationDateValue = notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                synchronizationDateValue = string.Empty;
            }

            var synchronizationDate = this.notifierInputFieldModelFactory.CreateLabelModel(
                settings.SynchronizationDate,
                synchronizationDateValue);

            return new InputModel(
                notifier.Id,
                userId,
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                region,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                other,
                ordered,
                notifier.IsActive,
                createdDate,
                changedDate,
                synchronizationDate);
        }
    }
}