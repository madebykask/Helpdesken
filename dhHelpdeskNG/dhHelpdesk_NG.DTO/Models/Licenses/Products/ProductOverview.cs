namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using System.Collections.Generic;
    using System.Linq;


    //public sealed class ProductOverview
    //{
    //    public ProductOverview(
    //            int productId,
    //            string productName,
    //            string[] regions,
    //            KeyValuePair<int, string>[] departments,
    //            KeyValuePair<int?, int>[] licensesNumbers,
    //            KeyValuePair<int?, int>[] usedLicensesNumber
    //        )
    //    {
    //        this.ProductId = productId;
    //        this.ProductName = productName;
    //        this.Regions = regions;            
    //        this.ProductLicenses = licensesNumbers.Select(ln => new  ProductLicenses(
    //                                                                         (ln.Key.HasValue && ln.Key.Value != 0)? 
    //                                                                               (ln) : 0,
    //                                                                        (ln.Key.HasValue)? ln.Key.Value : 0,
    //                                                                         departments.Where(d => d.Key == ln.Key).FirstOrDefault().Value,
    //                                                                         "",
    //                                                                         ln.Value,
    //                                                                         usedLicensesNumber.Where(u=> u.Key == ln.Key).Distinct().Count()
    //                                                                        )).ToArray();                      
    //    }

    //    public int ProductId { get; private set; }

    //    public string ProductName { get; private set; }

    //    [NotNull]
    //    public string[] Regions { get; private set; }

    //    public ProductLicenses[] ProductLicenses { get; private set; }        
    
    //}

    public sealed class ProductOverview
    {
        public ProductOverview(
                int productId,
                string productName)
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.ProductLicenses = new List<ProductLicense>();
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }
        
        public List<ProductLicense> ProductLicenses { get; set; }

    }

    public sealed class ProductLicense
    {
        public ProductLicense()
        {
        }
        public ProductLicense(
                int? regionId,
                int? departmentId,
                string regionName,
                string departmentName,
                int numberOfLicenses,
                int numberOfUsedLicenses)
        {
            this.RegionId = regionId;
            this.DepartmentId = departmentId;
            this.RegionName = regionName;
            this.DepartmentName = departmentName;
            this.NumberOfLicenses = numberOfLicenses;
            this.NumberOfUsedLicenses = numberOfUsedLicenses;
            this.LicensesDifference = numberOfLicenses - numberOfUsedLicenses;
        }

        public int? RegionId { get; private set; }

        public int? DepartmentId { get; private set; }

        public string RegionName { get; private set; }

        public string DepartmentName { get; private set; }

        public int NumberOfLicenses { get; set; }

        public int NumberOfUsedLicenses { get; private set; }

        public int LicensesDifference { get; private set; }
    } 
}