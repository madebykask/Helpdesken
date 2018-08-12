using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Web.Models.CaseSolution
{
    public class CaseSolutionSettingsFieldModel
    {
        public int Id { get; set; }
        
        public string CaseSolutionConditionId { get; set; }

        public int CaseSolutionId { get; set; }

        public string PropertyName { get; set; }

        public string CaseSolutionConditionGuid { get; set; }

        public string Text { get; set; }

        public List<string> SelectedValues { get; set; }

        public List<SelectListItem> SelectList { get; set; }

        public string Table { get; set; }

        public static CaseSolutionSettingsFieldModel Create(CaseSolutionSettingsField data)
        {
            return new CaseSolutionSettingsFieldModel
            {
                Id = data.Id,
                CaseSolutionConditionId = data.CaseSolutionConditionId,
                CaseSolutionId = data.CaseSolutionId,
                PropertyName = data.PropertyName,
                CaseSolutionConditionGuid = data.CaseSolutionConditionGuid,
                Text = data.Text,
                SelectedValues = data.SelectedValues,
                SelectList = data.SelectList != null && data.SelectList.Any() ? data.SelectList.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.FieldGuid,
                    Selected = x.Selected,
                    Disabled = !x.Status
                }).ToList() : null,
                Table = data.Table
            };
        }
    }
}