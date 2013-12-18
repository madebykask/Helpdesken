namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NewNotifierModelFactory : INewNotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NewNotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        #region Public Methods and Operators

        public NotifierInputModel Create(
            DisplayFieldSettingsDto displaySettings, 
            List<ItemOverviewDto> domains, 
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions, 
            List<ItemOverviewDto> managers, 
            List<ItemOverviewDto> groups)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.UserId, null);

            NotifierInputDropDownModel domain;

            if (displaySettings.Domain.Show)
            {
                var content = new DropDownContent(domains.Select(d => new DropDownItem(d.Name, d.Value)).ToList());

                domain = new NotifierInputDropDownModel(
                    true, displaySettings.Domain.Caption, content, displaySettings.Domain.Required);
            }
            else
            {
                domain = new NotifierInputDropDownModel(false);
            }

            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.LoginName, null);
            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.FirstName, null);
            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Initials, null);
            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.LastName, null);
            
            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.DisplayName, null);

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Place, null);
            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Phone, null);
            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.CellPhone, null);
            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Email, null);
            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Code, null);
            
            var postalAddress =
                this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.PostalAddress, null);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.PostalCode, null);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.City, null);
            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Title, null);

            DropDownContent regionContent = null;
            NotifierInputDropDownModel department;

            if (displaySettings.Department.Show)
            {
                regionContent = new DropDownContent(regions.Select(r => new DropDownItem(r.Name, r.Value)).ToList());
                var content = new DropDownContent(departments.Select(d => new DropDownItem(d.Name, d.Value)).ToList());

                department = new NotifierInputDropDownModel(
                    true, displaySettings.Department.Caption, content, displaySettings.Department.Required);
            }
            else
            {
                department = new NotifierInputDropDownModel(false);
            }

            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Unit, null);

            NotifierInputDropDownModel organizationUnit;

            if (displaySettings.OrganizationUnit.Show)
            {
                var content = new DropDownContent(divisions.Select(u => new DropDownItem(u.Name, u.Value)).ToList());

                organizationUnit = new NotifierInputDropDownModel(true, displaySettings.OrganizationUnit.Caption, content, displaySettings.OrganizationUnit.Required);
            }
            else
            {
                organizationUnit = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel division;

            if (displaySettings.Division.Show)
            {
                var content =
                    new DropDownContent(
                        divisions.Select(d => new DropDownItem(d.Name, d.Value))
                                  .ToList());

                division = new NotifierInputDropDownModel(true, displaySettings.Division.Caption, content, displaySettings.Division.Required);
            }
            else
            {
                division = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel manager;

            if (displaySettings.Manager.Show)
            {
                var content =
                    new DropDownContent(
                        groups.Select(g => new DropDownItem(g.Name, g.Value))
                                  .ToList());

                manager = new NotifierInputDropDownModel(
                    true, displaySettings.Manager.Caption, content, displaySettings.Manager.Required);
            }
            else
            {
                manager = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel group;

            if (displaySettings.Group.Show)
            {
                var content =
                    new DropDownContent(
                        groups.Select(g => new DropDownItem(g.Name, g.Value))
                                  .ToList());

                group = new NotifierInputDropDownModel(
                    true, displaySettings.Group.Caption, content, displaySettings.Group.Required);
            }
            else
            {
                group = new NotifierInputDropDownModel(false);
            }

            var password = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Password, null);
            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Other, null);
            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(displaySettings.Ordered, false);

            return new NotifierInputModel(
                true,
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
                false,
                null,
                null,
                null);
        }

        #endregion
    }
}