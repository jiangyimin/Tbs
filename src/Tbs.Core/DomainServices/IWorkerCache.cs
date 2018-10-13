using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IWorkerCache
    {
        List<Worker> GetList(int depotId);

        Worker Get(string workerCn);

        Worker GetOrNull(string workerCn);
    }
}