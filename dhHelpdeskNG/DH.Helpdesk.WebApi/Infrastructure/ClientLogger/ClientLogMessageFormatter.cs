using System;
using System.Text;

namespace DH.Helpdesk.WebApi.Infrastructure.ClientLogger
{
    public interface IClientLogMessageFormatter
    {
        string Format(IClientLogEntry entry);
    }

    public class ClientLogMessageFormatter : IClientLogMessageFormatter
    {
        public string Format(IClientLogEntry entry)
        {
            var lineSeparator = new string('-', 60);
            var str = new StringBuilder();

            str.AppendLine(lineSeparator);
            str.AppendLine($"\t{DateTime.Now}");
            str.AppendLine(lineSeparator);

            WriteFormatted(str, "UniqueId", entry.UniqueId);
            WriteFormatted(str, "Level", entry.Level.ToString());
            WriteFormatted(str, "Url", entry.Url);
            WriteFormatted(str, "Message", entry.Message);
            WriteFormatted(str, "Stack", entry.Stack);
            WriteFormatted(str, "Param1", entry.Param1);
            WriteFormatted(str, "Param2", entry.Param2);
            WriteFormatted(str, "Param3", entry.Param3);
           
            str.AppendLine(lineSeparator);
            return str.ToString();
        }

        public void WriteFormatted(StringBuilder strBld, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            var msg = FormatLine(key, value);
            strBld.AppendLine(msg);
        }

        private string FormatLine(string title, string value)
        {
            return $"\t{title,-9}: {value}";
        }
    }
}