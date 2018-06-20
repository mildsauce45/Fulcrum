using Fulcrum.Emitter;
using Fulcrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fulcrum
{
    public static class Connect
    {
        private static readonly Type UntypedTaskType = typeof(Task);

        private static IDictionary<Type, IEnumerable<EndpointConfig>> _validApiInterfaces =
            new Dictionary<Type, IEnumerable<EndpointConfig>>();

        private static IDictionary<Type, HeaderCollection> _apiGlobalHeaders =
            new Dictionary<Type, HeaderCollection>();

        public static T To<T>(string baseUrl, IAuthenticationProvider authProvider = null) where T : class
        {
            var apiType = typeof(T);

            PrimeAndValidateType(apiType);

            var context = TypeCreationContext.ImplementInterface<T>();

            var settings = new RequestSettings(baseUrl, authProvider, _apiGlobalHeaders[apiType]);

            RequestConfigurationManager.ConfigureInterface(context, settings, _validApiInterfaces[apiType]);

            return context.CreateInstance() as T;
        }

        private static void PrimeAndValidateType(Type apiType)
        {
            if (!apiType.IsInterface)
                throw new ArgumentException($"{apiType} is not an interface.");

            if (!apiType.IsPublic && !apiType.IsNestedPublic)
                throw new NonPublicInterfaceException();

            if (!_validApiInterfaces.ContainsKey(apiType))
                PrimeApiDictionary(apiType);

            if (!_validApiInterfaces.ContainsKey(apiType))
                throw new MissingHttpMethodException();

            if (!_apiGlobalHeaders.ContainsKey(apiType))
                PrimeApiHeaderDictionary(apiType);
        }

        private static void PrimeApiDictionary(Type apiType)
        {
            var methods = apiType.GetMethods();
            if (!methods.Any())
                return;

            var decoratedMethods = new List<EndpointConfig>();
            foreach (var m in methods)
            {
                var customAttributes = m.GetCustomAttributes();
                if (customAttributes.OfType<MethodAttribute>().Any())
                {
                    if (UntypedTaskType.IsAssignableFrom(m.ReturnType)) // Catches both Task and Task<T> because Task<T> inherits from Task
                        decoratedMethods.Add(new EndpointConfig(m, customAttributes.OfType<Attribute>()));
                    else
                        throw new SynchronousEndpointException(m.Name);
                }
            }

            if (decoratedMethods.Count > 0)
                _validApiInterfaces.Add(apiType, decoratedMethods);
        }

        private static void PrimeApiHeaderDictionary(Type apiType)
        {
            var interfaceAttributes = apiType.GetCustomAttributes<Attribute>();

            var globalHeaders = new HeaderCollection(interfaceAttributes.OfType<HeaderAttribute>().Select(ha => ha.GetHeader(null)));

            _apiGlobalHeaders.Add(apiType, globalHeaders);
        }
    }
}
