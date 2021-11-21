using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TodoAPI.Utilities.Helpers;

namespace TodoAPI.Utilities
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string strErrorMsg = string.Empty;

                switch (error)
                {
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        {
                            strErrorMsg = "Some error occurred";
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        }
                }

                logger.LogError("Error Message : " + error.Message +"|| Stact Trace:"+error.StackTrace);
                var result = JsonSerializer.Serialize(new { message = strErrorMsg.Length>1? strErrorMsg: error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
