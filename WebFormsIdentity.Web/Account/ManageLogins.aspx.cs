using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class ManageLogins : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        public ManageLogins(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        protected string SuccessMessage
        {
            get;
            private set;
        }
        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        private bool HasPassword()
        {
            return UserManager.HasPassword(
                User.Identity.GetUserId().ToGuid());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CanRemoveExternalLogins = UserManager.GetLogins(
                User.Identity.GetUserId().ToGuid()).Count() > 1;

            SuccessMessage = String.Empty;
            successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
        }

        public IEnumerable<UserLoginInfo> GetLogins()
        {
            var accounts = UserManager.GetLogins(
                User.Identity.GetUserId().ToGuid());

            CanRemoveExternalLogins = accounts.Count() > 1 || HasPassword();
            return accounts;
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            var result = UserManager.RemoveLogin(User.Identity.GetUserId().ToGuid(), new UserLoginInfo(loginProvider, providerKey));
            string msg = String.Empty;
            if (result.Succeeded)
            {
                var user = UserManager.FindById(User.Identity.GetUserId().ToGuid());
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                msg = "?m=RemoveLoginSuccess";
            }
            Response.Redirect("~/Account/ManageLogins" + msg);
        }
    }
}