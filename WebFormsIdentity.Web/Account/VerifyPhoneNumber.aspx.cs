using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class VerifyPhoneNumber : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        private ApplicationSignInManager SignInManager { get; set; }

        public VerifyPhoneNumber(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var phonenumber = Request.QueryString["PhoneNumber"];
            var code = UserManager.GenerateChangePhoneNumberToken(User.Identity.GetUserId().ToGuid(), phonenumber);
            PhoneNumber.Value = phonenumber;
        }

        protected void Code_Click(object sender, EventArgs e)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid code");
                return;
            }

            var result = UserManager.ChangePhoneNumber(User.Identity.GetUserId().ToGuid(), PhoneNumber.Value, Code.Text);

            if (result.Succeeded)
            {
                var user = UserManager.FindById(User.Identity.GetUserId().ToGuid());

                if (user != null)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    Response.Redirect("/Account/Manage?m=AddPhoneNumberSuccess");
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
        }
    }
}