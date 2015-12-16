namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Common.ValidationAttributes;
    using System.Collections.Generic;

    public sealed class InvoiceArticle
    {
        public InvoiceArticle(
                    int id, 
                    int? parentId, 
                    InvoiceArticle parent, 
                    string number, 
                    string name, 
                    string nameEng,
                    string description,
                    int? unitId, 
                    InvoiceArticleUnit unit, 
                    decimal? ppu, 
                    List<Domain.ProductArea> productAreas,        
                    int customerId, 
                    bool textDemand,
                    bool blocked,
                    CustomerOverview customer)
        {
            this.Description = description;
            this.NameEng = nameEng;
            this.Customer = customer;
            this.CustomerId = customerId;
            this.ProductAreas = productAreas;
            this.Ppu = ppu;
            this.Unit = unit;
            this.UnitId = unitId;
            this.Name = name;
            this.Number = number;
            this.Parent = parent;
            this.ParentId = parentId;
            this.TextDemand = textDemand;
            this.Blocked = blocked;
            this.Id = id;
        }

        public InvoiceArticle(
                    InvoiceArticle parent, 
                    string number, 
                    string name, 
                    string nameEng,
                    string description,
                    List<Domain.ProductArea> productAreas,
                    InvoiceArticleUnit unit,
                    decimal? ppu)
        {
            this.Parent = parent;
            this.Number = number;
            this.Name = name;
            this.NameEng = nameEng;
            this.Description = description;
            this.ProductAreas = productAreas;
            this.Unit = unit;
            this.Ppu = ppu;
        }

        public InvoiceArticle(
                    string number, 
                    string name,
                    List<Domain.ProductArea> productAreas)
        {
            this.Number = number;
            this.Name = name;
            this.NameEng = string.Empty;
            this.ProductAreas = new List<Domain.ProductArea>();
        }

        [IsId]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public InvoiceArticle Parent { get; private set; }

        public string Number { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public string NameEng { get; private set; }

        public string Description { get; private set; }

        public int? UnitId { get; set; }

        public InvoiceArticleUnit Unit { get; private set; }

        public decimal? Ppu { get; private set; }

        [ScriptIgnore]
        public List<Domain.ProductArea> ProductAreas { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        public bool TextDemand { get; set; }

        public bool Blocked { get; set; }

        [NotNull]
        public CustomerOverview Customer { get; private set; }
    }
}