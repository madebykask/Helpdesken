using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Linq;
using dhHelpdesk_NG.DTO.DTOs.Case;
using dhHelpdesk_NG.DTO.Utils;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICaseSearchRepository
    {
        IList<CaseSearchResult> Search(CaseSearchFilter f, IList<CaseSettings> csl, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, GlobalSetting gs, Setting customerSetting, ISearch s);
    }

    public class CaseSearchRepository : ICaseSearchRepository
    {
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly IProductAreaRepository _productAreaRepository;

        public CaseSearchRepository(ICustomerUserRepository customerUserRepository, IProductAreaRepository productAreaRepository)
        {
            _customerUserRepository = customerUserRepository;
            _productAreaRepository = productAreaRepository; 
        }

        public IList<CaseSearchResult> Search(CaseSearchFilter f, IList<CaseSettings> csl, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, GlobalSetting gs, Setting customerSetting, ISearch s)
        {
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;
            var customerUserSetting = _customerUserRepository.GetCustomerSettings(f.CustomerId, userId);
            IList<ProductArea> pal = _productAreaRepository.GetMany(x => x.Customer_Id == f.CustomerId).OrderBy(x => x.Name).ToList(); 
            IList<CaseSearchResult> ret = new List<CaseSearchResult>();

            var sql = ReturnCaseSearchSql(f, customerSetting, customerUserSetting, userId, userUserId, showNotAssignedWorkingGroups, userGroupId, restrictedCasePermission, gs, s);

            using (var con = new OleDbConnection(dsn)) 
            {
                using (var cmd = new OleDbCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        var dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var row = new CaseSearchResult();  
                                IList<Universal> cols = new List<Universal>();
                                var toolTip = string.Empty;
                                var sortOrder = string.Empty; 

                                foreach (var c in csl)
                                {
                                    var fieldExists = false; 

                                    //kolumner som visas beroende på inställningar
                                    for (var i = 0; i < dr.FieldCount; i++)
                                    {
                                        if (string.Compare(dr.GetName(i), c.Name, true, CultureInfo.InvariantCulture) == 0)
                                        {
                                            if (!dr.IsDBNull(i))
                                            {
                                                bool translateField = false;
                                                if (c.Line == 1)
                                                {
                                                    string value = GetDatareaderValue(dr, i, c.Name, customerSetting, pal, out translateField);
                                                    cols.Add(new Universal { StringValue = value, TranslateThis = translateField });
                                                    if (string.Compare(s.SortBy, c.Name, true, CultureInfo.InvariantCulture) == 0)
                                                        sortOrder = value;
                                                }
                                                else
                                                    // TODO översättning, behövs?
                                                    toolTip += GetDatareaderValue(dr, i, c.Name, customerSetting, pal, out translateField) + Environment.NewLine;
                                                fieldExists = true;
                                            }
                                            break; 
                                        }
                                    }
                                    // finns inte kolumnen eller den är null så lägg till ett tomt värde
                                    if (!fieldExists && c.Line == 1)
                                        cols.Add(new Universal { StringValue = string.Empty});
                                }

                                row.SortOrder = sortOrder; 
                                row.Tooltip = toolTip; 
                                row.CaseIcon = GetCaseIcon(dr);
                                row.Id = dr.SafeGetInteger("Id"); 
                                row.Columns = cols;
                                ret.Add(row); 
                            }
                        }
                        dr.Close(); 
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return SortSearchResult(ret, s);
        }

        private IList<CaseSearchResult> SortSearchResult(IList<CaseSearchResult> csr, ISearch s)
        {
            //tid kvar samt produktområde kan inte sorteras i databasen
            if (string.Compare(s.SortBy, "ProductArea_Id", true, CultureInfo.InvariantCulture) == 0 || string.Compare(s.SortBy, "_temporary_.LeadTime", true, CultureInfo.InvariantCulture) == 0)
            {
                if (s.Ascending) 
                    return csr.OrderBy(x => x.SortOrder).ToList();
                return csr.OrderByDescending(x => x.SortOrder).ToList();
            }
            return csr;
        }

        private static GlobalEnums.CaseIcon GetCaseIcon(IDataReader dr)
        {
            var ret = GlobalEnums.CaseIcon.Normal;

            // TODO Hantera urgent
            if (dr.SafeGetNullableDateTime("FinishingDate") != null)
                if (dr.SafeGetNullableDateTime("ApprovedDate") == null && string.Compare("1", dr.SafeGetString("RequireApproving"), true, CultureInfo.InvariantCulture) == 0)
                    ret = GlobalEnums.CaseIcon.FinishedNotApproved;
                else
                    ret = GlobalEnums.CaseIcon.Finished;

            return ret;
        }

        private static string GetDatareaderValue(IDataReader dr, int col, string fieldName, Setting customerSetting, IList<ProductArea> pal, out bool translateField) 
        {
            var ret = string.Empty;
            var sep = " - ";
            translateField = false; 

            switch (fieldName.ToLower())
            {
                case "regtime":
                    //TODO TimezoneOffset skall beräknas
                    ret = dr[col].ToString();
                    break;
                case "department_id":
                    if (customerSetting.DepartmentFormat == 1)
                        ret = dr.SafeGetString("DepertmentName") + sep + dr.SafeGetString("SearchKey") + sep + dr.SafeGetString("DepartmentId");
                    //else if (customerSetting.DepartmentFormat == 0)
                    //    ret = dr.SafeGetString("DepartmentId");
                    else
                       ret = dr.SafeGetString("DepertmentName");
                    break;
                case "priority_id":
                    ret = dr.SafeGetString("PriorityName");
                    translateField = true;
                    break;
                case "casetype_id": case "status_id":
                    ret = dr[col].ToString();
                    translateField = true;
                    break;
                case "plandate":
                    if (customerSetting.PlanDateFormat == 0)
                       ret = dr.SafeGetFormatedDateTime(col);
                    else
                       ret = dr.SafeGetDateTimeWithWeek(col);
                    break;
                case "sms": case "contactbeforeaction":
                    ret = dr.SafeGetIntegerAsYesNo(col);
                    translateField = true;
                    break;
                case "verified":
                    ret = dr.SafeGetIntegerAsYesNo(col, true);
                    translateField = true;
                    break;
                case "leadtime":
                    // TODO hur gör vi här, värdet ska räknas ut, i db anrop? 
                    ret = "-";
                    break;
                case "productarea_id":
                    ProductArea p = dr.SafeGetInteger("ProductArea_Id").getProductAreaItem(pal);
                    if (p != null)
                        ret = p.getProductAreaParentPath();
                    break;
                default:
                    if (string.Compare(dr[col].GetType().FullName, "System.DateTime", true, CultureInfo.InvariantCulture) == 0)
                       ret = dr.SafeGetFormatedDateTime(col);
                    else                                
                       ret = dr[col].ToString();
                    break;
            }

            return ret; 
        }

        private string ReturnCaseSearchSql(CaseSearchFilter f, Setting customerSetting, CustomerUser customerUserSetting, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, GlobalSetting gs, ISearch s)
        {
            StringBuilder sb = new StringBuilder();

            // fields
            sb.Append("select ");
            //vid avslutade ärenden visas bara första 500, TODO fungerar inte i Oracle 
            if (f.CaseProgress == "1")
                sb.Append(" top 500 ");

	        sb.Append("tblCase.Id ");
            //sb.Append(", tblCase.CaseGUID");
            sb.Append(", tblCase.CaseNumber");
            sb.Append(", tblCase.Place");
            sb.Append(", tblCase.Customer_Id as CustomerInternal_Id");
            sb.Append(", tblCustomer.Name as Customer_Id");
            sb.Append(", tblRegion.Region as Region_Id");
            sb.Append(", tblOU.OU as OU_Id");
            sb.Append(", tblCase.UserCode");
            sb.Append(", tblCase.[User_Id]");
            sb.Append(", tblCase.Priority_Id");
            sb.Append(", tblCase.Department_Id");
            sb.Append(", tblCase.Persons_Name");
            sb.Append(", tblCase.Persons_EMail");
            sb.Append(", tblCase.Persons_Phone");
            sb.Append(", tblCase.Persons_CellPhone");
            sb.Append(", tblCase.FinishingDate");
            sb.Append(", tblCase.FinishingDescription");
            sb.Append(", tblCase.Caption");
            sb.Append(", tblCase.[Description] ");
            sb.Append(", tblCase.Miscellaneous");
            sb.Append(", tblCase.[Status] ");
            //sb.Append(", tblCase.StateSecondary_Id");
            sb.Append(", tblCase.ExternalTime");
            sb.Append(", tblCase.RegTime");
            sb.Append(", tblCase.ReportedBy");
            sb.Append(", tblCase.ProductArea_Id");
            sb.Append(", tblCase.InvoiceNumber");
            sb.Append(", tblCustomer.Name");
            sb.Append(", tblDepartment.Department as DepertmentName");
            sb.Append(", tblDepartment.DepartmentId");
            sb.Append(", tblDepartment.SearchKey");
            sb.Append(", tblCase.ReferenceNumber");
            sb.Append(", coalesce(tblUsers.SurName, '') + ' ' + coalesce(tblUsers.FirstName, '') as Performer_User_Id");
            sb.Append(", tblUsers.UserId as UserId");
            sb.Append(", tblStatus.StatusName as Status_Id");
            sb.Append(", tblStatus.Id as Status_Id_Value");
            sb.Append(", tblSupplier.Supplier as Supplier_Id");
            sb.Append(", case when coalesce(tblOrder.Id, 0) = 0 then tblStateSecondary.StateSecondary else cast(tblOrder.Id as varchar(15))  + ' - ' + coalesce(tblStateSecondary.StateSecondary, '') end as StateSecondary_Id ");
            //sb.Append(", tblStateSecondary.StateSecondary");
            sb.Append(", coalesce(tblStateSecondary.IncludeInCaseStatistics, 1) as IncludeInCaseStatistics");
            sb.Append(", tblPriority.[Priority]");
            sb.Append(", tblPriority.PriorityName");
            //sb.Append(", coalesce(tblPriority.SolutionTime, 0) as SolutionTime");
            sb.Append(", tblCase.WatchDate");
            sb.Append(", tblCaseType.RequireApproving");
            sb.Append(", tblCase.ApprovedDate");
            sb.Append(", coalesce(tblOrder.Id, 0) as Order_Id");
            sb.Append(", tblOrderState.OrderState as OrderStatus");
            sb.Append(", coalesce(tblAccount.Id, 0) as Account_Id");
            sb.Append(", tblCase.ContactBeforeAction");
            sb.Append(", tblCase.SMS");
            sb.Append(", tblCase.Available");
            sb.Append(", tblCase.Cost");
            sb.Append(", tblCase.PlanDate");
            sb.Append(", tblCase.ProductAreaSetDate");
            //sb.Append(", tblSettings.LeadtimeFromProductAreaSetDate");
            sb.Append(", coalesce(tblUsers2.Surname, '') + ' ' + coalesce(tblUsers2.Firstname, '') as User_Id");
            sb.Append(", coalesce(tblUsers3.Surname, '') + ' ' + coalesce(tblUsers3.Firstname, '') as CaseResponsibleUser_Id");
            //sb.Append(", tblSettings.DepartmentFormat");
            //sb.Append(", tblSettings.CaseDateFormat");
            sb.Append(", tblWorkingGroup.WorkingGroup as WorkingGroup_Id");
            sb.Append(", tblCase.ChangeTime");
            sb.Append(", tblCaseType.CaseType as CaseType_Id");
            sb.Append(", tblCase.RegistrationSource");
            sb.Append(", tblCase.InventoryNumber");
            sb.Append(", tblCase.InventoryType as ComputerType_Id");
            sb.Append(", tblCase.InventoryLocation");
            sb.Append(", tblCategory.Category as Category_Id");
            sb.Append(", tblCase.Problem_Id");
            sb.Append(", coalesce(tblUsers4.Surname, '') + ' ' + coalesce(tblUsers4.Firstname, '') as ResponsibleUser_Id");
            sb.Append(", tblCase.SolutionRate");
            sb.Append(", tblSystem.SystemName as System_Id");
            sb.Append(", tblUrgency.Urgency as Urgency_Id");
            sb.Append(", tblImpact.Impact as Impact_Id");
            sb.Append(", tblCase.Verified");
            sb.Append(", tblCase.VerifiedDescription");
            sb.Append(", coalesce(tblDepartment.HolidayHeader_Id, 1) as HolidayHeader_Id");
            sb.Append(", '-' as [_temporary_.LeadTime] ");

            // tables
            sb.Append("from tblCase ");
            sb.Append("inner join tblCustomer on tblCase.Customer_Id = tblCustomer.Id "); 
            //sb.Append("inner join tblSettings on tblCase.Customer_Id = tblSettings.Customer_Id ");  
            sb.Append("inner join tblCustomerUser on tblCase.Customer_Id = tblCustomerUser.Customer_Id ");  
            sb.Append("left outer join tblDepartment on tblDepartment.Id = tblCase.Department_Id ");  
            sb.Append("left outer join tblRegion on tblCase.Region_Id = tblRegion.Id ");  
            sb.Append("left outer join tblOU on tblCase.OU_Id=tblOU.Id ");  
            sb.Append("left outer join tblSupplier on tblCase.Supplier_Id=tblSupplier.Id "); 
            sb.Append("left outer join tblSystem on tblCase.System_Id = tblSystem.Id ");  
            sb.Append("left outer join tblUrgency on tblCase.Urgency_Id = tblUrgency.Id ");  
            sb.Append("left outer join tblImpact on tblCase.Impact_Id = tblImpact.Id ");  

            if (customerSetting.CaseWorkingGroupSource == 0)
            {
                sb.Append("left outer join tblUsers ");
                sb.Append("left outer join tblWorkingGroup on tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id on tblCase.Performer_User_Id = tblUsers.Id ");
            }
            else
            {
                sb.Append("left outer join tblUsers on tblCase.Performer_user_Id = tblUsers.Id ");
                sb.Append("left outer join tblWorkingGroup on tblCase.WorkingGroup_Id = tblWorkingGroup.Id ");
            }

            sb.Append("left outer join tblStatus on tblCase.Status_Id = tblStatus.Id ");  
            sb.Append("left outer join tblCategory on tblCase.category_Id = tblCategory.Id ");  
            sb.Append("left outer join tblStateSecondary on tblCase.StateSecondary_Id = tblStateSecondary.Id ");  
            sb.Append("left outer join tblPriority on tblCase.Priority_Id = tblPriority.Id ");  
            sb.Append("inner join tblCaseType on tblCase.CaseType_Id = tblCaseType.Id ");  

            sb.Append("left outer join tblOrder on tblCase.Casenumber = tblOrder.CaseNumber "); 
            sb.Append("    and tblOrder.customer_Id = tblCase.Customer_Id ");  
            sb.Append("    and tblOrder.Deleted = 0 ");  

            // TODO fungerar för oracle?
            sb.Append("    and tblOrder.Id in ");  
            sb.Append("            ( "); 
            sb.Append("            select max(Id) from tblorder where (CaseNumber = tblCase.caseNumber) "); 
            sb.Append("            ) ");  

            sb.Append("left outer join tblAccount on tblCase.Casenumber = tblAccount.CaseNumber and tblAccount.Deleted = 0 ");  
            sb.Append("left outer join tblOrderState on tblOrder.OrderState_Id = tblOrderState.Id ");
            sb.Append("left outer join tblUsers as tblUsers2 on tblCase.[User_Id] = tblUsers2.Id ");
            sb.Append("left outer join tblUsers as tblUsers3 on tblCase.CaseResponsibleUser_Id = tblUsers3.Id  "); 
            sb.Append("left outer join tblProblem on tblCase.Problem_Id = tblProblem.Id ");
            sb.Append("left outer join tblUsers as tblUsers4 on tblProblem.ResponsibleUser_Id = tblUsers4.Id ");

            //where
            sb.Append(ReturnCaseSearchWhere(f, customerSetting, userId, userUserId, showNotAssignedWorkingGroups, userGroupId, restrictedCasePermission, gs));

            // order by
            sb.Append("order by ");
            sb.Append(s.SortBy.Replace("_temporary_.", string.Empty));
            if (!s.Ascending)
                sb.Append(" desc");

            return sb.ToString();
        }

        private string ReturnCaseSearchWhere(CaseSearchFilter f, Setting customerSetting, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, GlobalSetting gs)
        {
            var sb = new StringBuilder();

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + f.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");
            sb.Append(" and (tblCustomerUser.User_Id = " + f.UserId + ")");

            // användaren får bara se avdelningar som den har behörighet till
            sb.Append(" and (tblCase.Department_Id In (select Department_Id from tblDepartmentUser where [User_Id] = " + userId + ")");
            sb.Append(" or not exists (select Department_Id from tblDepartmentUser where ([User_Id] = " + userId + "))");
            sb.Append(") ");

            // finns kryssruta på användaren att den bara får se sina egna ärenden
            if (restrictedCasePermission == 1)
            {
                if (userGroupId == 2)
                    sb.Append(" and (tblCase.Performer_User_Id = " + userId + " or tblcase.CaseResponsibleUser_Id = " + userId + ")");
                else if (userGroupId == 1)
                    sb.Append(" and (lower(tblCase.reportedBy) = lower(" + userUserId + ") or tblcase.UserId = " + userId + ")");
            }

            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case "1":
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case "2":
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case "3":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null)");
                    break;
                case "4":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case "5":
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case "6":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case "7":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case "8":
                    sb.Append(" and (tblCase.FollowUpdate is not null)");
                    break;
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }

            // working group 
            if (!string.IsNullOrWhiteSpace(f.WorkingGroup))
            {
                if (customerSetting.CaseWorkingGroupSource == 0)
                    sb.Append(" and (tblUsers.Default_WorkingGroup_Id in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
                else
                    sb.Append(" and (coalesce(workingGroup_Id, 0) in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
            }

            // performer/utförare
            if (!string.IsNullOrWhiteSpace(f.UserPerformer))
            {
                sb.Append(" and (tblCase.Performer_User_Id in (" + f.UserPerformer.SafeForSqlInject() + ") or tblCase.CaseResponsibleUser_Id in (" + f.UserPerformer.SafeForSqlInject() + ")  or tblProblem.ResponsibleUser_Id IN (" + f.UserPerformer.SafeForSqlInject() + "))");
            }
            // ansvarig
            if (!string.IsNullOrWhiteSpace(f.UserResponsible))
                sb.Append(" and (tblCase.CaseResponsibleUser_Id in (" + f.UserResponsible.SafeForSqlInject() + "))"); 
            // land/country
            if (!string.IsNullOrWhiteSpace(f.Country))
                sb.Append(" and (tblDepartment.Country_Id in (" + f.Country.SafeForSqlInject() + "))"); 
            // case type
            if (f.CaseType != 0)
                sb.Append(" and (tblcase.CaseType_Id = " + f.CaseType + ")");
            // Product area 
            if (!string.IsNullOrWhiteSpace(f.ProductArea))
                if (string.Compare(f.ProductArea, "0", true, CultureInfo.InvariantCulture) != 0)  
                    sb.Append(" and (tblcase.ProductArea_Id in (" + f.ProductArea.SafeForSqlInject() + "))");
            // department / avdelning
            if (!string.IsNullOrWhiteSpace(f.Department))
                sb.Append(" and (tblCase.Department_Id in (" + f.Department.SafeForSqlInject() + "))");
            // användare / user
            if (!string.IsNullOrWhiteSpace(f.User))
                sb.Append(" and (tblCase.User_Id in (" + f.User.SafeForSqlInject() + "))");
            // region
            if (!string.IsNullOrWhiteSpace(f.Region ))
                sb.Append(" and (tblDepartment.Region_Id in (" + f.Region.SafeForSqlInject() + "))");
            // prio
            if (!string.IsNullOrWhiteSpace(f.Priority))
                sb.Append(" and (tblcase.Priority_Id in (" + f.Priority.SafeForSqlInject() + "))");
            // katagori / category
            if (!string.IsNullOrWhiteSpace(f.Category))
                sb.Append(" and (tblcase.Category_Id in (" + f.Category.SafeForSqlInject() + "))");
            // status
            if (!string.IsNullOrWhiteSpace(f.Status ))
                sb.Append(" and (tblcase.Status_Id in (" + f.Status.SafeForSqlInject() + "))");
            // state secondery
            if (!string.IsNullOrWhiteSpace(f.StateSecondary))
                sb.Append(" and (tblcase.StateSecondary_Id in (" + f.StateSecondary.SafeForSqlInject() + "))");
            // free text
            if (!string.IsNullOrWhiteSpace(f.FreeTextSearch))
            {
                string searchFor = f.FreeTextSearch.SafeForSqlInject().ToLower().createDBsearchstring(); 
                sb.Append(" and (");
                sb.Append(" lower(tblCase.CaseNumber) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.ReportedBy) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Persons_Name) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Persons_EMail) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Persons_Phone) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Persons_CellPhone) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Place) like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Caption) like '" + searchFor + "' ");
                sb.Append(" or " + InsensitiveSearch("tblCase.Description") + " like '" + searchFor + "' ");
                sb.Append(" or lower(tblCase.Miscellaneous) like '" + searchFor + "' ");
                sb.Append(" or lower(tblDepartment.Department) like '" + searchFor + "' ");
                sb.Append(" or lower(tblDepartment.DepartmentId) like '" + searchFor + "' ");
                sb.Append(" or tblCase.Id in (select Case_Id from tblLog where " + InsensitiveSearch("tblLog.Text_Internal") + " like '" + searchFor + "' or " + InsensitiveSearch("tblLog.Text_External") + " like '" + searchFor + "')");
                sb.Append(" or tblCase.Id in (select Case_Id from tblFormFieldValue where lower(FormFieldValue) like '" + searchFor + "')");
                sb.Append(")");
            }

            //LockCaseToWorkingGroup
            if (userGroupId < 3 && gs.LockCaseToWorkingGroup == 1)
            {
                sb.Append(" and (tblCase.WorkingGroup_Id in ");
                sb.Append(" (");
                sb.Append("select WorkingGroup_Id from tblUserWorkingGroup where [User_Id] = " + userId + " ");
                sb.Append("and WorkingGroup_Id in (select Id from tblWorkingGroup where Customer_Id = " + f.CustomerId + ") "); 
                sb.Append(" )");

                if (showNotAssignedWorkingGroups == 1)
                    sb.Append(" or tblCase.WorkingGroup_Id is null ");

                sb.Append(" or not exists (select id from tblWorkingGroup where Customer_Id = " + f.CustomerId + ")");
                sb.Append(") ");
            }

            return sb.ToString();
        }

        private string InsensitiveSearch(string fieldName)
        {
            var ret = fieldName;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;

            if (!dsn.Contains("SQLOLEDB")) 
                ret = "lower(" + fieldName + ")";  

            return ret;
        }
        
    }
}