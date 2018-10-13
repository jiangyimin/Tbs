using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface ITaskTypeCache
    {
        TaskType Get(int id);

        TaskType GetOrNull(int id);

    }
}