using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb
{
    public interface IDynamicControllerResourceProvider
    {
        List<(Type modelType, Type keyType)> GetResources();
    }

    public class AcmeResourceProvider : IDynamicControllerResourceProvider
    {
        public List<(Type modelType, Type keyType)> GetResources()
        {
            return new List<(Type modelType, Type keyType)> {
                (typeof(Dal.Ticket), typeof(long)),
                (typeof(Dal.Customer), typeof(long)),
                (typeof(Dal.Item), typeof(long)),
                (typeof(Dal.Order), typeof(long)),
                (typeof(Dal.Task), typeof(long)),
                (typeof(Dal.OrderItem), typeof(long)),
            };
        }
    }
}
