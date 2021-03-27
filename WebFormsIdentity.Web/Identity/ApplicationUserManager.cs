using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;

namespace WebFormsIdentity.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<IdentityUser, Guid>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, Guid> store, IDataProtectionProvider dataProtectionProvider)
            : base(store)
        {
            Configure(dataProtectionProvider);
        }

        private void Configure(IDataProtectionProvider dataProtectionProvider)
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<IdentityUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            EmailService = new EmailService();
            SmsService = new SmsService();
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<IdentityUser, Guid>(
                    dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}