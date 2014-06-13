namespace DH.Helpdesk.BusinessData.Models.ActionSetting
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class ActionSetting
    {        
        public int CustomerId { get; set; }

        public int ObjectId { get; set; }
        
        [StringLength(800)]
        public string ObjectValue { get; set; }

        [StringLength(100)]
        public string ObjectClass { get; set; }

        public bool Visibled { get; set; }
                      
    }
}

