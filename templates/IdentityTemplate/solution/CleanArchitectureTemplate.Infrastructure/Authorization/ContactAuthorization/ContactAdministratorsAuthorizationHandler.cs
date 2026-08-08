using CleanArchitectureTemplate.ApplicationCore.Constants;
using CleanArchitectureTemplate.ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CleanArchitectureTemplate.Infrastructure.Authorization.ContactAuthorization;

public class ContactAdministratorsAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
{
    /// <summary>
    /// Handles authorization for administrator users accessing <see cref="Contact"/> resources.
    /// </summary>
    /// <param name="context">
    /// The authorization context containing the current user principal.
    /// </param>
    /// <param name="requirement">
    /// The operation requirement (Create, Read, Update, Delete) being evaluated.
    /// </param>
    /// <param name="resource">
    /// The <see cref="Contact"/> resource being accessed.
    /// </param>
    /// <returns>
    /// - Calls <c>context.Succeed(requirement)</c> if the current user is in the administrator role.
    /// - Returning <see cref="Task.CompletedTask"/> without calling <c>context.Succeed</c>
    ///   or <c>context.Fail</c> makes no explicit success or failure determination,
    ///   allowing other handlers to participate in the decision.
    /// - To explicitly deny access, call <c>context.Fail()</c>.
    /// </returns>
    /// <remarks>
    /// This handler grants unconditional access to users in the administrator role,
    /// effectively bypassing ownership or other role-based checks.
    /// It is intended to run alongside other handlers such as owner-based or manager-based handlers.
    /// Because this handler does not call <c>context.Fail</c> when the user is not an administrator,
    /// it preserves the ability of other handlers to authorize the request based on additional logic
    /// such as ownership or managerial permissions.
    /// </remarks>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Contact resource
    )
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }

        // Administrators can do anything.
        if (context.User.IsInRole(RoleConstants.AdministratorsRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
