using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using AutoMapper;
using GenericRepository;
using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.CreateDoctor
{
    internal sealed class CreateDoctorCommandHandler(
        IDoctorRepository doctorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateDoctorCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = mapper.Map<Doctor>(request);

            /* Map kullanıldığı için buna gerek kalmadı
            {
               FirstName = request.FirstName,
                LastName = request.LastName,
                Department = DepartmentEnum.FromValue(request.Department),
            };
            */


            await doctorRepository.AddAsync(doctor, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Doctor created successfully";
        }
    }
}
