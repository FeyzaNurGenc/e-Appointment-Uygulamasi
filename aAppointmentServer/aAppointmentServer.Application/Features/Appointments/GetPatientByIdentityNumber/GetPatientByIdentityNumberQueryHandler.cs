﻿using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Appointments.GetPatientByIdentityNumber
{
    internal sealed class GetPatientByIdentityNumberQueryHandler(
        IPatientRepository patientRepository) : IRequestHandler<GetPatientByIdentityNumberQuery, Result<Patient>>
    {
        public async Task<Result<Patient>> Handle(GetPatientByIdentityNumberQuery request, CancellationToken cancellationToken)
        {
            Patient? patient = await patientRepository.GetByExpressionAsync(p=>p.IdentityNumber== request.IdentityNumber,cancellationToken);

            return patient;
        }
    }

}
