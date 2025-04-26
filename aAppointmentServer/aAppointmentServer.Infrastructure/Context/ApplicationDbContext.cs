using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Infrastructure.Configurations;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace aAppointmentServer.Infrastructure.Context
{
    public class ApplicationDbContext
        : IdentityDbContext<AppUser, AppRole, Guid,
           
            IdentityUserClaim<Guid>,  // Kullanıcıya ait claim'ler
            AppUserRole,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>,
            IdentityUserToken<Guid>
        >, IUnitOfWork
    {    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
            
        }
        public DbSet<Doctor>? Doctors { get; set; }
        public DbSet<Patient>? Patients { get; set; }
        public DbSet<Appointment>? Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<IdentityUserClaim<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();
            builder.Ignore<IdentityUserLogin<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();


         

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
