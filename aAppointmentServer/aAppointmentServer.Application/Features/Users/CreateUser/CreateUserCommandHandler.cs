using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Users.CreateUser
{
    internal sealed class CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        IUserRoleRepository userRoleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<CreateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await userManager.Users.AnyAsync(p => p.Email == request.Email))
            {
                return (HttpStatusCode.NotFound, "Email already exists");
            }

            if (await userManager.Users.AnyAsync(p => p.UserName == request.UserName))
            {
                return (HttpStatusCode.NotFound, "User name already exists");
            }

            AppUser user = mapper.Map<AppUser>(request);
            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return (HttpStatusCode.BadRequest, result.Errors.Select(s => s.Description).ToList());

            }
            if (request.RoleIds.Any())
            {
                List<AppUserRole> userRoles = new();
                foreach (var roleId in request.RoleIds)
                {
                    AppUserRole userRole = new()
                    {
                        RoleId = roleId,
                        UserId = user.Id
                    };
                    userRoles.Add(userRole);
                }
                await userRoleRepository.AddRangeAsync(userRoles, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return "User create is successful";
        }
    }
}
