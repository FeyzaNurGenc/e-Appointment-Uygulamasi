using aAppointmentServer.Domain.Entities;

namespace aAppointmentServer.Application
{
    public static class Constants
    {
        public static List<AppRole> GetRoles(){
            List<string> roles = new()
            {
                "Admin",
                "Doctor",
                "Personel",
                "Patient"
            };
            return roles.Select(s => new AppRole() { Name = s }).ToList();
        }

        //YUKARIDA LİSTE YAPTIĞIMIZ İÇİN BUNA GEREK KALMADI
        //public static List<AppRole> Roles = new()
        //{
        //    new()
        //    {
               
        //        Name= "Admin"
        //    },
        //    new()
        //    {
               
        //        Name="Doctor"
        //    },
        //    new()
        //    {
              
        //        Name="Personel"
        //    }
        //};
    }
}

