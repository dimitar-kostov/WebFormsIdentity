using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        public ManagePassword(
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
            return UserManager.HasPassword(User.Identity.GetUserId().ToGuid());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword())
                {
                    changePasswordHolder.Visible = true;
                }
                else
                {
                    setPassword.Visible = true;
                    changePasswordHolder.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                IdentityResult result = UserManager.ChangePassword(User.Identity.GetUserId().ToGuid(), CurrentPassword.Text, NewPassword.Text);
                if (result.Succeeded)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId().ToGuid());
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                IdentityResult result = UserManager.AddPassword(User.Identity.GetUserId().ToGuid(), password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    AddErrors(result);
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
    }
}