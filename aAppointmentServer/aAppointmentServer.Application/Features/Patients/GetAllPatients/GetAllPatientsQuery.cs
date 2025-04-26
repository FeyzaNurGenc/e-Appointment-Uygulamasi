using aAppointmentServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.GetAllPatien
{
    public sealed record GetAllPatientsQuery(): IRequest<Result<List<Patient>>>;
}
