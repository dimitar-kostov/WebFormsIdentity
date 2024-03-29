﻿using System.Threading;
using System.Threading.Tasks;
using WebFormsIdentity.Domain.Entities;

namespace WebFormsIdentity.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}
