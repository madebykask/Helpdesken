using System.Linq;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;

namespace DH.Helpdesk.WebApi.Logic
{
    public class CaseEditModeCalcStrategy
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;

        #region ctor()

        public CaseEditModeCalcStrategy(IGlobalSettingService globalSettingService, 
                                        IUserService userService, 
                                        IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            _userService = userService;
            _globalSettingService = globalSettingService;
        }

        #endregion

        public AccessMode CalcEditMode(int customerId, int userId, Case @case, bool temporaryHasAccessToWG = false)
        {
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            var accessToWorkinggroups = _userService.GetWorkinggroupsForUserAndCustomer(userId, customerId);
            var departmensForUser = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);

            var currentUser = _userService.GetUserOverview(userId);
            if (departmensForUser != null)
            {
                var accessToDepartments = departmensForUser.Select(d => d.Id).ToList();

                if (currentUser.UserGroupId < (int)BusinessData.Enums.Admin.Users.UserGroup.CustomerAdministrator)
                {
                    if (accessToDepartments.Count > 0 && @case.Department_Id.HasValue)
                    {
                        if (!accessToDepartments.Contains(@case.Department_Id.Value))
                        {
                            return AccessMode.NoAccess;
                        }
                    }
                }
            }

            // In new case shouldn't check
            /*Updated in this way:*/
            /*If user does not have access to WG, if last action was "Save", user can see the Case in readonly mode 
             * there is no ticket. (Per knows more info)
             */
            if (accessToWorkinggroups != null && @case.Id != 0)
            {
                if (currentUser.UserGroupId < (int)BusinessData.Enums.Admin.Users.UserGroup.CustomerAdministrator)
                {
                    if (accessToWorkinggroups.Any() && @case.WorkingGroup_Id.HasValue)
                    {
                        var wg = accessToWorkinggroups.FirstOrDefault(w => w.WorkingGroup_Id == @case.WorkingGroup_Id.Value);
                        if (wg == null && (gs != null && gs.LockCaseToWorkingGroup == 1))
                        {
                            return temporaryHasAccessToWG ? AccessMode.ReadOnly : AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {
                            return AccessMode.ReadOnly;
                        }
                    }
                }
            }

            if (@case.FinishingDate.HasValue)
            {
                return AccessMode.ReadOnly;
            }

            //case lock condition will be checked on client separately
            //if (lockModel != null && lockModel.IsLocked) return AccessMode.ReadOnly;

            return AccessMode.FullAccess;
        }
    }
}