namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeRequestFactory : INewChangeRequestFactory
    {
        public NewChangeRequest Create(
            InputModel model,
            List<WebTemporaryFile> registrationFiles,
            int currentUserId,
            int currentCustomerId,
            DateTime createdDateAndTime)
        {
            var newChange = CreateNewChange(model, currentUserId, currentCustomerId, createdDateAndTime);
            var newFiles = CreateNewFiles(registrationFiles, createdDateAndTime);

            return new NewChangeRequest(
                newChange,
                model.Registration.AffectedProcessIds,
                model.Registration.AffectedDepartmentIds,
                model.Analyze.RelatedChangeIds,
                newFiles);
        }

        private static NewChange CreateNewChange(
            InputModel model, int currentUserId, int currentCustomerId, DateTime createdDateAndTime)
        {
            var orderer = CreateNewOrdererPart(model.Orderer);
            var general = CreateNewGeneralPart(model.General, createdDateAndTime);
            var registration = CreateNewRegistrationPart(model.Registration, currentUserId, createdDateAndTime);

            return new NewChange(currentCustomerId, orderer, general, registration);
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
                model.Id != null ? model.Id.Value : null,
                model.Name != null ? model.Name.Value : null,
                model.Phone != null ? model.Phone.Value : null,
                model.CellPhone != null ? model.CellPhone.Value : null,
                model.Email != null ? model.Email.Value : null,
                model.DepartmentId);
        }

        private static NewGeneralFields CreateNewGeneralPart(GeneralModel model, DateTime createdDateAndTime)
        {
            return new NewGeneralFields(
                model.Priority != null ? model.Priority.Value : 0,
                model.Title != null ? model.Title.Value : null,
                model.StatusId,
                model.SystemId,
                model.ObjectId,
                model.WorkingGroupId,
                model.AdministratorId,
                model.FinishingDate != null ? model.FinishingDate.Value : null,
                createdDateAndTime,
                model.Rss != null ? model.Rss.Value : false);
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
                model.Description != null ? model.Description.Value : null,
                model.BusinessBenefits != null ? model.BusinessBenefits.Value : null,
                model.Consequence != null ? model.Consequence.Value : null,
                model.Impact != null ? model.Impact.Value : null,
                model.DesiredDateAndTime != null ? model.DesiredDateAndTime.Value : null,
                model.Verified != null ? model.Verified.Value : false,
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                model.RejectExplanation != null ? model.RejectExplanation.Value : null);
        }
    }
}