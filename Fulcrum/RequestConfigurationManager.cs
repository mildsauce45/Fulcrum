using Fulcrum.Emitter;
using Fulcrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fulcrum
{
    internal class RequestConfigurationManager
    {
        private static readonly Type CreationContextType = typeof(TypeCreationContext);
        private static readonly Type ProxyType = typeof(HttpCallProxy);

        internal static void ConfigureInterface(TypeCreationContext context, RequestSettings settings, IEnumerable<EndpointConfig> endpointMethods)
        {
            foreach (var em in endpointMethods)
                ConfigureEndpoint(context, settings, em);
        }

        private static void ConfigureEndpoint(TypeCreationContext context, RequestSettings settings, EndpointConfig endpoint)
        {
            var methodTypes = endpoint.ParamterTypes;

            var addImplMethod = CreationContextType
                .GetMethods()
                .First(m => m.Name == nameof(TypeCreationContext.AddImplementation) && m.GetGenericArguments().Length == methodTypes.Length)
                .MakeGenericMethod(methodTypes);

            var proxy = new HttpCallProxy(settings, endpoint);
            var proxyCallInfo = GetProxyCallInfo(methodTypes.Length);

            var proxiedCall = ProxyType
                .GetMethod(proxyCallInfo.Item1)
                .MakeGenericMethod(methodTypes)
                .CreateDelegate(proxyCallInfo.Item2.MakeGenericType(methodTypes), proxy);

            addImplMethod.Invoke(context, new object[] { endpoint.Name, proxiedCall });
        }

        private static object CreateClientCall(RequestSettings settings, EndpointConfig endpoint)
        {
            switch (endpoint.Method)
            {
                case HttpRequestMethod.GET:
                    return CreateGetCall(settings, endpoint);
                default:
                    throw new InvalidOperationException("I only support GET right now");
            }
        }

        private static object CreateGetCall(RequestSettings settings, EndpointConfig endpoint)
        {
            //using (var client = new HttpClient())
            //{
            //    var baseUrl = $"{settings.BaseUrl}{endpoint.Route}";
            //}
            return null;
        }

        private static Tuple<string, Type> GetProxyCallInfo(int numParams)
        {
            switch (numParams)
            {
                case 1:
                    return Tuple.Create(nameof(HttpCallProxy.NoParamCall), typeof(Func<>));
                case 2:
                    return Tuple.Create(nameof(HttpCallProxy.SingleParamCall), typeof(Func<,>));
                case 3:
                    return Tuple.Create(nameof(HttpCallProxy.TwoParamCall), typeof(Func<,,>));
                case 4:
                    return Tuple.Create(nameof(HttpCallProxy.ThreeParamCall), typeof(Func<,,,>));
                case 5:
                    return Tuple.Create(nameof(HttpCallProxy.FourParamCall), typeof(Func<,,,,>));
                default:
                    throw new NotImplementedException("Up to 7 parameters will be supported");
            }
        }
    }
}
