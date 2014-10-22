using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain.Faq
{
    using global::System;
    using global::System.Collections.Generic;

    public class FaqEntity : Entity, ISingleWorkingGroupEntity, IOptionalCustomerEntity, IStartPageEntity, IDatedEntity
    {
        #region Public Properties

        public string Answer { get; set; }

        public string Answer_Internal { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int? Customer_Id { get; set; }

        public virtual FaqCategoryEntity FAQCategory { get; set; }

        public int FAQCategory_Id { get; set; }

        public virtual ICollection<FaqFileEntity> FAQFiles { get; set; }

        public string FAQQuery { get; set; }

        public int InformationIsAvailableForNotifiers { get; set; }

        public int ShowOnStartPage { get; set; }

        public string URL1 { get; set; }

        public string URL2 { get; set; }

        public virtual WorkingGroupEntity WorkingGroup { get; set; }

        public int? WorkingGroup_Id { get; set; }

        #endregion
    }
}