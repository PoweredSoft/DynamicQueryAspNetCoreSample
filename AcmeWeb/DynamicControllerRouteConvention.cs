using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeWeb
{
    public class DynamicControllerRouteConvention : IControllerModelConvention
    {
        private ServiceProvider serviceProvider;

        public DynamicControllerRouteConvention(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var genericType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = genericType.Name;
            }
        }
    }
}