namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public enum Tabs
    {
        Inventory = 0,
        Settings = 1,
        Reports = 2,
        MasterData = 3
    }

    public enum WorkstationEditTabs
    {
        Workstation = 0,
        Storage = 1,
        Programs = 2,
        Hotfix = 3,
        Log = 4,
        Accessories = 5,
        Cases = 6
    }

    public enum ServerEditTabs
    {
        Server = 0,
        Storage = 1,
        Programs = 2,
        Hotfix = 3,
        OperationLog = 4,
        Cases = 5
    }

    public enum PrinterEditTabs
    {
        Printer = 0,
        Cases = 1
    }

    public enum CustomInventoryTabs
    {
        CustomInventory = 0,
        Cases = 1
    }
}