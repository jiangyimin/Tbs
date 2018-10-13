using Tbs.DomainModels;

namespace Tbs.DomainServices
{
    public interface IArticleTypeCache
    {
        ArticleType Get(int id);

        ArticleType GetOrNull(int id);

    }
}