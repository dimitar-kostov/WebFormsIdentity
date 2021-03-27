using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class ResetPassword : Page
    {
        private ApplicationUserManager UserManager { get; set; }

        public ResetPassword(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            if (code != null)
            {
                var user = UserManager.FindByName(Email.Text);
                if (user == null)
                {
                    ErrorMessage.Text = "No user found";
                    return;
                }
                var result = UserManager.ResetPassword(user.Id, code, Password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/ResetPasswordConfirmation");
                    return;
                }
                ErrorMessage.Text = result.Errors.FirstOrDefault();
                return;
            }

            ErrorMessage.Text = "An error has occurred";
        }
    }
}