using aAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TS.Result;


namespace aAppointmentServer.Application.Features.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider; // ✅ JWT Provider ekledik

        // Dependency Injection (Bağımlılıkları enjekte etme)
        public LoginCommandHandler(UserManager<AppUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı adı veya e-posta ile kullanıcıyı bul
            AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(p =>
                p.UserName == request.UserNameOrEmail || p.Email == request.UserNameOrEmail,
                cancellationToken);

            if (appUser is null)
            {
                return (HttpStatusCode.NotFound, "User not found");
            }

            // Şifre kontrolü
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(appUser, request.Password);
            if (!isPasswordCorrect)
            {
                return (HttpStatusCode.BadRequest, "Password is wrong");
            }

            // ✅ Kullanıcı ve şifre doğrulandı, JWT Token oluştur
            string token = await _jwtProvider.CreateTokenAsync(appUser);

            // ✅ Token'i döndür
            return new Result<LoginCommandResponse>(new LoginCommandResponse(token));
        }
    }
}
