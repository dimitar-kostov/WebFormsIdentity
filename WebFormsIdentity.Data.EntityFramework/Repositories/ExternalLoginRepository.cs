﻿using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFormsIdentity.Domain.Entities;
using WebFormsIdentity.Domain.Repositories;

namespace WebFormsIdentity.Data.EntityFramework.Repositories
{
    internal class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
    {
        internal ExternalLoginRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken);
        }
    }
}
