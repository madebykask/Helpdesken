using DH.Helpdesk.WebApi.Infrastructure.ClientLogger.Enums;

namespace DH.Helpdesk.WebApi.Models
{
    public class ClientLogItemModel 
    {
        public string UniqueId { get; set; }
        public bool IsAuthenticated { get; set; }
        public string SessionId { get; set; }
        public string Url { get; set; }
        public ClientLogLevel Level { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }
}