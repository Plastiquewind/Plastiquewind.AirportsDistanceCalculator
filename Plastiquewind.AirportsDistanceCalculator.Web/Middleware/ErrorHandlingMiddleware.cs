using System;
using System.Net;
using System.Threading.Tasks;

using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Plastiquewind.AirportsDistanceCalculator.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is FlurlHttpException httpEx)
            {
                var result = JsonConvert.SerializeObject(new { error = httpEx.Call.Exception.Message });

                context.Response.StatusCode = (int)httpEx.Call.Response.StatusCode;
                return context.Response.WriteAsync(result);
            }
            else
            {
                var result = JsonConvert.SerializeObject(new { error = ex.Message });

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(result);
            }
        }
    }
}
