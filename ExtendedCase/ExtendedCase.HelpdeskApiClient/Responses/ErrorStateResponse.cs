using System.Collections.Generic;

namespace ExtendedCase.HelpdeskApiClient.Responses
{
    public class ErrorStateResponse
    {
        public string Message { get; set; }
        public IDictionary<string, string[]> ModelState { get; set; }
    }
}