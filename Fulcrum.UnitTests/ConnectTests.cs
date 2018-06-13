using FluentAssertions;
using Fulcrum.UnitTests.Apis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class ConnectTests
    {
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
    }
}
