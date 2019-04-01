using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.BusinessData.Enums.Case
{
	public enum CaseInitiatorSearchScope
	{
		[Display(Name = "Anmälare och Angående")]
		UserAndIsAbout = 0,
		[Display(Name = "Anmälare")]
		User = 1,
		[Display(Name = "Angående")]
		IsAbout = 2
	}
}
