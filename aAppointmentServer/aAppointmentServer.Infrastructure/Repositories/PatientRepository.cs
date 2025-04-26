using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using aAppointmentServer.Infrastructure.Context;
using GenericRepository;

namespace aAppointmentServer.Infrastructure.Repositories
{
    internal class PatientRepository : Repository<Patient, ApplicationDbContext>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
