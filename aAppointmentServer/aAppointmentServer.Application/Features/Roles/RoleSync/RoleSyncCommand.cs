using MediatR;
using TS.Result;

namespace aAppointmentServer.Application.Features.Roles.RoleSync
{
    public sealed record RoleSyncCommand():IRequest<Result<string>>;
}
