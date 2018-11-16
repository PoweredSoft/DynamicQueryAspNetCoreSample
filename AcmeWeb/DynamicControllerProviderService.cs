using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb
{
    public interface IDynamicControllerResourceProvider
    {
        List<Type> GetResources();
    }

    public class AcmeResourceProvider : IDynamicControllerResourceProvider
    {
        public List<Type> GetResources()
        {
            return new List<Type> {
                typeof(Dal.Ticket),
                typeof(Dal.Customer),
                typeof(Dal.Item),
                typeof(Dal.Order),
                typeof(Dal.Task),
                typeof(Dal.OrderItem)
            };
        }
    }
}
