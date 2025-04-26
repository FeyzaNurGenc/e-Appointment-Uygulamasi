using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Users.UpdateUser
{
    internal sealed class UpdateUserCommandHandler(
        UserManager<AppUser> userManager,
        IUserRoleRepository userRoleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
            {
                return (HttpStatusCode.NotFound, "User not found");
            }
            if(user.Email != request.Email)
            {
                  if (await userManager.Users.AnyAsync(p => p.Email == request.Email))
                            {
                                return (HttpStatusCode.NotFound, "Email already exists");
                            }
            }
          
            if(user.UserName != request.UserName)
            {
                    if (await userManager.Users.AnyAsync(p => p.UserName == request.UserName))
                            {
                                return (HttpStatusCode.NotFound, "User name already exists");
                            }

            }
           
            mapper.Map(request,user);

            IdentityResult result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return (HttpStatusCode.BadRequest, result.Errors.Select(s => s.Description).ToList());

            }
            if (request.RoleIds.Any())
            { //Neden List<> kullanıyoruz? --- Burada userRoleRepository üzerinden yapılan sorgu, kullanıcıya ait tüm roller döndüren bir filtreleme işlemidir. Bu sorgu, kullanıcıya ait birden fazla rol olabileceği için dönen sonuçlar bir koleksiyon (örneğin bir liste) olmalıdır.
                //userRoleRepository kullanılarak, kullanıcıya ait mevcut roller veritabanından çekiliyor.
                //Bu sorgu, user.Id ile eşleşen tüm AppUserRole öğelerini alır.
                List<AppUserRole> userRoles = await userRoleRepository.Where(p=> p.UserId == user.Id).ToListAsync();
                userRoleRepository.DeleteRange(userRoles);
                await unitOfWork.SaveChangesAsync(cancellationToken);//DeleteRange işlemi, veritabanında değişiklik yapar, ancak bu değişikliklerin veritabanına kaydedilmesi gerekir. unitOfWork.SaveChangesAsync çağrısı, silme işlemini veritabanına kaydeder.

                //Bu liste, kullanıcının tüm rollerini toplamak için kullanılır. Yani, her bir rolün AppUserRole nesnesi burada saklanacak.
                //userRoles: Kullanıcının tüm rollerini tutan bir liste.
                userRoles = new();

                //request.RoleIds, kullanıcının yeni rollerinin id'lerini içeriyor. Yani, kullanıcının atanacak olan yeni rollerini belirtir.
                foreach (var roleId in request.RoleIds)
                {
                    //Her bir roleId için, kullanıcı-rol ilişkisinin bir kaydı olan AppUserRole nesnesi oluşturuluyor.
                    //userRole: Her bir rol için geçici olarak oluşturulan tek bir kullanıcı-rol ilişki nesnesi.
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
            return "User update is successful";
        }
    }
    }

