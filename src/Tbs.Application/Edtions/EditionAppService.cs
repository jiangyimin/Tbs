using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Security;
using Tbs.Authorization;
using Tbs.Authorization.Roles;
using Tbs.Authorization.Users;
using Tbs.Editions;
using Tbs.Editions.Dto;
using Tbs.Features;
using Tbs.Dto;

namespace Tbs.Editions
{
    [AbpAuthorize(PermissionNames.HostPages)]
    public class EditionAppService : TbsAppServiceBase, IEditionAppService
    {
        private readonly EditionManager _editionManager;
        private readonly FeatureValueStore _featureValueStore;
        public EditionAppService(EditionManager editionManager, FeatureValueStore FeatureValueStore)
        {
            _editionManager = editionManager;
            _featureValueStore = FeatureValueStore;
        }

        public ListResultDto<EditionDto> GetEditions()
        {
            return new ListResultDto<EditionDto>( 
                ObjectMapper.Map<List<EditionDto>>(
                    _editionManager.Editions.ToList()
                )
            );
        }

        public async Task CreateEdition(EditionDto input)
        {
            //Create edition
            var edition = ObjectMapper.Map<Edition>(input);
            await _editionManager.CreateAsync(edition);
        }

        public async Task RemoveEdition(string name)
        {
            var edition = await _editionManager.FindByNameAsync(name);
            await _editionManager.DeleteAsync(edition);
        }

        public List<PropertyDto> GetFeatures(int id)
        {
            var edition = _editionManager.FindByIdAsync(id).Result;

            List<PropertyDto> lst = new List<PropertyDto>();
            foreach (Feature f in FeatureManager.GetAll())
            {
                string v = _featureValueStore.GetEditionValueOrNullAsync(edition.Id, f.Name).Result;
                lst.Add(new PropertyDto(f, v));              
            }
            return lst;
        }

        public async Task SaveFeatureChanges(int editionId, List<PropertyDto> features) 
        {
            foreach(PropertyDto f in features)
            {
                string name = f.Name.Split(' ')[0];
                await _featureValueStore.SetEditionFeatureValueAsync(editionId, name, f.Value);
            }
        }
    }
}