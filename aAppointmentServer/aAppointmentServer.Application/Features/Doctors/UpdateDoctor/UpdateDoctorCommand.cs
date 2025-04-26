using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.UpdateDoctor
{
    public sealed record UpdateDoctorCommand(
        Guid Id,
        string FirstName,
        string LastName,
        int DepartmentValue): IRequest<Result<string>>;
}
