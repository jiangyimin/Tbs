using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IWarehouseCache
    {
        List<Warehouse> GetList(int depotId);

        Warehouse Get(int id);
        Warehouse GetOrNull(int id);
    }
}