namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region FORM

    public interface IFormRepository : IRepository<Form>
    {
        IList<Form> GetForms(int caseid);
        void SaveEmptyForm(Guid formGuid, int caseId);
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

        public void SaveEmptyForm(Guid formGuid, int caseId)
        {
            var form = this.DataContext.Forms.FirstOrDefault(x => x.FormGUID == formGuid);

            if(form == null)
                return;

            var formFields = this.DataContext.FormField.Where(x => x.Form_Id == form.Id).Select(x => x.Id).ToList();

            foreach(var formfield in formFields)
            {
                this.DataContext.FormFieldValue.Add(new FormFieldValue { Case_Id = caseId, FormField_Id = formfield, FormFieldValues = "" });
                // first row is good enough for now
                break;
            }

            this.DataContext.Commit();
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
