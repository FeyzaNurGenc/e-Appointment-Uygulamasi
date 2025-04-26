using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.CreatePatient
{
    public sealed record CreatePatientCommand(
        string FirstName,
        string Lastname,
        string IdentityNumber,
        string City,
        string Town,
        string FullAddress): IRequest<Result<string>>;
}
