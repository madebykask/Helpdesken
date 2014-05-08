namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport
{
    using System;

    public sealed class ExportFileNameFormatter : IExportFileNameFormatter
    {
        public string Format(string prefix, string extension)
        {
            var currentDateAndTime = DateTime.Now;
            var currentDate = currentDateAndTime.ToShortDateString();
            var currentTime = currentDateAndTime.Hour + "." + currentDateAndTime.Minute;

            return string.Format("{0} {1} {2}.{3}", prefix, currentDate, currentTime, extension);
        }
    }
}