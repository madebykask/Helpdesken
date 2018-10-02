namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    public sealed class UserOverview
    {
        public UserOverview(
                string user, 
                string notifier, 
                string email, 
                string phone, 
                string cellPhone, 
                string customer, 
                string region, 
                string department, 
                string unit, 
                string place, 
                string ordererCode,
                string costcentre,
                string isaboutuser,
                string isaboutpersonsname,
                string isaboutpersonsphone,
                string isaboutusercode,
                string isaboutpersonsemail,
                string isaboutpersonscellphone,
                string isaboutcostcentre,
                string isaboutplace,
                string isaboutdepartment,
                string isaboutou,
                string isaboutregion)
        {
            this.OrdererCode = ordererCode;
            this.Place = place;
            this.Unit = unit;
            this.Department = department;
            this.Region = region;
            this.Customer = customer;
            this.CellPhone = cellPhone;
            this.Phone = phone;
            this.Email = email;
            this.Notifier = notifier;
            this.User = user;
            this.CostCentre = costcentre;
            this.IsAbout_User = isaboutuser;
            this.IsAbout_Persons_Name = isaboutpersonsname;
            this.IsAbout_Persons_Phone = isaboutpersonsphone;
            this.IsAbout_UserCode = isaboutusercode;
            this.IsAbout_Persons_Email = isaboutpersonsemail;
            this.IsAbout_Persons_CellPhone = isaboutpersonscellphone;
            this.IsAbout_ConstCentre = isaboutcostcentre;
            this.IsAbout_Place = isaboutplace;
            this.IsAbout_Department = isaboutdepartment;
            this.IsAbout_OU = isaboutou;
            this.IsAbout_Region = isaboutregion;
        }

        public string User { get; private set; }

        public string Notifier { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Customer { get; private set; }

        public string Region { get; private set; }

        public string Department { get; private set; }

        public string Unit { get; private set; }

        public string Place { get; private set; }

        public string OrdererCode { get; private set; }

        public string CostCentre { get; set; }

        public string IsAbout_User { get; private set; }

        public string IsAbout_Persons_Name { get; set; }

        public string IsAbout_Persons_Phone { get; set; }

        public string IsAbout_Persons_CellPhone { get; set; }

        public string IsAbout_Department { get; set; }

        public string IsAbout_UserCode { get; set; }

        public string IsAbout_Persons_Email { get; set; }

        public string IsAbout_Region { get; set; }

        public string IsAbout_OU { get; set; }

        public string IsAbout_ConstCentre { get; set; }

        public string IsAbout_Place { get; set; }
    }
}