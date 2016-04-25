
using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Web.Mvc;
using System.Linq;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel
{
    public enum ModuleTypes
    {
        OperatingSystem = 1,

        Processor = 2,

        Ram = 3,

        NetworkAdapter = 4,

        ComputerModel = 5,

        ComputerType = 6
    }

    public static class ModuleTypeExtention
    {
        public static SelectList MapToSelectList(this Enum enumeration, string selected)
        {
            var list =
                (from ModuleTypes d in Enum.GetValues(enumeration.GetType())
                 select new { ID = Convert.ToInt32(d), Name = Translation.GetCoreTextTranslation(d.GetCaption()) }).ToList();
            return new SelectList(list, "ID", "Name", selected);
        }

        public static string GetCaption(this ModuleTypes module)
        {
            switch (module)
            {
                case ModuleTypes.OperatingSystem:
                    return "Operativsystem";                    

                case ModuleTypes.Processor:
                    return "Processor";                    

                case ModuleTypes.Ram:
                    return "RAM";                    

                case ModuleTypes.NetworkAdapter:
                    return "Nätverkskort/modem";                    

                case ModuleTypes.ComputerModel:
                    return "Datormodell";                    

                case ModuleTypes.ComputerType:
                    return "Datortyp";
                    
                default:
                    return string.Empty;
            }
        }    
    }    
}