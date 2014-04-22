﻿namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IPriorityService
    {
        IList<Priority> GetPriorities(int customerId);
        Priority GetPriority(int id);
        DeleteMessage DeletePriority(int id);
        Helpdesk.Domain.PriorityLanguage GetPriorityLanguage(int id);
        Helpdesk.Domain.PriorityLanguage GetPriorityLanguageByLanguageId(int id, int languageId);

        bool FileExists(int priorityId, string fileName);
        int? GetDefaultId(int customerId);
        int GetPriorityIdByImpactAndUrgency(int impactId, int urgencyId);
        void AddFile(Priority priority, string filename);
        void SavePriority(Priority priority, out IDictionary<string, string> errors);
        void SavePriorityLanguage(PriorityLanguage priorityLanguage, bool update, out IDictionary<string, string> errors);
        void UpdateSavedFile(Priority priority);
        void Commit();
    }

    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        private readonly IPriorityLanguageRepository _priorityLangaugeRepository;
        private readonly IPriorityImpactUrgencyRepository _priorityImpactUrgencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PriorityService(
            IPriorityRepository priorityRepository,
            IPriorityLanguageRepository priorityLanguageRepository,
            IPriorityImpactUrgencyRepository priorityImpactUrgencyRepository,
            IUnitOfWork unitOfWork)
        {
            this._priorityRepository = priorityRepository;
            this._priorityLangaugeRepository = priorityLanguageRepository;
            this._priorityImpactUrgencyRepository = priorityImpactUrgencyRepository;
            this._unitOfWork = unitOfWork;
        }
        //
        public IList<Priority> GetPriorities(int customerId)
        {
            return this._priorityRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public Priority GetPriority(int id)
        {
            return this._priorityRepository.GetById(id);
        }

        public int? GetDefaultId(int customerId)
        {
            var r = this._priorityRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public int GetPriorityIdByImpactAndUrgency(int impactId, int urgencyId)
        {
            return this._priorityImpactUrgencyRepository.GetPriorityIdByImpactAndUrgency(impactId, urgencyId);    
        }

        public Helpdesk.Domain.PriorityLanguage GetPriorityLanguage(int id)
        {
            return this._priorityLangaugeRepository.Get(x => x.Priority_Id == id);
        }

        public Helpdesk.Domain.PriorityLanguage GetPriorityLanguageByLanguageId(int id, int languageId)
        {
            return this._priorityLangaugeRepository.Get(x => x.Priority_Id == id && x.Language_Id == languageId);
        }

        public DeleteMessage DeletePriority(int id)
        {
            var priority = this._priorityRepository.GetById(id);

            if (priority != null)
            {
                try
                {
                    this._priorityRepository.Delete(priority);
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
                this._priorityRepository.Add(priority);
            else
                this._priorityRepository.Update(priority);

            if (priority.IsDefault == 1)
                this._priorityRepository.ResetDefault(priority.Id);

            if (priority.IsEmailDefault == 1)
                this._priorityRepository.ResetEmailDefault(priority.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SavePriorityLanguage(PriorityLanguage priorityLanguage, bool update, out IDictionary<string, string> errors)
        {
            if (priorityLanguage == null)
                throw new ArgumentNullException("priority");

           
            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(priorityLanguage.InformUserText))
                errors.Add("PriorityLanguage.InformUserText", "Du måste ange text");


            if (!update)
                this._priorityLangaugeRepository.Add(priorityLanguage);
            else
                this._priorityLangaugeRepository.Update(priorityLanguage);

            if (errors.Count == 0)
                this.Commit();
        }

        public void AddFile(Priority priority, string filename)
        {

            priority.FileName = filename;

            this._priorityRepository.Update(priority);
            this.Commit();
            
        }

        public void UpdateSavedFile(Priority priority)
        {
            if (priority.Id != 0)
            {
                this._priorityRepository.Update(priority);
                this.Commit();
            }
        }

        public bool FileExists(int priorityId, string fileName)
        {
            var priority = this._priorityRepository.GetById(priorityId);

            if (priority.FileName == fileName || priority.FileName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
