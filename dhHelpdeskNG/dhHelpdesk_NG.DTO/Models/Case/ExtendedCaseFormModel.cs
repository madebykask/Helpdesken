using System;
using DH.Helpdesk.Common.Enums.Cases;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    /*TODO: Needs refactoring */
    public class ExtendedCaseFormForCaseModel
    {
        public int CaseId { get; set; }
        public int Id { get; set; }
        public CaseSectionType SectionType { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int UserRole { get; set; } //Case.Working group
        public int CaseStatus { get; set; }  //Case.StateSecondary
        public int LanguageId { get; set; }
        public Guid UserGuid { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int Version { get; set; }
    }

    public class ExtendedCaseDataModel
    {
        public ExtendedCaseDataModel()
        {
            FormModel = new ExtendedCaseFormForCaseModel();
        }

        public int Id { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public ExtendedCaseFormForCaseModel FormModel { get; set; }
    }

    public class ExtendedCaseValueModel
    {
        public int Id { get; set; }
        public int ExtendedCaseDataId { get; set; }
        public string FieldId { get; set; }
        public string Value { get; set; }
        public string SecondaryValue { get; set; }

    }

	public class ExtendedCaseFormModel
	{
		public string Name { get; set; }
		public int Id { get; set; }
	}

    public class ExtendedCaseFormsForCustomer
    {
        public DH.Helpdesk.Domain.Customer Customer { get; set; }
        public int? LanguageId { get; set; }
        public IList<ExtendedCaseFormModel> ExtendedCaseFormModels { get; set; }
    }

    public class ExtendedCaseFormWithCaseSolutionsModel
	{
		public string Name { get; set; }
		public int Id { get; set; }

		public IEnumerable<CaseSolutionOverview> CaseSolutions { get; set; }
	}
}