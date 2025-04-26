using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using aAppointmentServer.Infrastructure.Context;
using GenericRepository;

namespace aAppointmentServer.Infrastructure.Repositories
{
    internal sealed class UserRoleRepository : Repository<AppUserRole, ApplicationDbContext>, IUserRoleRepository 
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
