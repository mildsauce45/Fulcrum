using Fulcrum.UnitTests.Models;
using System.Threading.Tasks;

namespace Fulcrum.UnitTests.Apis
{
    internal interface INonPublicInterface
    {
        [Get("/sets/{setname}")]
        Task<Set> GetUser(string setname);
    }
}
