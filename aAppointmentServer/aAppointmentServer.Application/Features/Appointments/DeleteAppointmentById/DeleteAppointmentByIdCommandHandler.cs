using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.DeleteAppointmentById
{
    internal sealed class DeleteAppointmentByIdCommandHandler(
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteAppointmentByIdCommand, Result<string>>

    {
        public async Task<Result<string>> Handle(DeleteAppointmentByIdCommand request, CancellationToken cancellationToken)
        {
            //GetByExpressionAsync Belirtilen ID’ye sahip randevuyu getirir.
            Appointment? appointment = await appointmentRepository.GetByExpressionAsync(p=> p.Id == request.Id, cancellationToken);

            if (appointment == null)
            {
                return (HttpStatusCode.NotFound, "Appointment not found");

            }
            if (appointment.IsCompleted)
            {
                return (HttpStatusCode.NotFound, "You cannot delete a completed appointment");
            }

            appointmentRepository.Delete(appointment);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Appointment delete is successful";
        }
    }
}

//🚀 Özet
//✅ Adım Adım İşleyiş:
//1️⃣ Belirtilen Id'ye sahip randevu aranıyor.
//2️⃣ Eğer randevu bulunamazsa 404 Not Found hatası döndürülüyor.
//3️⃣ Eğer randevu tamamlanmışsa silinmesine izin verilmiyor.
//4️⃣ Randevu başarıyla siliniyor.
//5️⃣ Silme işlemi unitOfWork.SaveChangesAsync() ile kaydediliyor.
//6️⃣ Başarı mesajı döndürülüyor.