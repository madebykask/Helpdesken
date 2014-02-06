namespace dhHelpdesk_NG.Service.AggregateDataLoader.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Data.Repositories.Changes.Concrete;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class ChangeOptionalDataLoader : IChangeOptionalDataLoader
    {
        private readonly IDepartmentRepository departmentRepository;

        private readonly ChangeStatusRepository changeStatusRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IUserRepository userRepository;

        private readonly IChangeGroupRepository changeGroupRepository;

        private readonly IChangeCategoryRepository changeCategoryRepository;

        private readonly IChangeRepository changeRepository;

        private readonly IChangePriorityRepository changePriorityRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IChangeImplementationStatusRepository changeImplementationStatusRepository;

        public ChangeOptionalDataLoader(
            IDepartmentRepository departmentRepository,
            ChangeStatusRepository changeStatusRepository,
            ISystemRepository systemRepository,
            IChangeObjectRepository changeObjectRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
            IUserRepository userRepository,
            IChangeGroupRepository changeGroupRepository,
            IChangeCategoryRepository changeCategoryRepository,
            IChangeRepository changeRepository,
            IChangePriorityRepository changePriorityRepository,
            ICurrencyRepository currencyRepository,
            IEmailGroupRepository emailGroupRepository,
            IEmailGroupEmailRepository emailGroupEmailRepository,
            IChangeImplementationStatusRepository changeImplementationStatusRepository)
        {
            this.departmentRepository = departmentRepository;
            this.changeStatusRepository = changeStatusRepository;
            this.systemRepository = systemRepository;
            this.changeObjectRepository = changeObjectRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.userRepository = userRepository;
            this.changeGroupRepository = changeGroupRepository;
            this.changeCategoryRepository = changeCategoryRepository;
            this.changeRepository = changeRepository;
            this.changePriorityRepository = changePriorityRepository;
            this.currencyRepository = currencyRepository;
            this.emailGroupRepository = emailGroupRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.changeImplementationStatusRepository = changeImplementationStatusRepository;
        }

        public ChangeOptionalData Load(int customerId, int changeId, ChangeEditSettings editSettings)
        {
            var departments = this.LoadDepartments(customerId, editSettings);
            var statuses = this.LoadStatuses(customerId, editSettings);
            var systems = this.LoadSystems(customerId, editSettings);
            var objects = this.LoadObjects(customerId, editSettings);

            var workingGroups = this.workingGroupRepository.FindActiveOverviews(customerId);
            var workingGroupIds = workingGroups.Select(g => int.Parse(g.Value)).ToList();
            var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
            var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
            var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

            var workingGroupsWithEmails = new List<GroupWithEmails>(workingGroups.Count);

            foreach (var workingGroup in workingGroups)
            {
                var groupId = int.Parse(workingGroup.Value);
                var groupUserIdsWithEmails = workingGroupsUserIds.Single(g => g.WorkingGroupId == groupId);

                var groupEmails =
                    userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                        .Select(e => e.Email)
                        .ToList();

                var groupWithEmails = new GroupWithEmails(groupId, workingGroup.Name, groupEmails);
                workingGroupsWithEmails.Add(groupWithEmails);
            }

            var users = this.LoadUsers(customerId, editSettings);
            var administrators = users;
            var changeGroups = this.LoadChangeGroups(customerId, editSettings);
            var owners = changeGroups;
            var processesAffected = changeGroups;
            var categories = this.changeCategoryRepository.FindOverviews(customerId);
            var relatedChanges = this.changeRepository.FindOverviewsExcludeChange(customerId, changeId);
            var priorities = this.changePriorityRepository.FindOverviews(customerId);
            var responsibles = users;
            var currencies = this.currencyRepository.FindOverviews();

            var emailGroups = this.emailGroupRepository.FindActiveOverviews(customerId);
            var emailGroupIds = emailGroups.Select(g => int.Parse(g.Value)).ToList();
            var emailGroupsEmails = this.emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

            var emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

            foreach (var emailGroup in emailGroups)
            {
                var groupId = int.Parse(emailGroup.Value);
                var groupEmails = emailGroupsEmails.Single(e => e.ItemId == groupId).Emails;
                var groupWithEmails = new GroupWithEmails(groupId, emailGroup.Name, groupEmails);

                emailGroupsWithEmails.Add(groupWithEmails);
            }

            var implementationStatuses = this.changeImplementationStatusRepository.FindOverviews(customerId);

            return new ChangeOptionalData(
                departments,
                statuses,
                systems,
                objects,
                workingGroupsWithEmails,
                administrators,
                owners,
                processesAffected,
                categories,
                relatedChanges,
                priorities,
                responsibles,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses);
        }

        private List<ItemOverviewDto> LoadStatuses(int customerId, ChangeEditSettings editSettings)
        {
            return editSettings.GeneralFields.State.Show ? this.changeStatusRepository.FindOverviews(customerId) : null;
        }

        private List<ItemOverviewDto> LoadSystems(int customerId, ChangeEditSettings editSettings)
        {
            return editSettings.GeneralFields.System.Show ? this.systemRepository.FindOverviews(customerId) : null;
        }

        private List<ItemOverviewDto> LoadObjects(int customerId, ChangeEditSettings editSettings)
        {
            return editSettings.GeneralFields.Object.Show ? this.changeObjectRepository.FindOverviews(customerId) : null;
        }

        private List<ItemOverviewDto> LoadDepartments(int customerId, ChangeEditSettings editSettings)
        {
            return editSettings.OrdererFields.Department.Show
                ? this.departmentRepository.FindActiveOverviews(customerId)
                : null;
        }

        private List<ItemOverviewDto> LoadUsers(int customerId, ChangeEditSettings editSettings)
        {
            return editSettings.GeneralFields.Administrator.Show || editSettings.AnalyzeFields.Responsible.Show
                ? this.userRepository.FindActiveOverviews(customerId)
                : null;
        }

        private List<ItemOverviewDto> LoadChangeGroups(int customerId, ChangeEditSettings editSettings)
        {
            if (editSettings.GeneralFields.Owner.Show || editSettings.RegistrationFields.ProcessesAffected.Show)
            {
                return this.changeGroupRepository.FindOverviews(customerId);
            }

            return null;
        } 
    }
}
