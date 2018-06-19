using FluentAssertions;
using Fulcrum.UnitTests.Auth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests
{
    [TestClass]
    public class ScalarTests
    {
        public interface IScalarApi
        {
            [Get("/scalar/bool/{val}")]
            Task<bool> GetBool(bool val);

            [Get("/scalar/int/{val}")]
            Task<int> GetInt(int val);

            [Get("/scalar/long/{val}")]
            Task<long> GetLong(long val);

            [Get("/scalar/short/{val}")]
            Task<short> GetShort(short val);
        }

        private static readonly IScalarApi api = Connect.To<IScalarApi>(TestConstants.TestBaseUrl, new SimpleAuthProvider());

        [TestMethod]
        public async Task Get_Scalar_Handles_Bool()
        {
            var res = await api.GetBool(true);

            res.Should().BeTrue();

            res = await api.GetBool(false);

            res.Should().BeFalse();
        }

        [TestMethod]
        public async Task Get_Scalar_Handles_Int()
        {
            var res = await api.GetInt(1);

            res.Should().Be(1);
        }

        [TestMethod]
        public async Task Get_Scalar_Handles_Long()
        {
            var res = await api.GetLong(42L);

            res.Should().Be(42L);
        }        
    }
}
