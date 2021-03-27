using Microsoft.AspNet.Identity;
using System;
using System.Web.UI;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class ForgotPassword : Page
    {
        private ApplicationUserManager UserManager { get; set; }

        public ForgotPassword(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user's email address
                IdentityUser user = UserManager.FindByName(Email.Text);
                if (user == null || !UserManager.IsEmailConfirmed(user.Id))
                {
                    FailureText.Text = "The user either does not exist or is not confirmed.";
                    ErrorMessage.Visible = true;
                    return;
                }
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send email with the code and the redirect to reset password page
                //string code = UserManager.GeneratePasswordResetToken(user.Id);
                //string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                //manager.SendEmail(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>.");
                loginForm.Visible = false;
                DisplayEmail.Visible = true;
            }
        }
    }
}