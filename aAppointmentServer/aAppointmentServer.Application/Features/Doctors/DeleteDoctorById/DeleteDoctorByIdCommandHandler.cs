using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.DeleteDoctorById
{
    internal sealed class DeleteDoctorByIdCommandHandler(
        IDoctorRepository doctorRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteDoctorByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteDoctorByIdCommand request, CancellationToken cancellationToken)
        {
           Doctor? doctor = await doctorRepository.GetByExpressionAsync(p => p.Id == request.Id , cancellationToken);
            if (doctor is null)
            {
                return (HttpStatusCode.NotFound, "Doctor not found");
            }
            doctorRepository.Delete(doctor);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Doctor delete is successful";
        }
    }
}
