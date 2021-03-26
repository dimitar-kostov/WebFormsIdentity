using Microsoft.AspNet.Identity;
using Microsoft.AspNet.WebFormsDependencyInjection.Unity;
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

            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(), new InjectionConstructor("DefaultConnection"));
            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new TransientLifetimeManager());
            container.RegisterType<RoleStore>(new TransientLifetimeManager());
        }
    }
}