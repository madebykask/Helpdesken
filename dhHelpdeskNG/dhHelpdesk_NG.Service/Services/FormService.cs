namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IFormService
    {
        IList<Form> GetForms(int caseid);
    }

    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(
            IFormRepository formRepository)
        {
            this._formRepository = formRepository;
        }

        public IList<Form> GetForms(int caseid)
        {
            return this._formRepository.GetForms(caseid).ToList();
        }
    }
}
