using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.DeleteDoctorById
{
    public sealed record DeleteDoctorByIdCommand(Guid Id): IRequest<Result<string>>;
}
