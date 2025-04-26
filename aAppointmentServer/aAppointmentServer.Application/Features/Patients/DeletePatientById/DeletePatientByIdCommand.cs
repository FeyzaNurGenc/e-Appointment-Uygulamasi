using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.DeletePatientById
{
    public sealed record DeletePatientByIdCommand(Guid Id): IRequest<Result<string>>;
}
