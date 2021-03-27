using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        public Manage(
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

        private bool HasPassword()
        {
            return UserManager.HasPassword(
                User.Identity.GetUserId().ToGuid());
        }

        private int GetLoginsCount()
        {
            return UserManager.GetLogins(
                User.Identity.GetUserId().ToGuid()).Count;
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            //HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId().ToGuid()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            //TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId().ToGuid());

            LoginsCount = GetLoginsCount();

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword())
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    CreatePassword.Visible = true;
                    ChangePassword.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : message == "AddPhoneNumberSuccess" ? "Phone number has been added"
                        : message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var result = UserManager.SetPhoneNumber(User.Identity.GetUserId().ToGuid(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = UserManager.FindById(User.Identity.GetUserId().ToGuid());
            if (user != null)
            {
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            UserManager.SetTwoFactorEnabled(User.Identity.GetUserId().ToGuid(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            UserManager.SetTwoFactorEnabled(User.Identity.GetUserId().ToGuid(), true);

            Response.Redirect("/Account/Manage");
        }
    }
}