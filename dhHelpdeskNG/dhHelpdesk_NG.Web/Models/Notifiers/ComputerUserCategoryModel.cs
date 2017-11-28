using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Notifiers
{
	public class ComputerUserCategoryModel
	{

		public ComputerUserCategoryModel(List<ComputerUserCategory> notReadOnlyCategories)
		{
			ExtendedCaseFormID = null;
			Name = "";
			IsReadOnly = false;
			ComputerUserCategoryID = null;
			ComputerUserCategories = new List<ComputerUserCategory>();

			var dropDownItems = ComputerUserCategories.Select(o => new DropDownItem(o.Name, o.ID.ToString())).ToList();

			// Todo: translation of non-categories computer users
			dropDownItems.Insert(0, new DropDownItem("Employee", "0"));

			ComputerUserCategoryDropDownContent = new DropDownContent(dropDownItems, ComputerUserCategoryID.HasValue ?
				ComputerUserCategoryID.Value.ToString() : "0");
		}
		public ComputerUserCategoryModel(ComputerUserCategory category, List<ComputerUserCategory> notReadOnlyCategories)
		{
			ExtendedCaseFormID = category.ExtendedCaseFormID; // 2003
			Name = category.Name; //"Vendor";
			IsReadOnly = category.IsReadOnly; //true;
			ComputerUserCategoryID = category.ID; //2;
			ComputerUserCategories = notReadOnlyCategories;
			/*new List<ComputerUserCategory>(new ComputerUserCategory[] {
			new ComputerUserCategory() { ID = 2, Name = "Vendor mock" }
		});*/
			ExtendedCasePath = string.Format("http://localhost/ExtendedCase/?formId={0}&autoLoad=true", ExtendedCaseFormID);

			var dropDownItems = ComputerUserCategories.Select(o => new DropDownItem(o.Name, o.ID.ToString())).ToList();

			// Todo: translation of non-categories computer users
			dropDownItems.Insert(0, new DropDownItem("Employee", "0"));

			ComputerUserCategoryDropDownContent = new DropDownContent(dropDownItems, ComputerUserCategoryID.HasValue ?
				ComputerUserCategoryID.Value.ToString() : "0");
		}
		public bool IsReadOnly { get; set; }
		public int? ExtendedCaseFormID { get; set; }

		public string ExtendedCasePath { get; set; }

		public string Name { get; set; }

		public int? ComputerUserCategoryID { get; set; }
		public List<ComputerUserCategory> ComputerUserCategories { get; set; }
		public DropDownContent ComputerUserCategoryDropDownContent { get; set; }
	}
}