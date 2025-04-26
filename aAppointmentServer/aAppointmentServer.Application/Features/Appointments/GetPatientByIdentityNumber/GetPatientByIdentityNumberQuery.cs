using aAppointmentServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.GetPatientByIdentityNumber
{
    public sealed record GetPatientByIdentityNumberQuery(string IdentityNumber):IRequest<Result<Patient>>;

}
