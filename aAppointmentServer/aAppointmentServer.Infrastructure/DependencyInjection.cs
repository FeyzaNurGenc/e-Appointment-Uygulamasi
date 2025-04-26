using aAppointmentServer.Application;
using aAppointmentServer.Domain.Entities;
using aAppointmentServer.Domain.Repositories;
using aAppointmentServer.Infrastructure.Context;
using aAppointmentServer.Infrastructure.Repositories;
using aAppointmentServer.Infrastructure.Services;
using GenericRepository;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace aAppointmentServer.Infrastructure
{
    public static class DependencyInjection
    {
        //Bu metodun amacı, uygulamanın Altyapı Katmanı (Infrastructure Layer) için
        //gerekli servisleri Dependency Injection (DI) konteynerine eklemektir.
        //**
        //IServiceCollection parametresi, servislerin DI konteynerine ekleneceği koleksiyondur.
        //IConfiguration parametresi, uygulamanın yapılandırma ayarlarına erişim sağlar(örneğin veritabanı bağlantı dizesi).
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //AddDbContext<ApplicationDbContext> servisi, Entity Framework Core'un DbContext sınıfını uygulamaya ekler.
            //Bu, veritabanı bağlantısı için gerekli olan konfigürasyonu sağlar.



            // 1. Entity Framework Core'un DbContext'ini kayıt ediyoruz:
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //UseSqlServer metodu, SQL Server veritabanı sağlayıcısını kullanır. Bağlantı dizesi, configuration.GetConnectionString("SqlServer") ile
                //appsettings.json dosyasından alınır.
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            // 2. Identity Ayarları:
            services.AddIdentity<AppUser, AppRole>(action =>
            {
                action.Password.RequiredLength = 1;
                action.Password.RequireUppercase = false;
                action.Password.RequireLowercase = false;
                action.Password.RequireNonAlphanumeric = false;
                action.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            //.AddEntityFrameworkStores<ApplicationDbContext>() metodu, Entity Framework Core'u Identity ile entegre eder.
            //Yani, kullanıcı ve rol bilgileri veritabanında ApplicationDbContext üzerinden saklanacaktır.
            // Bu yapı, Identity'nin kullanıcı (AppUser) ve rol (AppRole) yönetimini ApplicationDbContext üzerinden yapmasını sağlar.

            // 3. UnitOfWork Kayıt:
            // IUnitOfWork, DbContext üzerinden sağlanıyor. Böylece Transaction yönetimi kolaylaşır.
            services.AddScoped<IUnitOfWork>(srv=>srv.GetRequiredService<ApplicationDbContext>());

            // 4. Scrutor ile Assembly Tarama:
            services.Scan(action =>
            {
                action
                .FromAssemblies(typeof(DependencyInjection).Assembly)// Bu assembly'deki sınıfları tarar.
                .AddClasses(publicOnly: false)     // Public olmayan sınıfları da dahil eder.
                .UsingRegistrationStrategy(registrationStrategy: RegistrationStrategy.Skip)// Eğer daha önce kayıt yapılmışsa, tekrar kaydetmez.
                .AsImplementedInterfaces()// Sınıfları implement ettikleri arayüzler ile kaydeder.
                .WithScopedLifetime(); // Her istek için yeni örnek oluşturur.
            });


            /*  ---yukarıda Scan kullanıldı bunlara ihtiyaç kalmadı---
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IJwtProvider , JwtProvider>();
            */



            // 5. Özel Servis Kaydı:
            // Yukarıdaki tarama (Scan) ile çoğu servis otomatik kayıt edilebilir ancak,
            // burada IJwtProvider özel olarak kayıt ediliyor.
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}

/*
  Infrastructure Katmanı DI Dosyası:

Veritabanı bağlantısı ve Entity Framework Core konfigürasyonunu yapar.
Identity sistemini, kullanıcı ve rol yönetimini yapılandırır.
UnitOfWork modelini DbContext üzerinden sağlar.
Scrutor kullanarak ilgili tüm servisleri otomatik olarak DI konteynerine ekler.
Belirli servisler (örn. IJwtProvider) için manuel kayıt yaparak, ihtiyaç duyulan özel konfigürasyonları uygular.
 */
