using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
            _formRepository = formRepository;
        }

        public IList<Form> GetForms(int caseid)
        {
            return _formRepository.GetForms(caseid).ToList();
        }
    }
}
