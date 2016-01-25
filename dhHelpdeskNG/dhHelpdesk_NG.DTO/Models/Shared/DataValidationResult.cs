using System;
namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class DataValidationResult
    {
        public DataValidationResult()
        {
            this.IsValid = true;
            this.LastMessage = string.Empty;
        }

        public DataValidationResult(bool isValid)
        {
            this.IsValid = isValid;
            this.LastMessage = string.Empty;
        }

        public DataValidationResult(bool isValid, string lastMessage)
        {
            this.IsValid = isValid;
            this.LastMessage = lastMessage;
        }

        public bool IsValid { get; private set; }

        public string LastMessage { get; private set; }        
    }
}