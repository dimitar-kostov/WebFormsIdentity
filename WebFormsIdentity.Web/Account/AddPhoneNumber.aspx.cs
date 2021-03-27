using Microsoft.AspNet.Identity;
using System;
using System.Web;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class AddPhoneNumber : System.Web.UI.Page
    {
        private ApplicationUserManager UserManager { get; set; }

        public AddPhoneNumber(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected void PhoneNumber_Click(object sender, EventArgs e)
        {
            var code = UserManager.GenerateChangePhoneNumberToken(User.Identity.GetUserId().ToGuid(), PhoneNumber.Text);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = PhoneNumber.Text,
                    Body = "Your security code is " + code
                };

                UserManager.SmsService.Send(message);
            }

            Response.Redirect("/Account/VerifyPhoneNumber?PhoneNumber=" + HttpUtility.UrlEncode(PhoneNumber.Text));
        }
    }
}