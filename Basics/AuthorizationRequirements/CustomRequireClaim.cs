using Microsoft.AspNetCore.Authorization;

namespace Basics.AuthorizationRequirements
{
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; set; }

        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType; 
        }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            bool hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            
            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
