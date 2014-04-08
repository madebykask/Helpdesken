namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport
{
    public interface IExportFileNameFormatter
    {
        string Format(string prefix, string extension);
    }
}