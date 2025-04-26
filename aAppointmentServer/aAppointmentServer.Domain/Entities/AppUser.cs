using Microsoft.AspNetCore.Identity;

namespace aAppointmentServer.Domain.Entities
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty; //Hata almamak için boş değer ataması yapıldı
        public string LastName { get; set; } = string.Empty;
        public string FullName => string.Join(" ", FirstName, LastName);   //AppUserdaki FullName çağrıldıktan sonra birleştirilmiş şekilde FullName ve LastName gelecek.

    }
}
