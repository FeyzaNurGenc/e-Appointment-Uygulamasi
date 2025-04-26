using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace aAppointmentServer.Application.Features.Users.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler(
        UserManager<AppUser> userManager,
        IUserRoleRepository userRoleRepository,
        RoleManager<AppRole> roleManager
     ) : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersQueryResponse>>>
    {
        public async Task<Result<List<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
           List<AppUser> users = await userManager.Users.OrderBy(p=> p.FirstName).ToListAsync(cancellationToken);
           List<GetAllUsersQueryResponse> response= 
                users.Select(s=> new GetAllUsersQueryResponse()
                {
                    Id = s.Id,
                    FirstName= s.FirstName,
                    LastName= s.LastName,
                    FullName= s.FullName,
                    UserName= s.UserName,
                    Email = s.Email,
                    RoleNames = new List<string?>(),  // Başlangıçta boş bir liste atayın
                    RoleIds = new List<Guid>()        // Başlangıçta boş bir liste atayın

                }).ToList();

             foreach (var item in response)
            {
                List<AppUserRole> userRoles = await userRoleRepository.Where(p=>p.UserId==item.Id).ToListAsync(cancellationToken);

                foreach(var userRole in userRoles)
                {
                    List<AppRole> roles = await roleManager.Roles.Where(p => p.Id == userRole.RoleId).ToListAsync(cancellationToken);

                    // List<string?> stringRoles = roles.Select(s=> s.Name).ToList();



                    // Rol isimlerini ve Id'lerini al
                    List<Guid> stringRoles = roles.Select(s => s.Id).ToList();
                    List<string?> stringRoleNames = roles.Select(s => s.Name).ToList();

                    // Kullanıcıya rol ekleme
                    item.RoleIds.AddRange(stringRoles);        // RoleIds listesini güncelle
                    item.RoleNames.AddRange(stringRoleNames);  // RoleNames listesini güncelle

                    //List<Guid> stringRoles = roles.Select(s => s.Id).ToList();
                    //List<string?> stringRoleNames = roles.Select(s => s.Name).ToList();

                    //item.RoleNames = stringRoleNames;
                    //item.RoleIds = stringRoles;


                }
            }
            return response;
        }
    }

}
