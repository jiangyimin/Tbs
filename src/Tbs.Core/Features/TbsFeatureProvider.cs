using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Tbs.Features
{
    public class TbsFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(FeatureNames.MaxDepots, "5", 
                displayName: new FixedLocalizableString("最大中心数"), inputType: new SingleLineStringInputType());
            
            context.Create(FeatureNames.MaxVehicles, "50", 
                displayName: new FixedLocalizableString("最大车辆数"), inputType: new SingleLineStringInputType());

            context.Create(FeatureNames.RouteTasksEnabled, defaultValue: "true", 
                displayName: new FixedLocalizableString("线路功能开关"), scope: FeatureScopes.Tenant, inputType: new CheckboxInputType());
        }
    }
}
