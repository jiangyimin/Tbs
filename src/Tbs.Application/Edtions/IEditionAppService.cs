using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Tbs.Dto;
using Tbs.Editions.Dto;

namespace Tbs.Editions
{
    public interface IEditionAppService : IApplicationService
    {
        ListResultDto<EditionDto> GetEditions();

        Task CreateEdition(EditionDto input);

        Task RemoveEdition(string name);

        List<PropertyDto> GetFeatures(int id);

        Task SaveFeatureChanges(int editionId, List<PropertyDto> features);
    }
}
