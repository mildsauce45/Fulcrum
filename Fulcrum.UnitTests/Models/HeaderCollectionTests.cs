using FluentAssertions;
using Fulcrum.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Fulcrum.UnitTests.Models
{
    [TestClass]
    public class HeaderCollectionTests
    {
        [TestMethod]
        public void HeaderCollection_Supports_MultipleHeaders()
        {
            var hc = new HeaderCollection();

            hc.Add("Header1", "foo");
            hc.Add("Header2", "bar");

            var headers = hc.GetHeaders();

            headers.Should().HaveCount(2);
        }

        [TestMethod]
        public void HeaderCollection_Combines_DuplicateHeaders()
        {
            var hc = new HeaderCollection();

            hc.Add("TestHeader", "foo");
            hc.Add("TestHeader", "bar");

            var headers = hc.GetHeaders();

            headers.Should().HaveCount(1);
            headers.First().Item2.Should().BeEquivalentTo("foo;bar");
        }

        [TestMethod]
        public void HeaderCollection_HeaderKey_IsCaseInsensitive()
        {
            var hc = new HeaderCollection();

            hc.Add("Testheader", "foo");
            hc.Add("testheader", "bar");
            hc.Add("tEsThEaDeR", "snafu");

            var headers = hc.GetHeaders();

            headers.Should().HaveCount(1);
            headers.First().Item2.Should().BeEquivalentTo("foo;bar;snafu");
        }

        [TestMethod]
        public void HeaderCollection_Add_AnotherCollection()
        {
            var hc1 = new HeaderCollection();
            hc1.Add("TestHeader1", "foo");
            hc1.Add("TestHeader2", "bar");

            var hc2 = new HeaderCollection();
            hc2.Add("TestHeader2", "snafu");
            hc2.Add("TestHeader3", "helloworld");

            hc1.Add(hc2);

            var headers = hc1.GetHeaders();

            headers.Should().HaveCount(3);
            headers.First(h => h.Item1 == "TestHeader2").Item2.Should().BeEquivalentTo("bar;snafu");
        }
    }
}
