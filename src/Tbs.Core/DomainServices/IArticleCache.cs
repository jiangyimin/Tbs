using System.Collections.Generic;
using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IArticleCache
    {
        List<Article> GetList(int depotId);
    }
}