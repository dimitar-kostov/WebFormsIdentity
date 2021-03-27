using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        private IAuthenticationManager AuthenticationManager { get; set; }

        public RegisterExternalLogin(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AuthenticationManager = authenticationManager;
        }

        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderAccountKey
        {
            get { return (string)ViewState["ProviderAccountKey"] ?? String.Empty; }
            private set { ViewState["ProviderAccountKey"] = value; }
        }

        private void RedirectOnFail()
        {
            Response.Redirect((User.Identity.IsAuthenticated) ? "~/Account/Manage" : "~/Account/Login");
        }

        protected void Page_Load()
        {
            // Process the result from an auth provider in the request
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName))
            {
                RedirectOnFail();
                return;
            }
            if (!IsPostBack)
            {
                var loginInfo = AuthenticationManager.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    RedirectOnFail();
                    return;
                }
                var user = UserManager.Find(loginInfo.Login);
                if (user != null)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else if (User.Identity.IsAuthenticated)
                {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = AuthenticationManager.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null)
                    {
                        RedirectOnFail();
                        return;
                    }

                    var result = UserManager.AddLogin(User.Identity.GetUserId().ToGuid(), verifiedloginInfo.Login);
                    if (result.Succeeded)
                    {
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    }
                    else
                    {
                        AddErrors(result);
                        return;
                    }
                }
                else
                {
                    email.Text = loginInfo.Email;
                }
            }
        }

        protected void LogIn_Click(object sender, EventArgs e)
        {
            CreateAndLoginUser();
        }

        private void CreateAndLoginUser()
        {
            if (!IsValid)
            {
                return;
            }
            var user = new IdentityUser() { UserName = email.Text, Email = email.Text };
            IdentityResult result = UserManager.Create(user);
            if (result.Succeeded)
            {
                var loginInfo = AuthenticationManager.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    RedirectOnFail();
                    return;
                }
                result = UserManager.AddLogin(user.Id, loginInfo.Login);
                if (result.Succeeded)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // var code = manager.GenerateEmailConfirmationToken(user.Id);
                    // Send this link via email: IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id)

                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    return;
                }
            }
            AddErrors(result);
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