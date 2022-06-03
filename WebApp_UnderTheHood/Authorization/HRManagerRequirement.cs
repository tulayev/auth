using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRManagerRequirement : IAuthorizationRequirement
    {
        public int ProbationPeriod { get; }
        public HRManagerRequirement(int probationPeriod)
        {
            ProbationPeriod = probationPeriod;
        }
    }

    public class HRManagerRequirementHandler : AuthorizationHandler<HRManagerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
            {
                return Task.CompletedTask;
            }

            var employeeHiredDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
            var period = DateTime.Now - employeeHiredDate;
            if (period.Days > 30 * requirement.ProbationPeriod)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
