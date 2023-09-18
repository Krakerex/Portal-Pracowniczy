using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace krzysztofb.Authorization
{
    public class OwnershipRequirementHandler : AuthorizationHandler<OwnershipRequirement>
    {
        int id { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipRequirement requirement)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return Task.CompletedTask;
            }
            int userID = Int32.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userID == requirement.Id)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}