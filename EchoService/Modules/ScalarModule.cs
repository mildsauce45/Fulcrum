using EchoService.Auth;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Extensions;
using Nancy.Responses;

namespace EchoService.Modules
{
    public class ScalarModule : NancyModule
    {
        public ScalarModule()
            : base("scalar")
        {
            StatelessAuthentication.Enable(this, SimpleOAuthConfiguration.GetOAuthConfiguration());

            this.AddBeforeHookOrExecute(ctx => ctx.CurrentUser == null ? new HtmlResponse(HttpStatusCode.Unauthorized) : null);

            Get["/bool/{val:bool}"] = _ =>
            {
                return Response.AsJson((bool)_.val);
            };

            Get["/int/{val:int}"] = _ =>
            {
                return Response.AsJson((int)_.val);
            };

            Get["/long/{val:long}"] = _ =>
            {
                return Response.AsJson((long)_.val);
            };
        }
    }
}