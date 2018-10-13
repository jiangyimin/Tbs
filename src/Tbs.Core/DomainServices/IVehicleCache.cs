using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IVehicleCache
    {
        List<Vehicle> GetList(int depotId);
    }
}