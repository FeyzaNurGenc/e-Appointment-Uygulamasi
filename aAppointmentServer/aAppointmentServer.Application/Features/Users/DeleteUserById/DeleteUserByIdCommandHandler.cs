using aAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Users.DeleteUserById
{
    internal sealed class DeleteUserByIdCommandHandler(
        UserManager<AppUser> userManager
        ) : IRequestHandler<DeleteUserByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());
            if (appUser is null)
            {
                return (HttpStatusCode.NotFound, "User not found");
            }
            IdentityResult result = await userManager.DeleteAsync(appUser);

            if (!result.Succeeded)
            {
                return (HttpStatusCode.BadRequest, result.Errors.Select(s => s.Description).ToList());

            }
            return "User delete is successful";
        }
    }
}
