using MediatR;  // MediatR kütüphanesini içe aktarıyor. CQRS (Command Query Responsibility Segregation) modelinde kullanılır.
using TS.Result; // TS.Result sınıfını içe aktarıyor, bu genellikle işlem sonuçlarını (başarılı/başarısız) döndürmek için kullanılır.

namespace aAppointmentServer.Application.Features.Appointments.CreateAppointment
{
    // `sealed record` olarak tanımlanan bir command sınıfı. `record`, immutable (değiştirilemez) veri taşıyıcılarıdır.
    public sealed record CreateAppointmentCommand(
        string StartDate,
        string EndDate,
        Guid DoctorId,
        Guid? PatientId,
        string firstName,
        string lastName,
        string IdentityNumber,
        string City,
        string Town,
        string FullAddress
 
        ) : IRequest<Result<string>>;// MediatR’ın IRequest arayüzünü implemente eder. Bu, bu command’in bir handler tarafından işleneceğini gösterir.

};
