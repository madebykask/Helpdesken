namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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

    #region FORMFIELDVALUE

    public interface IFormFieldValueRepository : IRepository<FormFieldValue>
    {
        IList<FormFieldValue> GetFormFieldValuesByCaseId(int caseid);
    }

    public class FormFieldValueRepository : RepositoryBase<FormFieldValue>, IFormFieldValueRepository
    {
        public FormFieldValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<FormFieldValue> GetFormFieldValuesByCaseId(int caseid)
        {
            return (from ffv in this.DataContext.FormFieldValue 
                    where ffv.Case_Id == caseid
                    select ffv).ToList();
        }
    }

    #endregion
}
