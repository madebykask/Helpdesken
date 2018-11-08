using DH.Helpdesk.WebApi.Infrastructure.ClientLogger.Enums;

namespace DH.Helpdesk.WebApi.Infrastructure.ClientLogger
{
    public interface IClientLogEntry
    {
        string UniqueId { get; }
        string Url { get; }
        ClientLogLevel Level { get; }
        string Message { get; }
        string Stack { get; }
        string Param1 { get; }
        string Param2 { get; }
        string Param3 { get; }
    }

    public class ClientLogEntry : IClientLogEntry
    {
        public string UniqueId { get; set; }
        public string Url { get; set; }
        public ClientLogLevel Level { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }
}