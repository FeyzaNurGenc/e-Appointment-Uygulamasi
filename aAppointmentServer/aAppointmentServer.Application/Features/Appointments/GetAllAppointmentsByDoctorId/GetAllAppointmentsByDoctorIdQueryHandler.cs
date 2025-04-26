using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TS.Result;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aAppointmentServer.Application.Features.Appointments.GetAllAppointments
{
    internal sealed class GetAllAppointmentsByDoctorIdQueryHandler(
        IAppointmentRepository appointmentRepository) : IRequestHandler<GetAllAppointmentsByDoctorIdQuery, Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>
{
        public async Task<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>> Handle(GetAllAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            //appointmentRepository.Where(p => p.DoctorId == request.DoctorId): Doktorun ID'sine göre veritabanında randevuları arar.4
            //Burada Where metodu, DoctorId'ye eşit olan randevuları filtreler.
            List<Appointment> appointments =
                await appointmentRepository
                .Where(p => p.DoctorId == request.DoctorId)
                .Include( p => p.Patient)//EF Core kullanılarak, her randevu ile ilişkili hasta bilgisini de yükler
                .ToListAsync(cancellationToken);

            //***BİLGİ***
            //Kullanılan Veritabanı Yöntemleri:
            //Where: EF Core'da, belirli bir koşulu sağlayan verileri filtreler.
            //Include: İlişkili veri(bu durumda hasta bilgileri) ile birlikte ana veriyi yükler. (Eager Loading)
            //ToListAsync: Veritabanı sorgusunu asenkron olarak çalıştırır ve liste olarak döndürür.


            //GetAllAppointmentsByDoctorIdQueryResponse formatında bir yanıta dönüştürür.
            //Bu sınıf, her randevu için gerekli bilgileri (ID, başlama tarihi, bitiş tarihi, hastanın adı vb.) içerir.
            List<GetAllAppointmentsByDoctorIdQueryResponse> responses = 
                appointments.Select(s=> new GetAllAppointmentsByDoctorIdQueryResponse(
                    s.Id,
                    s.StartDate,
                    s.EndDate,
                    s.Patient!.FullName,     
                    s.Patient
                    )).ToList();
            return responses;
        }
    }
}
