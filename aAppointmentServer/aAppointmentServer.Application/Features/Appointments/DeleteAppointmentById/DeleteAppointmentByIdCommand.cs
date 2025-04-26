using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.DeleteAppointmentById
{
    public sealed record DeleteAppointmentByIdCommand(Guid Id): IRequest<Result<string>>;
}
