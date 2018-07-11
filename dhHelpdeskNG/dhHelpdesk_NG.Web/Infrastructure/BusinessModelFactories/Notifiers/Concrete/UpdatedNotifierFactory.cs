namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;

    public sealed class UpdatedNotifierFactory : IUpdatedNotifierFactory
    {
        public Notifier Create(InputModel model, DateTime changedDateAndTime)
        {
            return Notifier.CreateUpdated(
                model.Id,
                StringFieldModel.GetValueOrDefault(model.UserId),
                model.DomainId,
                StringFieldModel.GetValueOrDefault(model.LoginName),
                StringFieldModel.GetValueOrDefault(model.FirstName),
                StringFieldModel.GetValueOrDefault(model.Initials),
                StringFieldModel.GetValueOrDefault(model.LastName),
                StringFieldModel.GetValueOrDefault(model.DisplayName),
                StringFieldModel.GetValueOrDefault(model.Place),
                StringFieldModel.GetValueOrDefault(model.Phone),
                StringFieldModel.GetValueOrDefault(model.CellPhone),
                StringFieldModel.GetValueOrDefault(model.Email),
                StringFieldModel.GetValueOrDefault(model.Code),
                StringFieldModel.GetValueOrDefault(model.PostalAddress),
                StringFieldModel.GetValueOrDefault(model.PostalCode),
                StringFieldModel.GetValueOrDefault(model.City),
                StringFieldModel.GetValueOrDefault(model.Title),
                model.DepartmentId,
                StringFieldModel.GetValueOrDefault(model.Unit),
                model.OrganizationUnitId,
                StringFieldModel.GetValueOrDefault(model.CostCentre),
                model.DivisionId,
                model.ManagerId,
                model.GroupId,
                StringFieldModel.GetValueOrDefault(model.Other),
                BooleanFieldModel.GetValueOrDefault(model.Ordered),
                model.IsActive,
                changedDateAndTime,
                model.LanguageId,
                model.CategoryId);
        }
    }
}