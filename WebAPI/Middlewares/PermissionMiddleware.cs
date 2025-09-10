using Application.Logic.UserService;
using GenericRepo_Dapper.Attributes;

namespace GenericRepo_Dapper.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var endpoint = context.GetEndpoint();
            var requiredPermission = endpoint?.Metadata.GetMetadata<PermissionAttribute>()?.Code;

            if (string.IsNullOrEmpty(requiredPermission))
            {
                await _next(context);
                return;
            }

            var username = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var user = await userService.GetUserWithPermissionsAsync(username);
            var permissions = user.UserGroups
                                .SelectMany(ug => ug.Group.GroupPermissions)
                                .Select(gp => gp.Permission.PermissionName)
                                .Distinct();

            if (user == null || !permissions.Contains(requiredPermission))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }
    }

}
