using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.GetAllAppointments
{
    public sealed record GetAllAppointmentsByDoctorIdQuery(
        Guid DoctorId):IRequest<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>;
    //List<GetAllAppointmentsByDoctorIdQueryResponse>: Doktorun tüm randevularını içeren bir liste.
    //Her randevu bilgisi, GetAllAppointmentsByDoctorIdQueryResponse sınıfı ile döndürülür.
}
