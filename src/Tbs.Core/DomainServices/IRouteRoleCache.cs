using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IRouteRoleCache
    {
        RouteRole Get(int id);

        RouteRole GetOrNull(int id);

        List<RouteRole> GetRouteRoles(int routeTypeId);
    }
}