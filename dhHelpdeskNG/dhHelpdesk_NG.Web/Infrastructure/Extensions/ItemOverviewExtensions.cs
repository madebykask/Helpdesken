using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class ItemOverviewExtensions
    {
        public static List<ItemOverview> Translate(this List<ItemOverview> list)
        {
            foreach (var selectListItem in list)
                selectListItem.Name = Translation.GetCoreTextTranslation(selectListItem.Name);
            return list;
        }
    }
}