using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IOutletCache
    {
        List<Outlet> GetList(int depotId);
    }
}