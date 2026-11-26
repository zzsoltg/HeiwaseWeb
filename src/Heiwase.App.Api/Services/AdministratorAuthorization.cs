using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Heiwase.App.Api.Services;

public static class AdministratorAuthorization
{
    public static async Task<IActionResult> ExecuteAsync(HttpRequest request, Func<Task<IActionResult>> action)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(action);

        var authentication = await request.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
        if (!authentication.Succeeded || authentication.Principal is null)
        {
            return new UnauthorizedResult();
        }

        if (!authentication.Principal.IsInRole("Administrator"))
        {
            return new ForbidResult();
        }

        return await action().ConfigureAwait(false);
    }
}
