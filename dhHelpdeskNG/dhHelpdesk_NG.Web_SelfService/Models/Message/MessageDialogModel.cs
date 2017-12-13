
namespace DH.Helpdesk.SelfService.Models.Message
{
    public enum MessageTypes
    {
        none,
        error,
        success,
        warning,
        info
    }

    public class MessageDialogModel
    {
        public MessageDialogModel()
        {
            MsgType = MessageTypes.none;
            Title = "";
            Details = "";
        }

        public MessageDialogModel(MessageTypes msgType, string title, string details)
        {
            MsgType = msgType;
            Title = title;
            Details = details;
        }

        public string Title { get; set; }
        public string Details { get; set; }
        public MessageTypes MsgType { get; set; }
    }


}