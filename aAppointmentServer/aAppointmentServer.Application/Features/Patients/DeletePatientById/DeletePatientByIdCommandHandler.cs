using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Patients.DeletePatientById
{
    internal sealed class DeletePatientByIdCommandHandler(
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeletePatientByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeletePatientByIdCommand request, CancellationToken cancellationToken)
        {
            Patient? patient = await patientRepository.GetByExpressionAsync(p=>p.Id==request.Id,cancellationToken);
            if (patient is null)
            {
                return (HttpStatusCode.NotFound, "Patient not found");
            }

            patientRepository.Delete(patient);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient delete is successfull";
        }
    }
}
