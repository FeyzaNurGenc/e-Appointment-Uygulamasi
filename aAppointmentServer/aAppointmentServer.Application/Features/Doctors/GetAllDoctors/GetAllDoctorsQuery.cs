using aAppointmentServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.GetAllDoctor
{
    public sealed record GetAllDoctorsQuery() : IRequest<Result<List<Doctor>>>;
}
