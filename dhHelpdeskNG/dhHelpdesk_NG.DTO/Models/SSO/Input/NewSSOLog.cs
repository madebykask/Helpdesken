using System;
namespace DH.Helpdesk.BusinessData.Models.SSO.Input
{
    public sealed class NewSSOLog
    {
        public string ApplicationId { get; set; }

        public string NetworkId { get; set; }

        public string ClaimData { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}