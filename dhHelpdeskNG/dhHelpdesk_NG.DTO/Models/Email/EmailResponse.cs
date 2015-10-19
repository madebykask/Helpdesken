namespace DH.Helpdesk.BusinessData.Models.Email
{
    using System;

    public sealed class EmailResponse
    {
        public EmailResponse()
        {            
        }

        public EmailResponse(DateTime? sendTime, string responseMessage)
        {
            this.SendTime = sendTime;
            this.ResponseMessage = responseMessage;
        }        
        
        public DateTime? SendTime { get; set; }
        
        public string ResponseMessage { get; set; }

    }
}