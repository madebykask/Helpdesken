namespace DH.Helpdesk.Services.AggregateDataLoader.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Services.AggregateData.Changes;

    public sealed class ChangeOptionsDataLoader : IChangeOptionsDataLoader
    {
        private readonly IDepartmentRepository departmentRepository;

        private readonly IChangeStatusRepository changeStatusRepository;

        private readonly ISystemRepository systemRepository;

        private readonly IChangeObjectRepository changeObjectRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IChangeGroupRepository changeGroupRepository;

        private readonly IChangeCategoryRepository changeCategoryRepository;

        private readonly IChangeRepository changeRepository;

        private readonly IChangePriorityRepository changePriorityRepository;

        private readonly ICurrencyRepository currencyRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IChangeImplementationStatusRepository changeImplementationStatusRepository;

        public ChangeOptionsDataLoader(
            IDepartmentRepository departmentRepository,
            IChangeStatusRepository changeStatusRepository,
            ISystemRepository systemRepository,
            IChangeObjectRepository changeObjectRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUserRepository userRepository,
            IUserWorkingGroupRepository userWorkingGroupRepository,
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
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.changeGroupRepository = changeGroupRepository;
            this.changeCategoryRepository = changeCategoryRepository;
            this.changeRepository = changeRepository;
            this.changePriorityRepository = changePriorityRepository;
            this.currencyRepository = currencyRepository;
            this.emailGroupRepository = emailGroupRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.changeImplementationStatusRepository = changeImplementationStatusRepository;
        }

        public ChangeEditOptionsData Load(int customerId, int changeId)
        {
            var departments = this.departmentRepository.FindActiveOverviews(customerId);
            var statuses = this.changeStatusRepository.FindOverviews(customerId);
            var systems = this.systemRepository.FindOverviews(customerId);
            var objects = this.changeObjectRepository.FindOverviews(customerId);

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

            var users = this.userRepository.FindActiveOverviews(customerId);
            var changeGroups = this.changeGroupRepository.FindOverviews(customerId);
            var categories = this.changeCategoryRepository.FindOverviews(customerId);
            var relatedChanges = this.changeRepository.FindOverviewsExcludeChange(customerId, changeId);
            var priorities = this.changePriorityRepository.FindOverviews(customerId);
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

            return new ChangeEditOptionsData(
                departments,
                statuses,
                systems,
                objects,
                workingGroupsWithEmails,
                users,
                changeGroups,
                categories,
                relatedChanges,
                priorities,
                currencies,
                emailGroupsWithEmails,
                implementationStatuses);
        }
    }
}
