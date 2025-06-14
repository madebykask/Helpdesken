﻿namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class CommunicationFields
    {
        public CommunicationFields(
            string networkAdapterName,
            string ipAddress,
            string macAddress,
            bool ras,
            string novellClient)
        {
            this.NetworkAdapterName = networkAdapterName;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.RAS = ras;
            this.NovellClient = novellClient;
        }

        public string NetworkAdapterName { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }

        public bool RAS { get; set; }

        public string NovellClient { get; set; }
    }
}