using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;

namespace DH.Helpdesk.Web.Areas.Inventory.Infrastucture.Extensions
{
    public static class TabSettingExtensions
    {
        public static string GetName(this TabSetting setting, string defaultValue)
        {
            return string.IsNullOrEmpty(setting.Caption) ? defaultValue : setting.Caption;
        }
    }
}