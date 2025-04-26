using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.CreatePatient
{
    internal sealed class CreatePatientCommandHandler(
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreatePatientCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            if (patientRepository.Any(p => p.IdentityNumber == request.IdentityNumber))
            {
                return (HttpStatusCode.NotFound, "This identity number already use");
            }
            
            Patient patient = mapper.Map<Patient>(request);
            await patientRepository.AddAsync(patient,cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient create is successful";
        }
    }
}
