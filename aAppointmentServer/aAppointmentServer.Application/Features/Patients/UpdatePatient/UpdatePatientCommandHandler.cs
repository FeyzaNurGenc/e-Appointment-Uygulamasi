using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.UpdatePatient
{
    internal sealed class UpdatePatientCommandHandler(
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdatePatientCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            Patient? patient = await patientRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (patient is null)
            {
                return (HttpStatusCode.NotFound, "Patient already recorded");
            }

            if (patient.IdentityNumber != request.IdentityNumber)
            {
                if (patientRepository.Any(p => p.IdentityNumber == request.IdentityNumber))
                {
                    return (HttpStatusCode.NotFound, "This identity number already use");
                }

            }

            mapper.Map(request, patient);
            patientRepository.Update(patient);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient update is successful";
        }
    }
}
