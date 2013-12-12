namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifierModelFactory : INotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        public NotifierInputModel Create(
            DisplayFieldsSettingsDto displaySettings,
            DisplayNotifierDto notifier,
            List<ItemOverviewDto> domains,
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions,
            List<ItemOverviewDto> managers,
            List<ItemOverviewDto> groups)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.UserId, notifier.UserId);

            var domainItems = domains.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

            var domainValue = notifier.DomainId.HasValue
                                  ? notifier.DomainId.Value.ToString(CultureInfo.InvariantCulture)
                                  : null;

            var domain = this.notifierInputFieldModelFactory.CreteDropDownModel(
                displaySettings.Domain, domainItems, domainValue);

            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.LoginName, notifier.LoginName);

            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.FirstName, notifier.FirstName);

            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Initials, notifier.Initials);

            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.LastName, notifier.LastName);

            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.DisplayName, notifier.DisplayName);

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Place, notifier.Place);

            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Phone, notifier.Phone);

            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.CellPhone, notifier.CellPhone);

            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Email, notifier.Email);

            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Code, notifier.Code);

            var postalAddress =
                this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                    displaySettings.PostalAddress, notifier.PostalAddress);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.PostalCode, notifier.PostalCode);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.City, notifier.City);

            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Title, notifier.Title);

            var regionItems = regions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
            var regionContent = new DropDownContent(regionItems);

            var departmentItems = departments.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

            var departmentValue = notifier.DepartmentId.HasValue
                                      ? notifier.DepartmentId.Value.ToString(CultureInfo.InvariantCulture)
                                      : null;

            var department = this.notifierInputFieldModelFactory.CreteDropDownModel(
                displaySettings.Department, departmentItems, departmentValue);

            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Unit, notifier.Unit);

            var organizationUnitItems =
                organizationUnits.Select(u => new KeyValuePair<string, string>(u.Value, u.Name)).ToList();

            var organizationUnitValue = notifier.OrganizationUnitId.HasValue
                                            ? notifier.OrganizationUnitId.Value.ToString(CultureInfo.InvariantCulture)
                                            : null;

            var organizationUnit =
                this.notifierInputFieldModelFactory.CreteDropDownModel(
                    displaySettings.OrganizationUnit, organizationUnitItems, organizationUnitValue);

            var divisionItems = divisions.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

            var divisionValue = notifier.DivisionId.HasValue
                                    ? notifier.DivisionId.Value.ToString(CultureInfo.InvariantCulture)
                                    : null;

            var division = this.notifierInputFieldModelFactory.CreteDropDownModel(
                displaySettings.Division, divisionItems, divisionValue);

            var managerItems = managers.Select(m => new KeyValuePair<string, string>(m.Value, m.Name)).ToList();

            var managerValue = notifier.ManagerId.HasValue
                                   ? notifier.ManagerId.Value.ToString(CultureInfo.InvariantCulture)
                                   : null;

            var manager = this.notifierInputFieldModelFactory.CreteDropDownModel(
                displaySettings.Manager, managerItems, managerValue);

            var groupItems = groups.Select(g => new KeyValuePair<string, string>(g.Value, g.Name)).ToList();

            var groupValue = notifier.GroupId.HasValue
                                   ? notifier.GroupId.Value.ToString(CultureInfo.InvariantCulture)
                                   : null;

            var group = this.notifierInputFieldModelFactory.CreteDropDownModel(
                displaySettings.Group, groupItems, groupValue);

            var password = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Password, notifier.Password);

            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Other, notifier.Other);

            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(
                displaySettings.Ordered, notifier.Ordered);

            var createdDate = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.CreatedDate, notifier.CreatedDate.ToString(CultureInfo.InvariantCulture));

            var changedDate = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.ChangedDate, notifier.ChangedDate.ToString(CultureInfo.InvariantCulture));

            string synchronizationDateValue;

            if (notifier.SynchronizationDate.HasValue)
            {
                synchronizationDateValue = notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                synchronizationDateValue = string.Empty;
            }

            var synchronizationDate =
                this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                    displaySettings.SynchronizationDate, synchronizationDateValue);

            return new NotifierInputModel(
                false,
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
                regionContent,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered,
                notifier.IsActive,
                createdDate,
                changedDate,
                synchronizationDate);
        }
    }
}