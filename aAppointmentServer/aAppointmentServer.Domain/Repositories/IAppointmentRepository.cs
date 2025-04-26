using aAppointmentServer.Domain.Entities;
using GenericRepository;

namespace aAppointmentServer.Domain.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment> { }
}
