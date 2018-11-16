using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeWeb
{
    public class DynamicControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private ServiceProvider serviceProvider;

        public DynamicControllerFeatureProvider(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var resourceProviders = this.serviceProvider.GetServices<IDynamicControllerResourceProvider>();
            foreach (var rp in resourceProviders)
            {
                var dynamicController = typeof(DynamicController<>);
                rp.GetResources().ForEach(resourceType =>
                {
                    var genericController = dynamicController.MakeGenericType(resourceType);
                    var typeInfo = genericController.GetTypeInfo();
                    feature.Controllers.Add(typeInfo);
                });
            }
        }
    }
}