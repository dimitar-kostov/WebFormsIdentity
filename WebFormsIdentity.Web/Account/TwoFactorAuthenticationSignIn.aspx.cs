using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class TwoFactorAuthenticationSignIn : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        public TwoFactorAuthenticationSignIn(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = SignInManager.GetVerifiedUserId();
            if (userId == default)
            {
                Response.Redirect("/Account/Error", true);
            }
            var userFactors = UserManager.GetValidTwoFactorProviders(userId);
            Providers.DataSource = userFactors.Select(x => x).ToList();
            Providers.DataBind();
        }

        protected void CodeSubmit_Click(object sender, EventArgs e)
        {
            bool rememberMe = false;
            bool.TryParse(Request.QueryString["RememberMe"], out rememberMe);

            var result = SignInManager.TwoFactorSignIn(SelectedProvider.Value, Code.Text, isPersistent: rememberMe, rememberBrowser: RememberBrowser.Checked);
            switch (result)
            {
                case SignInStatus.Success:
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    Response.Redirect("/Account/Lockout");
                    break;
                case SignInStatus.Failure:
                default:
                    FailureText.Text = "Invalid code";
                    ErrorMessage.Visible = true;
                    break;
            }
        }

        protected void ProviderSubmit_Click(object sender, EventArgs e)
        {
            if (!SignInManager.SendTwoFactorCode(Providers.SelectedValue))
            {
                Response.Redirect("/Account/Error");
            }

            var user = UserManager.FindById(SignInManager.GetVerifiedUserId());
            if (user != null)
            {
                var code = UserManager.GenerateTwoFactorToken(user.Id, Providers.SelectedValue);
            }

            SelectedProvider.Value = Providers.SelectedValue;
            sendcode.Visible = false;
            verifycode.Visible = true;
        }
    }
}