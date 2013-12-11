using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OUInputViewModel
    {
        public OU OU { get; set; }
        public Customer Customer { get; set; }

        public IList<ComputerUserGroup> ComputerUserGroups { get; set; }

        public IList<SelectListItem> Departments { get; set; }
        public IList<SelectListItem> OUs { get; private set; }
        public IList<SelectListItem> Regions { get; set; }

        public IList<SDepartment> SDepartments { get; set; }

        public OUInputViewModel(IList<OU> ous, int selectedId)
        {
            var selectListItems = new List<SelectListItem>();

            foreach (var ou in ous)
            {
                selectListItems.AddRange(GetSelectListItem(new List<OU> { ou }, "", selectedId));
            }

            OUs = selectListItems;
        }

        private static IEnumerable<SelectListItem> GetSelectListItem(IEnumerable<OU> ous, string iteration, int selectedId)
        {
            var items = new List<SelectListItem>();

            foreach (var ou in ous)
            {
                items.Add(new SelectListItem
                {
                    Text = string.Format("{0} {1}", iteration, ou.Name),
                    Value = ou.Id.ToString(),
                    Selected = (ou.Parent_OU_Id == selectedId)
                });

                if (!ou.SubOUs.Any()) continue;

                var subOUs = GetSelectListItem(ou.SubOUs, iteration + "-", selectedId);
                items.AddRange(subOUs);
            }

            return items;
        }
    }

    public struct SDepartment
    {
        public int Id { get; set; }
        public int? Region_Id { get; set; }
        public string Name { get; set; }
    }
}