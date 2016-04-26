using System;
using System.Web.Mvc;
using System.Linq;
using DH.Helpdesk.Web.Infrastructure;
namespace DH.Helpdesk.Web.Enums.Inventory
{
    public enum ReportTypes
    {
        OperatingSystem = -8,

        ServicePack = -7,

        Processor = -6,

        Ram = -5,

        NetworkAdapter = -4,

        InstaledPrograms = -3,

        Inventory = -2,

        Location = -1,
    }

    public static class ReportTypeExtention
    {
        public static SelectList MapToSelectList(this Enum enumeration, string selected)
        {
            var list =
                (from ReportTypes d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(d.GetCaption()) }).ToList();
            return new SelectList(list, "ID", "Name", selected);
        }

        public static string GetCaption(this ReportTypes module)
        {
            switch (module)
            {
                case ReportTypes.OperatingSystem:
                    return "Operativsystem";

                case ReportTypes.ServicePack:
                    return "Operativsystem/Servicepack";

                case ReportTypes.Processor:
                    return "Processor";

                case ReportTypes.Ram:
                    return "RAM";

                case ReportTypes.NetworkAdapter:
                    return "Nätverkskort";

                case ReportTypes.InstaledPrograms:
                    return "Installerade program";

                case ReportTypes.Inventory:
                    return "Inventarier";

                case ReportTypes.Location:
                    return "Placering";

                default:
                    return string.Empty;
            }
        }
    }    
}