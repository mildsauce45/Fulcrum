using FluentAssertions;
using Fulcrum.UnitTests.Apis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class GetTests
    {
        private const string TestBaseUrl = "http://localhost/echoservice";

        private static readonly IEchoApi interfaceUnderTest = Connect.To<IEchoApi>(TestBaseUrl);

        [TestMethod]
        public async Task Get_Calls_Route()
        {
            var task = interfaceUnderTest.GetEcho(null);

            task.Should().NotBeNull();

            await task;

            task.Result.Should().NotBeNull();
            task.Result.Route.Should().BeEquivalentTo("/public");
            task.Result.QueryParams.Should().BeNull();
        }

        [TestMethod]
        public async Task Get_Accepts_OneParam()
        {
            var task = interfaceUnderTest.GetEchoRouteReplace("aer", null);

            task.Should().NotBeNull();

            await task;

            task.Result.Should().NotBeNull();
            task.Result.Route.Should().BeEquivalentTo("/public/aer");
            task.Result.QueryParams.Should().BeNull();
        }

        [TestMethod]
        public async Task Get_Replaces_MultipleRouteSlugs()
        {
            var res = await interfaceUnderTest.GetReplaceMultipleRoutes("hello", "worlds");

            res.Should().NotBeNull();
            res.Route.Should().BeEquivalentTo("/public/hello/multiple/worlds");
        }

        [TestMethod]
        public async Task Get_Adds_QueryParams()
        {
            var queryParams = new Dictionary<string, string>
            {
                { "pretty", true.ToString() },
                { "set_type", "core" }
            };

            var request = await interfaceUnderTest.GetEcho(queryParams);

            request.Should().NotBeNull();
            request.QueryParams.Should().NotBeNull();
            request.QueryParams.Should().BeEquivalentTo(queryParams);
        }
    }
}
