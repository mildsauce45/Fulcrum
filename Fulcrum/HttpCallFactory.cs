using Fulcrum.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Fulcrum
{
    internal static class HttpCallFactory
    {
        private static readonly MethodInfo GenericFromResult =
            typeof(Task).GetMethods().First(m => m.Name == nameof(Task.FromResult) && m.IsGenericMethod);

        internal static TRet MakeCall<TRet>(EndpointConfig config, RequestSettings settings, params object[] parms)
        {
            using (var client = new HttpClient())
            {
                var url = GetUrl(config, settings, parms);
                AddRequestHeaders(config, settings, client, parms);
                var content = GetHttpContent(config, parms);

                var queryTask = GetHttpMethodCall(config.Method, client, url, content);

                Task.WaitAll(queryTask);

                var responseString = ParseHttpResponse(queryTask);

                var returnObject = ConvertToResponseType<TRet>(responseString);

                return (TRet)returnObject;
            }
        }

        #region Request Builders

        private static string GetUrl(EndpointConfig config, RequestSettings settings, params object[] obj) => config.GetUrl(GetBaseUrl(config, settings), obj);

        private static string GetBaseUrl(EndpointConfig config, RequestSettings settings) => $"{settings.BaseUrl}{(config.Route.StartsWith("/") ? config.Route : "/" + config.Route)}";

        private static void AddRequestHeaders(EndpointConfig config, RequestSettings settings, HttpClient client, params object[] obj)
        {
            var requestHeaders = new HeaderCollection();

            if (settings.GlobalHeaders != null && settings.GlobalHeaders.Count > 1)
                requestHeaders.Add(settings.GlobalHeaders);

            if (settings.AuthenticationProvider != null)
            {
                var authHeaderTask = settings.AuthenticationProvider.GetAuthorizationHeader();
                Task.WhenAll(authHeaderTask);

                if (authHeaderTask.Result != null)
                    requestHeaders.Add(authHeaderTask.Result.Item1, authHeaderTask.Result.Item2);
            }

            requestHeaders.Add(config.GetRequestHeaders(obj));

            foreach (var h in requestHeaders.GetHeaders())
                client.DefaultRequestHeaders.Add(h.Item1, h.Item2);
        }

        private static HttpContent GetHttpContent(EndpointConfig config, params object[] obj) => config.GetRequestBody(obj);

        private static Task<HttpResponseMessage> GetHttpMethodCall(HttpRequestMethod method, HttpClient client, string url, HttpContent content = null)
        {
            switch (method)
            {
                case HttpRequestMethod.GET:
                    return client.GetAsync(url);
                case HttpRequestMethod.POST:
                    return client.PostAsync(url, content);
                case HttpRequestMethod.PUT:
                    return client.PutAsync(url, content);
                case HttpRequestMethod.DELETE:
                    return client.DeleteAsync(url);
                default:
                    throw new NotImplementedException("Given HTTP method is not supported right now");
            }
        }

        #endregion

        #region Response Handlers

        private static string ParseHttpResponse(Task<HttpResponseMessage> response)
        {
            if (response.Result.StatusCode != HttpStatusCode.OK)
                return null;

            var readTask = response.Result.Content.ReadAsStringAsync();

            Task.WhenAll(readTask);

            return readTask.Result;
        }

        private static object ConvertToResponseType<TRet>(string json)
        {
            var returnType = typeof(TRet);
            if (returnType.GenericTypeArguments.Length == 0)
                // I'm going to limit this to Task and Task<T> so we should just be able to return a completed task here
                throw new NotImplementedException("Untyped tasks are not implemented yet");

            returnType = returnType.GetGenericArguments()[0];

            object obj = null;
            if (!string.IsNullOrWhiteSpace(json))
                obj = JsonConvert.DeserializeObject(json, returnType);

            return GenericFromResult.MakeGenericMethod(returnType).Invoke(null, new object[] { obj });
        }

        #endregion
    }
}
