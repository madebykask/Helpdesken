namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport
{
    using System;

    public sealed class ExportFileNameFormatter : IExportFileNameFormatter
    {
        public string Format(string prefix, string extension)
        {
            return string.Format(
                "{0} {1} {2}.{3}",
                prefix,
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToShortTimeString(),
                extension);
        }
    }
}