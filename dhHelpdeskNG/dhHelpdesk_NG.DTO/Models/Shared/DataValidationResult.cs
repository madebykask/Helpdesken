﻿namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class DataValidationResult
    {
        public DataValidationResult()
        {
            IsValid = true;
            LastMessage = string.Empty;
        }

        public DataValidationResult(bool isValid)
        {
            IsValid = isValid;
            LastMessage = string.Empty;
        }

        public DataValidationResult(bool isValid, string lastMessage)
        {
            IsValid = isValid;
            LastMessage = lastMessage;
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
            ProcessName = processName;
            ResultType = resultType;
            switch (resultType)
            {
                case ResultTypeEnum.SUCCESS:
                    IsSucceed = true;
                    break;

                case ResultTypeEnum.WARNING:
                    IsSucceed = true;
                    break;

                case ResultTypeEnum.ERROR:
                    IsSucceed = false;
                    break;                
            }            

            LastMessage = lastMessage;
            Data = data;
        }        

        public string ProcessName { get; private set; }

        public object Data { get; private set; }

        public bool IsSucceed { get; private set; }

        public string LastMessage { get; private set; }

        public ResultTypeEnum ResultType { get; private set; }
    }
}