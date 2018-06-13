using System.Threading.Tasks;

namespace Fulcrum.UnitTests.Apis
{
    public interface IUndecoratedApi
    {
        Task<string> GetAuthToken(string encodedUnPw);
    }
}
