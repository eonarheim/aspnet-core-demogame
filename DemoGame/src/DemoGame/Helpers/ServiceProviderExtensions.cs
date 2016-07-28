using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoGame.Helpers
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Get strongly-typed service extension method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
