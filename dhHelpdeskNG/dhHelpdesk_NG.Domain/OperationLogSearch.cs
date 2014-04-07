namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.ComponentModel.DataAnnotations;

    public interface IOperationLogSearch : ISearch
    {
        int CustomerId { get; set; }
        string Text_Filter { get; set; }
        int[] OperationObject_Filter { get; set; }
        int[] OperationCategory_Filter { get; set; }
        [DataType(DataType.Date)]
        DateTime? PeriodFrom { get; set; }
        [DataType(DataType.Date)]
        DateTime ?PeriodTo { get; set; }
    }

    public class OperationLogSearch : Search, IOperationLogSearch
    {
        public int CustomerId { get; set; }
        public string Text_Filter { get; set; }
        public int[] OperationObject_Filter { get; set; }
        public int[] OperationCategory_Filter { get; set; }        
        public DateTime? PeriodFrom { get; set; }        
        public DateTime? PeriodTo { get; set; }


    }
}
