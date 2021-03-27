using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.WebFormsDependencyInjection.Unity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Web;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using WebFormsIdentity.Data.EntityFramework;
using WebFormsIdentity.Domain;
using WebFormsIdentity.Identity;

namespace WebFormsIdentity
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpApplication application)
        {
            var container = application.AddUnity();

            container.RegisterType<IUnitOfWork, UnitOfWork>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor("DefaultConnection"));

            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new TransientLifetimeManager());
            container.RegisterType<RoleStore>(new TransientLifetimeManager());

            // Identity
            //container.RegisterType<ApplicationUserManager>(new TransientLifetimeManager());
            //container.RegisterType<ApplicationSignInManager>(new TransientLifetimeManager());

            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new TransientLifetimeManager());

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(x =>
                    HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IDataProtectionProvider>(
                new InjectionFactory(x =>
                    HttpContext.Current
                               .GetOwinContext()
                               .Get<DataProtectionProviderFactory>()
                               .DataProtectionProvider));
        }
    }
}