using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IVaultCache
    {
        List<Vault> GetList(int depotId);

    }
}