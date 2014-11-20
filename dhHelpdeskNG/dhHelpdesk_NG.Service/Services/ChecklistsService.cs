namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IChecklistsService
    {
        IDictionary<string, string> Validate(Checklists checklistToValidate);

        void Commit();
    }

    public class ChecklistsService : IChecklistsService
    {
        private readonly IChecklistsRepository _checklistsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChecklistsService(
            IChecklistsRepository checklistsRepository,
            IUnitOfWork unitOfWork)
        {
            this._checklistsRepository = checklistsRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Checklists checklistToValidate)
        {
            if (checklistToValidate == null)
                throw new ArgumentNullException("checkliststovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
