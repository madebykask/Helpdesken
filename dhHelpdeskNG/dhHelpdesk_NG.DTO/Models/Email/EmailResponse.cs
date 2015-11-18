namespace DH.Helpdesk.BusinessData.Models.Email
{
    using System;

    public sealed class EmailResponse
    {
        public EmailResponse()
        {            
        }

        public EmailResponse(DateTime? sendTime, string responseMessage, int numberOfTry)
        {
            this.SendTime = sendTime;
            this.ResponseMessage = responseMessage;
            this.NumberOfTry = numberOfTry; 
        }        
        
        public DateTime? SendTime { get; set; }
        
        public string ResponseMessage { get; set; }

        public int NumberOfTry { get; set; }

        public static EmailResponse GetEmptyEmailResponse()
        {
            return new EmailResponse(DateTime.Now, string.Empty, 1);
        }

    }
}