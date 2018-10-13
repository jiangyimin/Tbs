using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IDepotCache
    {
        Depot Get(int depotId);

        Depot Get(string depotCn);

        Depot GetOrNull(string depotCn);

        Depot GetOrNull(int depotId);

        List<Depot> GetList();
    }
}