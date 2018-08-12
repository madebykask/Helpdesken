using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;

namespace DH.Helpdesk.Domain.Computers
{
	public class ComputerUserCategory
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public Guid ComputerUsersCategoryGuid { get; set; }
		public bool IsReadOnly { get; set; }
        public bool IsEmpty { get; set; }
		public int? CaseSolutionID { get; set; }
	    public int? ExtendedCaseFormID { get; set; }
	    public int CustomerID { get; set; }

        public CaseSolution CaseSolution { get; set; }
		public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }
		public Customer Customer { get; set; }

	    #region Empty Category

	    public static int EmptyCategoryId = 0;
	    public static string EmptyCategoryDefaultName = "Employee";
	    public static ComputerUserCategory CreateEmptyCategory()
	    {
	        return new ComputerUserCategory()
	        {
	            ID = EmptyCategoryId,
	            IsEmpty = true,
	            Name = EmptyCategoryDefaultName
	        };
	    }

	    #endregion
	}
}
