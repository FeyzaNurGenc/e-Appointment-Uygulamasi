using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Application;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using aAppointmentServer.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace aAppointmentServer.Infrastructure.Services
{
    internal sealed class JwtProvider(
        IConfiguration configuration,
        IUserRoleRepository userRoleRepository,
        RoleManager<AppRole> roleManager) : IJwtProvider
    {
        public async Task<string> CreateTokenAsync(AppUser user)
        {
            List<AppUserRole> appUserRoles = await userRoleRepository.Where(p => p.UserId == user.Id).ToListAsync();

            List<AppRole> roles = new();

            foreach (var userRole in appUserRoles)
            {
               AppRole? role = await roleManager.Roles.Where(p=> p.Id == userRole.RoleId).FirstOrDefaultAsync();
                if(role is not null)
                {
                    roles.Add(role);
                }
            }

            List<string?> stringRoles= roles.Select(s => s.Name).ToList();

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email?? string.Empty),
                new Claim("UserName", user.Email?? string.Empty),
                new Claim(ClaimTypes.Role, JsonSerializer.Serialize(stringRoles))
            };
            DateTime expres = DateTime.Now.AddDays(1);
            string? secretKey = configuration["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is missing or empty in appsettings.json.");
            }

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));
            //configuration.GetSection("SecretKey").Value ?? "")
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: configuration.GetSection("Jwt:Issuer").Value,
                audience: configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(jwtSecurityToken);
            return token;
        }
    }

 
}
