using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.WebApi.Models.Output
{
    public class CaseFavoriteFilterModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ItemOverview> Fields { get; set; }
    }
}