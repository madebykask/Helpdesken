using System;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IUserService _userService;

        #region ctor()

        public PasswordController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckPasswordUnique(int userId, string pwd)
        {
            var user = _userService.GetUser(userId);
            return Json(new { isUnique = !user.Password.Equals(pwd, StringComparison.OrdinalIgnoreCase) });
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ChangePassword(int id, string newPassword, string confirmPassword)
        {
            if (!string.IsNullOrEmpty(newPassword) && newPassword == confirmPassword)
            {
                _userService.SavePassword(id, newPassword);
                return Json(new { isSuccess = true });
            }

            return Json(new { isSuccess = false, Error = "Invalid password" });
        }
    }
}