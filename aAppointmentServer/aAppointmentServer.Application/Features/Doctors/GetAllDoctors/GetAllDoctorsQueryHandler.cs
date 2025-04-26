using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace aAppointmentServer.Application.Features.Doctors.GetAllDoctor
{
    internal sealed class GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository) : IRequestHandler<GetAllDoctorsQuery, Result<List<Doctor>>>
    {
        public async Task<Result<List<Doctor>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            List<Doctor> doctors = await doctorRepository
                .GetAll()
                .OrderBy(p => p.Department)
                .ThenBy(p => p.FirstName)
                .ToListAsync(cancellationToken);
            return doctors;
        }
    }
}
