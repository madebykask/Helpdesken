
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.TaskScheduler.Enums
{
    public class FieldLenght
    {
        //Note names mus be lower
        [MaxLength(50)]
        public string userid { get; set; }

        [MaxLength(50)]
        public string logonname { get; set; }

        [MaxLength(50)]
        public string firstname { get; set; }

        [MaxLength(10)]
        public string initials { get; set; }

        [MaxLength(50)]
        public string surname { get; set; }

        [MaxLength(50)]
        public string fullname { get; set; }

        [MaxLength(50)]
        public string displayname { get; set; }

        [MaxLength(100)]
        public string location { get; set; }

        [MaxLength(50)]
        public string phone { get; set; }

        [MaxLength(50)]
        public string cellphone { get; set; }

        [MaxLength(100)]
        public string email { get; set; }

        [MaxLength(50)]
        public string usercode { get; set; }

        [MaxLength(50)]
        public string postaladdress { get; set; }

        [MaxLength(50)]
        public string postalcode { get; set; }

        [MaxLength(50)]
        public string city { get; set; }

        [MaxLength(50)]
        public string title { get; set; }

        [MaxLength(100)]
        public string ou { get; set; }

        [MaxLength(500)]
        public string info { get; set; }

        [MaxLength(50)]
        public string costcentre { get; set; }        

    }
}
