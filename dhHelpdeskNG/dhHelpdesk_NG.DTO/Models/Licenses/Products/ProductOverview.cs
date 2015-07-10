namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using System.Collections.Generic;
    using System.Linq;


    public sealed class ProductOverview
    {
        public ProductOverview(
                int productId,
                string productName,
                string[] regions,
                KeyValuePair<int, string>[] departments,
                KeyValuePair<int?, int>[] licensesNumbers,
                KeyValuePair<int?, int>[] usedLicensesNumber
            )
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.Regions = regions;            
            this.DepartmentLicenses = licensesNumbers.Select(ln => new  DepartmentLicens(
                                                                            (ln.Key.HasValue)? ln.Key.Value : 0,
                                                                             departments.Where(d => d.Key == ln.Key).FirstOrDefault().Value,
                                                                             ln.Value,
                                                                             usedLicensesNumber.Where(u=> u.Key == ln.Key).Distinct().Count()
                                                                            )).ToArray();                      
        }

        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        [NotNull]
        public string[] Regions { get; private set; }
        
        public DepartmentLicens[] DepartmentLicenses { get; private set; }        
    
    }

    public sealed class DepartmentLicens
    {
        public DepartmentLicens(
                int? departmentId,
                string departmentName,
                int numberOfLicenses,
                int numberOfUsedLicenses)
        {
            this.DepartmentId = departmentId;
            this.DepartmentName = departmentName;
            this.NumberOfLicenses = numberOfLicenses;
            this.NumberOfUsedLicenses = numberOfUsedLicenses;
            this.LicensesDifference = numberOfLicenses - numberOfUsedLicenses;
        }

        public int? DepartmentId { get; private set; }

        public string DepartmentName { get; private set; }

        public int NumberOfLicenses { get; private set; }

        public int NumberOfUsedLicenses { get; private set; }

        public int LicensesDifference { get; private set; }
    } 
}