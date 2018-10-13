using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IVaultTypeCache
    {
        VaultType GetFirst();
        VaultType Get(int id);

        VaultType GetOrNull(int id);
    }
}