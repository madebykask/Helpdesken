namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class MyFavoriteFilter
    {
        public MyFavoriteFilter()
        {
            this.Fields = new MyFavoriteFilterFields();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public MyFavoriteFilterFields Fields { get; set; }
    }

    public class MyFavoriteFilterFields
    {
        #region Constructor

        public MyFavoriteFilterFields()
        {
            this.RegionIds = new SelectedItems();        
            this.DepartmentIds = new SelectedItems();
            this.RegisteredByIds = new SelectedItems();
            this.CaseTypeIds = new SelectedItems();
            this.ProductAreaIds = new SelectedItems();
            this.WorkingGroupIds = new SelectedItems();
            this.ResponsibleIds = new SelectedItems();
            this.AdministratorIds = new SelectedItems();
            this.PriorityIds = new SelectedItems();
            this.StatusIds = new SelectedItems();
            this.SubStatusIds = new SelectedItems();
            this.RemainingTimeId = new SelectedItems();
            this.ClosingReason = new SelectedItems();
            this.RegistrationDate = new DateToDate();        
            this.WatchDate = new DateToDate();        
            this.ClosingDate = new DateToDate();            
        }

        #endregion

        #region Properties

        public SelectedItems RegionIds { get; set; }
        
        public SelectedItems DepartmentIds { get; set; }

        public SelectedItems RegisteredByIds { get; set; }

        public SelectedItems CaseTypeIds { get; set; }

        public SelectedItems ProductAreaIds { get; set; }

        public SelectedItems WorkingGroupIds { get; set; }

        public SelectedItems ResponsibleIds { get; set; }

        public SelectedItems AdministratorIds { get; set; }

        public SelectedItems PriorityIds { get; set; }

        public SelectedItems StatusIds { get; set; }

        public SelectedItems SubStatusIds { get; set; }

        public SelectedItems RemainingTimeId { get; set; }

        public SelectedItems ClosingReason { get; set; }

        public DateToDate RegistrationDate { get; set; }
        
        public DateToDate WatchDate { get; set; }
        
        public DateToDate ClosingDate { get; set; }
                

        #endregion
    }

}