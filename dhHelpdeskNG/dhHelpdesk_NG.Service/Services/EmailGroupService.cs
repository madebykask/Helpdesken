namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IEmailGroupService
    {
        IList<EmailGroupEntity> GetEmailGroups(int customerId);
        List<GroupWithEmails> GetEmailGroupsWithEmails(int customerId);
        EmailGroupEntity GetEmailGroup(int id);

        DeleteMessage DeleteEmailGroup(int id);

        void SaveEmailGroup(EmailGroupEntity emailGroup, out IDictionary<string, string> errors);
        void Commit();
    }

    public class EmailGroupService : IEmailGroupService
    {
        private readonly IEmailGroupRepository _emailGroupRepository;
        private readonly IEmailGroupEmailRepository _emailGroupEmailRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public EmailGroupService(
            IEmailGroupRepository emailGroupRepository,
            IEmailGroupEmailRepository emailGroupEmailRepository,
            IUnitOfWork unitOfWork)
        {
            this._emailGroupRepository = emailGroupRepository;
            this._emailGroupEmailRepository = emailGroupEmailRepository; 
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<EmailGroupEntity> GetEmailGroups(int customerId)
        {
            return this._emailGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public List<GroupWithEmails> GetEmailGroupsWithEmails(int customerId)
        {
            List<GroupWithEmails> emailGroupsWithEmails = null;
            var emailGroups = this._emailGroupRepository.FindActiveIdAndNameOverviews(customerId);
            var emailGroupIds = emailGroups.Select(g => g.Id).ToList();
            var emailGroupsEmails = this._emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

            emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

            foreach (var emailGroup in emailGroups)
            {
                var groupEmails = emailGroupsEmails.FirstOrDefault(e => e.ItemId == emailGroup.Id).Emails;
                if (groupEmails != null)
                {
                    var groupWithEmails = new GroupWithEmails(emailGroup.Id, emailGroup.Name, groupEmails);
                    emailGroupsWithEmails.Add(groupWithEmails);
                }
            }

            return emailGroupsWithEmails;
        }

        public EmailGroupEntity GetEmailGroup(int id)
        {
            return this._emailGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteEmailGroup(int id)
        {
            var emailGroup = this._emailGroupRepository.GetById(id);

            if (emailGroup != null)
            {
                try
                {
                    this._emailGroupRepository.Delete(emailGroup);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveEmailGroup(EmailGroupEntity emailGroup, out IDictionary<string, string> errors)
        {
            if (emailGroup == null)
                throw new ArgumentNullException("emailgroup");

            emailGroup.Members = emailGroup.Members ?? "";

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(emailGroup.Name))
                errors.Add("EMailGroup.Name", "Du måste ange en e-postgrupp");

            emailGroup.ChangedDate = DateTime.UtcNow;

            if (emailGroup.Id == 0)
                this._emailGroupRepository.Add(emailGroup);
            else
                this._emailGroupRepository.Update(emailGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
