using Microsoft.AspNet.Identity;
using System;
using System.Web.UI;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class Confirm : Page
    {
        private ApplicationUserManager UserManager { get; set; }

        public Confirm(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);
            if (code != null && userId != null)
            {
                var result = UserManager.ConfirmEmail(userId.ToGuid(), code);
                if (result.Succeeded)
                {
                    successPanel.Visible = true;
                    return;
                }
            }
            successPanel.Visible = false;
            errorPanel.Visible = true;
        }
    }
}