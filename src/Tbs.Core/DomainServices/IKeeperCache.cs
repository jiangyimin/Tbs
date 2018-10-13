using System;
using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IKeeperCache
    {
        string Get(int depotId, DateTime carryoutDate, string whName);
    }
}