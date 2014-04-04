using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Users.Input
{
    public sealed class UserModule
    {
        public int Id { get; set; }

        [IsId]
        public int User_Id { get; set; }

        [IsId]
        public int Module_Id { get; set; }

        public int Position { get; set; }

        public bool isVisible { get; set; }

        public int? NumberOfRows { get; set; }
    }
}