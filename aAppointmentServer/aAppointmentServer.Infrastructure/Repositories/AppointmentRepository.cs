using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using aAppointmentServer.Infrastructure.Context;
using GenericRepository;

namespace aAppointmentServer.Infrastructure.Repositories
{
    internal class AppointmentRepository : Repository<Appointment, ApplicationDbContext>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
