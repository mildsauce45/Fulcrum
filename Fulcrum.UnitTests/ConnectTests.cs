using FluentAssertions;
using Fulcrum.UnitTests.Apis;
using Fulcrum.UnitTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class ConnectTests
    {
        public interface ISyncApi
        {
            [Get("/sets/{setCode}")]
            Set GetSet(string setCode);
        }

        public interface IPseudoSyncApi
        {
            Set UpdateSet(Set set);

            [Get("/sets/{setCode}")]
            Task<Set> GetSet(string setCode);
        }

        [TestMethod]
        public void Connect_To_ReturnsMockedInterface()
        {
            IEchoApi liveApi = null;

            Action action = () => liveApi = Connect.To<IEchoApi>("https://doesntmatter.io");

            action.Should().NotThrow("because this is a valid interface");

            liveApi.Should().NotBeNull();
        }

        [TestMethod]
        public void Connect_To_ThrowsForNonInterface()
        {
            Action action = () => Connect.To<string>("https://a.com");

            action.Should().Throw<ArgumentException>("because System.String is not an interface");
        }

        [TestMethod]
        public void Connect_To_ThrowsForUndecoratedInterface()
        {
            Action action = () => Connect.To<IUndecoratedApi>("foo");

            action.Should().Throw<MissingHttpMethodException>("because no method is marked with a subclass of MethodAttribute");
        }

        [TestMethod]
        public void Connect_To_ThrowsForNonPublicInterface()
        {
            Action action = () => Connect.To<INonPublicInterface>("bar");

            action.Should().Throw<NonPublicInterfaceException>("because we cannot use emit a non public interface");
        }

        [TestMethod]
        public void Connect_To_ThrowsForNonAsyncMethod()
        {
            Action action = () => Connect.To<ISyncApi>("https://doesntmatter.io");

            action.Should().Throw<SynchronousEndpointException>("because the interface contains a synchronous method.");
        }

        [TestMethod]
        public void Connect_To_IgnoresUndecoratedSyncMethod()
        {
            IPseudoSyncApi api = null;

            Action action = () => api = Connect.To<IPseudoSyncApi>("https://doesntmatter.org");

            action.Should().NotThrow("because the only decorated method is async");

            var s = api.UpdateSet(new Set());

            s.Should().BeNull("because by default the emitter library returns the default for the type if no implmentation is provided");
        }
    }
}
