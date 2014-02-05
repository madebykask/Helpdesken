using System;
using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Domain
{
    public interface IOperationLogSearch : ISearch
    {
        int CustomerId { get; set; }
        string Text_Filter { get; set; }
        int[] OperationObject_Filter { get; set; }
        int[] OperationCategory_Filter { get; set; }
        [DataType(DataType.Date)]
        string PeriodFrom { get; set; }
        [DataType(DataType.Date)]
        string PeriodTo { get; set; }
    }

    public class OperationLogSearch : Search, IOperationLogSearch
    {
        public int CustomerId { get; set; }
        public string Text_Filter { get; set; }
        public int[] OperationObject_Filter { get; set; }
        public int[] OperationCategory_Filter { get; set; }        
        public string PeriodFrom { get; set; }        
        public string PeriodTo { get; set; }


    }
}
