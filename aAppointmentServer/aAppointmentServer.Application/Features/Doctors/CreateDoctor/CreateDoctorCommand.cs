using aAppointmentServer.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.CreateDoctor
{
    public sealed record CreateDoctorCommand(
        string FirstName,
        string LastName,
        int DepartmentValue
        ) : IRequest<Result<string>>; //IRequest<Result<string>>, bu komutun bir MediatR isteği (request) olduğunu belirtir.
}
