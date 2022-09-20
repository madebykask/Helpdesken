using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM
{
    public class ADALConfiguration
    {

        public ADALConfiguration(string ADAL_Microsoft_Base_Url, string ADAL_Tenant_Name, string ADAL_Client_ID, string ADAL_Resource_ID, string ADAL_Username, string ADAL_Password, int ADAL_Retry_Connection_Count, int ADAL_Retry_Connection_Increment_MS)
        {
            this.ADAL_Microsoft_Base_Url = ADAL_Microsoft_Base_Url;
            this.ADAL_Tenant_Name = ADAL_Tenant_Name;
            this.ADAL_Client_ID = ADAL_Client_ID;
            this.ADAL_Resource_ID = ADAL_Resource_ID;
            this.ADAL_Username = ADAL_Username;
            this.ADAL_Password = ADAL_Password;
            this.ADAL_Retry_Connection_Count = ADAL_Retry_Connection_Count;
            this.ADAL_Retry_Connection_Increment_MS = ADAL_Retry_Connection_Increment_MS;

        }

        public string ADAL_Microsoft_Base_Url { get; set; }

        public string ADAL_Tenant_Name { get; set; }

        public string ADAL_Client_ID { get; set; }

        public string ADAL_Resource_ID { get; set; }

        public string ADAL_Username { get; set; }
        
        public string ADAL_Password { get; set; }

        public int ADAL_Retry_Connection_Count { get; set; }

        public int ADAL_Retry_Connection_Increment_MS { get; set; }


        /// <summary>
        /// Check if all the properties is valid
        /// </summary>
        /// <returns>True or false</returns>
        public bool ValidConfiguration()
        {

            if (String.IsNullOrEmpty(ADAL_Microsoft_Base_Url))
            {
                return false;
            }

            if (String.IsNullOrEmpty(ADAL_Tenant_Name))
            {
                return false;
            }

            if (String.IsNullOrEmpty(ADAL_Client_ID))
            {
                return false;
            }

            if (String.IsNullOrEmpty(ADAL_Resource_ID))
            {
                return false;
            }

            if (String.IsNullOrEmpty(ADAL_Username))
            {
                return false;
            }

            if (String.IsNullOrEmpty(ADAL_Password))
            {
                return false;
            }

            if (ADAL_Retry_Connection_Count < 0)
            {
                return false;
            }

            if (ADAL_Retry_Connection_Increment_MS < 0)
            {
                return false;
            }

            return true;

        }

    }
}
