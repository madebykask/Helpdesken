namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Notifiers.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.SelfService.Models.Notifiers;
    using DH.Helpdesk.SelfService.Models.Notifiers.ConfigurableFields;

    public sealed class NewNotifierFactory : INewNotifierFactory
    {
        public Notifier Create(InputModel model, int customerId, DateTime createdDateAndTime)
        {
            return Notifier.CreateNew(
                customerId,
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
                createdDateAndTime,
                model.LanguageId,
                model.CategoryId);
        }
    }
}