using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IRouteTypeCache
    {
        RouteType GetFirst();
        
        RouteType Get(int id);

        RouteType GetOrNull(int id);

    }
}