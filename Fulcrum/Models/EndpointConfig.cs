using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Fulcrum.Models
{
    internal class EndpointConfig
    {
        private Type[] _paramterTypes;
        private MethodInfo _endpointMethod;
        private ParameterConfig[] _parameterConfigs;

        internal string Route { get; private set; }
        internal HeaderCollection EndpointHeaders { get; }
        internal HttpRequestMethod Method { get; private set; }

        internal string Name => _endpointMethod.Name;
        internal Type ReturnType => _endpointMethod.ReturnType;
        internal Type[] ParamterTypes => _paramterTypes ?? (_paramterTypes = _endpointMethod.GetParameters().Select(pi => pi.ParameterType).Concat(new[] { ReturnType }).ToArray());

        internal EndpointConfig(MethodInfo endpointMethod, IEnumerable<Attribute> attributes)
        {
            _endpointMethod = endpointMethod;

            ExtractRoute(attributes);

            EndpointHeaders = new HeaderCollection();

            InspectEndpoint(attributes);
            InspectParameters();
        }

        private void ExtractRoute(IEnumerable<Attribute> attributes)
        {
            var methodAttribute = attributes.OfType<MethodAttribute>().FirstOrDefault();
            if (methodAttribute == null)
                throw new MissingHttpMethodException();

            Route = methodAttribute.Route;
            Method = methodAttribute.Method;
        }

        private void InspectEndpoint(IEnumerable<Attribute> attributes)
        {
            var headerAttributes = attributes.OfType<HeaderAttribute>();
            if (headerAttributes.Any())
            {
                foreach (var ha in headerAttributes)
                {
                    var staticHeader = ha.GetHeader(null);
                    EndpointHeaders.Add(staticHeader.Item1, staticHeader.Item2);
                }
            }
        }

        private void InspectParameters()
        {
            var parms = _endpointMethod.GetParameters();
            _parameterConfigs = new ParameterConfig[parms.Length];

            for (int i = 0; i < parms.Length; i++)
                _parameterConfigs[i] = new ParameterConfig(parms[i]);
        }

        /// <summary>
        ///  TODO: This probably does not belong here
        /// </summary>
        internal string GetUrl(string originalUrl, IEnumerable<object> liveParameterMap)
        {
            var url = originalUrl;

            for (int i = 0; i < liveParameterMap.Count(); i++)
            {
                var paramConfig = _parameterConfigs[i];
                if (!paramConfig.IsRouteReplacement && !paramConfig.IsQueryParams)
                    continue;

                var parm = liveParameterMap.ElementAt(i);

                if (paramConfig.IsRouteReplacement)
                    url = paramConfig.ReplaceRoute(url, parm);

                if (paramConfig.IsQueryParams && parm != null)
                {
                    var map = parm as IDictionary<string, string>;
                    var queryString = ("?" + string.Join("&", map.Select(kvp => $"{kvp.Key}={kvp.Value.ToString()}")));
                    url += queryString;
                }
            }

            return url;
        }

        /// <summary>
        /// TODO: This probably does not belong here either
        /// </summary>
        internal HeaderCollection GetRequestHeaders(IEnumerable<object> liveParameterMap)
        {
            var hc = new HeaderCollection();

            for (int i = 0; i < liveParameterMap.Count(); i++)
            {
                var paramConfig = _parameterConfigs[i];
                if (!paramConfig.IsRequestHeader)
                    continue;

                var parm = liveParameterMap.ElementAt(i);

                var headers = paramConfig.GetHeaders(parm);
                if (headers.Count == 0)
                    continue;

                hc.Add(headers);
            }

            if (EndpointHeaders.Count > 0)
                hc.Add(EndpointHeaders);

            return hc;
        }

        /// <summary>
        /// TODO: This probably does not belong here either
        /// </summary>
        internal HttpContent GetRequestBody(IEnumerable<object> liveParameterMap)
        {
            for (int i = 0; i < liveParameterMap.Count(); i++)
            {
                var paramConfig = _parameterConfigs[i];
                if (!paramConfig.IsRequestBody)
                    continue;

                /// TODO: I know the API technically allows for other types of content, but for now I'm sticking with JSON
                return new StringContent(JsonConvert.SerializeObject(liveParameterMap.ElementAt(i)), Encoding.UTF8, "application/json");
            }

            return null;
        }
    }
}
