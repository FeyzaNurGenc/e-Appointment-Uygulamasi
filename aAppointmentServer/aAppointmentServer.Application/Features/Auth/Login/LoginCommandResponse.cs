namespace aAppointmentServer.Application.Features.Auth.Login
{
    
    public sealed record LoginCommandResponse(string Token);
    //Result<LoginCommandResponse> kullanılmasının sebebi, işlemin başarılı olup olmadığını ve
    //başarısızsa neden başarısız olduğunu daha iyi yönetmektir.
    //Result<LoginCommandResponse> → Sonuç tipi, bu işlem başarılı olursa LoginCommandResponse döndürür,
    //başarısız olursa hata mesajlarını veya hata kodlarını içeren bir sonuç döndürebilir.


}
