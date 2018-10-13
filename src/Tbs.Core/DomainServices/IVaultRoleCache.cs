using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IVaultRoleCache
    {
        VaultRole Get(int id);

        VaultRole GetOrNull(int id);

        List<VaultRole> GetVaultRoles(int vaultTypeId);
    }
}