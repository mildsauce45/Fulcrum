using FluentAssertions;
using Fulcrum.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class HeaderTests
    {
        [Header("Accept", "application/json")]
        [Header("X-Unused", "foobar")]
        public interface IHeaderApi
        {
            [Get("public")]
            Task<Response> Get();

            [Get("public")]
            [Header("X-Endpoint", "helloworld")]
            Task<Response> GetMethodLevelHeaders();

            [Get("public")]
            Task<Response> GetParameterLevelHeader([Header("X-Parameter")] string value);
        }

        private const string TestBaseUrl = "http://localhost/echoservice";

        private static readonly IHeaderApi interfaceUnderTest = Connect.To<IHeaderApi>(TestBaseUrl);

        [TestMethod]
        public async Task Headers_InterfaceLevel_AddedProperly()
        {
            var result = await interfaceUnderTest.Get();

            result.Should().NotBeNull();
            result.Headers.Should().ContainKey("accept");
            result.Headers["accept"].Should().Contain("application/json");

            result.Headers.Should().ContainKey("x-Unused");
            result.Headers["x-Unused"].Should().BeEquivalentTo("foobar");
        }

        [TestMethod]
        public async Task Headers_MethodLevel_AddedProperly()
        {
            var result = await interfaceUnderTest.GetMethodLevelHeaders();

            result.Should().NotBeNull();
            result.Headers.Should().ContainKey("x-Endpoint");
            result.Headers["x-Endpoint"].Should().BeEquivalentTo("helloworld");
        }

        [TestMethod]
        public async Task Headers_ParameterLevel_AddedProperly()
        {
            var result = await interfaceUnderTest.GetParameterLevelHeader("foobar");

            result.Should().NotBeNull();
            result.Headers.Should().ContainKey("x-Parameter");
            result.Headers["x-Parameter"].Should().BeEquivalentTo("foobar");
        }
    }
}
