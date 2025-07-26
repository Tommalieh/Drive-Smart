using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Drivia.Controllers;

public abstract class AppControllerBase : ControllerBase
{
    protected Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            throw new UnauthorizedAccessException("User ID not found in token.");
        return Guid.Parse(claim.Value);
    }
}