using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region FORM

    public interface IFormRepository : IRepository<Form>
    {
        IList<Form> GetForms(int caseid);
    }

    public class FormRepository : RepositoryBase<Form>, IFormRepository
    {
        public FormRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<Form> GetForms(int caseid)
        {
            IList<Form> f = (from du in this.DataContext.Set<Form>()
                             join d in this.DataContext.FormField on du.Id equals d.Form_Id
                             join ffv in this.DataContext.FormFieldValue on d.Id equals ffv.FormField_Id
                             where ffv.Case_Id == caseid
                             select du).ToList();

            return f;
        }
    }

    #endregion

    #region FORMFIELD

    public interface IFormFieldRepository : IRepository<FormField>
    {
    }

    public class FormFieldRepository : RepositoryBase<FormField>, IFormFieldRepository
    {
        public FormFieldRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
