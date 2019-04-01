using System;

namespace Common.Logging
{
    public class DefaultErrorFormatter : IErrorMessageFormatter
    {
        public virtual string FormatError(string errorId)
        {
            return FormatError("", errorId);
        }

        public virtual string FormatError(string messageText, string errorId)
        {
            const string messageTemplate = "ErrorID: {0}.\r\nMessage: {1}.";
            return string.Format(messageTemplate, errorId, messageText);
        }

        public virtual string FormatError(string messageText, string errorId, Exception ex)
        {
            return FormatError("", errorId);
        }

        public virtual string FormatWarning(string errorId)
        {
            return FormatError("", errorId);
        }

        public virtual string FormatWarning(string messageText, string errorId)
        {
            return FormatError("", errorId);
        }

        public virtual string FormatWarning(string messageText, string errorId, Exception ex)
        {
            return FormatError("", errorId);
        }
    }

    public interface IErrorMessageFormatter
    {
        string FormatError(string errorId);

        string FormatError(string messageText, string errorId);

        string FormatError(string messageText, string errorId, Exception ex);

        string FormatWarning(string errorId);

        string FormatWarning(string messageText, string errorId);

        string FormatWarning(string messageText, string errorId, Exception ex);
    }
}