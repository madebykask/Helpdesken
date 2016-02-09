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

    public class ProcessResult
    {
        public enum ResultTypeEnum
        {            
            SUCCESS = 1,
            WARNING = 2,
            ERROR   = 3,
        }

        public ProcessResult(string processName)
        {
            CreatePrecessResult(processName, ResultTypeEnum.SUCCESS, string.Empty, null);
        }

        public ProcessResult(string processName, object data)
        {
            CreatePrecessResult(processName, ResultTypeEnum.SUCCESS, string.Empty, data);
        }

        public ProcessResult(string processName, ResultTypeEnum resultType)
        {
            CreatePrecessResult(processName, resultType, string.Empty, null);
        }

        public ProcessResult(string processName, ResultTypeEnum resultType, string message, object data = null)
        {
            CreatePrecessResult(processName, resultType, message, data);
        }

        private void CreatePrecessResult(string processName, ResultTypeEnum resultType, string lastMessage, object data)
        {
            this.ProcessName = processName;
            this.ResultType = resultType;
            switch (resultType)
            {
                case ResultTypeEnum.SUCCESS:
                    this.IsSuccess = true;
                    break;

                case ResultTypeEnum.WARNING:
                    this.IsSuccess = true;
                    break;

                case ResultTypeEnum.ERROR:
                    this.IsSuccess = false;
                    break;                
            }            

            this.LastMessage = lastMessage;
            this.Data = data;
        }        

        public string ProcessName { get; private set; }

        public object Data { get; private set; }

        public bool IsSuccess { get; private set; }

        public string LastMessage { get; private set; }

        public ResultTypeEnum ResultType { get; private set; }
    }
}