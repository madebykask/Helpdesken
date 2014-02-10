namespace DH.Helpdesk.Domain.Servers
{
    using global::System;

    public class ServerSoftware : Entity
    {
        public int Server_Id { get; set; }
        public string Install_directory { get; set; }
        public string Installed_on { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string Product_key { get; set; }
        public string Registration_code { get; set; }
        public string Registered_to { get; set; }
        public string Uninstall_executable { get; set; }
        public string Uninstalled_on { get; set; }
        public string URL_info_about { get; set; }
        public string Version { get; set; }
        public string Version_major { get; set; }
        public string Version_minor { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Server Server { get; set; }
    }
}
