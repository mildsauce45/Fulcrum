using Fulcrum.Emitter;
using Fulcrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                case 6:
                    return Tuple.Create(nameof(HttpCallProxy.FiveParamCall), typeof(Func<,,,,,>));
                case 7:
                    return Tuple.Create(nameof(HttpCallProxy.SixParamCall), typeof(Func<,,,,,,>));
                case 8:
                    return Tuple.Create(nameof(HttpCallProxy.SevenParamCall), typeof(Func<,,,,,,,>));
                default:
                    throw new NotImplementedException("Up to 7 IN parameters will be supported");
            }
        }
    }
}
