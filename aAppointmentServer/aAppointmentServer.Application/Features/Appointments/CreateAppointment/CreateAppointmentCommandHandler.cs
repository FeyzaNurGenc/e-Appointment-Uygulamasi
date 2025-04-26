using aAppointmentServer.Application.Features.Appointments.CreateAppointment;
using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

internal sealed class CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork,//Bütün işlemleri tek bir transaction içinde kaydetmeyi sağlar.
        IPatientRepository patientRepository
        ) : IRequestHandler<CreateAppointmentCommand, Result<string>>//string döndürerek başarı veya hata mesajı verir.
{

       // MediatR Handle metodu çalıştırıldığında, yeni randevu oluşturma işlemi başlar.
        public async Task<Result<string>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {

        //Kullanıcının gönderdiği başlangıç (StartDate) ve bitiş (EndDate) tarihleri DateTime formatına çevriliyor.
        DateTime startDate = Convert.ToDateTime(request.StartDate);
        DateTime endDate = Convert.ToDateTime(request.EndDate);


        Patient patient = new() ;
        if ( request.PatientId is null ) { // Eğer hasta zaten kayıtlı değilse(request.PatientId is null) yeni hasta oluşturuluyor.
            patient = new()
            {
                FirstName = request.firstName,
                LastName = request.lastName,
                IdentityNumber = request.IdentityNumber,
                City = request.City,
                Town = request.Town,
                FullAddress = request.FullAddress

            };
            await patientRepository.AddAsync( patient,cancellationToken ); //patientRepository.AddAsync() ile hastayı veritabanına ekliyor.
        }

        //Aynı doktora ait başka bir randevu olup olmadığı kontrol ediliyor.
        //Randevu saatleri çakışıyorsa hata döndürülecek.
        bool isAppointmentDateNotAvailable =
             await appointmentRepository
             .AnyAsync(p => p.DoctorId == request.DoctorId &&
             ((p.StartDate < endDate && p.StartDate >= startDate) ||//Mevcut randevunun bitişi, diğer randevunun başlangıcıyla çakışıyor
             (p.EndDate > startDate && p.EndDate <= endDate) ||//Mevcut randevunun başlangıcı,diğer randevunun bitişiyle çakışıyor
             (p.StartDate >= startDate && p.EndDate <= endDate) ||//Mevcut randevu,diğer randevu içinde tamamen
             (p.StartDate <= startDate && p.EndDate >= endDate)),
             cancellationToken); //Mevcut randevu, diğer randevuyu tamamen kapsıyor

        if (isAppointmentDateNotAvailable)
        {
            return (HttpStatusCode.NotFound, "Appointment date is not available");
        }

        //Yeni randevu nesnesi oluşturuluyor.
        Appointment appointment = new()
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId ?? patient.Id,//Eğer hasta zaten kayıtlı değilse, yukarıda oluşturulan yeni hastanın ID’si atanıyor.
            StartDate = Convert.ToDateTime(request.StartDate),
            EndDate = Convert.ToDateTime(request.EndDate),
            IsCompleted = false//Randevu tamamlanmadığı için IsCompleted = false olarak ayarlanıyor.
        };


        //Tüm işlemler unitOfWork.SaveChangesAsync() ile tek seferde kaydediliyor.
        await appointmentRepository.AddAsync( appointment,cancellationToken);//Randevu repository'e ekleniyor.
        await unitOfWork.SaveChangesAsync();//Tüm işlemler unitOfWork.SaveChangesAsync() ile tek seferde kaydediliyor.

        return "Appointment create is successful";
    }
}

//📌 Kodun işleyiş sırası:
//1️⃣ Kullanıcının gönderdiği randevu tarihi ve bilgileri alınıyor.
//2️⃣ Hasta daha önce kayıtlı değilse, yeni bir hasta kaydediliyor.
//3️⃣ Aynı doktora ait çakışan randevu olup olmadığı kontrol ediliyor.
//4️⃣ Eğer tarih doluysa hata mesajı dönüyor.
//5️⃣ Yeni randevu oluşturuluyor.
//6️⃣ Veritabanına ekleniyor ve işlemler kaydediliyor.
//7️⃣ Başarı mesajı döndürülüyor.