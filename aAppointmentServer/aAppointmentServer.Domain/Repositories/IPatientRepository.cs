using aAppointmentServer.Domain.Entities;
using GenericRepository;

namespace aAppointmentServer.Domain.Repositories
{
    public interface IPatientRepository : IRepository<Patient> { }
}
