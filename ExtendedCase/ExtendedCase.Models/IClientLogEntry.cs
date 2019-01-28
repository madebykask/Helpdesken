using Common.Logging.ClientLogger.Enums;

namespace ExtendedCase.Models
{
    public interface IClientLogEntry
    {
        string ErrorId { get; }
        string Url { get; }
        ClientLogLevel Level { get; }
        string Message { get; }
        string Stack { get; }
        string Param1 { get; }
        string Param2 { get; }
        string Param3 { get; }
    }
}