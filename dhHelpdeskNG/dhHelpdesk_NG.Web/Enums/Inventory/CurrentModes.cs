using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Web.Enums.Inventory
{
    public enum CurrentModes
    {
        /// <summary>
        /// The workstations.
        /// </summary>
        Workstations = -3,

        /// <summary>
        /// The servers.
        /// </summary>
        Servers = -2,

        /// <summary>
        /// The printers.
        /// </summary>
        Printers = -1,
    }

    public static class CurrentModesExtention
    {
        public static SelectList MapToSelectList(this Enum enumeration, string selected)
        {
            var list =
                (from CurrentModes d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(d.GetCaption()) }).ToList();
            return new SelectList(list, "ID", "Name", selected);


            
        }

        public static string GetCaption(this CurrentModes module)
        {
            switch (module)
            {
                case CurrentModes.Workstations:
                    return "Arbetsstationer";

                case CurrentModes.Servers:
                    return "Server";

                case CurrentModes.Printers:
                    return "Skrivare";

                default:
                    return string.Empty;
            }
        }
    }    
}