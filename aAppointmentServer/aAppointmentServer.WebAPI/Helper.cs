using aAppointmentServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace aAppointmentServer.WebAPI
{
    public static class Helper
    {
        public static async Task CreateUserAsync(WebApplication app)
        {
            //hiç kullanıcı olmazsa bunu getirecek
            using (var scoped = app.Services.CreateScope())
            {
                var UserManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (!UserManager.Users.Any())
                {
                    await UserManager.CreateAsync(new()
                    {
                        FirstName = "Feyza",
                        LastName = "Genc",
                        Email = "fyz@gmail.com",
                        UserName = "feyza",
                    }, "1");
                }
            }
        }
    }
}
