using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IDepotSigninCache
    {
        List<DepotSignin> GetList(int depotId);
    }
}