using Fulcrum.UnitTests.Apis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentAssertions;
using Fulcrum.UnitTests.Auth;
using Fulcrum.UnitTests.Models;
using Newtonsoft.Json;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class PostTests
    {
        private const string TestBaseUrl = "http://localhost/echoservice";

        private static readonly IEchoApi interfaceUnderTest = Connect.To<IEchoApi>(TestBaseUrl);

        public interface IPostApi
        {
            [Post("private")]
            Task<Response> Post([Body]Set set, [Authorization] string bearerToken);
        }

        [TestMethod]
        public async Task Post_UnauthorizedRequest_ReturnsDefault()
        {
            var set = new Set();

            var res = await interfaceUnderTest.Post(set);

            res.Should().BeNull();
        }

        [TestMethod]
        public async Task Post_AuthorizedRequest_ReturnsResult()
        {
            var liveApi = Connect.To<IEchoApi>(TestBaseUrl, new SimpleAuthProvider());

            var set = new Set { Code = "aer", Name = "Aether Revolt" };

            var res = await liveApi.Post(set);

            res.Should().NotBeNull();
            res.User.Should().NotBeNull();
            res.Body.Should().NotBeNull();
            res.Route.Should().BeEquivalentTo("/private");

            var setFromServer = JsonConvert.DeserializeObject<Set>(res.Body);

            set.Code.Should().BeEquivalentTo(setFromServer.Code);
            set.Name.Should().BeEquivalentTo(setFromServer.Name);
        }

        [TestMethod]
        public async Task Post_AuthorizationAttribute_ReturnsResult()
        {
            var api = Connect.To<IPostApi>(TestBaseUrl);

            var set = new Set { Code = "aer", Name = "Aether Revolt" };

            var res = await api.Post(set, new SimpleAuthProvider().BearerToken);

            res.Should().NotBeNull();
            res.User.Should().NotBeNull();
            res.Body.Should().NotBeNull();
            res.Route.Should().BeEquivalentTo("/private");

            var setFromServer = JsonConvert.DeserializeObject<Set>(res.Body);

            set.Code.Should().BeEquivalentTo(setFromServer.Code);
            set.Name.Should().BeEquivalentTo(setFromServer.Name);
        }

        [TestMethod]
        public async Task Post_Handles_UntypedTask()
        {
            var liveApi = Connect.To<IEchoApi>(TestBaseUrl, new SimpleAuthProvider());

            var task = liveApi.PostUntyped(new Set { Code = "aer", Name = "Aether Revolt" });

            task.Should().NotBeNull();

            await task;

            task.IsCompleted.Should().BeTrue();
        }
    }
}
