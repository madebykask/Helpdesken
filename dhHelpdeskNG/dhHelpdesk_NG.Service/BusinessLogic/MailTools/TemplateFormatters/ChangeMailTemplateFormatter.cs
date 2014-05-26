namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;

    public sealed class ChangeMailTemplateFormatter : MailTemplateFormatter<UpdatedChange>,
        IMailTemplateFormatter<UpdatedChange>
    {
        #region Fields

        private readonly IChangeCategoryRepository changeCategoryRepository;

        private readonly IChangeGroupRepository changeGroupRepository;

        private readonly IChangeImplementationStatusRepository changeImplementationStatusRepository;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IChangePriorityRepository changePriorityRepository;

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IUserRepository userRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        #endregion

        #region Constructors and Destructors

        public ChangeMailTemplateFormatter(
            IChangeStatusRepository changeStatusRepository,
            IChangeObjectRepository changeObjectRepository,
            IChangeGroupRepository changeGroupRepository,
            IDepartmentRepository departmentRepository,
            ISystemRepository systemRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUserRepository userRepository,
            IChangeCategoryRepository changeCategoryRepository,
            IChangePriorityRepository changePriorityRepository,
            IChangeImplementationStatusRepository changeImplementationStatusRepository)
        {
            this.changeStatusRepository = changeStatusRepository;
            this.changeObjectRepository = changeObjectRepository;
            this.changeGroupRepository = changeGroupRepository;
            this.departmentRepository = departmentRepository;
            this.systemRepository = systemRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.userRepository = userRepository;
            this.changeCategoryRepository = changeCategoryRepository;
            this.changePriorityRepository = changePriorityRepository;
            this.changeImplementationStatusRepository = changeImplementationStatusRepository;
        }

        #endregion

        #region Methods

        protected override Dictionary<string, string> GetMarkValues(
            MailTemplate template,
            UpdatedChange businessModel,
            int customerId,
            int languageId)
        {
            var markValues = new Dictionary<string, string>();

            this.AddOrdererMarkValues(markValues, businessModel.Orderer);
            this.AddGeneralMarkValues(markValues, businessModel.General);
            this.AddRegistrationMarkValues(markValues, businessModel.Registration);
            this.AddAnalyzeMarkValues(markValues, businessModel.Analyze);
            this.AddImplementationMarkValues(markValues, businessModel.Implementation);
            this.AddEvaluationMarkValues(markValues, businessModel.Evaluation);

            return markValues;
        }

        private void AddAnalyzeMarkValues(Dictionary<string, string> markValues, UpdatedAnalyzeFields fields)
        {
            var category = fields.CategoryId.HasValue
                ? this.changeCategoryRepository.GetCategoryName(fields.CategoryId.Value)
                : null;

            var priority = fields.PriorityId.HasValue
                ? this.changePriorityRepository.GetPriorityName(fields.PriorityId.Value)
                : null;

            string responsible = null;

            if (fields.ResponsibleId.HasValue)
            {
                var userName = this.userRepository.GetUserName(fields.ResponsibleId.Value);
                responsible = userName != null ? userName.FirstName + userName.LastName : string.Empty;
            }

            var solution = fields.Solution;
            var cost = fields.Cost.ToString(CultureInfo.InvariantCulture);
            var yearlyCost = fields.YearlyCost.ToString(CultureInfo.InvariantCulture);

            var estimatedTimeInHours = fields.EstimatedTimeInHours.ToString(CultureInfo.InvariantCulture);
            var risk = fields.Risk;
            var startDate = fields.StartDate.ToString();
            var finishDate = fields.FinishDate.ToString();
            var rejectExplanation = fields.RejectExplanation;

            markValues.Add("[#32]", category);
            markValues.Add("[#11]", priority);
            markValues.Add("[#15]", responsible);
            markValues.Add("[#16]", solution);
            markValues.Add("[#17]", cost);
            markValues.Add("[#18]", yearlyCost);
            markValues.Add("[#22]", estimatedTimeInHours);
            markValues.Add("[#48]", risk);
            markValues.Add("[#19]", startDate);
            markValues.Add("[#20]", finishDate);
            markValues.Add("[#45]", rejectExplanation);
        }

        private void AddEvaluationMarkValues(Dictionary<string, string> markValues, UpdatedEvaluationFields fields)
        {
            var changeEvaluation = fields.ChangeEvaluation;

            markValues.Add("[#43]", fields.ChangeEvaluation);
        }

        private void AddGeneralMarkValues(Dictionary<string, string> markValues, UpdatedGeneralFields fields)
        {
            var priority = fields.Priority.HasValue ? fields.Priority.ToString() : null;
            var title = fields.Title;

            var status = fields.StatusId.HasValue
                ? this.changeStatusRepository.GetStatusName(fields.StatusId.Value)
                : null;

            var system = fields.SystemId.HasValue ? this.systemRepository.GetSystemName(fields.SystemId.Value) : null;

            var @object = fields.ObjectId.HasValue
                ? this.changeObjectRepository.GetObjectName(fields.ObjectId.Value)
                : null;

            var workingGroup = fields.WorkingGroupId.HasValue
                ? this.workingGroupRepository.GetWorkingGroupName(fields.WorkingGroupId.Value)
                : null;

            string administrator = null;

            if (fields.AdministratorId.HasValue)
            {
                var userName = this.userRepository.GetUserName(fields.AdministratorId.Value);
                administrator = userName != null ? userName.FirstName + userName.LastName : string.Empty;
            }

            var finishingDate = fields.FinishingDate.ToString();

            markValues.Add("[#2]", priority);
            markValues.Add("[#5]", title);
            markValues.Add("[#1]", status);
            markValues.Add("[#30]", system);
            markValues.Add("[#6]", @object);
            markValues.Add("[#34]", workingGroup);
            markValues.Add("[#13]", administrator);
            markValues.Add("[#64]", finishingDate);
        }

        private void AddImplementationMarkValues(
            Dictionary<string, string> markValues,
            UpdatedImplementationFields fields)
        {
            var implementationStatus = fields.StatusId.HasValue
                ? this.changeImplementationStatusRepository.GetStatusName(fields.StatusId.Value)
                : null;

            var realStartDate = fields.RealStartDate.ToString();
            var finishingDate = fields.FinishingDate.ToString();
            var deviation = fields.Deviation;

            markValues.Add("[#21]", implementationStatus);
            markValues.Add("[#24]", realStartDate);
            markValues.Add("[#23]", finishingDate);
            markValues.Add("[#41]", deviation);
        }

        private void AddOrdererMarkValues(Dictionary<string, string> markValues, UpdatedOrdererFields fields)
        {
            var id = fields.Id;
            var name = fields.Name;
            var phone = fields.Phone;
            var cellPhone = fields.CellPhone;
            var email = fields.Email;

            var department = fields.DepartmentId.HasValue
                ? this.departmentRepository.GetDepartmentName(fields.DepartmentId.Value)
                : null;

            markValues.Add("[#50]", id);
            markValues.Add("[#27]", name);
            markValues.Add("[#28]", phone);
            markValues.Add("[#51]", cellPhone);
            markValues.Add("[#29]", email);
            markValues.Add("[#52]", department);
        }

        private void AddRegistrationMarkValues(Dictionary<string, string> markValues, UpdatedRegistrationFields fields)
        {
            var owner = fields.OwnerId.HasValue
                ? this.changeGroupRepository.GetChangeGroupName(fields.OwnerId.Value)
                : null;

            var description = fields.Description;
            var businessBenefits = fields.BusinessBenefits;
            var consequence = fields.Consequence;
            var impact = fields.Impact;
            var desiredDate = fields.DesiredDate.ToString();
            var rejectExplanation = fields.RejectExplanation;

            markValues.Add("[#7]", owner);
            markValues.Add("[#9]", description);
            markValues.Add("[#10]", businessBenefits);
            markValues.Add("[#33]", consequence);
            markValues.Add("[#47]", impact);
            markValues.Add("[#31]", desiredDate);
            //            markValues.Add("[#50]", rejectExplanation);
        }

        #endregion
    }
}