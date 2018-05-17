using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Web.Models.Notifiers
{
	public class ComputerUserCategoryModel
	{

		public ComputerUserCategoryModel(List<ComputerUserCategoryOverview> notReadOnlyCategories)
		{
			ExtendedCaseFormID = null;
			Name = "";
			IsReadOnly = false;
			ComputerUserCategoryID = null;
			ComputerUserCategories = new List<ComputerUserCategoryOverview>();

			var dropDownItems = ComputerUserCategories.Select(o => new DropDownItem(o.Name, o.Id.ToString())).ToList();

			// Todo: translation of non-categories computer users
			dropDownItems.Insert(0, new DropDownItem(Translation.Get("Employee", DH.Helpdesk.Web.Infrastructure.Enums.TranslationSource.TextTranslation), "0"));

			ComputerUserCategoryDropDownContent = new DropDownContent(dropDownItems, ComputerUserCategoryID.HasValue ?
				ComputerUserCategoryID.Value.ToString() : "0");
		}

		public ComputerUserCategoryModel(ComputerUserCategory category, List<ComputerUserCategoryOverview> notReadOnlyCategories)
		{
			ExtendedCaseFormID = category.ExtendedCaseFormID; // 2003
			Name = category.Name; //"Vendor";
			IsReadOnly = category.IsReadOnly; //true;
			ComputerUserCategoryID = category.ID; //2;
			ComputerUserCategories = notReadOnlyCategories;
			/*new List<ComputerUserCategory>(new ComputerUserCategory[] {
			new ComputerUserCategory() { ID = 2, Name = "Vendor mock" }
		});*/
			ExtendedCasePath = string.Format("/ExtendedCase/?formId={0}&autoLoad=true", ExtendedCaseFormID);

			var dropDownItems = ComputerUserCategories.Select(o => new DropDownItem(o.Name, o.Id.ToString())).ToList();

			// Todo: translation of non-categories computer users
			dropDownItems.Insert(0, new DropDownItem(Translation.Get("Employee", DH.Helpdesk.Web.Infrastructure.Enums.TranslationSource.TextTranslation), "0"));

			ComputerUserCategoryDropDownContent = new DropDownContent(dropDownItems, ComputerUserCategoryID.HasValue ?
				ComputerUserCategoryID.Value.ToString() : "0");
		}
		public bool IsReadOnly { get; set; }
		public int? ExtendedCaseFormID { get; set; }

		public string ExtendedCasePath { get; set; }

		public string Name { get; set; }

		public int? ComputerUserCategoryID { get; set; }
		public List<ComputerUserCategoryOverview> ComputerUserCategories { get; set; }
		public DropDownContent ComputerUserCategoryDropDownContent { get; set; }
	}
}