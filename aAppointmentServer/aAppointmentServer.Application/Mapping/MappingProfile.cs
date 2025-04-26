using aAppointmentServer.Application.Features.Doctors.CreateDoctor;
using aAppointmentServer.Application.Features.Doctors.UpdateDoctor;
using aAppointmentServer.Application.Features.Patients.CreatePatient;
using aAppointmentServer.Application.Features.Patients.UpdatePatient;
using aAppointmentServer.Application.Features.Users.CreateUser;
using aAppointmentServer.Application.Features.Users.UpdateUser;
using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Enums;
using AutoMapper;

namespace aAppointmentServer.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile() {

            //Aşağıdaki kodun amacı CreateDoctorCommand nesnesindeki DepartmentValue değerini, Doctor nesnesindeki Department özelliğine dönüştürmek.
            //Tek tek açıklayalım
            //CreateMap<CreateDoctorCommand, Doctor>() : CreateDoctorCommand sınıfındaki verileri Doctor nesnesine dönüştüren bir eşleme (mapping) oluşturur.
            //.ForMember(member => member.Department, options => { ... }) : 
            // Doctor nesnesindeki Department özelliği için özel bir eşleme(mapping) tanımlar.
            CreateMap<CreateDoctorCommand, Doctor>().ForMember(member => member.Department, options =>
            {
                options.MapFrom(map => DepartmentEnum.FromValue(map.DepartmentValue));
                //CreateDoctorCommand nesnesindeki DepartmentValue adlı değeri alır.
                //Bunu DepartmentEnum içindeki FromValue metodunu kullanarak bir DepartmentEnum türüne çevirir`.
                //Son olarak, elde edilen sonucu Doctor nesnesinin Department özelliğine atar.
            });

            CreateMap<UpdateDoctorCommand, Doctor>().ForMember(member => member.Department, options =>
            {
                options.MapFrom(map => DepartmentEnum.FromValue(map.DepartmentValue));
            });

            CreateMap<CreatePatientCommand, Patient>();
            CreateMap<UpdatePatientCommand, Patient>();

            CreateMap<CreateUserCommand, AppUser>();
            CreateMap<UpdateUserCommand, AppUser>();
        }
    }
}
