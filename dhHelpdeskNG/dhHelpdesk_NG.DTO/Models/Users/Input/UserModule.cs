using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Users.Input
{
    public sealed class UserModule
    {
        [IsId]
        public int Id { get; set; }

        [IsId]
        public int User_Id { get; set; }

        [IsId]
        public int Module_Id { get; set; }

        [MinValue(1)]
        [MaxValue(9)]
        public int Position { get; set; }

        public bool isVisible { get; set; }

        [MinValue(0)]
        [MaxValue(10)]
        public int NumberOfRows { get; set; }         
    }
}