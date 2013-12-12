using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IPriorityService
    {
        IList<Priority> GetPriorities(int customerId);
        Priority GetPriority(int id);
        DeleteMessage DeletePriority(int id);
        int? GetDefaultId(int customerId); 

        void SavePriority(Priority priority, out IDictionary<string, string> errors);
        void UpdateSavedFile(Priority priority);
        void Commit();
    }

    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        private readonly IUnitOfWork _unitOfWork;
        // test 2013
        public PriorityService(
            IPriorityRepository priorityRepository,
            IUnitOfWork unitOfWork)
        {
            _priorityRepository = priorityRepository;
            _unitOfWork = unitOfWork;
        }
        
        public IList<Priority> GetPriorities(int customerId)
        {
            return _priorityRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Priority GetPriority(int id)
        {
            return _priorityRepository.GetById(id);
        }

        public int? GetDefaultId(int customerId)
        {
            var r = _priorityRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public DeleteMessage DeletePriority(int id)
        {
            var priority = _priorityRepository.GetById(id);

            if (priority != null)
            {
                try
                {
                    _priorityRepository.Delete(priority);
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

        public void SavePriority(Priority priority, out IDictionary<string, string> errors)
        {
            if (priority == null)
                throw new ArgumentNullException("priority");

            priority.Description = priority.Description == null ? string.Empty : priority.Description;
            priority.RelatedField = priority.RelatedField == null ? string.Empty : priority.RelatedField;
            priority.EMailList = priority.EMailList == null ? string.Empty : priority.EMailList;
            priority.FileName = priority.FileName == null ? string.Empty : priority.FileName;

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(priority.Code))
                errors.Add("Priority.Code", "Du måste ange prioritet");

            if (string.IsNullOrEmpty(priority.Description))
                errors.Add("Priority.Description", "Du måste ange en beskrivning");

            if (string.IsNullOrEmpty(priority.Name))
                errors.Add("Priority.Name", "Du måste ange ett namn");

            if (string.IsNullOrEmpty(priority.EMailList))
                errors.Add("Priority.EMailList", "Du måste fylla i listan");

            //if (string.IsNullOrEmpty(priority.FileName))
            //    errors.Add("Priority.FileName", "Du måste bifoga en fil");

            if (priority.Id == 0)
                _priorityRepository.Add(priority);
            else
                _priorityRepository.Update(priority);

            if (priority.IsDefault == 1)
                _priorityRepository.ResetDefault(priority.Id);

            if (priority.IsEmailDefault == 1)
                _priorityRepository.ResetEmailDefault(priority.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void UpdateSavedFile(Priority priority)
        {
            if (priority.Id != 0)
            {
                _priorityRepository.Update(priority);
                this.Commit();
            }
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
