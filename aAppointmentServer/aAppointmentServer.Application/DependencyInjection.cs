using Microsoft.Extensions.DependencyInjection;

namespace aAppointmentServer.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            //Application katmanındaki MediatR bağımlılıklarını DI konteynerine ekler.
            //MediatR, uygulama içindeki istek/cevap (request/response) ve bildirim (notification) tabanlı iletişimi kolaylaştıran bir kütüphanedir.
            services.AddMediatR(configuration =>
            {
                //bu katmanda bulunan tüm MediatR handler’larını (istek işleyicileri, bildirim dinleyicileri vb.) otomatik olarak tarar ve kaydeder.
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            return services;
        }
    }
}

//DependencyInjection yapısı, uygulama içerisindeki iş akışlarının (örneğin, komutların veya sorguların) merkezi bir şekilde yönetilmesini sağlar.
//Böylece, farklı parçaların birbirleriyle doğrudan iletişim kurması yerine, MediatR üzerinden iletişim kurulmuş olur.