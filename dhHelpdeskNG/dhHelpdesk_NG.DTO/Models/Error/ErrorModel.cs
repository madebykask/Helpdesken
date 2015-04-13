namespace DH.Helpdesk.BusinessData.Models.Error
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class ErrorModel
    {
        public ErrorModel(string message)
        {            
            this.Message = message;            
        }

        public ErrorModel(int errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;            
        }

        public int ErrorCode { get; private set; }

        public string Message { get; private set; }        
    }
}