using System;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;

        public TemplateService(
            IStatusRepository statusRepository, 
            IUnitOfWork unitOfWork)
        {
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
        }

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
