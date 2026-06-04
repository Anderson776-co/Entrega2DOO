using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class CustomUnauthorizedHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomUnauthorizedHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.NoResult());

        // 401 - No autenticado / token inválido
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                message = "No autenticado. Token inválido o expirado.",
                statusCode = 401
            });
            return Response.WriteAsync(result);
        }

        // 403 - Autenticado pero sin el rol necesario
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                message = "No tienes permisos para acceder a este recurso.",
                statusCode = 403
            });
            return Response.WriteAsync(result);
        }
    }
}
