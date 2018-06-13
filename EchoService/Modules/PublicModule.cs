using EchoService.Extensions;
using EchoService.Models;
using Nancy;

namespace EchoService.Modules
{
    public class PublicModule : NancyModule
    {
        public PublicModule()
            : base("public")
        {
            Get["/"] = _ =>
            {
                var res = new GetRequest();
                res.Route = Request.Path;

                this.AddQueryParams(res);
                this.AddHeaders(res);

                return Response.AsJson(res);
            };

            Get["/{routeReplacement}"] = _ =>
            {
                var res = new GetRequest();
                res.Route = Request.Path;

                this.AddQueryParams(res);
                this.AddHeaders(res);

                return Response.AsJson(res);
            };

            Get["/{replace}/multiple/{routes}"] = _ =>
            {
                var res = new GetRequest();
                res.Route = Request.Path;

                this.AddQueryParams(res);
                this.AddHeaders(res);

                return Response.AsJson(res);
            };
        }       
    }
}