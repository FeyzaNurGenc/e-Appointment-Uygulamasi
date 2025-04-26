using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using System.Net;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.UpdateDoctor
{
    internal sealed class UpdateDoctorCommandHandler(
        IDoctorRepository doctorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateDoctorCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor? doctor = await doctorRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (doctor is null)
            {
                return (HttpStatusCode.NotFound, "Doctor not found");
            }
            mapper.Map(request, doctor);
            doctorRepository.Update(doctor);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Doctor update is successful";

        }
        
    }
}
