namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;

    public class OUInputViewModel
    {
        public OUInputViewModel(IList<OU> ous, int selectedId, int parentId)
        {
            var selectListItems = new List<SelectListItem>();

            foreach (var ou in ous)
            {
                selectListItems.AddRange(GetSelectListItem(new List<OU> { ou }, string.Empty, selectedId, parentId));
            }

            this.OUs = selectListItems;
        }


        public OU OU { get; set; }

        public Customer Customer { get; set; }

        public IList<ComputerUserGroup> ComputerUserGroups { get; set; }

        public IList<SelectListItem> Departments { get; set; }

        public IList<SelectListItem> OUs { get; private set; }

        public IList<SelectListItem> Regions { get; set; }

        public IList<SDepartment> SDepartments { get; set; }

        private static IEnumerable<SelectListItem> GetSelectListItem(IEnumerable<OU> ous, string iteration, int selectedId, int parentId)
        {
            var items = new List<SelectListItem>();
            foreach (var ou in ous)
            {
                if (ou.Id == selectedId)
                {
                    /// we dont want to add self item and items that lays below self item
                    continue;
                }

                items.Add(new SelectListItem
                {
                    Text = string.Format("{0} {1}", iteration, ou.Name),
                    Value = ou.Id.ToString(),
                    Selected = ou.Parent_OU_Id == parentId
                });
                if (ou.SubOUs.Any())
                {
                    var subOUs = GetSelectListItem(ou.SubOUs, iteration + "-", selectedId, parentId);
                    items.AddRange(subOUs);    
                }
            }

            return items;
        }
    }
}