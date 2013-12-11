using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IEmailService
    {
        IList<EMailGroup> GetEmailGroups(int customerId);

        EMailGroup GetEmailGroup(int id);

        DeleteMessage DeleteEmailGroup(int id);

        void SaveEmailGroup(EMailGroup emailGroup, out IDictionary<string, string> errors);
        void Commit();
    }

    public class EmailService : IEmailService
    {
        private readonly IEMailGroupRepository _emailGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailService(
            IEMailGroupRepository emailGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _emailGroupRepository = emailGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<EMailGroup> GetEmailGroups(int customerId)
        {
            return _emailGroupRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public EMailGroup GetEmailGroup(int id)
        {
            return _emailGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteEmailGroup(int id)
        {
            var emailGroup = _emailGroupRepository.GetById(id);

            if (emailGroup != null)
            {
                try
                {
                    _emailGroupRepository.Delete(emailGroup);
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

        public void SaveEmailGroup(EMailGroup emailGroup, out IDictionary<string, string> errors)
        {
            if (emailGroup == null)
                throw new ArgumentNullException("emailgroup");

            emailGroup.Members = emailGroup.Members ?? "";

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(emailGroup.Name))
                errors.Add("EMailGroup.Name", "Du måste ange en e-postgrupp");

            emailGroup.ChangedDate = DateTime.UtcNow;

            if (emailGroup.Id == 0)
                _emailGroupRepository.Add(emailGroup);
            else
                _emailGroupRepository.Update(emailGroup);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
