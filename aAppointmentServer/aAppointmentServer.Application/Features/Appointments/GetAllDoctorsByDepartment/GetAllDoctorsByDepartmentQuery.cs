using aAppointmentServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.GetAllDoctorByDepartment
{
    public sealed record GetAllDoctorsByDepartmentQuery(
        int DepartmentValue): IRequest<Result<List<Doctor>>>;
}
