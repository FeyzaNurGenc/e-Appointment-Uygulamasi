using aAppointmentServer.Domain.Entities;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aAppointmentServer.Domain.Repositories
{
    public interface IUserRoleRepository: IRepository<AppUserRole>
    {
    }
}
