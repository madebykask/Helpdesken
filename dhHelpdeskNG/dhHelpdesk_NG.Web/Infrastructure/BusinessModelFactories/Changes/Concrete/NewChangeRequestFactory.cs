namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeRequestFactory : INewChangeRequestFactory
    {
        public NewChangeRequest Create(
            InputModel model,
            List<WebTemporaryFile> registrationFiles,
            int currentUserId,
            int currentCustomerId,
            int currentLanguageId,
            DateTime createdDateAndTime)
        {
            var newChange = CreateNewChange(
                model, currentUserId, currentCustomerId, currentLanguageId, createdDateAndTime);

            var newFiles = CreateNewFiles(registrationFiles, createdDateAndTime);

            return new NewChangeRequest(
                newChange,
                model.Registration.AffectedProcessIds,
                model.Registration.AffectedDepartmentIds,
                newFiles);
        }

        private static NewChange CreateNewChange(
            InputModel model,
            int currentUserId,
            int currentCustomerId,
            int currentLanguageId,
            DateTime createdDateAndTime)
        {
            var orderer = CreateNewOrdererPart(model.Orderer);
            var general = CreateNewGeneralPart(model.General, createdDateAndTime);
            var registration = CreateNewRegistrationPart(model.Registration, currentUserId, createdDateAndTime);

            return new NewChange(currentCustomerId, currentLanguageId, orderer, general, registration);
        }

        private static List<NewFile> CreateNewFiles(
            List<WebTemporaryFile> registrationFiles, DateTime createdDateAndTime)
        {
            return
                registrationFiles.Select(f => new NewFile(Subtopic.Registration, f.Content, f.Name, createdDateAndTime))
                                 .ToList();
        }

        private static NewOrdererFields CreateNewOrdererPart(OrdererModel model)
        {
            return new NewOrdererFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Id),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Name),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                model.DepartmentId);
        }

        private static NewGeneralFields CreateNewGeneralPart(GeneralModel model, DateTime createdDateAndTime)
        {
            return new NewGeneralFields(
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Priority),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.StatusId,
                model.SystemId,
                model.ObjectId,
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                createdDateAndTime,
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Rss));
        }

        private static NewRegistrationFields CreateNewRegistrationPart(
            RegistrationModel model, int currentUserId, DateTime createdDateAndTime)
        {
            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == RegistrationApprovalResult.Approved)
            {
                approvedDateAndTime = createdDateAndTime;
                approvedByUserId = currentUserId;
            }

            return new NewRegistrationFields(
                model.OwnerId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Description),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.BusinessBenefits),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Consequence),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Impact),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.DesiredDateAndTime),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Verified),
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.RejectExplanation));
        }
    }
}