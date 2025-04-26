using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using aAppointmentServer.Infrastructure.Context;
using GenericRepository;

namespace aAppointmentServer.Infrastructure.Repositories
{
    internal class DoctorRepository : Repository<Doctor, ApplicationDbContext>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
