namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ITemplateService
    {
        IDictionary<string, string> ValidateTemplate(Status statusToValidate);
    }

    // Private readonly means the variable only be instantiated in the constructor.
    //
    // Contructor: 
    //      Dependency injection is used here. http://en.wikipedia.org/wiki/Dependency_injection
    //      IStatusRepository is set to StatusRepository during "resolve time" using Ninject. http://ninject.org/
    //

    public class TemplateService : ITemplateService
    {
        private readonly IStatusRepository _statusRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public TemplateService(
            IStatusRepository statusRepository, 
            IUnitOfWork unitOfWork)
        {
            this._statusRepository = statusRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> ValidateTemplate(Status statusToValidate)
        {
            if(statusToValidate == null)
                throw new ArgumentNullException("statustovalidate");

            var errors = new Dictionary<string, string>();

            if(!Guard.HasValue(statusToValidate.Name))
                errors.Add("name", "Name är obligatorisk");

            return errors;
        }
    }
}
