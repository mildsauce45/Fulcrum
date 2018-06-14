using EchoService.Auth;
using EchoService.Extensions;
using EchoService.Models;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Extensions;
using Nancy.Responses;

namespace EchoService.Modules
{
    public class PrivateModule : NancyModule
    {
        public PrivateModule()
            : base("private")
        {
            StatelessAuthentication.Enable(this, SimpleOAuthConfiguration.GetOAuthConfiguration());
            this.AddBeforeHookOrExecute(ctx => ctx.CurrentUser == null ? new HtmlResponse(HttpStatusCode.Unauthorized) : null);

            Get["/resource/{key}"] = _ =>
            {
                var resp = new GetRequest { Route = Request.Path, User = Context.CurrentUser.UserName };

                this.AddQueryParams(resp);

                return Response.AsJson(resp);
            };

            Post["/"] = _ =>
            {
                var resp = ContentRequest.Post(Request.Path, Context.CurrentUser.UserName, Request.Body.AsString());

                return Response.AsJson(resp);
            };

            Put["/"] = _ =>
            {
                var resp = ContentRequest.Put(Request.Path, Context.CurrentUser.UserName, Request.Body.AsString());

                return Response.AsJson(resp);
            };

            Delete["/resource/{key}"] = _ =>
            {
                var resp = new DeleteRequest { Route = Request.Path, User = Context.CurrentUser.UserName };

                this.AddQueryParams(resp);

                return Response.AsJson(resp);
            };
        }
    }
}