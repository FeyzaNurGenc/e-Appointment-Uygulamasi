using aAppointmentServer.Domain.Entities;

namespace aAppointmentServer.Application.Features.Appointments.GetAllAppointments
{
    public sealed record GetAllAppointmentsByDoctorIdQueryResponse(
        Guid Id,
        DateTime StartDate,
        DateTime EndDate,
        string Title,
        Patient? Patient);
}
