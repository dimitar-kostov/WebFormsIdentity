using Microsoft.Owin.Security.DataProtection;
using System;

namespace WebFormsIdentity.Identity
{
    public class DataProtectionProviderFactory : IDisposable
    {
        public IDataProtectionProvider DataProtectionProvider { get; private set; }

        public DataProtectionProviderFactory(IDataProtectionProvider dataProtectionProvider)
        {
            DataProtectionProvider = dataProtectionProvider;
        }
        public void Dispose()
        {
        }
    }
}