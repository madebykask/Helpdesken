namespace DH.Helpdesk.SelfService.Models
{
    public interface IConfigurableFieldModel
    {
        string Caption { get; }

        bool IsRequired { get; }

        bool Show { get; }
    }
}