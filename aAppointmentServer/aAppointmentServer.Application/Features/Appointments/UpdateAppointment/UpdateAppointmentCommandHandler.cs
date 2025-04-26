using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.UpdateAppointment
{
    internal sealed class UpdateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork): IRequestHandler<UpdateAppointmentCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            DateTime startDate= Convert .ToDateTime(request.StartDate);
            DateTime endDate = Convert .ToDateTime(request.EndDate);

            Appointment? appointment = 
            await appointmentRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id,cancellationToken);

            if (appointment is null)
            {
                return (HttpStatusCode.NotFound, "Appointment not found");

            }

            bool isAppointmentDateNotAvailable = 
                await appointmentRepository
                .AnyAsync(p => p.DoctorId == appointment.DoctorId &&
                ((p.StartDate<endDate&&p.StartDate>=startDate)||//Mevcut randevunun bitişi, diğer randevunun başlangıcıyla çakışıyor
                (p.EndDate>startDate && p.EndDate<=endDate)||//Mevcut randevunun başlangıcı,diğer randevunun bitişiyle çakışıyor
                (p.StartDate >= startDate && p.EndDate <= endDate) ||//Mevcut randevu,diğer randevu içinde tamamen
                (p.StartDate<=startDate&& p.EndDate>= endDate)),
                cancellationToken); //Mevcut randevu, diğer randevuyu tamamen kapsıyor

            if (isAppointmentDateNotAvailable)
            {
                return (HttpStatusCode.NotFound, "Appointment date is not available");
            }

            appointment.StartDate = Convert.ToDateTime(request.StartDate);
            appointment.EndDate = Convert.ToDateTime(request.EndDate);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Appointments update is successful";
        }
    }
}
