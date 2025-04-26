using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Auth.Login
{
    //LoginCommand bir Command (Komut) nesnesidir ve kullanıcıdan gelen veriyi taşır.
    //Kullanıcı giriş yapmaya çalışırken bu komut çalıştırılır.
    public sealed record LoginCommand(
        string UserNameOrEmail,
        string Password): IRequest<Result<LoginCommandResponse>>;

    //IRequest<Result<LoginCommandResponse>> → MediatR kütüphanesini kullanarak bu komutun bir "istek" olduğunu belirtiyor.

}
