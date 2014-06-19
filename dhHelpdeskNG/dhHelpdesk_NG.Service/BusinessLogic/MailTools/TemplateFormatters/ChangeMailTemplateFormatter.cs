namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Services.Helpers;

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

        private readonly ICustomerRepository customerRepository;

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
            IChangeImplementationStatusRepository changeImplementationStatusRepository,
            ICustomerRepository customerRepository)
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
            this.customerRepository = customerRepository;
        }

        #endregion

        #region Methods

        protected override EmailMarkValues GetMarkValues(
            MailTemplate template,
            UpdatedChange businessModel,
            int customerId,
            int languageId)
        {
            var markValues = new EmailMarkValues();
            
            markValues.Add("[#3]", businessModel.Id.ToString());
            var customer = this.customerRepository.GetOverview(customerId);
            if (customer != null)
            {
                markValues.Add("[#4]", customer.Name);
            }

            this.AddOrdererMarkValues(markValues, businessModel.Orderer);
            this.AddGeneralMarkValues(markValues, businessModel.General);
            this.AddRegistrationMarkValues(markValues, businessModel.Registration);
            this.AddAnalyzeMarkValues(markValues, businessModel.Analyze);
            this.AddImplementationMarkValues(markValues, businessModel.Implementation);
            this.AddEvaluationMarkValues(markValues, businessModel.Evaluation);
            this.AddLogMarkValues(markValues, businessModel.Log);

            return markValues;
        }

        private void AddLogMarkValues(EmailMarkValues markValues, UpdatedLogFields fields)
        {
            var logNote = fields.LogNote;
            markValues.Add("[#59]", logNote);
        }

        private void AddAnalyzeMarkValues(EmailMarkValues markValues, UpdatedAnalyzeFields fields)
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
            var implementationPlan = fields.HasImplementationPlan.ToYesNoString();
            var recoveryPlan = fields.HasRecoveryPlan.ToYesNoString();
            var approval = fields.Approval.StatusToString();
            var logNote = fields.LogNote;

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
            markValues.Add("[#35]", implementationPlan);
            markValues.Add("[#36]", recoveryPlan);
            markValues.Add("[#49]", approval);
            markValues.Add("[#56]", logNote);
        }

        private void AddEvaluationMarkValues(EmailMarkValues markValues, UpdatedEvaluationFields fields)
        {
            var changeEvaluation = fields.ChangeEvaluation;
            var evaluationReady = fields.EvaluationReady.ToYesNoString();
            var logNote = fields.LogNote;

            markValues.Add("[#43]", changeEvaluation);
            markValues.Add("[#54]", evaluationReady);
            markValues.Add("[#58]", logNote);
        }

        private void AddGeneralMarkValues(EmailMarkValues markValues, UpdatedGeneralFields fields)
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

            var inventory = string.Empty;
            if (fields.Inventories != null)
            {
                var sb = new StringBuilder();
                foreach (var inv in fields.Inventories)
                {
                    sb.AppendLine(inv);
                }

                inventory = sb.ToString();
            }

            markValues.Add("[#2]", priority);
            markValues.Add("[#5]", title);
            markValues.Add("[#1]", status);
            markValues.Add("[#30]", system);
            markValues.Add("[#6]", @object);
            markValues.Add("[#55]", inventory);
            markValues.Add("[#34]", workingGroup);
            markValues.Add("[#13]", administrator);
            markValues.Add("[#64]", finishingDate);
        }

        private void AddImplementationMarkValues(
            EmailMarkValues markValues,
            UpdatedImplementationFields fields)
        {
            var implementationStatus = fields.StatusId.HasValue
                ? this.changeImplementationStatusRepository.GetStatusName(fields.StatusId.Value)
                : null;

            var realStartDate = fields.RealStartDate.ToString();
            var finishingDate = fields.FinishingDate.ToString();
            var deviation = fields.Deviation;
            var buildImplemented = fields.BuildImplemented.ToYesNoString();
            var implementationPlanUsed = fields.ImplementationPlanUsed.ToYesNoString();
            var recoveryPlanUsed = fields.RecoveryPlanUsed.ToYesNoString();
            var implementationReady = fields.ImplementationReady.ToYesNoString();
            var logNote = fields.LogNote;

            markValues.Add("[#21]", implementationStatus);
            markValues.Add("[#24]", realStartDate);
            markValues.Add("[#23]", finishingDate);
            markValues.Add("[#41]", deviation);
            markValues.Add("[#40]", buildImplemented);
            markValues.Add("[#39]", implementationPlanUsed);
            markValues.Add("[#38]", recoveryPlanUsed);
            markValues.Add("[#53]", implementationReady);
            markValues.Add("[#57]", logNote);
        }

        private void AddOrdererMarkValues(EmailMarkValues markValues, UpdatedOrdererFields fields)
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

        private void AddRegistrationMarkValues(EmailMarkValues markValues, UpdatedRegistrationFields fields)
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

            var name = string.Empty;
            var phone = string.Empty;
            var email = string.Empty;
            var company = string.Empty;
            if (fields.Contacts != null)
            {
                var contact = fields.Contacts.FirstOrDefault();
                if (contact != null)
                {
                    name = contact.Name;
                    phone = contact.Phone;
                    email = contact.Email;
                    company = contact.Company;
                }
            }

            var verified = fields.Verified.ToYesNoString();
            var approval = fields.Approval.StatusToString(); 

            markValues.Add("[#60]", name);
            markValues.Add("[#61]", phone);
            markValues.Add("[#62]", email);
            markValues.Add("[#63]", company);
            markValues.Add("[#7]", owner);
            markValues.Add("[#9]", description);
            markValues.Add("[#10]", businessBenefits);
            markValues.Add("[#33]", consequence);
            markValues.Add("[#47]", impact);
            markValues.Add("[#31]", desiredDate);
            markValues.Add("[#14]", verified);
            markValues.Add("[#46]", approval);
            markValues.Add("[#50]", rejectExplanation);
        }

        #endregion
    }
}