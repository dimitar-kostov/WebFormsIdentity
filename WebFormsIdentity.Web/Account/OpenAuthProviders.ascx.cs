using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity.Account
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        private IAuthenticationManager AuthenticationManager { get; set; }

        public OpenAuthProviders(IAuthenticationManager authenticationManager)
        {
            AuthenticationManager = authenticationManager;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var provider = Request.Form["provider"];
                if (provider == null)
                {
                    return;
                }
                // Request a redirect to the external login provider
                string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/Account/RegisterExternalLogin?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
                var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
                // Add xsrf verification when linking accounts
                if (Context.User.Identity.IsAuthenticated)
                {
                    properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
                }
                AuthenticationManager.Challenge(properties, provider);
                Response.StatusCode = 401;
                Response.End();
            }
        }

        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return AuthenticationManager.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}