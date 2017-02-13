namespace DH.Helpdesk.Domain
{
	using DH.Helpdesk.Domain.Interfaces;

	using global::System;
	using global::System.Collections.Generic;

	public class OrderType : Entity, ICustomerEntity, INamedEntity
	{
		public OrderType()
		{
			SubOrderTypes = new List<OrderType>();
			Users = new List<User>();
		}

		public int? CreateCase_CaseType_Id { get; set; }
		public int Customer_Id { get; set; }
		public int? Document_Id { get; set; }
		public int IsActive { get; set; }
		public int IsDefault { get; set; }
		public int? Parent_OrderType_Id { get; set; }
		public string CaptionOrdererInfo { get; set; }
		public string CaptionUserInfo { get; set; }
        public string CaptionReceiverInfo { get; set; }
        public string CaptionGeneral { get; set; }
        public string CaptionOrder { get; set; }
        public string CaptionOrderInfo { get; set; }
        public string CaptionDeliveryInfo { get; set; }
        public string CaptionProgram { get; set; }
        public string CaptionOther { get; set; }
        public string Description { get; set; }
		public string EMail { get; set; }
		public string Name { get; set; }
		public DateTime ChangedDate { get; set; }
		public DateTime CreatedDate { get; set; }

		public virtual Case CreateCase_CaseType { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual Document Document { get; set; }
		public virtual OrderType ParentOrderType { get; set; }
		public virtual ICollection<OrderType> SubOrderTypes { get; set; }
		public virtual ICollection<User> Users { get; set; }
	}
}
