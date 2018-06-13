using FluentAssertions;
using Fulcrum.UnitTests.Auth;
using Fulcrum.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class AliasTests
    {
        private const string TestBaseUrl = "http://localhost/echoservice";

        public interface IAliasApi
        {
            [Get("public/{foo}")]
            Task<Response> Get([Alias("foo")] string route);

            [Post("{bar}")]
            Task<Response> Post([Alias("bar")] string route);

            [Put("{snafu}")]
            Task<Response> Put([Alias("snafu")] string route);

            [Delete("private/resource/{magoo}")]
            Task<Response> Delete([Alias("magoo")] string route, [QueryParams] IDictionary<string, string> queryMap);
        }

        [TestMethod]
        public async Task AliasAttribute_Get_DoesReplacement()
        {
            var api = Connect.To<IAliasApi>(TestBaseUrl);

            var res = await api.Get("helloworld");

            res.Should().NotBeNull();
            res.Method.Should().BeEquivalentTo("GET");
            res.Route.Should().BeEquivalentTo("/public/helloworld");
        }

        [TestMethod]
        public async Task AliasAttribute_Post_DoesReplacement()
        {
            var api = Connect.To<IAliasApi>(TestBaseUrl, new SimpleAuthProvider());

            var res = await api.Post("private");

            res.Should().NotBeNull();
            res.Method.Should().BeEquivalentTo("POST");
            res.Route.Should().BeEquivalentTo("/private");
        }

        [TestMethod]
        public async Task AliasAttribute_Put_DoesReplacement()
        {
            var api = Connect.To<IAliasApi>(TestBaseUrl, new SimpleAuthProvider());

            var res = await api.Put("private");

            res.Should().NotBeNull();
            res.Method.Should().BeEquivalentTo("PUT");
            res.Route.Should().BeEquivalentTo("/private");
        }

        [TestMethod]
        public async Task AliasAttribute_Delete_DoesReplacement()
        {
            var api = Connect.To<IAliasApi>(TestBaseUrl, new SimpleAuthProvider());

            var queryParams = new Dictionary<string, string>
            {
                { "soft_delete", true.ToString() }                
            };

            var res = await api.Delete("1", queryParams);

            res.Should().NotBeNull();
            res.Method.Should().BeEquivalentTo("DELETE");
            res.Route.Should().BeEquivalentTo("/private/resource/1");

            res.QueryParams.Should().NotBeNull();
            res.QueryParams.Should().BeEquivalentTo(queryParams);
        }
    }
}
